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

namespace GUI_logo
{
    [Serializable]
    /// <summary>
    /// Interakční logika pro WindowSetOut.xaml
    /// </summary>
    public partial class WindowSetOut : Window
    {
        private Gpio vystup;
        private Gpio gpioTmp;
        private SwitchClock switchClock;
        private ProgTimer progTimer;


        public WindowSetOut(Gpio vystup)
        {
            InitializeComponent();
            this.vystup = vystup;
            DataContext = this;

        }



        private void rbSpinacky_Checked(object sender, RoutedEventArgs e)
        {
            showSwitchClock();
        }

        /// <summary>
        /// 
        /// </summary>
        private void showSwitchClock()
        {
            switchClock = vystup.SwitchingClock;
            if (vystup.GpOut.minuteSpan.Count == 0) switchClock.AddTimeSpan(0, 0, 0);
            else
            {

                //switchClock.stpMain.Children.RemoveRange(0, vystup.GpOut.minuteSpan.Count);
                if(switchClock.TimeSpans.Count>0) for (int i = vystup.GpOut.minuteSpan.Count-1; i >=0 ; i--)
                {
                    switchClock.RemoveTimeSpan(i);
                }

                for (int i = 0; i < vystup.GpOut.minuteSpan.Count; i++)
                {
                    MinuteSpan ms = vystup.GpOut.minuteSpan[i];
                    switchClock.AddTimeSpan(i, ms.startTime, ms.stopTime);
                }
            }
          
            switchClock.UpdateWin += SwitchClock_UpdateWin;
            switchClock.Margin = new Thickness(20);
            contCtrl.Content = switchClock;
            this.UpdateLayout();
            this.Height = mainGrid.ActualHeight + 80;
        }

        private void SwitchClock_UpdateWin(object sender, RoutedEventArgs e)
        {
            SwitchClock sc = (SwitchClock)e.Source;
            this.UpdateLayout();
            this.Height = this.ActualHeight + sc.ThisHeight;
        }

        private void Bt_Click(object sender, RoutedEventArgs e)
        {


        }

        private void rbTimer_Checked(object sender, RoutedEventArgs e)
        {
            showProgTimer();
        }

        /// <summary>
        /// 
        /// </summary>
        private void showProgTimer()
        {
            int a, h, m,s;
            a = vystup.GpOut.controlTimes.timeOfDelay;
            h = a / 3600;
            m = (a / 60) - (h * 60);
            s = (a % 3600)-(m*60);
            progTimer = vystup.ProgTim;
            progTimer.timerCtrl1.Hod = h;
            progTimer.timerCtrl1.Min = m;
            progTimer.timerCtrl1.Sec = s;
            contCtrl.Content = progTimer;
            this.UpdateLayout();
            this.Height = mainGrid.ActualHeight + 80;
        }

        private void chbCasem_Checked(object sender, RoutedEventArgs e)
        {
            grbCasovac.IsEnabled = true;
            contCtrl.IsEnabled = true;
        }

        private void chbCasem_Unchecked(object sender, RoutedEventArgs e)
        {
            grbCasovac.IsEnabled = false;
            contCtrl.IsEnabled = false;
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.projectSaved = false;
            vystup.GpOut.IsTimeControl = (bool)chbCasem.IsChecked;
            vystup.GpOut.IsInputControl = (bool)chbVstupy.IsChecked;
            vystup.GpOut.IsExtControl = (bool)chbExtern.IsChecked;
            vystup.GpOut.IsUseSwitchClk = (bool)rbSpinacky.IsChecked;
            vystup.GpOut.IsUseProgTmr = (bool)rbTimer.IsChecked;

            MinuteSpan ms = new MinuteSpan();
            if (switchClock != null)
            {
                vystup.GpOut.minuteSpan.Clear();
                foreach (StackPanel stp in switchClock.TimeSpans)
                {
                    TimePicker tp1 = (TimePicker)stp.Children[0];
                    TimePicker tp2 = (TimePicker)stp.Children[1];

                    ms.startTime = (tp1.Hod * 60) + tp1.Min;
                    ms.stopTime = (tp2.Hod * 60) + tp2.Min;
                    vystup.GpOut.minuteSpan.Add(ms);

                }

            }



            //if (switchClock != null) MainWindow.gpioData.outputs[i].minuteSpan = vystup.GpOut.minuteSpan;
            if (progTimer != null)
            {
                ControlTimes ct = new ControlTimes();
                ct.timeOfDelay = (progTimer.timerCtrl1.Hod * 3600) + (progTimer.timerCtrl1.Min*60)+ progTimer.timerCtrl1.Sec;
                ct.timeOfPulse = (progTimer.timerCtrl2.Hod * 3600) + (progTimer.timerCtrl2.Min * 60) + progTimer.timerCtrl2.Sec;
                vystup.GpOut.controlTimes = ct;
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            vystup = gpioTmp;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            chbCasem.IsChecked = vystup.GpOut.IsTimeControl;
            chbVstupy.IsChecked = vystup.GpOut.IsInputControl;
            chbExtern.IsChecked = vystup.GpOut.IsExtControl;
            rbSpinacky.IsChecked = vystup.GpOut.IsUseSwitchClk;
            //if (vystup.GpOut.IsUseSwitchClk == true)
            //    showSwitchClock();
            rbTimer.IsChecked = vystup.GpOut.IsUseProgTmr;
            if (vystup.GpOut.IsUseProgTmr == true)
                showProgTimer();
            grbCasovac.IsEnabled = (bool)chbCasem.IsChecked ? true : false;
            this.Height = mainGrid.ActualHeight + 80;
        }
    }
}
