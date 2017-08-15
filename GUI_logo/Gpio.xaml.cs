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
using System.Xml.Serialization;

namespace GUI_logo
{
    [Serializable]
    /// <summary>
    /// Interakční logika pro Gpio.xaml
    /// </summary>
    /// 
    public partial class Gpio : UserControl
    {
   
        public string Popis { get; set; }
        public Out GpOut { get; set; } = new Out();
        public In GpIn { get; set; } = new In();
        public SwitchClock SwitchingClock { get; set; } = new SwitchClock();
        public ProgTimer ProgTim { get; set; } = new ProgTimer();
        public Gpio()
        {
            InitializeComponent();
            Popis = this.tbPopis.Text;
         
        }

    }
}
