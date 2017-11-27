using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Media;
using MahApps.Metro.Controls;

namespace EyeRest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : MetroWindow
    {
        private System.Windows.Threading.DispatcherTimer timeS = new System.Windows.Threading.DispatcherTimer();
        private SoundPlayer sp = new SoundPlayer();
        private int time_s = 0, time_m = 0, k = 0, min_main, min_delay;
        public MainWindow()
        {
            InitializeComponent();

            string[] settings = System.IO.File.ReadAllLines("settings.ini");

            slider_main.Value = Convert.ToInt32(settings[0]);
            slider_delay.Value = Convert.ToInt32(settings[1]);

            if (settings[2] == "True")
                sound_check.IsChecked = true;
            else
                sound_check.IsChecked = false;

            time_m = Convert.ToInt32(slider_main.Value);
            min_main = Convert.ToInt32(slider_main.Value);
            min_delay = Convert.ToInt32(slider_delay.Value);

            if (min_main < 10)
                RemainingM.Content = "0" + min_main;
            else
                RemainingM.Content = min_main;

            timeS.Tick += new EventHandler(ShowTimeS);
            timeS.Interval = new TimeSpan(0, 0, 1);
            timeS.Start();

            System.Windows.Forms.ContextMenuStrip cntx_menu = new System.Windows.Forms.ContextMenuStrip();
            cntx_menu.Items.Add("Настройки");
            cntx_menu.Items.Add("Выход");
            cntx_menu.Items[1].Click += Close_Click;
            cntx_menu.Items[0].Click += Settings_button_Click;

            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("icon_eye.ico");
            ni.Visible = true;
            ni.DoubleClick += ni_DoubleClick;
            ni.ContextMenuStrip = cntx_menu;

            sp.SoundLocation = "home15.wav";
            sp.Load();

            waiting_layer.Visibility = System.Windows.Visibility.Visible;
            message_layer.Visibility = System.Windows.Visibility.Hidden;
            settings_layer.Visibility = System.Windows.Visibility.Collapsed;

            //Mn.Hide();
        }

        void ni_DoubleClick(object sender, EventArgs e)
        {
            if (time_s < 10)
                RemainingS.Content = "0" + Convert.ToString(time_s);
            else
                RemainingS.Content = Convert.ToString(time_s);
            if (time_m < 10)
                RemainingM.Content = "0" + Convert.ToString(time_m);
            else
                RemainingM.Content = Convert.ToString(time_m);

            Mn.Activate();
            Mn.Show();
            Mn.ShowInTaskbar = true;
            Mn.WindowState = WindowState.Normal;
        }

        private void ShowTimeS(object sender, EventArgs e)
        {
            if (time_s == 0)
            {
                if (time_m == 0)
                {
                    ShowMessage();
                }
                else
                {
                    time_m--;
                    time_s = 59;
                }
            }
            else
            {
                time_s--;
            }

            if(Mn.IsVisible == true)
            {
                if (time_s < 10)
                    RemainingS.Content = "0" + Convert.ToString(time_s);
                else
                    RemainingS.Content = Convert.ToString(time_s);
                if (time_m < 10)
                    RemainingM.Content = "0" + Convert.ToString(time_m);
                else
                    RemainingM.Content = Convert.ToString(time_m);
            }
        }

        private void ShowMessage()
        {
            timeS.Stop();
            if (sound_check.IsChecked == true)
                sp.Play();
            Mn.Show();
            System.Diagnostics.Process.Start("http://blimb.su/");
            System.Threading.Thread.Sleep(35);
            Mn.Activate();
            waiting_layer.Visibility = System.Windows.Visibility.Hidden;
            message_layer.Visibility = System.Windows.Visibility.Visible;
        }

        private void Finish(object sender, RoutedEventArgs e)
        {
            waiting_layer.Visibility = System.Windows.Visibility.Visible;
            message_layer.Visibility = System.Windows.Visibility.Hidden;
            Mn.Visibility = Visibility.Hidden;
            RemainingS.Content = "00";
            time_m = min_main;
            time_s = 0;
            if (min_main < 10)
                RemainingM.Content = "0" + min_main;
            else
                RemainingM.Content = min_main;
            timeS.Start();
        }

        private void Delay(object sender, RoutedEventArgs e)
        {
            waiting_layer.Visibility = System.Windows.Visibility.Visible;
            message_layer.Visibility = System.Windows.Visibility.Hidden;
            Mn.Visibility = Visibility.Hidden;
            RemainingS.Content = "00";
            if (min_delay < 10)
                RemainingM.Content = "0" + min_delay;
            else
                RemainingM.Content = min_delay;
            timeS.Start();
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            Mn.Hide();
            Mn.ShowInTaskbar = false;
        }

        public void Close_Click(object sender, EventArgs e)
        {
            timeS.Stop();
            Environment.Exit(0);
        }

        private void Settings_button_Click(object sender, EventArgs e)
        {
            timeS.Stop();

            Mn.Show();

            Settings_button.Visibility = System.Windows.Visibility.Hidden;

            switch (waiting_layer.IsVisible == true)
            {
                case true:
                    k = 1;
                    break;
                case false:
                    k = 2;
                    break;
            }

            waiting_layer.Visibility = System.Windows.Visibility.Hidden;
            message_layer.Visibility = System.Windows.Visibility.Hidden;
            settings_layer.Visibility = System.Windows.Visibility.Visible;

        }

        private void button_back_settings_Click(object sender, RoutedEventArgs e)
        {
            Settings_button.Visibility = System.Windows.Visibility.Visible;
            settings_layer.Visibility = System.Windows.Visibility.Hidden;
            switch (k)
            {
                case 1:
                    waiting_layer.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 2:
                    message_layer.Visibility = System.Windows.Visibility.Visible;
                    break;
            }


            if (min_main < 10)
                RemainingM.Content = "0" + min_main;
            else
                RemainingM.Content = min_main;
            RemainingS.Content = "00";

            timeS.Start();

            string set = Convert.ToString(min_main) + "\n" + Convert.ToString(min_delay) + "\n" + Convert.ToString(sound_check.IsChecked);
            System.IO.File.WriteAllText("settings.ini", set);
        }

        private void slider_main_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            time_m = min_main = Convert.ToInt32(slider_main.Value);
            time_s = 0;
            RemainingS.Content = "00";

        }

        private void slider_delay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            min_delay = Convert.ToInt32(slider_delay.Value);
        }
    }
}
