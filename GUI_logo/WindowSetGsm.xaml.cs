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
    /// <summary>
    /// Interakční logika pro WindowSetGsm.xaml
    /// </summary>
    public partial class WindowSetGsm : Window
    {
        private GsmData gsmData;
        
        public WindowSetGsm(GsmData gsmData)
        {
            InitializeComponent();
            this.gsmData = gsmData;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {

            gsmData.isEnabled = (bool)chbGsmEnable.IsChecked;
             gsmData.isResponse = (bool)chbGsmConfirmEn.IsChecked;
            gsmData.telNumber = tbxTel.Text;
            //gsmData.telNumbers[1] = tbxTel2.Text;
            //gsmData.telNumbers[2] = tbxTel3.Text;
            foreach(RadioButton rb in stpOutselect.Children)
            {
                if ((bool)rb.IsChecked) gsmData.outNmb = rb.Uid;
            }
            Close();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void stpTboxes_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            if (tb.Text.Length > 8) e.Handled = true;
            //validace zadaneho znaku - pouze cislice
            if (!char.IsDigit(e.Text, 0)) e.Handled = true;
            //if
        }

        private void stpTboxes1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            if (tb.Text.Length > 2) e.Handled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            chbGsmEnable.IsChecked = gsmData.isEnabled;
            chbGsmConfirmEn.IsChecked = gsmData.isResponse;
            tbxTel.Text = gsmData.telNumber;
            //tbxTel2.Text = gsmData.telNumbers[1];
            //tbxTel3.Text = gsmData.telNumbers[2];
            foreach (RadioButton rb in stpOutselect.Children)
            {
                if (rb.Uid==gsmData.outNmb)rb.IsChecked=true;
            }
        }

        private void chbGsmConfirmEn_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
