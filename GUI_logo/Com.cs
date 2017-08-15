using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GUI_logo
{
    class Com
    {
        public SerialPort serialPort = new SerialPort();
        public string[] DejPorty { get { return SerialPort.GetPortNames(); } }
        public Com()
        {
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            serialPort.BaudRate = 38400;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.DataBits = 8;
            serialPort.Handshake = Handshake.None;
            serialPort.ReadTimeout = -1;
            serialPort.WriteTimeout = -1;

        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //string s = 
            MainWindow.txBuffer = serialPort.ReadLine();

            //for (int i = 0; i < s.Length; i++)
            //{
            //    //if ((MainWindow.VYSTUPY[i] = s[i] - 0x30) == 1) MainWindow.ledOUT[i].Fill = RSboard.barvyLedOut[1];
            //    //else MainWindow.ledOUT[i].Fill = RSboard.barvyLedOut[0];
            //}


        }

        public void send()
        {
            StringBuilder sb = new StringBuilder();

            //for (int i = 0; i < MainWindow.VSTUPY.Count; i++)
            //{
            //    sb.Append(MainWindow.VSTUPY[i]);
            //}
            serialPort.WriteLine(MainWindow.txBuffer);
        }

        public void send(string command)
        {
            serialPort.WriteLine(command);
        }

        public bool OpenPort()
        {
            try
            {
                if (serialPort != null)
                {
                    if (!serialPort.IsOpen)
                    {
                        // port neotevren,otevrit 
                        serialPort.Open();
                        // Povol_cteni_portu = true;
                        MainWindow.inputs[0].Led.Fill = Brushes.Lime;
                    }
                    else
                    {
                        //port otevren, zavrit
                        //  Povol_cteni_portu = false;
                        serialPort.DiscardInBuffer();
                        //   while (!zavritPortFlag) ;
                        serialPort.Close();
                        //   zavritPortFlag = false;
                    }

                }
                return true;
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}
