using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_logo
{
    [Serializable()]
    public class In
    {
        public int funcIndex { get; set; }

        public ObservableCollection<bool?> outs
        {
            get;
            set;
        } = new ObservableCollection<bool?>();// { false, false, false, false, false, false };

        public ObservableCollection<string> Tel
        {
            get;
            set;
        } = new ObservableCollection<string>();// { "000000000" };
        public ObservableCollection<string> Sms
        {
            get;
            set;
        } = new ObservableCollection<string>();// { "" };
        public ObservableCollection<string> nazev
        {
            get;
            set;
        } = new ObservableCollection<string>();// { "" };

        public In()
        {
           
        }
    }
}
