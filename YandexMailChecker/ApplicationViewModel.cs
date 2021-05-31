using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows;

namespace YandexMailChecker
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Account> accountList { get; set; }
        private RelayCommand addAccountCommand;
        private RelayCommand testConnectionCommand;

        public ApplicationViewModel()
        {
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
