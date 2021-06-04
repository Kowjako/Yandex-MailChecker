using AE.Net.Mail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YandexMailChecker
{
    public class Account
    {
        private List<string> filters;       //lista znalezionych filtrow
        private string email;
        private string password;

        public string Email => email;
        public string Password => password;
        public int getFiltersCount => filters.Count;

        public string Filters
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                string[] stringFilter = filters.ToArray();
                for (int i = 0; i < stringFilter.Length-1; i++)
                    sb.Append(stringFilter[i] + ", ");
                sb.Append(stringFilter[stringFilter.Length - 1]);
                return sb.ToString();
            }
        }

        public Account(string email, string password)
        { 
            this.email = email;
            this.password = password;
            filters = new List<string>();
        }

        public bool CheckAccount(ObservableCollection<string> userFilters)
        {
            MessageBox.Show($"Email = {email}, Pass = {password}");
            try
            {
                ImapClient imapClient = new ImapClient("imap.yandex.ru", email, password, AuthMethods.Login, 993, true);
                foreach (var s in userFilters)
                {
                    var msgs = imapClient.SearchMessages(SearchCondition.From(s));
                    if (msgs.Count() != 0) filters.Add(s);
                }
                return true;
            }
            catch
            {
                return false;   //false - blad podczas sprawdzania profilu
            }
        }

    }
}
