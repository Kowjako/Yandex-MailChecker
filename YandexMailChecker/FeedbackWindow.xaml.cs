using System.Windows.Input;
using System.Net.Mail;
using System.Windows;
using System.Windows.Documents;
using System.Net;

namespace YandexMailChecker
{
    /// <summary>
    /// Interaction logic for FeedbackWindow.xaml
    /// </summary>
    public partial class FeedbackWindow : Window
    {
        public FeedbackWindow()
        {
            InitializeComponent();
        }

        private void header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void opnionBtn_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(Message.Document.ContentStart, Message.Document.ContentEnd);

            MailAddress from = new MailAddress("kowyako@yandex.ru", "YandexMailChecker");
            MailAddress to = new MailAddress("kovyakovladimir@gmail.com");
            MailMessage message = new MailMessage(from, to) { Subject = Title.Text, Body = textRange.Text, IsBodyHtml = false };

            using (SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587))
            {
                smtp.Credentials = new NetworkCredential("kowyako@yandex.ru", "");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
        }
    }
}
