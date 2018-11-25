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
using System.Windows.Input;
using System.Runtime.InteropServices;

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
        public static string[] DejPorty { get { return SerialPort.GetPortNames(); } }
        public bool IsRxEvent { get; set; }
        public bool IsAck { get; set; }
        public string txBuffer { get; set; }
        public string rxBuffer { get; set; }

        public Com(string deviceName, int baudrate)
        {
            try
            {
                if (deviceName.Contains("COM")) { serialPort.PortName = deviceName; }
                else { serialPort.PortName = SearchDevice(deviceName); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            serialPort.BaudRate = baudrate;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.DataBits = 8;
            serialPort.Handshake = Handshake.None;
            serialPort.ReadTimeout = -1;
            serialPort.WriteTimeout = -1;

        }


        public string SearchDevice(string deviceName)
        {
            List<string> tmp = new List<string>();
            string comName = "";

            try
            {

                bool nalezeno = false;
                //instance vyhledávače objektů(portů)
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort");

                //projede kolekci nalezených objektů
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    //vyseparuje název
                    tmp.Add(queryObj["Caption"].ToString());

                    //když najde požadovaný název v seznamu přiřadí mu místní serialPort("Port")
                    //vypíše hlášku o úspěchu/ neúspěchu
                    if (queryObj["Caption"].ToString().Contains(deviceName))
                    {
                        comName = queryObj["DeviceID"].ToString();
                        nalezeno = true;
                    }
                    //eZ430-ChronosAP
                    //{
                    //    MessageBox.Show("Arduino nalezeno na portu: " + queryObj["DeviceID"]);
                    //    //ez43serial.PortName = queryObj["DeviceID"].ToString();
                    //    //retval = true;

                    //}

                }

                if (nalezeno == false)
                {
              
                    if (MessageBox.Show("Nebylo nalezeno žádné zařízení ArDaLu na USB \n chcete pokračovat přes bluetooth?", "POZOR", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        return "COM100";
                    }
                    else
                    {
                        comName = "none";
                    }
                }

            }

            catch (ManagementException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return comName;
        }

        public void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //string s = 
            rxBuffer = serialPort.ReadLine();
            IsRxEvent = true;
            serialPort.DiscardInBuffer();
            MainWindow.textMsg[1] = rxBuffer;
            try
            {
                if (rxBuffer.Contains("dt>"))
                {
                    int pom = 0;
                    int.TryParse(rxBuffer.Substring(rxBuffer.IndexOf('<') + 11, 2), out pom);
                    MainWindow.textMsg[0] = rxBuffer.Substring(3, rxBuffer.IndexOf('<') - 3);
                    MainWindow.temperatures[0] = rxBuffer.Substring(rxBuffer.IndexOf('<') + 1, 4) + " °C";
                    MainWindow.temperatures[1] = rxBuffer.Substring(rxBuffer.IndexOf('<') + 6, 4) + " °C";
                    MainWindow.gsmSigValue[0] = pom;
                }
                else
                {

                    if (rxBuffer.Contains("rec"))
                        MessageBox.Show("data upload succesful");
                    if (rxBuffer.Contains("Ok"))
                    {
                        IsAck = true;
                    }
                    else if (rxBuffer.Contains("Err"))
                    {
                        MessageBox.Show("chyba přenosu dat - zkuste znovu");
                        MainWindow.curs = Cursors.Arrow;
                    }
                    if (rxBuffer.Contains("flg"))
                    {
                        int c1 = Convert.ToInt16(rxBuffer[0] - 0x30);
                        MainWindow.texts_counters[c1] = "";
                    }

                    if (rxBuffer.Contains("in"))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            MainWindow.VSTUPY[i] = Convert.ToInt16(rxBuffer[i + 2] - 0x30);
                        }
                    }

                    if (rxBuffer.Contains("out"))
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            MainWindow.VYSTUPY[i] = Convert.ToInt16(rxBuffer[i + 3] - 0x30);
                        }
                    }
                    if (rxBuffer.Contains("tmr"))
                    {
                        try
                        {
                            MainWindow.textMsg[1] = rxBuffer;
                            //Thread.Sleep(500);
                            int i1 = rxBuffer.IndexOf("-") + 1;
                            int i2 = rxBuffer.IndexOf("<");
                            int c2 = Convert.ToInt16(rxBuffer.Substring(i1, i2 - i1));
                            int c1 = Convert.ToInt16(rxBuffer[0] - 0x30);
                            MainWindow.texts_counters[c1] = Convert.ToString(c2 / 60).PadLeft(2, '0') + ':' + Convert.ToString(c2 % 60).PadLeft(2, '0');
                        }
                        catch { }

                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        public void send()
        {
            StringBuilder sb = new StringBuilder();

            //for (int i = 0; i < MainWindow.VSTUPY.Count; i++)
            //{
            //    sb.Append(MainWindow.VSTUPY[i]);
            //}
            serialPort.WriteLine(rxBuffer);
        }

        public void send(string command)
        {
            int checksum = 0;

            byte[] pole = Encoding.ASCII.GetBytes(command);
            //checksum calc
            for (int i = 0; i < pole.Length - 3; i++)
            {
                checksum += pole[i];
            }
            checksum &= 0xFF;
            checksum = (byte)(0 - checksum);

            pole[command.Length - 2] = (byte)checksum;
            serialPort.Write(pole, 0, command.Length);

        }

        public void TogglePort()
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
                //MessageBox.Show(e.Message);
                MessageBox.Show("chyba");
            }
        }

        public bool GetAck(string s, int timeout)
        {
            int cnt = 0;
            send(s);
            IsRxEvent = false;
            while (!IsRxEvent)
            {
                Thread.Sleep(1);
                if (++cnt > timeout) break;
            }

            //IsRxEvent = false;
            return (cnt > timeout) ? false : true;
        }
    }
}
