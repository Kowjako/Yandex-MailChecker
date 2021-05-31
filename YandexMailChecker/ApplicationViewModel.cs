using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows;

namespace YandexMailChecker
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        public string LoadedAccountsCount
        {
            get { return Convert.ToString(loadedAccountList == null ? 0 : loadedAccountList.Count); }
        }

        private List<Account> loadedAccountList { get; set; }
        private List<Proxy> loadedProxyList { get; set; }

        public ObservableCollection<Account> accountList { get; set; }

        private RelayCommand addAccountCommand;
        private RelayCommand testConnectionCommand;
        private RelayCommand loadDatabaseCommand;
        private RelayCommand changeSeparatorsCommand;
        private RelayCommand loadProxiesCommand;

        protected IDialogService dialogService;

        public ApplicationViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            accountList = new ObservableCollection<Account>();
            accountList.Add(new Account("kowyako@yandex.ru", "5667309vavan+", new List<string>() { "Steam", "Apple" }));
            accountList.Add(new Account("andreypevniy@yandex.ru", "pevniy2021", new List<string>() { "LeagueOfLegends", "MusicalShop" }));
            accountList.Add(new Account("leanidp@yandex.ru", "dkaslkdalskflas", new List<string>() { "Steam", "Apple", "Beverly Hills" }));
            accountList.Add(new Account("rager13@yandex.ru", "qazsew123", new List<string>() { "Steam", "Apple", "BruteForce" }));
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
                                        loadedAccountList.Add(new Account(record.Split(' ')[0], record.Split(' ')[1], null));
                                }
                                OnPropertyChanged("LoadedAccountsCount");   //po wczytaniu musimy poinformowac View ze zmienil sie parametr LoadedAccountsCount
                                dialogService.ShowMessage("Database was loaded successfully");
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
                                        loadedProxyList.Add(new Proxy(record.Split(' ')[0], record.Split(' ')[1]));
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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
