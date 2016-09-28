using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace SerialInterface
{
    public class SerialInterface
    {
        private SerialPort serialPort;

        private byte[] data;
        private byte[] msg;

        public SerialInterface()
        {

            serialPort = new SerialPort();
            Console.Write("Serial port initializing .....");
            serialPort.PortName = FindPerfectSerialPort();
            serialPort.BaudRate = 115200;
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
            Console.WriteLine(serialPort.ReadLine());
        }

        public void ClosePort()
        {
            serialPort.Close();
            Console.WriteLine("Port is closed");
        }

        private void WriteMsg(string msg)
        {
            serialPort.WriteLine(msg);
        }
    }
}
