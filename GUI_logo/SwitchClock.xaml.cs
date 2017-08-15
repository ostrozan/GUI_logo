using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GUI_logo
{
    [Serializable]
    /// <summary>
    /// Interakční logika pro SwitchClock.xaml
    /// </summary>
    public partial class SwitchClock : UserControl
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public string Name { get; set; }
        private int Hod { get; set; }
        private int Min { get; set; }
        private int Sec { get; set; }
        public ObservableCollection<StackPanel> TimeSpans { get; set; } = new ObservableCollection<StackPanel>();
        public Button BtAdd { get => btAdd; set => btAdd = value; }
        public Button BtRemove { get => btRemove; set => btRemove = value; }


        public StackPanel stpTimeSpan;
        private StackPanel stpAddRem;
        private Button btAdd;
        private Button btRemove;
        private int numberOfTimeSpan;
        public int ThisHeight{ get; set; }
        Binding bnd = new Binding();
    public SwitchClock()
        {
            InitializeComponent();
         
            //AddTimeSpan(0,0,0);
            bnd.Source = TimeSpans;
          
        }

        private int prewMinutesValue;
        public void AddTimeSpan(int idx,int start, int stop)
        {
            
            stpTimeSpan = new StackPanel() { Orientation = Orientation.Horizontal };         
            stpTimeSpan.Children.Add(new TimePicker(start));
            stpTimeSpan.Children.Add(new TimePicker(stop));
            stpAddRem = new StackPanel() { Margin = new Thickness(0, 3, 0, 2) };
            btAdd = new Button() {  FontSize = 10, Content = "+", Width = 20, Height = 12, Padding = new Thickness(0, -3, 0, 0), Margin = new Thickness(2, 0, 0, 1) };
            btRemove = new Button() { FontSize = 10, Content = "-", Width = 20, Height = 12, Padding = new Thickness(0, -3, 0, 0), Margin = new Thickness(2, 0, 0, 1) };
            stpAddRem.Children.Add(btRemove);
            stpTimeSpan.Children.Add(stpAddRem);
            stpAddRem.Children.Add(btAdd);          
            stpMain.Children.Insert(idx,stpTimeSpan);
            TimeSpans.Insert(idx, stpTimeSpan);
            SortMainStack();
        }

        public void RemoveTimeSpan(int idx)
        {
            TimeSpans.Remove(TimeSpans[idx]);
            stpMain.Children.RemoveAt(idx);
            SortMainStack();
        }

        private void SortMainStack( )
        {
            string uid;
            int i = 0;

            foreach (StackPanel sp1 in stpMain.Children)
            {
                StackPanel sp2 = (StackPanel)sp1.Children[2];
                uid = i++.ToString();
                sp2.Children[0].Uid = uid;
                sp2.Children[1].Uid = uid;
            }
        }
        #region my event

        public static readonly RoutedEvent UpdateWindov = EventManager.RegisterRoutedEvent("UpdateWin", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SwitchClock));

        public event RoutedEventHandler UpdateWin
        {
            add { AddHandler(UpdateWindov, value); }
            remove { RemoveHandler(UpdateWindov, value); }
        }
        void RaiseUpdateWin()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SwitchClock.UpdateWindov);
            RaiseEvent(newEventArgs);
        }


        #endregion

        private void stpMain_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)e.Source;
            StackPanel stp = (StackPanel)sender;
            int idx = int.Parse(btn.Uid) + 1;
            if(btn.Content=="+")
            {
            AddTimeSpan(idx,0,0);
            ThisHeight = 30;
            }
            else if (btn.Content == "-")
            {
                RemoveTimeSpan(idx-1);
                ThisHeight = -30;
               
            }
             RaiseUpdateWin();

        }
    }   
}
