using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexMailChecker
{
    public class Account
    {
        private List<string> filters;
        private string email;
        private string password;

        public string Email => email;
        public string Password => password;
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

        public Account(string email, string password, List<string> filters)
        { 
            this.email = email;
            this.password = password;
            this.filters = filters;
        }
    }
}
