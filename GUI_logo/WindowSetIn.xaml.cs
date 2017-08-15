using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interakční logika pro WindowSetIn.xaml
    /// </summary>
    public partial class WindowSetIn : Window
    {
        private Gpio vstup;
        private Gpio tmp_vstup;
        string[] funcs = new string[] { "zapni výstup ", "vypni výstup ", "blokuj výstup ","spusť časování ", "pošli sms ","prozvoň "};
        string pripojStr;
        private bool?[] out_check = new bool?[] {false, false, false, false, false, false};
        private int funcIndex;
        private string sms;
        private string telNmb;
        public WindowSetIn (Gpio vstup)
        {
            InitializeComponent();
            this.vstup = vstup;
            this.Title+=":  "+vstup.GpIn.nazev;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (cmbFunc.SelectedIndex < 3) stpOuts.Visibility = Visibility.Visible;
            else stpOuts.Visibility = Visibility.Collapsed;
            if (cmbFunc.SelectedIndex > 3) stpTelNmb.Visibility = Visibility.Visible;
            else stpTelNmb.Visibility = Visibility.Collapsed;
            if (cmbFunc.SelectedIndex == 4) stpSms.Visibility = Visibility.Visible;
            else stpSms.Visibility = Visibility.Collapsed;

            int x = 0;
            if (vstup.GpIn.outs.Count == 0) vstup.GpIn.outs = new ObservableCollection<bool?> { false, false, false, false, false, false };

            foreach (CheckBox chb in stpOuts.Children)
            {
               /* if(vstup.GpIn.outs.Count!=0)*/chb.IsChecked = vstup.GpIn.outs[x];      
                out_check[x++] = chb.IsChecked;
            }
            this.funcIndex = cmbFunc.SelectedIndex = vstup.GpIn.funcIndex;
            if (vstup.GpIn.Sms.Count == 0) vstup.GpIn.Sms.Add("");
            sms = tbTSmsText.Text = vstup.GpIn.Sms[0];

            if (vstup.GpIn.Tel.Count == 0) vstup.GpIn.Tel.Add("000000000");
            telNmb = tbTelNmb.Text = vstup.GpIn.Tel[0];

        }

        private void stpOuts_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chb = e.Source as CheckBox;
            out_check[int.Parse(chb.Uid)] = chb.IsChecked;

        }

        private void stpOuts_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chb = e.Source as CheckBox;
            out_check[int.Parse(chb.Uid)] = chb.IsChecked;
        }


        private void cmbFunc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFunc.SelectedIndex < 4) stpOuts.Visibility = Visibility.Visible;
            else stpOuts.Visibility = Visibility.Collapsed;
            if (cmbFunc.SelectedIndex >= 4) stpTelNmb.Visibility = Visibility.Visible;
            else stpTelNmb.Visibility = Visibility.Collapsed;
            if (cmbFunc.SelectedIndex == 4) stpSms.Visibility = Visibility.Visible;
            else stpSms.Visibility = Visibility.Collapsed;
            funcIndex = cmbFunc.SelectedIndex;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.projectSaved = false;
            int x = 0;
            foreach (Gpio gp in MainWindow.outputs)gp.GpOut.IsInputControl = out_check[x++];
            x = 0;
            for(int i=0;i<6;i++)vstup.GpIn.outs[i] = out_check[i];
            vstup.GpIn.funcIndex =this.funcIndex  ;
            vstup.GpIn.Tel[0] = telNmb;
            vstup.GpIn.Sms[0] = sms;
            //vstup.nazev[0] = tBoxDesc.Text;
            //if (cmbFunc.SelectedIndex > 3) pripojStr = "na číslo " + tbTelNmb.Text;
            //vstup.func[0] = funcs[cmbFunc.SelectedIndex]+pripojStr;
            this.Close();
        }

        private void tbTelNmb_TextChanged(object sender, TextChangedEventArgs e)
        {
            telNmb = tbTelNmb.Text;
        }

        private void tbTSmsText_TextChanged(object sender, TextChangedEventArgs e)
        {
            sms = tbTSmsText.Text;
        }

    }
}
