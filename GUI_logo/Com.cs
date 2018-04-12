using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Management;
using System.Threading;
using System.Windows.Controls;

namespace GUI_logo
{
    public class Com
    {
        public enum comState
        {
            fail, open, close
        }
        public comState State { get; private set; }
        public SerialPort serialPort = new SerialPort();
        public string[] DejPorty { get { return SerialPort.GetPortNames(); } }
        public bool IsRxEvent { get; set; }
        public bool IsAck { get; set; }
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


        private string[] PortNames()
        {
            List<string> tmp = new List<string>();
            try
            {
                //instance vyhledávače objektů(portů)
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort");

                //projede kolekci nalezených objektů
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    //vyseparuje název
                    tmp.Add(queryObj["Caption"].ToString());

                    //když najde požadovaný název v seznamu přiřadí mu místní serialPort ("Port")
                    //vypíše hlášku o úspěchu/neúspěchu
                    //if (s.IndexOf(deviceName) != -1)//eZ430-ChronosAP
                    //{
                    //    MessageBox.Show("AP byl nalezen v portu: " + queryObj["DeviceID"]);
                    //    ez43serial.PortName = queryObj["DeviceID"].ToString();
                    //    retval = true;
                    //    nalezeno = true;
                    //}

                }

                //if (nalezeno == false) MessageBox.Show("Nebylo nalezeno požadované zařízení", "POZOR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            catch (ManagementException e2)
            {

            }
            return tmp.ToArray();
        }

        public void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //string s = 
            MainWindow.txBuffer = serialPort.ReadLine();
            IsRxEvent = true;
            serialPort.DiscardInBuffer();
            MainWindow.textMsg[1] = MainWindow.txBuffer;
            if (MainWindow.txBuffer.Contains("dt>"))
            {
                MainWindow.textMsg[0] = MainWindow.txBuffer.Substring(3, MainWindow.txBuffer.IndexOf('<') - 3);
                MainWindow.temperatures[0] = MainWindow.txBuffer.Substring(MainWindow.txBuffer.IndexOf('<') + 1, 4) + " °C";
                MainWindow.temperatures[1] = MainWindow.txBuffer.Substring(MainWindow.txBuffer.IndexOf('<') + 6, 4) + " °C";
                MainWindow.gsmSigValue[0] = Convert.ToInt32(MainWindow.txBuffer.Substring(MainWindow.txBuffer.IndexOf('<') + 11, 2));
            }
            else
            {

                if (MainWindow.txBuffer.Contains("rec")) MessageBox.Show("data upload succesful");
                if (MainWindow.txBuffer.Contains("Ok"))
                {
                    int a = 10;
                    IsAck = true;
                } 
                else if (MainWindow.txBuffer.Contains("Err")) MessageBox.Show("chyba přenosu dat - zkuste znovu");
                if (MainWindow.txBuffer.Contains("flg"))
                {
                    int c1 = Convert.ToInt16(MainWindow.txBuffer[0] - 0x30);
                    MainWindow.texts_counters[c1] = "";
                }

                if (MainWindow.txBuffer.Contains("in"))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        MainWindow.VSTUPY[i] = Convert.ToInt16(MainWindow.txBuffer[i + 2] - 0x30);
                    }
                }

                if (MainWindow.txBuffer.Contains("out"))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        MainWindow.VYSTUPY[i] = Convert.ToInt16(MainWindow.txBuffer[i + 3] - 0x30);
                    }
                }
                if (MainWindow.txBuffer.Contains("tmr"))
                {
                    try
                    {
                        MainWindow.textMsg[1] = MainWindow.txBuffer;
                        //Thread.Sleep(500);
                        int i1 = MainWindow.txBuffer.IndexOf("-") + 1;
                        int i2 = MainWindow.txBuffer.IndexOf("<");
                        int c2 = Convert.ToInt16(MainWindow.txBuffer.Substring(i1, i2 - i1));
                        int c1 = Convert.ToInt16(MainWindow.txBuffer[0] - 0x30);
                        MainWindow.texts_counters[c1] = Convert.ToString(c2 / 60).PadLeft(2, '0') + ':' + Convert.ToString(c2 % 60).PadLeft(2, '0');
                    }
                    catch { }

                }

                if (MainWindow.txBuffer.Contains("tmr"))
                {
                }
            }



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
            int checksum = 0;

            byte[] pole = Encoding.ASCII.GetBytes(command);
           
            for(int i =0;i<pole.Length-3;i++)
            {
                checksum += pole[i];
            }
            checksum &= 0xFF;
            checksum = (byte)(0-checksum);
            
            pole[command.Length - 2] =(byte) checksum;
            serialPort.Write(pole, 0, command.Length);

        }

        public void OpenPort()
        {
            State = comState.fail;
            try
            {
                if (serialPort != null)
                {
                    if (!serialPort.IsOpen)
                    {
                        // port neotevren,otevrit 
                        serialPort.Open();
                        State = comState.open;
                        //MainWindow.inputs[0].Led.Fill = Brushes.Lime;
                    }
                    else
                    {
                        //port otevren, zavrit
                        State = comState.close;
                        serialPort.DiscardInBuffer();
                        //   while (!zavritPortFlag) ;
                        serialPort.Close();
                        //   zavritPortFlag = false;
                    }

                }

            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        public bool GetAck(string s)
        {
            int cnt = 0;
            send(s);
            while (!IsRxEvent)
            {
                Thread.Sleep(10);
                if (++cnt > 100) break;
            }

            IsRxEvent = false;
            return (cnt > 100) ? false : true;
        }
    }
}
