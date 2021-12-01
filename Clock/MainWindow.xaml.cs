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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool exit;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void timer()
        {
            var culture = System.Globalization.CultureInfo.GetCultureInfo("ja-JP");

            while (!exit)
            {
                Dispatcher.Invoke(() => txtDate.Text = string.Format(culture, "{0:yyyy/MM/dd dddd}", DateTime.Today));
                Dispatcher.Invoke(() => txtTime.Text = DateTime.Now.ToLongTimeString());
                await Task.Delay(1000);
            }

            Dispatcher.Invoke(Application.Current.Shutdown);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e) => exit = true;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Location = new System.Drawing.Point((int)Left, (int)Top);
            Properties.Settings.Default.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(timer);
            Left = Properties.Settings.Default.Location.X;
            Top = Properties.Settings.Default.Location.Y;
        }
    }
}
