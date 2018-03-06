using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;

namespace GUI_logo
{
    /// <summary>
    /// Interakční logika pro WindowEvent.xaml
    /// </summary>
    public partial class WindowEvent : Window
    {

        private Com com;
        private string rx_data;
        Brush color;
        private List<string> events = new List<string>();
        private Paragraph par = new Paragraph();
        private TextRange tr;

        public WindowEvent(Com com,string rx_data)
        {
            InitializeComponent();
            this.com = com;
            this.rx_data = rx_data;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Nacti_historii();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //spojeni.Texxt[0] = null;

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Text Files (*.rtf;)|*.rtf;";
            sfd.Filter = "Text Files (*.txt;)|*.txt;";
            sfd.InitialDirectory = MainWindow.pathProj;
            if (sfd.ShowDialog() == true)
            {
                //using (FileStream fs = new FileStream(sfd.SafeFileName, FileMode.Create))
                //{

                //    //TextRange tr = new TextRange(document.ContentStart, document.ContentEnd);
                //    tr.Save(fs, DataFormats.Text);
                //}
                using (StreamWriter outputFile = new StreamWriter(sfd.FileName))
                {
                    

                        outputFile.WriteLine(tbMain.Text);
                }
            }

        }

        public void Nacti_historii()
        {
            //List<string> events = new List<string>();
            //StringBuilder bldr = new StringBuilder();
          

            
            events = rx_data.Split('>').ToList();
            events.RemoveAt(0);//prvni odebrat
            events.RemoveAt(events.Count - 1);//posledni odebrat
            for (int i=0; i< events.Count;i++)
            {
                string[] sx = events[i].Split(' ');
                if(sx[0]!="65535" )
                {
                    sx[6] = sx[6].PadLeft(4, '0');
                    UInt16 udalost = Convert.ToUInt16(sx[6], 16);                 
                    string radek = sx[2] + '.' + sx[1] + '.' + sx[0] + "   " + sx[3] + ':' + sx[4] + ':' + sx[5] + "   ";
                    if (udalost == 0) radek += "RESET ";
                    else if (udalost >= 0x200) radek += "vstup " + VyberVstup(udalost);
                    else radek += "výstup " + VyberVystup(udalost);
                    radek += "  " + VyberUdalost(udalost) + '\n';
                    tbMain.Text += radek;
                    //tr = new TextRange(par.ContentStart, par.ContentEnd);              
                    //sx[6] = sx[6].PadLeft(4, '0');
                    //tr.Text = sx[2]+'.'+ sx[1] + '.' + sx[0] + "   " + sx[3] + ':' + sx[4] + ':' + sx[5]+ "   ";
                    //tr.Text += "výstup " + VyberVystup(Convert.ToUInt16(sx[6],16));
                    //tr.Text += "  " +VyberUdalost(Convert.ToUInt16(sx[6],16));
                    //par = new Paragraph(new Run(tr.Text));
                    //par.Foreground = color;
                    //document.Blocks.Add(par);
                }


            }
            //for (int i = 12; i <= (int)(((spojeni.ComData[2][1] + 1) * 8) + 14); i += 8)
            //{
            //    if (spojeni.ComData[2][i - 6] > 20)
            //    {
            //        if (spojeni.ComData[2][i - 6] < 30) bldr.Append(' ');
            //        bldr.Append(spojeni.ComData[2][i - 6] - 20);
            //        bldr.Append('.');
            //        if (spojeni.ComData[2][i - 5] < 30) bldr.Append(' ');
            //        bldr.Append(spojeni.ComData[2][i - 5] - 20);
            //        bldr.Append('.');
            //        bldr.Append("   ");

            //        if (spojeni.ComData[2][i - 4] < 30) bldr.Append(' ');
            //        bldr.Append(spojeni.ComData[2][i - 4] - 20);
            //        bldr.Append(':');
            //        if (spojeni.ComData[2][i - 3] < 30) bldr.Append('0');
            //        bldr.Append(spojeni.ComData[2][i - 3] - 20);
            //        bldr.Append(':');
            //        if (spojeni.ComData[2][i - 2] < 30) bldr.Append('0');
            //        bldr.Append(spojeni.ComData[2][i - 2] - 20);

            //        tr.Text += " " + bldr.ToString();
            //        bldr.Remove(0, bldr.Length);



            //        string str = VyberUdalost(spojeni.ComData[2][i]);
            //        if (str == "poplach")
            //            tr.Text += "   " + str + " " + VyberPopisSmycky((int)spojeni.ComData[2][i - 1]) + '\n';
            //        else tr.Text += "   " + str + '\n';
            //        // tr.ApplyPropertyValue(TextElement.ForegroundProperty,color);
            //        /// bldr.Remove (0, bldr.Length);
            //    }

            //    else tr.Text += "- - - - - - - - -\n";

            //    tr.Text = "";
            //}


        }

        private string VyberVystup(UInt16 typ_udalosti)
        {
            typ_udalosti &= 0xF;
            string ret = "";
            switch(typ_udalosti)
            {
                case 0x0: ret = "relé 1"; break;
                case 0x1: ret = "relé 2"; break;
                case 0x2: ret = "out 3"; break;
                case 0x3: ret = "out 4"; break;
                case 0x4: ret = "out 5"; break;
                case 0x5: ret = "out 6"; break;
            }
            
            return ret;
        }

        private string VyberVstup(UInt16 typ_udalosti)
        {
            typ_udalosti &= 0xF;
            string ret = "";
            switch (typ_udalosti)
            {
                case 0x0: ret = "in 1"; break;
                case 0x1: ret = "in 2"; break;
                case 0x2: ret = "in 3"; break;
                case 0x3: ret = "in 4"; break;
            }

            return ret;
        }


        private string VyberUdalost(UInt16 typ_udalosti)
        {
            string str = "";
            UInt16 pom_typ = typ_udalosti;
            pom_typ &= 0xFFF0;//horni nible
            switch (pom_typ)
            {
                case 0x10: str = "zapnut vstupem"; color = Brushes.Black; break;
                case 0x20: str = "vypnut vstupem"; color = Brushes.Black; break;
                case 0x30: str = "zapnut čidlem 1"; color = Brushes.Black; break;
                case 0x40: str = "vypnut čidlem 1"; color = Brushes.Black; break;
                case 0x50: str = "zapnut čidlem 2"; color = Brushes.Black; break;
                case 0x60: str = "vypnut čidlem 2"; color = Brushes.Black; break;
                case 0x70: str = "spuštěno časování"; color = Brushes.Black; break;
                case 0x80: str = "ukončeno časování"; color = Brushes.Black; break;
                case 0x90: str = "zapnut spínacími hodinami"; color = Brushes.Red; break;
                case 0xa0: str = "vypnut spínacími hodinami"; color = Brushes.Green; break;
                case 0xb0: str = "zapnut přes sms"; color = Brushes.Green; break;
                case 0xc0: str = "vypnut přes sms"; color = Brushes.Green; break;
                case 0xd0: str = "zapnut prozvoněním"; color = Brushes.Blue; break;
                case 0xe0: str = "vypnut prozvoněním"; color = Brushes.Blue; break;
                case 0xf0: str = "zapnut ručně z PC"; color = Brushes.Blue; break;
                case 0x100: str = "vypnut ručně z PC"; color = Brushes.Green; break;
                case 0x200: str = "odeslána sms"; color = Brushes.Green; break;
                case 0x300: str = "prozvonění"; color = Brushes.Green; break;
                case 0x400: str = "odeslána sms + prozvonění"; color = Brushes.Green; break;
            }
            return str;
        }

        private void buttonDeleteHistory_Click(object sender, RoutedEventArgs e)
        {
            com.send("C#...............\n");
            Close();
            
        }
    }
}
