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

namespace GUI_logo
{
    [Serializable()]
    /// <summary>
    /// Interakční logika pro Thermostat.xaml
    /// </summary>
    public partial class Thermostat : UserControl
    {
        public float alarmLo { get; set; }
        public float alarmHi { get; set; }
        public float temperature { get; set; }
        public float hystreresis { get; set; }
        public bool IsSensor1 { get; set; }
        public bool IsSensor2 { get; set; }
        public bool IsAlarmHi { get; set; }
        public bool IsAlarmLo { get; set; }
        public char KtereCidlo { get; set; }
        public Thermostat()
        {
            InitializeComponent();
        }

        private void UserControl_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ",") return;
            if (!char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void rbtCidlo1_Checked(object sender, RoutedEventArgs e)
        {
            IsSensor1 = true;
            IsSensor2 = false;
            KtereCidlo = '1';
        }

        private void rbtCidlo2_Checked(object sender, RoutedEventArgs e)
        {
            IsSensor1 = false;
            IsSensor2 = true;
            KtereCidlo = '2';
        }

        //private void tbxAlarmHi_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if(tbxAlarmHi.Text!="")
        //    {
        //    float f;
        //    if(!float.TryParse(tbxAlarmHi.Text, out f))MessageBox.Show("zadejte správný formát čísla");
        //        alarmHi = f;
        //    }
        //}

        private void tbxTemp_TextChanged(object sender, TextChangedEventArgs e)
        {
            float f;
            if (!float.TryParse(tbxTemp.Text, out f))MessageBox.Show("zadejte správný formát čísla");
            temperature = f;
        }

        //private void tbxAlarmLo_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    float f;
        //    if (!float.TryParse(tbxAlarmLo.Text,out f))MessageBox.Show("zadejte správný formát čísla");
        //    alarmLo = f;
        //}

        private void tbxOffset_TextChanged(object sender, TextChangedEventArgs e)
        {
            float f;
            if (!float.TryParse(tbxOffset.Text, out f))MessageBox.Show("zadejte správný formát čísla");
            hystreresis = f;
        }

        //private void chbAlarmHi_Checked(object sender, RoutedEventArgs e)
        //{
        //    IsAlarmHi = true;
        //    tbxAlarmHi.IsEnabled = true;
        //}

        //private void chbAlarmHi_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    IsAlarmHi = false;
        //    tbxAlarmHi.IsEnabled = false;
        //}

        //private void chbAlarmLo_Checked(object sender, RoutedEventArgs e)
        //{
        //    IsAlarmLo = true;
        //    tbxAlarmLo.IsEnabled = true;
        //}

        //private void chbAlarmLo_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    IsAlarmLo = false;
        //    tbxAlarmLo.IsEnabled = false;
        //}
    }
}
