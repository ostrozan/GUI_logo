using System;
using System.Windows;
using System.Windows.Controls;

namespace GUI_logo
{
    [Serializable]
    /// <summary>
    /// Interakční logika pro WindowCasovac.xaml
    /// </summary>
    public partial class WindowCasovac : Window
    {

        public WindowCasovac()
        {
            InitializeComponent();
        }

        private void rbTrvale_Checked(object sender, RoutedEventArgs e)
        {
            timerCtrl2.IsEnabled = false;
        }

        private void rbPuls_Checked(object sender, RoutedEventArgs e)
        {
            timerCtrl2.IsEnabled = true;
        }

        private void stpPrepinac_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)e.Source;
            grbStav.IsEnabled = int.Parse(rb.Uid) > 2 ? false : true;         
        }
    }
}
