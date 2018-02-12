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
    /// Interakční logika pro ProgTimer.xaml
    /// </summary>
    public partial class ProgTimer : UserControl
    {

        public bool IsTrvale { get; set; }
        public bool IsNastCas { get; set; }
        public bool IsSwitchOn { get; set; }
        public bool IsSwitchOff { get; set; }
        public bool IsAnyChange { get; set; }

        public ProgTimer()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbTrvale_Checked(object sender, RoutedEventArgs e)
        {
           IsTrvale = true;
           IsNastCas= /*timerCtrl2.IsEnabled =*/ false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbPuls_Checked(object sender, RoutedEventArgs e)
        {
            IsTrvale = false;
            IsNastCas = /*timerCtrl2.IsEnabled =*/ true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stpPrepinac_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)e.Source;
            int x = int.Parse(rb.Uid);
            //grbStav.IsEnabled = (x > 2) ? false : true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stpOuts_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chb = e.Source as CheckBox;
            //tmp_vstup.outs[chb.TabIndex] = true;
        }
    }
}
