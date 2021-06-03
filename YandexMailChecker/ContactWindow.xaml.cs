using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YandexMailChecker
{
    /// <summary>
    /// Interaction logic for ContactWindow.xaml
    /// </summary>
    public partial class ContactWindow : Window
    {
        private string type;
        private string defaultString = "Contact with me via ";

        public ContactWindow()
        {
            InitializeComponent();
        }

        public ContactWindow(string type) : this()    //wywolanie domyslnego konstruktora aby zainicjalizowac View, bo ten jest przeciazony
        {
            this.type = type;
            Hyperlink hyperLink = new Hyperlink();
            switch(type)
            {
                case "gmail":
                    infoLabel.Text = defaultString + "E-mail";
                    contactData.Inlines.Clear();
                    contactData.Text = "kovyakovladimir@gmail.com";
                    break;
                case "instagram":
                    infoLabel.Text = defaultString + "Instagram";
                    contactData.Inlines.Clear();
                    hyperLink.Inlines.Clear();
                    hyperLink.NavigateUri = new Uri("https://instagram.com/wlodzimierzyk");
                    hyperLink.Inlines.Add("https://instagram.com/wlodzimierzyk");
                    hyperLink.RequestNavigate += HyperLink_RequestNavigate;
                    contactData.Inlines.Add(hyperLink);
                    break;
                case "vk":
                    infoLabel.Text = defaultString + "VK";
                    contactData.Inlines.Clear();
                    hyperLink.Inlines.Clear();
                    hyperLink.NavigateUri = new Uri("https://vk.com/kovyako");
                    hyperLink.Inlines.Add("https://vk.com/kovyako");
                    hyperLink.RequestNavigate += HyperLink_RequestNavigate;
                    contactData.Inlines.Add(hyperLink);
                    break;
                case "facebook":
                    infoLabel.Text = defaultString + "Facebook";
                    contactData.Inlines.Clear();
                    hyperLink.Inlines.Clear();
                    hyperLink.NavigateUri = new Uri("https://www.facebook.com/kowyako");
                    hyperLink.Inlines.Add("https://www.facebook.com/kowyako");
                    hyperLink.RequestNavigate += HyperLink_RequestNavigate;
                    contactData.Inlines.Add(hyperLink);
                    break;
            }
        }

        private void HyperLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(((Hyperlink)sender).NavigateUri.ToString()));
            e.Handled = true;
        }

        private void closeBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
