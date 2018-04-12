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
        private Thermostat thermostat;

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

        /// <summary>
        /// 
        /// </summary>
        private void showThermostat()
        {
            thermostat = vystup.Therm;

            contCtrl.Content = thermostat;
            this.UpdateLayout();
            this.Height = mainGrid.ActualHeight + 80;
            thermostat.tbxTemp.IsEnabled = true;
            thermostat.tbxOffset.IsEnabled = true;
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
            rbSpinacky.IsChecked = false;
            rbTimer.IsChecked = false;
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.projectSaved = false;
            vystup.GpOut.IsTimeControl = (bool)rbCasem.IsChecked;
            vystup.GpOut.IsInputControl = (bool)rbVstupy.IsChecked;
            vystup.GpOut.IsExtControl = (bool)rbExtern.IsChecked;
            vystup.GpOut.IsUseSwitchClk = (bool)rbSpinacky.IsChecked;
            vystup.GpOut.IsUseProgTmr = (bool)rbTimer.IsChecked;
            vystup.GpOut.IsUseThermostat = (bool)rbTemp.IsChecked;

            vystup.stPanel.Children.RemoveRange(0, vystup.stPanel.Children.Count);

            if ((bool)vystup.GpOut.IsUseSwitchClk)
            {
                vystup.img.Source = vystup.watchImg;
                vystup.stPanel.Children.Add(vystup.img);
                vystup.stPanel.Children.Add(vystup.tblCounter);
            }
            else if ((bool)vystup.GpOut.IsUseProgTmr)
            {
                vystup.img.Source = vystup.stopwatchImg;
                vystup.stPanel.Children.Add(vystup.img);
                vystup.stPanel.Children.Add(vystup.tblCounter);
            }
            else if ((bool)vystup.GpOut.IsUseThermostat)
            {
                vystup.img.Source = vystup.tempMeterImg;
                vystup.stPanel.Children.Add(vystup.img);
            }
            else if ((bool)vystup.GpOut.IsInputControl)
            {              
                vystup.img.Source = vystup.inputImg;
                vystup.stPanel.Children.Add(vystup.img);
            }
            else if ((bool)vystup.GpOut.IsExtControl)
            {
                vystup.img.Source = vystup.extImg;
                vystup.stPanel.Children.Add(vystup.img);
            }

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
                //ct.timeOfPulse = (progTimer.timerCtrl2.Hod * 3600) + (progTimer.timerCtrl2.Min * 60) + progTimer.timerCtrl2.Sec;
                vystup.GpOut.controlTimes = ct;
                //vystup.GpOut.IsTrvale = progTimer.IsTrvale;
                //vystup.GpOut.IsNastCas = progTimer.IsNastCas;
                //vystup.GpOut.IsSwitchOn = progTimer.IsSwitchOn;
                //vystup.GpOut.IsSwitchOff = progTimer.IsSwitchOff;
                //vystup.GpOut.IsAnyChange = progTimer.IsAnyChange;
            }

            if(thermostat != null)
            {
                vystup.GpOut.temperature = thermostat.temperature;
                vystup.GpOut.hystreresis = thermostat.hystreresis;
                vystup.GpOut.alarmHi = thermostat.alarmHi;
                vystup.GpOut.alarmLo = thermostat.alarmLo;
                vystup.GpOut.KtereCidlo = thermostat.KtereCidlo;
                vystup.GpOut.IsAlarmHi = thermostat.IsAlarmHi;
                vystup.GpOut.IsAlarmLo = thermostat.IsAlarmLo;
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

            rbCasem.IsChecked = vystup.GpOut.IsTimeControl;
            rbVstupy.IsChecked = vystup.GpOut.IsInputControl;
            rbExtern.IsChecked = vystup.GpOut.IsExtControl;
            rbTemp.IsChecked = vystup.GpOut.IsUseThermostat;
            rbSpinacky.IsChecked = vystup.GpOut.IsUseSwitchClk;
            //if (vystup.GpOut.IsUseSwitchClk == true)
            //    showSwitchClock();
            if((bool)rbTemp.IsChecked)
            {
                thermostat.tbxTemp.Text = vystup.GpOut.temperature.ToString();
                thermostat.tbxOffset.Text = vystup.GpOut.hystreresis.ToString();
                thermostat.rbtCidlo1.IsChecked = vystup.GpOut.KtereCidlo == '1' ? true : false;
                thermostat.rbtCidlo2.IsChecked = vystup.GpOut.KtereCidlo == '2' ? true : false;
            }
            rbTimer.IsChecked = vystup.GpOut.IsUseProgTmr;
            if (vystup.GpOut.IsUseProgTmr == true)
                showProgTimer();
            grbCasovac.IsEnabled = (bool)rbCasem.IsChecked ? true : false;
            this.Height = mainGrid.ActualHeight + 80;
        }

        private void rbTemp_Checked(object sender, RoutedEventArgs e)
        {
            contCtrl.Content = null;
            showThermostat();
        }

        private void rbVstupy_Checked(object sender, RoutedEventArgs e)
        {
            contCtrl.Content = null;
            this.UpdateLayout();
            this.Height = mainGrid.ActualHeight + 80;
        }

        private void rbExtern_Checked(object sender, RoutedEventArgs e)
        {
            //contCtrl.Content = null;
            //this.UpdateLayout();
            //this.Height = mainGrid.ActualHeight + 80;
        }
    }
}
