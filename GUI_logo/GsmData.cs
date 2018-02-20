using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_logo
{
    public class GsmData
    {
        //public string out1onCmd { get; set; }
        //public string out1offCmd { get; set; }
        //public string out2onCmd { get; set; }
        //public string out2offCmd { get; set; }
        //public string out3onCmd { get; set; }
        //public string out3offCmd { get; set; }
        //public string out4onCmd { get; set; }
        //public string out4offCmd { get; set; }
        //public string out5onCmd { get; set; }
        //public string out5offCmd { get; set; }
        //public string out6onCmd { get; set; }
        //public string out6offCmd { get; set; }
        //public string statusCmd { get; set; }
        public bool isEnabled { get; set; }
        public bool isResponse { get; set; }
        public string telNumber { get; set; }// { "000000000", "000000000", "000000000" };
        public string outNmb;

        public GsmData()
        {

        }
    }
}
