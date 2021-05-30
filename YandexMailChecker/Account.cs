using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexMailChecker
{
    class Account
    {
        private List<string> filters;
        private string email;
        private string password;

        public string Email => email;
        public string Password => password;
        public List<string> Filters => filters;

        public Account(string email, string password, List<string> filters)
        { 
            this.email = email;
            this.password = password;
            this.filters = filters;
        }
    }
}
