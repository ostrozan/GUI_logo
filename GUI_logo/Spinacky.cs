using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GUI_logo
{
    [Serializable]
    class Spinacky :INotifyPropertyChanged
    {
         
        
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public string Name { get; set; }
        private int Hod { get; set; }
        private int Min { get; set; }
        private int Sec { get; set; }
        public ObservableCollection<StackPanel> TimeSpans { get; set; }
        public Button BtAdd { get => btAdd; set => btAdd = value; }
        public Button BtRemove { get => btRemove; set => btRemove = value; }

        private StackPanel stpMain;
        private StackPanel stpTimeSpan;
        private Button btAdd = new Button() { Content = "+" };
        private Button btRemove = new Button() { Content = "+" };
        public GroupBox grBox = new GroupBox() { Header = "Spínací hodiny" };
        public Spinacky()
        {

            stpMain = new StackPanel() { Orientation = Orientation.Vertical };
            stpTimeSpan.Children.Add(new TimePicker(0));
            stpTimeSpan.Children.Add(new TimePicker(1));
            stpMain.Children.Add(stpTimeSpan);
            TimeSpans.Add(stpTimeSpan);
            grBox.Content = stpMain;
            btAdd.Click += BtAdd_Click;
            btRemove.Click += BtRemove_Click;
        }

        private void BtRemove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RemoveTimeSpan();
        }

        private void BtAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddTimeSpan();
        }

        private void AddTimeSpan()
        {
            StackPanel stpAdRem = new StackPanel() { Orientation = Orientation.Vertical };
            stpAdRem.Children.Add(btRemove);
            stpAdRem.Children.Add(btAdd);

            StackPanel stp1 = TimeSpans.Last();
            TimePicker tc = (TimePicker)stp1.Children[1];
            int minutes = (tc.Hod * 60) + tc.Min;
            stpTimeSpan.Children.Add(new TimePicker(minutes + 1));
            stpTimeSpan.Children.Add(new TimePicker(minutes + 2));
            stpTimeSpan.Children.Add(stpAdRem);
            stpMain.Children.Add(stpTimeSpan);
        }

         private void RemoveTimeSpan()
        {
            TimeSpans.Remove(TimeSpans.Last());
        }
     
    }

    }
