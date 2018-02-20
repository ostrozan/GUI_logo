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
            //gsmData.out1onCmd = tbxCmd1On.Text;
            //gsmData.out1offCmd = tbxCmd1Off.Text;
            //gsmData.out2onCmd = tbxCmd2On.Text;
            //gsmData.out2offCmd = tbxCmd2Off.Text;
            //gsmData.out3onCmd = tbxCmd3On.Text;
            //gsmData.out3offCmd = tbxCmd3Off.Text;
            //gsmData.out4onCmd = tbxCmd4On.Text;
            //gsmData.out4offCmd = tbxCmd4Off.Text;
            //gsmData.out5onCmd = tbxCmd5On.Text;
            //gsmData.out5offCmd = tbxCmd5Off.Text;
            //gsmData.out6onCmd = tbxCmd6On.Text;
            //gsmData.out6offCmd = tbxCmd6Off.Text;
            //gsmData.statusCmd = tbxCmdStatus.Text;
            gsmData.isEnabled = (bool)chbGsmEnable.IsChecked;
             gsmData.isResponse = (bool)chbGsmConfirmEn.IsChecked;
            gsmData.telNumbers[0] = tbxTel1.Text;
            gsmData.telNumbers[1] = tbxTel2.Text;
            gsmData.telNumbers[2] = tbxTel3.Text;
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
            //tbxCmd1On.Text =gsmData.out1onCmd;
            //tbxCmd1Off.Text = gsmData.out1offCmd;
            //tbxCmd2On.Text = gsmData.out2onCmd;
            //tbxCmd2Off.Text = gsmData.out2offCmd;
            //tbxCmd3On.Text = gsmData.out3onCmd;
            //tbxCmd3Off.Text = gsmData.out3offCmd;
            //tbxCmd4On.Text = gsmData.out4onCmd;
            //tbxCmd4Off.Text = gsmData.out4offCmd;
            //tbxCmd5On.Text = gsmData.out5onCmd;
            //tbxCmd5Off.Text = gsmData.out5offCmd;
            //tbxCmd6On.Text = gsmData.out6onCmd;
            //tbxCmd6Off.Text = gsmData.out6offCmd;
            //tbxCmdStatus.Text = gsmData.statusCmd;
            chbGsmEnable.IsChecked = gsmData.isEnabled;
            chbGsmConfirmEn.IsChecked = gsmData.isResponse;
            tbxTel1.Text = gsmData.telNumbers[0];
            tbxTel2.Text = gsmData.telNumbers[1];
            tbxTel3.Text = gsmData.telNumbers[2];
        }

        private void chbGsmConfirmEn_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
