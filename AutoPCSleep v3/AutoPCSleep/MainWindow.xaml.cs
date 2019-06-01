using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Diagnostics;


namespace AutoPCSleep
{
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        int seconds_passed = 0;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            
            int seconds_left = Convert.ToInt32(TimeCalculate()) - seconds_passed++;
            Clock.Content = ConvertSecondstoClock(seconds_left);
            if (seconds_left<610 && seconds_left>590)
            {
                WindowsManager.KillWinByT("Ваш сеанс будет завершен");
                cmd.Content += "working \n";
                
            }
            
        }

        public string ConvertSecondstoClock(int sec)
        {
            int h = sec/3600;
            int m = (sec - h * 3600) / 60;
            int s = sec - h * 3600 - m * 60;           

            string clock=h.ToString("00")+":"+ m.ToString("00") + ":" + s.ToString("00");
          


            return clock;
        }


        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;

        }

        private void Button_Click_ShutDown(object sender, RoutedEventArgs e)
        {
          
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            string cmd_command = "";
            cmd_command = "/c shutdown -s -f -t " + TimeCalculate();

            if (Convert.ToInt32(TimeCalculate()) > 60)
            {
                Process.Start("cmd", cmd_command);
            }
            else
            {
                MessageBox.Show("Too little time");
            }

        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd", "/c shutdown -a");
            dispatcherTimer.Stop();
            seconds_passed = 0;
            Clock.Content = ConvertSecondstoClock(0);
        }

        private string TimeCalculate()
        {   
            int minutes = 10000;
            string hours = hh.Text;
            string min = mm.Text;
            minutes = Convert.ToInt32(hours) * 3600 + Convert.ToInt32(min) * 60;

            string c = minutes.ToString();
            return (c);
        }

    }

}
