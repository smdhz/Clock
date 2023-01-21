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

namespace Clock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool exit;
        private readonly CultureInfo culture = CultureInfo.GetCultureInfo("ja-JP");

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void tick()
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

            Dispatcher.Invoke(Application.Current.Shutdown);
        }

        //private async void tock()
        //{
        //    while (!exit)
        //    {
        //        Dispatcher.Invoke(() =>
        //        {
        //            pgsCpu.Value = cpu.Sensors.Where(s => s.SensorType == SensorType.Load).Max(i => i.Value ?? 0);
        //            pgsMem.Value = pgsMem.Maximum - memUsage.NextValue();
        //        });
        //        await Task.Delay(5000);
        //    }
        //}

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

            Task.Run(tick);
        }
    }
}
