using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    [Serializable()]
    public struct GpioData
    {
        public ObservableCollection<In> inputs;
        public ObservableCollection<Out> outputs;
    }
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> folderList;
        private ObservableCollection<string> projectList = new ObservableCollection<string>();
        public static string txBuffer;
        public static string rxBuffer;
        public static List<int> VSTUPY = new List<int>() { 0, 0, 0, 0 };
        public static List<int> VYSTUPY = new List<int>() { 0, 0, 0, 0, 0, 0 };
        public static List<In> ins = new List<In>() { new In(), new In(), new In(), new In() };
        public static List<Out> outs = new List<Out>() { new Out(), new Out(), new Out(), new Out(), new Out(), new Out() };
        public static ObservableCollection<Gpio> inputs;
        public static GpioData gpioData = new GpioData();
        public static ObservableCollection<Gpio> outputs;
        public List<int> test = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
        private string path;
        public static string pathProj;
        public static ObservableCollection<string> projPaths;
        private string currFile;
        Com com = new Com();
        bool handControlEnable, changeEnable;
        public static bool projectSaved;
        MyThumb myThumb2;
        bool isAddNew = false;
        public MainWindow()
        {
            InitializeComponent();

            Out1.tbPopis.Text = "rele 1";
            Out2.tbPopis.Text = "rele 2";
            Out3.tbPopis.Text = "out 3";
            Out4.tbPopis.Text = "out 4";
            Out5.tbPopis.Text = "out 5";
            Out6.tbPopis.Text = "out 6";
            In1.tbPopis.Text = "in 1";
            In2.tbPopis.Text = "in 2";
            In3.tbPopis.Text = "in 3";
            In4.tbPopis.Text = "in 4";
            Out1.Led.Fill = Brushes.PaleGoldenrod;
            Out2.Led.Fill = Brushes.PaleGoldenrod;
            Out3.Led.Fill = Brushes.PaleGoldenrod;
            Out4.Led.Fill = Brushes.PaleGoldenrod;
            Out5.Led.Fill = Brushes.PaleGoldenrod;
            Out6.Led.Fill = Brushes.PaleGoldenrod;
            In1.Led.Fill = Brushes.PaleGreen;
            In2.Led.Fill = Brushes.PaleGreen;
            In3.Led.Fill = Brushes.PaleGreen;
            In4.Led.Fill = Brushes.PaleGreen;
            inputs = new ObservableCollection<Gpio>() { In1, In2, In3, In4 };
            outputs = new ObservableCollection<Gpio>() { Out1, Out2, Out3, Out4, Out5, Out6 };


        }



        private void btnSerCom_Click(object sender, RoutedEventArgs e)
        {
            if (com.OpenPort())
            {
                if (btnSerCom.Content == "Připojit") btnSerCom.Content = "Odpojit";
                else btnSerCom.Content = "Připojit";
            }
        }

        private void cmbComPorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            com.serialPort.PortName = cmbComPorts.SelectedItem.ToString();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            projPaths = new ObservableCollection<string>();
            path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SmartRelay");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            cmbComPorts.ItemsSource = com.DejPorty;
            gpioData.inputs = new ObservableCollection<In>() { In1.GpIn, In2.GpIn, In3.GpIn, In4.GpIn };
            gpioData.outputs = new ObservableCollection<Out>() { Out1.GpOut, Out2.GpOut, Out3.GpOut, Out4.GpOut, Out5.GpOut, Out6.GpOut };
            GetProjNames();
        }

        private void stPanelIns_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


        private void Out1_Click(object sender, RoutedEventArgs e)
        {
            int a = 10;
        }

        private void Out_MouseEnter(object sender, MouseEventArgs e)
        {
            if (changeEnable || handControlEnable)
            {
                Gpio gpio = e.Source as Gpio;
                gpio.Background = Brushes.Green;
            }
        }

        private void Out_MouseLeave(object sender, MouseEventArgs e)
        {
            if (changeEnable || handControlEnable)
            {
                Gpio gpio = e.Source as Gpio;
                gpio.Background = null;
            }
        }


        private void GGpio_Dbl_click(object sender, MouseButtonEventArgs e)
        {


            if (changeEnable)
            {
                Gpio gpio = e.Source as Gpio;
                StackPanel stp = gpio.Parent as StackPanel;
                


                if (stp.Uid == "in")
                {
                    gpio.GpIn = gpioData.inputs[int.Parse(gpio.Uid)];
                    WindowSetIn setIn = new WindowSetIn(gpio);
                    setIn.ShowDialog();
                }

                else
                {
                    gpio.GpOut = gpioData.outputs[int.Parse(gpio.Uid)];
                    WindowSetOut setOut = new WindowSetOut(gpio);
                    setOut.ShowDialog();
                }

            }
            else if (handControlEnable)
            {
                Gpio gpio = e.Source as Gpio;
                StackPanel stp = gpio.Parent as StackPanel;
                if (stp.Uid == "in")
                {
                    if (gpio.Led.Fill == Brushes.Lime)
                    {
                        gpio.Led.Fill = Brushes.PaleGreen;
                        com.send("in" + int.Parse(gpio.Uid) + "ON");
                    }

                    else
                    {
                        gpio.Led.Fill = Brushes.Lime;
                        com.send("in" + int.Parse(gpio.Uid) + "OFF");
                    }
                }
                else if (stp.Uid == "out")
                {
                    if (gpio.Led.Fill == Brushes.Gold)
                    {
                        gpio.Led.Fill = Brushes.PaleGoldenrod;
                        com.send("out" + int.Parse(gpio.Uid) + "ON");
                    }

                    else
                    {
                        gpio.Led.Fill = Brushes.Gold;
                        com.send("out" + int.Parse(gpio.Uid) + "OFF");
                    }
                }
            }
        }

        private void rbAuto_Checked(object sender, RoutedEventArgs e)
        {
            handControlEnable = changeEnable = false;
        }

        private void rbHand_Checked(object sender, RoutedEventArgs e)
        {
            handControlEnable = true;
            changeEnable = false;
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isAddNew)
            {
                // Create new thumb object
                MyThumb newThumb = new MyThumb();
                // Assign our custom template to it
                ControlTemplate template = new ControlTemplate();
                var fec = new FrameworkElementFactory(typeof(Input));
                template.VisualTree = fec;
                newThumb.Template = template;

                // Calling ApplyTemplate enables us to navigate the visual tree right now (important!)
                newThumb.ApplyTemplate();
                // Add the "onDragDelta" event handler that is common to all objects
                newThumb.DragDelta += new DragDeltaEventHandler(onDragDelta);
                // Put newly created thumb on the canvas
                mainCanvas.Children.Add(newThumb);

                // Access the image element of our custom template and assign it to the new image
                //Image img = (Image)newThumb.Template.FindName("tplImage", newThumb);
                //img.Source = new BitmapImage(new Uri("Images/gear_connection.png", UriKind.Relative));

                // Access the textblock element of template and change it too
                //TextBlock txt = (TextBlock)newThumb.Template.FindName("tplTextBlock", newThumb);
                //txt.Text = "System action";

                // Set the position of the object according to the mouse pointer                
                Point position = e.GetPosition(this);
                Canvas.SetLeft(newThumb, position.X);
                Canvas.SetTop(newThumb, position.Y);
                // Move our thumb to the front to be over the lines
                Canvas.SetZIndex(newThumb, 1);
                // Manually update the layout of the thumb (important!)
                newThumb.UpdateLayout();

                // Create new path and put it on the canvas
                System.Windows.Shapes.Path newPath = new System.Windows.Shapes.Path();
                newPath.Stroke = Brushes.Black;
                newPath.StrokeThickness = 1;
                mainCanvas.Children.Add(newPath);

                // Create new line geometry element and assign the path to it
                LineGeometry newLine = new LineGeometry();
                newPath.Data = newLine;

                // Predefined "myThumb2" element will host the starting point
                myThumb2.StartLines.Add(newLine);
                // Our new thumb will host the ending point of the line
                newThumb.EndLines.Add(newLine);

                // Update the layout of line geometry
                UpdateLines(newThumb);
                UpdateLines(myThumb2);

                isAddNew = false;
                Mouse.OverrideCursor = null;
                //btnNewAction.IsEnabled = true;
                e.Handled = true;
            }
        }

        // Event handler for enabling new thumb creation by left mouse button click
        private void btnNewAction_Click(object sender, RoutedEventArgs e)
        {
            isAddNew = true;
            Mouse.OverrideCursor = Cursors.SizeAll;
            //btnNewAction.IsEnabled = false;
        }
        // Event hanlder for dragging functionality support same to all thumbs
        private void onDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            MyThumb thumb = e.Source as MyThumb;

            double left = Canvas.GetLeft(thumb) + e.HorizontalChange;
            double top = Canvas.GetTop(thumb) + e.VerticalChange;

            Canvas.SetLeft(thumb, left);
            Canvas.SetTop(thumb, top);

            // Update lines's layouts
            UpdateLines(thumb);
        }

        // This method updates all the starting and ending lines assigned for the given thumb 
        // according to the latest known thumb position on the canvas
        private void UpdateLines(MyThumb thumb)
        {
            double left = Canvas.GetLeft(thumb);
            double top = Canvas.GetTop(thumb);

            for (int i = 0; i < thumb.StartLines.Count; i++)
                thumb.StartLines[i].StartPoint = new Point(left + thumb.ActualWidth / 2, top + thumb.ActualHeight / 2);

            for (int i = 0; i < thumb.EndLines.Count; i++)
                thumb.EndLines[i].EndPoint = new Point(left + thumb.ActualWidth / 2, top + thumb.ActualHeight / 2);
        }

        #region menu

        private void save_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.ShowDialog();
            SaveData();
        }

        private void saveAs_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }



        private void new_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!projectSaved)
            {
                if (MessageBox.Show("chcete uložit projekt"+"\""+tbProjName.Text + "\" ?", "pozor", MessageBoxButton.YesNo) == MessageBoxResult.Yes) SaveData();

            }
            NewProjWindow newWind = new NewProjWindow(path);
            newWind.ShowDialog();
            if(newWind.DialogResult==true)
            {
                tbProjName.Text = "";
            }
        }



        #endregion





        private void open_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void rbParam_Checked(object sender, RoutedEventArgs e)
        {
            handControlEnable = false;
            changeEnable = true;
        }

        #region serialize

        private void SaveData()
        {
            string folder = new DirectoryInfo(path).GetDirectories().OrderByDescending(f => f.LastWriteTime).First().FullName;
            //string filename = new DirectoryInfo(folder).GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName;
            try
            {

                XmlSerializer serializer = new XmlSerializer(typeof(GpioData));

                using (StreamWriter sw = new StreamWriter(pathProj))
                {
                    serializer.Serialize(sw, gpioData);
                }
                projectSaved = true;
                GetProjNames();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveData();
        }

        private void lbProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //gpioData = new GpioData();
            if (!projectSaved)
            {
                if (MessageBox.Show("chcete uložit projekt" + "\"" + tbProjName.Text + "\" ?", "pozor", MessageBoxButton.YesNo) == MessageBoxResult.Yes) SaveData();

            }
            XmlSerializer ser = new XmlSerializer(typeof(GpioData));
            string filePath = new DirectoryInfo(folderList[lbProjects.SelectedIndex]).GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName;
            pathProj = filePath;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                try
                {
                    //gpioData = new GpioData();
                    gpioData =(GpioData) ser.Deserialize(fs);
                    tbProjName.Text = projectList[lbProjects.SelectedIndex];
                    //GetProjNames();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
           

        }

        private void GetProjNames()
        {
            folderList = new List<string>(Directory.GetDirectories(path));
            projectList = new ObservableCollection<string>();
            foreach(string str in folderList)
            {
                projectList.Add(new DirectoryInfo(str).Name);
            }
            lbProjects.ItemsSource = projectList;
           
        }

        //zrusit
        private void mainmenu_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void LoadData()
        {

        }


        #endregion


    }
}
