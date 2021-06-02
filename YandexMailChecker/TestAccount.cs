using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace YandexMailChecker
{
    class TestAccount : INotifyPropertyChanged
    {
        private string login;
        private string password;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public bool CheckEmail()
        {
            try
            {
                MailAddress email = new MailAddress(login);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool ValidateCredentials()
        {
            return false;
        }
    }
}
