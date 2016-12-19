using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace SerialInterface
{
    public class SerialPortInterface
    {
        private SerialPort serialPort;


        private byte[] data = new byte[8];
        private byte[] msg;

        public SerialPortInterface()
        {

            serialPort = new SerialPort();
            Console.Write("Serial port initializing .....");
            serialPort.PortName = FindPerfectSerialPort();
            serialPort.BaudRate = 9600;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            OpenPort();
            Console.Write("  [OK]  \n");
        }

        private string FindPerfectSerialPort()
        {
            string[] availablePorts = SerialPort.GetPortNames();
            Console.WriteLine("founded COM ports: ");
            foreach (string name in availablePorts)
            {
                Console.WriteLine(name);
            }
            Console.WriteLine("Selected port: " + availablePorts[1]);
            return availablePorts[1];
            
        }

        public void OpenPort()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                Console.WriteLine("Port is reopened");
            }
            serialPort.DataReceived += new SerialDataReceivedEventHandler(HandleIncomingData);
            serialPort.Open();
            serialPort.DtrEnable = true;
            Console.WriteLine("Port is open");

        }

        private void HandleIncomingData(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] bytes = new byte[8];
            serialPort.Read(bytes, 0, 8);
            //Console.WriteLine(serialPort.ReadLine());
            Console.WriteLine("read value: ");
            foreach(byte element in bytes)
            {
                Console.WriteLine(element);
            }
        }

        public void ClosePort()
        {
            serialPort.Close();
            Console.WriteLine("Port is closed");
        }

        public void WriteMsg(string msg)
        {
            serialPort.WriteLine(msg);
            
        }

        /// <summary>
        /// Method for sending byte-vector to the serial line. 
        /// </summary>
        private void WriteBytes()
        {
            serialPort.Write(data, 0, 8); //8, because this is the lenght of the waited signal in the arduino
        }

        /// <summary>
        /// Method for sending indicator before starting the streaming og RGB codes
        /// </summary>
        public void sendIndicatorOfRGBstream()
        {
            data[0] = 255;
            WriteBytes();
        }


        /// <summary>
        /// Method for encoding and sending RGB codes for two specified LED on the strip
        /// </summary>
        /// <param name="id1">first LED's ID</param>
        /// <param name="id2"></param>
        /// <param name="R1"></param>
        /// <param name="G1"></param>
        /// <param name="B1"></param>
        /// <param name="R2"></param>
        /// <param name="G2"></param>
        /// <param name="B2"></param>
        public void sendRGBcodes(byte id1, byte id2, byte R1, byte G1, byte B1, byte R2, byte G2, byte B2)
        {
            data[0] = id1;
            data[1] = id2;
            data[2] = R1;
            data[3] = G1;
            data[4] = B1;
            data[5] = R2;
            data[6] = G2;
            data[7] = B2;
            WriteBytes();
        }
    }
}
