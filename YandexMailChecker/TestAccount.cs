using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using AE.Net.Mail;
using System.Windows;
using System.Threading.Tasks;
using System.Threading;

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

        public void ValidateCredentials()
        {
            ImapClient imapClient = new ImapClient("imap.yandex.ru", Login, Password, AuthMethods.Login, 993, true);
        }
    }
}
