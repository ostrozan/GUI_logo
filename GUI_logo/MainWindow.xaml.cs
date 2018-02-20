using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Xml.Serialization;

namespace GUI_logo
{
    [Serializable()]
    public struct GpioData
    {
        public ObservableCollection<In> inputs;
        public ObservableCollection<Out> outputs;
        public ObservableCollection<GsmData> gsmData;
    }
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer timer = new DispatcherTimer();
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
        public static ObservableCollection<string> texts_counters { get; set; } = new ObservableCollection<string> { "", "", "", "", "", "" };
        public static List<TextBlock> tbls_counters;
        private string path;
        public static string pathProj;
        public static string projectName;
        public static ObservableCollection<string> projPaths;
        public static ObservableCollection<string> temperatures { get; set; } = new ObservableCollection<string>() { "", "" };
        public static ObservableCollection<string> textMsg { get; set; } = new ObservableCollection<string>() { "", "" };
        public static ObservableCollection<int> gsmSigValue { get; set; } = new ObservableCollection<int>() { 0 };
        //private string currFile;
        Com com;
        bool handControlEnable, changeEnable;
        public static bool projectSaved;
        //MyThumb myThumb2;
        //bool isAddNew = false;
        public MainWindow()
        {
            InitializeComponent();
            tbls_counters = new List<TextBlock> { tbCounter1, tbCounter2, tbCounter3, tbCounter4, tbCounter5, tbCounter6 };
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
            Out1.img.IsEnabled = false;
            In1.Led.Fill = Brushes.DimGray;
            In2.Led.Fill = Brushes.DimGray;
            In3.Led.Fill = Brushes.DimGray;
            In4.Led.Fill = Brushes.DimGray;
            inputs = new ObservableCollection<Gpio>() { In1, In2, In3, In4 };
            outputs = new ObservableCollection<Gpio>() { Out1, Out2, Out3, Out4, Out5, Out6 };
            com = new Com();
            DataContext = this;
            btnAktCas.IsEnabled = false;
            btnUpload.IsEnabled = false;
            //Out1.stPanel.Children.Add(Out1.img);
            //Out1.stPanel.Children.Add(Out1.tblCounter);
            //tbCounter1.Text = "99:24";
        }

        #region obsluhy udalosti
        #region butons clicks
        private void btnSerCom_Click(object sender, RoutedEventArgs e)
        {
            com.OpenPort();
            if (com.State != Com.comState.fail)
            {
                if (com.State == Com.comState.open)
                {
                    btnAktCas.IsEnabled = true;
                    btnUpload.IsEnabled = true;
                    btnSerCom.Content = "Odpojit";
                    MessageBox.Show(com.serialPort.PortName + " byl úspěšně  připojen");
                }
                else
                {
                    btnAktCas.IsEnabled = false;
                    btnUpload.IsEnabled = false;
                    btnSerCom.Content = "Připojit";
                    MessageBox.Show(com.serialPort.PortName + " byl úspěšně  odpojen");
                }
            }
        }

        private void Out1_Click(object sender, RoutedEventArgs e)
        {
            int a = 10;
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
                if (gpio.Name.Contains("In")) return;
                StackPanel stp = gpio.Parent as StackPanel;
                int pom = int.Parse(gpio.Uid);
                if (stp.Uid == "in")
                {
                    if (VSTUPY[pom] == 0)
                    {
                        VSTUPY[pom] = 1;
                        com.send("M-I-" + int.Parse(gpio.Uid) + "-1\n");
                    }

                    else
                    {
                        VSTUPY[pom] = 0;
                        com.send("M-I-" + int.Parse(gpio.Uid) + "-0\n");
                    }
                }
                else if (stp.Uid == "out")
                {
                    if (VYSTUPY[pom] == 0)
                    {
                        VYSTUPY[pom] = 1;
                        com.send("M-O-" + int.Parse(gpio.Uid) + "-1\n");
                    }

                    else
                    {
                        VYSTUPY[pom] = 0;
                        com.send("M-O-" + int.Parse(gpio.Uid) + "-0\n");
                    }
                }
            }
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (isAddNew)
            //{
            //    // Create new thumb object
            //    MyThumb newThumb = new MyThumb();
            //    // Assign our custom template to it
            //    ControlTemplate template = new ControlTemplate();
            //    var fec = new FrameworkElementFactory(typeof(Input));
            //    template.VisualTree = fec;
            //    newThumb.Template = template;

            //    // Calling ApplyTemplate enables us to navigate the visual tree right now (important!)
            //    newThumb.ApplyTemplate();
            //    // Add the "onDragDelta" event handler that is common to all objects
            //    newThumb.DragDelta += new DragDeltaEventHandler(onDragDelta);
            //    // Put newly created thumb on the canvas
            //    mainCanvas.Children.Add(newThumb);

            //    // Access the image element of our custom template and assign it to the new image
            //    //Image img = (Image)newThumb.Template.FindName("tplImage", newThumb);
            //    //img.Source = new BitmapImage(new Uri("Images/gear_connection.png", UriKind.Relative));

            //    // Access the textblock element of template and change it too
            //    //TextBlock txt = (TextBlock)newThumb.Template.FindName("tplTextBlock", newThumb);
            //    //txt.Text = "System action";

            //    // Set the position of the object according to the mouse pointer                
            //    Point position = e.GetPosition(this);
            //    Canvas.SetLeft(newThumb, position.X);
            //    Canvas.SetTop(newThumb, position.Y);
            //    // Move our thumb to the front to be over the lines
            //    Canvas.SetZIndex(newThumb, 1);
            //    // Manually update the layout of the thumb (important!)
            //    newThumb.UpdateLayout();

            //    // Create new path and put it on the canvas
            //    System.Windows.Shapes.Path newPath = new System.Windows.Shapes.Path();
            //    newPath.Stroke = Brushes.Black;
            //    newPath.StrokeThickness = 1;
            //    mainCanvas.Children.Add(newPath);

            //    // Create new line geometry element and assign the path to it
            //    LineGeometry newLine = new LineGeometry();
            //    newPath.Data = newLine;

            //    // Predefined "myThumb2" element will host the starting point
            //    myThumb2.StartLines.Add(newLine);
            //    // Our new thumb will host the ending point of the line
            //    newThumb.EndLines.Add(newLine);

            //    // Update the layout of line geometry
            //    UpdateLines(newThumb);
            //    UpdateLines(myThumb2);

            //    isAddNew = false;
            //    Mouse.OverrideCursor = null;
            //    //btnNewAction.IsEnabled = true;
            //    e.Handled = true;
            //}
        }

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

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            string data = "";// "#:3:100000:123456789:ahoj :#:3:010000:000000000::#:4:000000:123456789:ahoj:#:0:000111:000000000::#:1:0:0:0:1:0:1:1:0:0:%4-2%:%240-0%184-6%:#:1:1:0:0:1:1:0:1:0:0:%6-0%:%:#:0:0:0:0:0:0:0:0:0:0:%0-0%:%:#:0:1:0:0:0:0:0:0:0:0:%0-0%:%:#:1:1:0:0:1:0:1:1:0:0:%7441-120%:%:#:1:0:0:1:0:0:0:0:0:0:%0-0%:%300-9%:#";
                             //com.send(data);
                             //return;
                             // string data = "D";
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                data += "D#I";
                data += i.ToString();
                data += ":" + gpioData.inputs[i].funcIndex.ToString();
                data += ":";
                foreach (bool b in gpioData.inputs[i].outs)
                {
                    data += b ? "1" : "0";
                }
                data += ":" + gpioData.inputs[i].Tel[0] + ' ';
                data += ":" + gpioData.inputs[i].Sms[0].PadLeft(21, '.') + ":\n";
                com.send(data);

                //index++
                Thread.Sleep(200);
                data = data.Remove(0);
            }
            //data += ":";
            index = 0;
            for (int i = 0; i < 6; i++)
            {
                data += "D#O";
                data += i.ToString();

                data += ":";//isTimeControl
                data += (bool)gpioData.outputs[i].IsTimeControl ? "1" : "0";
                data += ":";//isInputControl
                data += (bool)gpioData.outputs[i].IsInputControl ? "1" : "0";
                data += ":";//isExtControl
                data += (bool)gpioData.outputs[i].IsExtControl ? "1" : "0";
                data += ":";//isSwitchClock
                data += (bool)gpioData.outputs[i].IsUseSwitchClk ? "1" : "0";
                data += ":";//isPrgTmr
                data += (bool)gpioData.outputs[i].IsUseProgTmr ? "1" : "0";
                data += ":";
                data += (bool)gpioData.outputs[i].IsUseThermostat ? "1" : "0";
                data += ":";
                //Thermostat
                data += Convert.ToString((int)(gpioData.outputs[i].temperature * 10));
                data += ":";
                data += Convert.ToString((int)(gpioData.outputs[i].hystreresis * 10));
                data += ":";
                data += Convert.ToString((int)(gpioData.outputs[i].alarmHi * 10));
                data += ":";
                data += Convert.ToString((int)(gpioData.outputs[i].alarmLo * 10));
                data += ":";
                data += (bool)gpioData.outputs[i].IsAlarmHi ? "1" : "0";
                data += ":";
                data += (bool)gpioData.outputs[i].IsAlarmLo ? "1" : "0";
                data += ":";
                if (gpioData.outputs[i].KtereCidlo == 0) data += '0';
                else data += gpioData.outputs[i].KtereCidlo;
                data += ":";
                //ProgTimer
                data += (bool)gpioData.outputs[i].IsTrvale ? "1" : "0";
                data += ":";
                data += (bool)gpioData.outputs[i].IsNastCas ? "1" : "0";
                data += ":";
                data += (bool)gpioData.outputs[i].IsSwitchOn ? "1" : "0";
                data += ":";
                data += (bool)gpioData.outputs[i].IsSwitchOff ? "1" : "0";
                data += ":";
                data += (bool)gpioData.outputs[i].IsAnyChange ? "1" : "0";
                data += ":";
                data += gpioData.outputs[i].controlTimes.timeOfDelay.ToString() + ':' + gpioData.outputs[i].controlTimes.timeOfPulse.ToString() + ":";

                for (int ii = 0; ii < 4; ii++)
                {
                    if (ii < gpioData.outputs[i].minuteSpan.Count)
                    {
                        data += gpioData.outputs[i].minuteSpan[ii].startTime.ToString() + ":" + gpioData.outputs[i].minuteSpan[ii].stopTime.ToString() + ":";
                    }
                    //else if (ii > 0) break;
                    else data += "1500" + ":" + "1500" + ":";//1500 nerealny pocet minut za den (max 1440)  - 0 potrebuju pro pulnoc

                }
                //data += ":";

                //if(index==5) data += "E#\n";
                //else
                data += "\n";
                index++;
                int l = data.Length;
                com.send(data);

                Thread.Sleep(200);
                data = data.Remove(0);
            }
            data = "D#G";
            data += (gpioData.gsmData[0].isEnabled) ? '1' : '0';
            data += (gpioData.gsmData[0].isResponse) ? '1' : '0';
            data += gpioData.gsmData[0].telNumbers[0] + ' ';
            data += gpioData.gsmData[0].telNumbers[1] + ' ';
            data += gpioData.gsmData[0].telNumbers[2] + ' ';
            data += "E#\n";
            com.send(data);
            Thread.Sleep(200);
            data = data.Remove(0);
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAktCas_Click(object sender, RoutedEventArgs e)
        {

            string str = "T#" + DateTime.Now.ToLocalTime().ToString();
            str = str.Replace('.', '#');
            str = str.Replace(':', '#');
            str = str.Replace(' ', '#');
            str += "#\n";
            com.send(str);
        }
        #endregion//end buttons clicks
        private void cmbComPorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            com.serialPort.PortName = cmbComPorts.SelectedItem.ToString();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Left;
            this.Top = desktopWorkingArea.Top;
            btnAktCas.IsEnabled = false;
            btnUpload.IsEnabled = false;
            projPaths = new ObservableCollection<string>();
            path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ArDaLu projekty");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            cmbComPorts.ItemsSource = com.DejPorty;
            gpioData.inputs = new ObservableCollection<In>() { In1.GpIn, In2.GpIn, In3.GpIn, In4.GpIn };
            gpioData.outputs = new ObservableCollection<Out>() { Out1.GpOut, Out2.GpOut, Out3.GpOut, Out4.GpOut, Out5.GpOut, Out6.GpOut };
            //gpioData.gsmData = new ObservableCollection<GsmData>();
            foreach (Gpio gp in stPanelOuts.Children)
            {
                gp.watchImg = new BitmapImage(new Uri("pack://application:,,,/Images/watch.bmp"));
                gp.stopwatchImg = new BitmapImage(new Uri("pack://application:,,,/Images/stopwatch.bmp"));
                gp.tempMeterImg = new BitmapImage(new Uri("pack://application:,,,/Images/tempmeter.bmp"));
            }
            GetProjNames();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int i = 0;
            foreach (Gpio gp in inputs)
            {
                gp.Led.Fill = VSTUPY[i++] == 1 ? Brushes.Gold : Brushes.PaleGoldenrod;
            }
            i = 0;
            foreach (Gpio gp in outputs)
            {
                gp.Led.Fill = VYSTUPY[i++] == 1 ? Brushes.Lime : Brushes.DimGray;
            }
        }

        private void stPanelIns_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Out_MouseEnter(object sender, MouseEventArgs e)
        {
            if (changeEnable || handControlEnable)
            {

                Gpio gpio = e.Source as Gpio;
                if (handControlEnable && gpio.Name.Contains("Out")) gpio.Background = Brushes.Green;
                else if (changeEnable) gpio.Background = Brushes.Green;
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

        private void rbAuto_Checked(object sender, RoutedEventArgs e)
        {
            handControlEnable = changeEnable = false;
            if (btnUpload != null) btnUpload.IsEnabled = false;
            //if (btnDownload != null) btnDownload.IsEnabled = false;
        }

        private void rbHand_Checked(object sender, RoutedEventArgs e)
        {
            handControlEnable = true;
            changeEnable = false;
            btnUpload.IsEnabled = false;
            //btnDownload.IsEnabled = false;
        }

        #endregion
        //// Event handler for enabling new thumb creation by left mouse button click
        //private void btnNewAction_Click(object sender, RoutedEventArgs e)
        //{
        //    isAddNew = true;
        //    Mouse.OverrideCursor = Cursors.SizeAll;
        //    //btnNewAction.IsEnabled = false;
        //}
        //// Event hanlder for dragging functionality support same to all thumbs
        //private void onDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        //{
        //    MyThumb thumb = e.Source as MyThumb;

        //    double left = Canvas.GetLeft(thumb) + e.HorizontalChange;
        //    double top = Canvas.GetTop(thumb) + e.VerticalChange;

        //    Canvas.SetLeft(thumb, left);
        //    Canvas.SetTop(thumb, top);

        //    // Update lines's layouts
        //    UpdateLines(thumb);
        //}

        //// This method updates all the starting and ending lines assigned for the given thumb 
        //// according to the latest known thumb position on the canvas
        //private void UpdateLines(MyThumb thumb)
        //{
        //    double left = Canvas.GetLeft(thumb);
        //    double top = Canvas.GetTop(thumb);

        //    for (int i = 0; i < thumb.StartLines.Count; i++)
        //        thumb.StartLines[i].StartPoint = new Point(left + thumb.ActualWidth / 2, top + thumb.ActualHeight / 2);

        //    for (int i = 0; i < thumb.EndLines.Count; i++)
        //        thumb.EndLines[i].EndPoint = new Point(left + thumb.ActualWidth / 2, top + thumb.ActualHeight / 2);
        //}

        #region menu


        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void new_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!projectSaved && tbProjName.Text != "")
            {
                if (MessageBox.Show("chcete uložit projekt" + "\"" + tbProjName.Text + "\" ?", "pozor", MessageBoxButton.YesNo) == MessageBoxResult.Yes) SaveData();

            }
            NewProjWindow newWind = new NewProjWindow(path);
            newWind.ShowDialog();
            if (newWind.DialogResult == true)
            {
                GetProjNames();
                tbProjName.Text = projectName;
                for (int i = 0; i < 4; i++) gpioData.inputs[i] = new In();
                for (int i = 0; i < 6; i++) gpioData.outputs[i] = new Out();
                foreach (In inp in gpioData.inputs)
                {
                    //    inp. = new In();

                    inp.funcIndex = 0;
                    inp.Sms.Add("");
                    inp.Tel.Add("000000000");
                    inp.outs.Add(false);
                    inp.outs.Add(false);
                    inp.outs.Add(false);
                    inp.outs.Add(false);
                    inp.outs.Add(false);
                    inp.outs.Add(false);
                }

                foreach (Out outp in gpioData.outputs)
                {
                    outp.IsTimeControl = false;
                    outp.IsInputControl = false;
                    outp.IsExtControl = false;
                    outp.IsUseSwitchClk = false;
                    outp.IsUseProgTmr = false;
                    outp.IsTrvale = false;
                    outp.IsNastCas = false;
                    outp.IsSwitchOn = false;
                    outp.IsSwitchOff = false;
                    outp.IsAnyChange = false;
                }

            }
        }

        private void open_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveData();
        }


        #endregion


        private void rbParam_Checked(object sender, RoutedEventArgs e)
        {
            handControlEnable = false;
            changeEnable = true;
            if (com.State == Com.comState.open) { btnUpload.IsEnabled = true; btnAktCas.IsEnabled = true; }
            else { btnUpload.IsEnabled = false; btnAktCas.IsEnabled = false; }
            //btnDownload.IsEnabled = true;
        }

        private void lbProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //gpioData = new GpioData();
            if (!projectSaved && tbProjName.Text != "")
            {
                if (MessageBox.Show("chcete uložit projekt" + "\"" + tbProjName.Text + "\" ?", "pozor", MessageBoxButton.YesNo) == MessageBoxResult.Yes) SaveData();

            }
            XmlSerializer ser = new XmlSerializer(typeof(GpioData));
            string filePath = new DirectoryInfo(folderList[lbProjects.SelectedIndex]).GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName;
            pathProj = filePath;
            //GpioData gpioData1 = new GpioData();
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                try
                {
                    
                    //gpioData1 = (GpioData)ser.Deserialize(fs);
                    gpioData = (GpioData)ser.Deserialize(fs);
                    tbProjName.Text = projectList[lbProjects.SelectedIndex];

                    foreach (Gpio gp in stPanelOuts.Children)
                    {
                        int idx = stPanelOuts.Children.IndexOf(gp);
                        gp.stPanel.Children.RemoveRange(0, gp.stPanel.Children.Count);
                        if ((bool)gpioData.outputs[idx].IsUseProgTmr == true)
                        {
                            gp.img.Source = gp.stopwatchImg;
                            gp.stPanel.Children.Add(gp.img);
                            gp.stPanel.Children.Add(gp.tblCounter);
                            //bind[idx] = new Binding("Text");
                            //bind[idx].Source = texts_counters[idx];
                            //gp.binding.Source = texts_counters[idx];
                            //gp.tblCounter.SetBinding(TextBlock.TextProperty, gp.binding);
                        }
                        if ((bool)gpioData.outputs[idx].IsUseSwitchClk == true)
                        {
                            gp.img.Source = gp.watchImg;
                            gp.stPanel.Children.Add(gp.img);
                            gp.stPanel.Children.Add(gp.tblCounter);
                        }
                        if ((bool)gpioData.outputs[idx].IsUseThermostat == true)
                        {
                            gp.img.Source = gp.tempMeterImg;
                            gp.stPanel.Children.Add(gp.img);
                            //gp.stPanel.Children.Add(gp.tblCounter);
                        }

                    }
                    //GetProjNames();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }

        #region serialize

        #region metody
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
        private void GetProjNames()
        {
            folderList = new List<string>(Directory.GetDirectories(path));
            projectList = new ObservableCollection<string>();
            foreach (string str in folderList)
            {
                projectList.Add(new DirectoryInfo(str).Name);
            }
            lbProjects.ItemsSource = projectList;

        }
        private void LoadData()
        {

        }

        private void In1_Loaded(object sender, RoutedEventArgs e)
        {

        }



        private void Rectangle_Click(object sender, MouseButtonEventArgs e)
        {
            WindowSetGsm setGsm = new WindowSetGsm(gpioData.gsmData[0]);
            setGsm.ShowDialog();
        }

        #endregion


        #endregion


    }
}
