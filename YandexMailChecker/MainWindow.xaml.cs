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
        private string path = @"D:\YandexMailChecker\";

        public MainWindow()
        {
            InitializeComponent();
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
    }
}
