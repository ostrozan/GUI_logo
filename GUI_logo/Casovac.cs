using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_logo
{
    [Serializable()]
    class Casovac : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public string Name { get; set; }
        public TimePicker timeControl { get; set; }

        public Casovac()
        {

        }


    }
}
