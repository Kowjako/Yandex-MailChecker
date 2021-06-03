using System;
using System.Collections.Generic;
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

        public ContactWindow(string type, int x) : this()    //wywolanie domyslnego konstruktora aby zainicjalizowac View, bo ten jest przeciazony
        {
            this.type = type;
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
                    contactData.Inlines.Add(new Hyperlink() { NavigateUri = new Uri("https://instagram.com/wlodzimierzyk") });
                    break;
                case "vk":
                    infoLabel.Text = defaultString + "VK";
                    contactData.Inlines.Clear();
                    contactData.Inlines.Add(new Hyperlink() { NavigateUri = new Uri("https://vk.com/kovyako") });
                    break;
                case "faceboook":
                    infoLabel.Text = defaultString + "Facebook";
                    contactData.Inlines.Clear();
                    contactData.Inlines.Add(new Hyperlink() { NavigateUri = new Uri("https://www.facebook.com/kowyako") });
                    break;
            }
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
