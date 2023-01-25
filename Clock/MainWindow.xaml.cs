using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using System.Windows.Threading;

namespace Clock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool exit;
        private readonly CultureInfo culture = CultureInfo.GetCultureInfo("ja-JP");
        private Theme theme = Theme.Light;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task tick()
        {
            while (!exit)
            {
                Dispatcher.Invoke(() =>
                {
                    txtDate.Text = string.Format(culture, "{0:yyyy/MM/dd dddd}", DateTime.Today);
                    txtTime.Text = DateTime.Now.ToLongTimeString();
                });

                await Task.Delay(1000);
            }
        }

        private async Task tock()
        {
            while (!exit)
            {
                Theme current = Application.Current.InDarkMode() ? Theme.Dark : Theme.Light;
                Dispatcher.Invoke(() =>
                {
                    if (theme != current)
                        LoadTheme(current);
                });
                await Task.Delay(5000);
            }
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
            System.Drawing.Point position = Properties.Settings.Default.Location;
            if (position.X < SystemParameters.VirtualScreenLeft ||
                position.Y < SystemParameters.VirtualScreenTop ||
                SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth < position.X ||
                SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight < position.Y)
            {
                Left = Top = 100;
            }
            else
            {
                Left = position.X;
                Top = position.Y;
            }

            Task.Run(() =>
            {
                Task.WaitAny(tick(), tock());
                Dispatcher.Invoke(Application.Current.Shutdown);
            });
        }

        /// <summary>
        /// 更新资源
        /// </summary>
        /// <param name="current"></param>
        private void LoadTheme(Theme current) 
        {
            theme = current;
            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(
                new ResourceDictionary
                {
                    Source = new Uri($"/Themes/{theme}.xaml", UriKind.RelativeOrAbsolute)
                });
        }
    }

    internal enum Theme { Light, Dark }
}
