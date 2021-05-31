using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;


namespace YandexMailChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Account> accountList { get; set; }

        private string path = @"D:\YandexMailChecker\";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
        }

        private void closeBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Thread.Sleep(100);
            this.Close();
        }

        private void catalogueBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DirectoryInfo outputDirectory = new DirectoryInfo(path);
            if (!outputDirectory.Exists) outputDirectory.Create();
            Process.Start("explorer.exe", path);
        }

        private void headerPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void addFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filterBox.Text)) return;
            Chip newFilter = new Chip() { IsDeletable = true, Content = filterBox.Text, Margin = new Thickness(0,10,10,0)};
            newFilter.DeleteClick += NewFilter_DeleteClick;
            filterPanel.Children.Add(newFilter);
        }

        private void NewFilter_DeleteClick(object sender, RoutedEventArgs e)
        {
            filterPanel.Children.Remove((Chip)sender);
        }
    }
}
