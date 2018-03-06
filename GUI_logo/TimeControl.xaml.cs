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
    [Serializable]
    /// <summary>
    /// Interakční logika pro TimeControl.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public int Hod { get; set; }
        public int Min { get; set; }
        public int Sec { get; set; }
        bool focHod, focMin, focSec;
        public TimePicker()
        {
            InitializeComponent();
        }

        public TimePicker( int minutes)
        {
            InitializeComponent();
            Hod = minutes / 60;
            Min = minutes % 60;
            tbHod.Text = Hod.ToString().PadLeft(2, '0');
            tbMin.Text = Min.ToString().PadLeft(2, '0');

        }

        private void tbHod_GotFocus(object sender, RoutedEventArgs e)
        {
            focHod = true;
            focMin = false;
            focSec = false;
            //tbHod.Background = Brushes.Aqua;
            //tbMin.Background = Brushes.White;
            //tbSec.Background = Brushes.White;
        }
        private void tbMin_GotFocus(object sender, RoutedEventArgs e)
        {
            focHod = false;
            focMin = true;
            focSec = false;
            //tbHod.Background = Brushes.White;
            //tbMin.Background = Brushes.Aqua;
            //tbSec.Background = Brushes.White;
        }

        private void tbSec_GotFocus(object sender, RoutedEventArgs e)
        {
            focHod = false;
            focMin = false;
            focSec = true;
            //tbHod.Background = Brushes.White;
            //tbMin.Background = Brushes.White;
            //tbSec.Background = Brushes.Aqua;
        }

        private void tbHod_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up) UpCount();
            if (e.Key == Key.Down) DnCount();
        }

        private void tbMin_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up) UpCount();
            if (e.Key == Key.Down) DnCount();
        }

        private void tbSec_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up) UpCount();
            if (e.Key == Key.Down) DnCount();
        }

        private void DnCount()
        {
            if (focHod)
            {
                if (--Hod < 0) Hod = 23;
                tbHod.Text = Hod.ToString().PadLeft(2, '0');
            }
            if (focMin)
            {
                if (--Min < 0) Min = 59;
                tbMin.Text = Min.ToString().PadLeft(2, '0');
            }
            if (focSec)
            {
                if (--Sec < 0) Sec = 59;
                tbSec.Text = Sec.ToString().PadLeft(2, '0');
            }
        }      

        private void UpCount()
        {
            if (focHod)
            {
                if (++Hod > 23) Hod = 0;
                tbHod.Text = Hod.ToString().PadLeft(2, '0');
            }
            if (focMin)
            {
                if (++Min > 50) Min = 0;
                tbMin.Text = Min.ToString().PadLeft(2, '0');
            }
            if (focSec)
            {
                if (++Sec > 59) Sec = 0;
                tbSec.Text = Sec.ToString().PadLeft(2, '0');
            }
        }

        private void tbHod_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (int.TryParse(tbHod.Text,out int x)&&x > 23)
            {
                tbHod.Text = "23";
                MessageBox.Show("den má jen 24 hodin");
            }
            else
            {
                Hod = x;
                tbHod.Text.PadLeft(2, '0');
            }
        }

        private void tbMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(tbMin.Text, out int x) && x > 59)
            {
                tbMin.Text = "59";
                MessageBox.Show("hodina má jen 60 minut");
            }
            else
            {
                Min = x;
                tbMin.Text.PadLeft(2, '0');
            }
        }

        private void tbSec_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(tbSec.Text, out int x) && x > 59)
            {
                tbSec.Text = "59";
                MessageBox.Show("minuta má jen 60 sekund");
            }
            else
            {
                Sec = x;
                tbSec.Text.PadLeft(2, '0');
            }

        }

        private void UserControl_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void btUp_Click(object sender, RoutedEventArgs e)
        {
            UpCount();
        }

        private void btDn_Click(object sender, RoutedEventArgs e)
        {
            DnCount();
        }


    }
}
