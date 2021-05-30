using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexMailChecker
{
    class ApplicationViewModel
    {
        public ObservableCollection<Account> accountList { get; set; }

        public ApplicationViewModel()
        {
            accountList = new ObservableCollection<Account>();
            accountList.Add(new Account("kowyako@yandex.ru", "5667309vavan+", new List<string>() { "Steam", "Apple" }));
            accountList.Add(new Account("andreypevniy@yandex.ru", "pevniy2021", new List<string>() { "LeagueOfLegends", "MusicalShop" }));
            accountList.Add(new Account("leanidp@yandex.ru", "dkaslkdalskflas", new List<string>() { "Steam", "Apple", "Beverly Hills" }));
            accountList.Add(new Account("rager13@yandex.ru", "qazsew123", new List<string>() { "Steam", "Apple", "BruteForce" }));
        }

    }
}
