using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_logo
{
    [Serializable()]
    public struct MinuteSpan
    {
        public int startTime;
        public int stopTime;
    }

    [Serializable()]
    public struct ControlTimes
    {
        public int timeOfDelay;
        public int timeOfPulse;
    }

    [Serializable()]
    public class Out
    {

        public bool? IsTimeControl { get; set; } = false;
        public bool ?IsInputControl { get; set; } = false;
        public bool ?IsExtControl { get; set; } = false;
        public bool ?IsUseSwitchClk { get; set; } = false;
        public bool ?IsUseProgTmr { get; set; } = false;
        public ObservableCollection<MinuteSpan> minuteSpan
        {
            get;
            set;
        } = new ObservableCollection<MinuteSpan>();

        public ControlTimes controlTimes
        {
            get;
            set;
        } = new ControlTimes();
        //public SwitchClock SwitchingClock { get; set; } = new SwitchClock();
        //public ProgTimer ProgTim { get; set; } = new ProgTimer();
        public Out()
        {

        }

    }
}
