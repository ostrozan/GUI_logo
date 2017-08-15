using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI_logo
{
    [Serializable]
    public class MyTimeSpan
    {
        public StackPanel stpTimeSpan = new StackPanel() { Orientation = Orientation.Horizontal};
        private StackPanel stpAddRem = new StackPanel() { Margin = new Thickness(0, 3, 0, 2) };
        public Button btAdd = new Button() { FontSize = 10, Content = "-", Width = 20, Height = 12, Padding = new Thickness(0, -3, 0, 0), Margin = new Thickness(2, 0, 0, 1) };
        public Button btRemove = new Button() { FontSize = 10, Content = "+", Width = 20, Height = 12, Padding = new Thickness(0, -3, 0, 0), Margin = new Thickness(2, 0, 0, 1) };
        public MyTimeSpan()
        {

            stpTimeSpan.Children.Add(new TimePicker());
            stpTimeSpan.Children.Add(new TimePicker());
            stpAddRem.Children.Add(btRemove);
            stpAddRem.Children.Add(btAdd);
            stpTimeSpan.Children.Add(stpAddRem);
        }
    }
}
