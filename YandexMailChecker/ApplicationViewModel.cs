using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace YandexMailChecker
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        public string LoadedAccountsCount
        {
            get { return Convert.ToString(loadedAccountList == null ? 0 : loadedAccountList.Count); }
        }

        private List<Account> loadedAccountList { get; set; }       //zaladowana baza danych
        private List<Proxy> loadedProxyList { get; set; }           //zaladowane proxy

        public TestAccount testAccount { get; set; }                //konto osobiste do sprawdzenia dzialania programu

        public ObservableCollection<Account> accountList { get; set; }      //konta wyswietlane na DataGrid
        public ObservableCollection<string> userFilters { get; set; }       //filtry wybrane przez uzytkownika

        private RelayCommand addAccountCommand;
        private RelayCommand testConnectionCommand;
        private RelayCommand loadDatabaseCommand;
        private RelayCommand changeSeparatorsCommand;
        private RelayCommand loadProxiesCommand;
        private RelayCommand verifyAccountCommand;
        private RelayCommand addFilterCommand;
        private RelayCommand startCheckerCommand;

        protected IDialogService dialogService;         //DialogService do zaladowania manadzera plikow

        public ApplicationViewModel(IDialogService dialogService)
        {
            testAccount = new TestAccount();
            userFilters = new ObservableCollection<string>();
            this.dialogService = dialogService;
            accountList = new ObservableCollection<Account>();
            //accountList.Add(new Account("kowyako@yandex.ru", "5667309vavan+", new List<string>() { "Steam", "Apple" }));
            //accountList.Add(new Account("andreypevniy@yandex.ru", "pevniy2021", new List<string>() { "LeagueOfLegends", "MusicalShop" }));
            //accountList.Add(new Account("leanidp@yandex.ru", "dkaslkdalskflas", new List<string>() { "Steam", "Apple", "Beverly Hills" }));
            //accountList.Add(new Account("rager13@yandex.ru", "qazsew123", new List<string>() { "Steam", "Apple", "BruteForce" }));
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

        public RelayCommand LoadDatabaseCommand
        {
            get
            {
                return loadDatabaseCommand ??
                    (loadDatabaseCommand = new RelayCommand(obj =>
                    {
                        try
                        {
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
                            dialogService.ShowMessage(ex.Message);
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
                                dialogService.ShowMessage("Splitters was changed");
                            }
                        }
                        catch (Exception ex)
                        {
                            dialogService.ShowMessage(ex.Message);
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
                            if (dialogService.OpenFileDialog() == true)
                            {
                                string record;
                                loadedProxyList = new List<Proxy>();
                                using (StreamReader reader = new StreamReader(dialogService.FilePath))
                                {
                                    while ((record = reader.ReadLine()) != null)
                                        loadedProxyList.Add(new Proxy(record.Split(':')[0], record.Split(':')[1]));
                                }
                                dialogService.ShowMessage($"{loadedProxyList.Count} proxies was loaded successfully");
                            }
                        }
                        catch (Exception ex)
                        {
                            dialogService.ShowMessage(ex.Message);
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
                       foreach(Account acc in loadedAccountList)
                       {
                           acc.CheckAccount(userFilters);
                           if(acc.getFiltersCount!=0)
                           {
                               AddAccountCommand.Execute(acc);
                           }
                       }
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
