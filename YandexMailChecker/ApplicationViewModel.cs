using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace YandexMailChecker
{
    class ApplicationViewModel : INotifyPropertyChanged
    {

        public string LoadedAccountsCount
        {
            get { return Convert.ToString(loadedAccountList == null ? 0 : loadedAccountList.Count); }
        }

        public string CheckedAccountsCount
        {
            get { return Convert.ToString(checkedAccountCount); }
        }

        public string WithFilterAccountCount
        {
            get { return Convert.ToString(withFilterAccountCount); }
        }

        public string ErrorAccountCount
        {
            get { return Convert.ToString(errorAccountCount); }
        }

        public string LoadProxiesCount
        {
            get { return Convert.ToString(loadProxiesCount); }
        }

        public string ValidateAccountCount
        {
            get { return Convert.ToString(validateAccountCount); }
        }

        public int AppProgressStatus
        {
            get { return (int)(appProgressStatus * 100); }
        }

        private List<Account> loadedAccountList { get; set; }       //zaladowana baza danych
        private List<Proxy> loadedProxyList { get; set; }           //zaladowane proxy

        public TestAccount testAccount { get; set; }                //konto osobiste do sprawdzenia dzialania programu

        public ObservableCollection<Account> accountList { get; set; }      //konta wyswietlane na DataGrid
        public ObservableCollection<string> userFilters { get; set; }       //filtry wybrane przez uzytkownika

        private CancellationTokenSource cts;

        private static NLog.Logger logger;

        private Random randomizer;

        private RelayCommand viewLoggerCommand;
        private RelayCommand addAccountCommand;
        private RelayCommand testConnectionCommand;
        private RelayCommand loadDatabaseCommand;
        private RelayCommand changeSeparatorsCommand;
        private RelayCommand loadProxiesCommand;
        private RelayCommand verifyAccountCommand;
        private RelayCommand addFilterCommand;
        private RelayCommand startCheckerCommand;
        private RelayCommand stopCheckerCommand;


        protected IDialogService dialogService;         //DialogService do zaladowania manadzera plikow

        private int checkedAccountCount = 0, withFilterAccountCount = 0, errorAccountCount = 0, validateAccountCount = 0, loadProxiesCount = 0;
        private double appProgressStatus = 0;

        public ApplicationViewModel(IDialogService dialogService)
        {
            randomizer = new Random();
            logger = NLog.LogManager.GetCurrentClassLogger();
            cts = new CancellationTokenSource();
            testAccount = new TestAccount();
            userFilters = new ObservableCollection<string>();
            this.dialogService = dialogService;
            accountList = new ObservableCollection<Account>();
            accountList.Add(new Account("sample1@yandex.ru", "qwerty5678", new List<string>() { "Steam", "Apple" }));
            accountList.Add(new Account("sample2@yandex.ru", "pevniy2021", new List<string>() { "LeagueOfLegends", "MusicalShop" }));
            accountList.Add(new Account("sample3@yandex.ru", "dkaslkdalskflas", new List<string>() { "Steam", "Apple", "Showroom.pl" }));
            accountList.Add(new Account("sample4@yandex.ru", "qazsew123", new List<string>() { "Steam", "Apple", "WebMoney", "PayPal" }));
        }

        public RelayCommand AddAccountCommand
        {
            get
            {
                return addAccountCommand ??
                    (addAccountCommand = new RelayCommand(obj =>
                    {
                        Account acc = obj as Account;
                        accountList.Add(acc);
                        using(StreamWriter sw = new StreamWriter(@"D:\YandexMailChecker\result.txt",false,Encoding.Default))
                        {
                            sw.WriteLineAsync($"{acc.Email}, {acc.Password}: {acc.Filters}");
                        }
                    }));
            }
        }

        public RelayCommand ViewLoggerCommand
        {
            get
            {
                return viewLoggerCommand ??
                    (viewLoggerCommand = new RelayCommand(obj =>
                    {
                        Process.Start("explorer.exe", @"D:\YandexMailChecker\logs\");
                    }));
            }
        }

        public RelayCommand TestConnectionCommand
        {
            get
            {
                return testConnectionCommand ??
                    (testConnectionCommand = new RelayCommand(obj =>
                    {
                        Ping p = new Ping();
                        PingReply reply = p.Send("google.com", 1000, new byte[255], new PingOptions());
                    if (reply.Status == IPStatus.Success) 
                        MessageBox.Show($"Connection with the Internet: Connected!","Connection verifying",MessageBoxButton.OK,MessageBoxImage.Information);
                    else
                        MessageBox.Show($"Connection with the Internet: Not connected", "Connection verifying", MessageBoxButton.OK, MessageBoxImage.Information);
                    }));
            }
        }

        public RelayCommand StopCheckerCommand
        {
            get
            {
                return stopCheckerCommand ??
                    (stopCheckerCommand = new RelayCommand(obj =>
                    {
                        cts.Cancel();
                    }));
            }
        }

        public RelayCommand LoadDatabaseCommand
        {
            get
            {
                return loadDatabaseCommand ??
                    (loadDatabaseCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            logger.Info("Starting loading database");
                            if(dialogService.OpenFileDialog() == true)
                            {
                                string record;
                                loadedAccountList = new List<Account>();
                                using (StreamReader reader = new StreamReader(dialogService.FilePath))
                                {
                                    while ((record = reader.ReadLine()) != null)
                                    {
                                        loadedAccountList.Add(new Account(record.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0], record.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]));
                                    }
                                }
                                OnPropertyChanged("LoadedAccountsCount");   //po wczytaniu musimy poinformowac View ze zmienil sie parametr LoadedAccountsCount
                            }
                        }
                        catch(Exception ex)
                        {
                            logger.Error(ex);
                        }
                        finally
                        {
                            logger.Info("Database was loaded successfully");
                        }
                    }));
            }
        }

        public RelayCommand ChangeSeparatorsCommand
        {
            get
            {
                return changeSeparatorsCommand ??
                    (changeSeparatorsCommand = new RelayCommand(obj =>
                    {
                        char[] splitters = { ':', ',', '|' };
                        string loadedFullText;
                        try
                        {
                            logger.Info("Start opening file for change separators");
                            if(dialogService.OpenFileDialog() == true)
                            {
                                using (StreamReader reader = new StreamReader(dialogService.FilePath))
                                {
                                    loadedFullText = reader.ReadToEnd();
                                    foreach (char splitter in splitters)
                                        loadedFullText = loadedFullText.Replace(splitter, ' ');
                                }
                                using (StreamWriter dataWriter = new StreamWriter(dialogService.FilePath, false))
                                {
                                    dataWriter.Write(loadedFullText);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex);
                        }
                        finally
                        {
                            logger.Info("Splitters was changed");
                        }
                    }));
            }
        }


        public RelayCommand LoadProxiesCommand
        {
            get
            {
                return loadProxiesCommand ??
                    (loadProxiesCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            logger.Info("Start opening file with proxies");
                            if (dialogService.OpenFileDialog() == true)
                            {
                                string record;
                                loadedProxyList = new List<Proxy>();
                                using (StreamReader reader = new StreamReader(dialogService.FilePath))
                                {
                                    while ((record = reader.ReadLine()) != null)
                                        loadedProxyList.Add(new Proxy(record.Split(':')[0], record.Split(':')[1]));
                                }
                                loadProxiesCount = loadedProxyList.Count;
                                OnPropertyChanged("LoadProxiesCount");
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex);
                        }
                        finally
                        {
                            logger.Info("Proxies was loaded successfully");
                        }
                    }));
            }
        }

        public RelayCommand VerifyAccountCommand
        {
            get
            {
                return verifyAccountCommand ??
                    (verifyAccountCommand = new RelayCommand(async obj =>
                    {
                        testAccount.Password = (obj as PasswordBox).Password;       //PasswordBox wystepuje jako parametr Command we View dla przycisku Verify application
                        if (testAccount.CheckEmail())
                        {
                            try
                            {
                                await Task.Run(() => {
                                    testAccount.ValidateCredentials();              //jezeli nie uda sie logowanie to ValidateCredentials wyrzuci blad i catch ponizej przechywci.
                                });
                                MessageBox.Show($"Successfully login with this credentials!", "Account verifying", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch(Exception)
                            {
                                MessageBox.Show($"Unfortunately canno't login with this credentials!", "Account verifying", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else MessageBox.Show($"Please write correct e-mail address", "Email verifying", MessageBoxButton.OK, MessageBoxImage.Information);
                    }));
            }
        }

        public RelayCommand AddFilterCommand
        {
            get
            {
                return addFilterCommand ??
                    (addFilterCommand = new RelayCommand(obj =>
                    {
                        if(!userFilters.Contains((string)obj))
                        userFilters.Add((string)obj);
                    }));
            }
        }

        public RelayCommand StartCheckerCommand
        {
            get
            {
                return startCheckerCommand ??
                    (startCheckerCommand = new RelayCommand(obj =>
                    {
                        var token = cts.Token;
                        var tasks = new Task[loadedAccountList.Count];
                        for (var i = 0; i < loadedAccountList.Count; i++)
                        {
                            var account = loadedAccountList[i];
                            tasks[i] = Task.Factory.StartNew(() =>
                            {
                                WebRequest.DefaultWebProxy = new WebProxy(loadedProxyList[randomizer.Next(0,loadedProxyList.Count)].address, Convert.ToInt32(loadedProxyList[randomizer.Next(0, loadedProxyList.Count)].port));
                                var isValidate = false;
                                isValidate = account.CheckAccount(userFilters, logger);
                                Task.Delay(100);

                                if (isValidate)
                                {
                                    validateAccountCount++;
                                    OnPropertyChanged("ValidateAccountCount");
                                    if (account.getFiltersCount > 0)
                                    {
                                        /* Jezeli sa filtry odnawiamy ilosc */
                                        withFilterAccountCount++;
                                        OnPropertyChanged("WithFilterAccountCount");
                                        Application.Current.Dispatcher.Invoke(() => AddAccountCommand.Execute(account));
                                    }
                                }
                                else
                                {
                                    errorAccountCount++;
                                    OnPropertyChanged("ErrorAccountCount");
                                }
                                checkedAccountCount++;
                                OnPropertyChanged("CheckedAccountsCount");
                                appProgressStatus = (double)checkedAccountCount / loadedAccountList.Count;
                                OnPropertyChanged("AppProgressStatus");
                            }, token);
                        }

                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                Task.WaitAll(tasks, token);
                            }
                            catch (OperationCanceledException)
                            {
                                logger.Info("Checker was stopped by user");
                            }
                        });

                    }));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
