using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Reflection;
using System.ComponentModel;
using System.Threading;
using System.IO;

namespace SerialInterface
{
    public class SerialDataEventArgs : EventArgs
    {
        public byte[] Data;
        public SerialDataEventArgs(byte[] dataInByteArray)
        {
            Data = dataInByteArray;
        }
    }
    public class SerialPortManager : IDisposable
    {
        #region __init__
        private SerialPort _serialPort = new SerialPort("COM5", 9600);
        public SerialSettings _currentSerialSettings = new SerialSettings();
        private string _latestRecieved = String.Empty;
        public event EventHandler<SerialDataEventArgs> NewSerialDataRecieved;

        private byte[] data; //container of received bytes
        private byte[] msg; //container of received bytes without CR LF chars
        public long receivedCounter = 0;

        public SerialSettings CurrentSerialSettings
        {
            get { return _currentSerialSettings; }
            set { _currentSerialSettings = value; }
        }


        public SerialPortManager()
        {
            //searching initialised Serial Ports
            _currentSerialSettings.PortNameCollection = SerialPort.GetPortNames();
            Console.WriteLine("founded COM ports:");
            foreach (string name in _currentSerialSettings.PortNameCollection)
            {
                Console.WriteLine(name);
            }

            //set parameters

            _currentSerialSettings.PortName = _currentSerialSettings.PortNameCollection[1];
            _currentSerialSettings.BaudRate = 9600;
            _currentSerialSettings.DataBits = 8;
            _currentSerialSettings.Parity = Parity.None;
            _currentSerialSettings.StopBits = StopBits.One;

            _serialPort.PortName = _currentSerialSettings.PortName;

        }

        ~SerialPortManager()
        {
            Dispose(false);
        }

        #endregion
        #region Handlers
        /// <summary>
        /// handler of received messages from COM port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int dataLength = _serialPort.BytesToRead;
            data = new byte[dataLength];
            int nbrDataRead = _serialPort.Read(data, 0, dataLength);
            if (nbrDataRead == 0)
                return;
            if (NewSerialDataRecieved != null)
                NewSerialDataRecieved(this, new SerialDataEventArgs(data));

            receivedCounter++;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Starting listening thread and open port.
        ///  - If port is open, we close and reopen it. 
        ///  - Set handler of messages from COM port
        ///  - send reset command to Arduino after opening
        /// </summary>
        public bool StartListening()
        {
            if (_serialPort != null && _serialPort.IsOpen)
                _serialPort.Close();

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
            _serialPort.Open();
            _serialPort.DtrEnable = true;
            return (_serialPort != null && _serialPort.IsOpen);
            //Console.WriteLine("listening started");

        }

        /// <summary>
        /// Close the serial port
        /// </summary>
        public bool StopListening()
        {
            _serialPort.Close();
            return (_serialPort != null && !_serialPort.IsOpen);
            //Console.WriteLine("listening stopped");
        }



        /// <summary>
        /// Method for sending messages
        /// </summary>
        /// <param name="msg">string</param>
        public void SendMessage(string msg)
        {
            _serialPort.Write(msg);
        }

        public byte[] getMessageAsBytes()
        {
            msg = data;
            if (msg != null)
            {
                Array.Resize<byte>(ref msg, msg.Length - 4);
                return msg;
            }
            else {
                return new byte[] { 0 };
            }

        }

        public string getMessageAsString()
        {
            msg = data;
            if (msg != null)
            {
                Array.Resize<byte>(ref msg, msg.Length - 4);
                return Encoding.ASCII.GetString(msg);
            }
            else
                return "";
           //  Array.Resize<byte>(ref msg, msg.Length - 2);
            //return Encoding.ASCII.GetString(msg);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serialPort.DataReceived -= new SerialDataReceivedEventHandler(_serialPort_DataReceived);
            }
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();

                _serialPort.Dispose();
            }
        }
        /// <summary>
        /// function for update the BAUD rate collection
        /// </summary>
        private void UpdateBaudRateCollection()
        {
            _serialPort = new SerialPort(_currentSerialSettings.PortName);
            _serialPort.Open();
            object p = _serialPort.BaseStream.GetType().GetField("commProp",
                BindingFlags.Instance |
                BindingFlags.NonPublic).GetValue(_serialPort.BaseStream);
            Int32 dwSettableBaud = (Int32)p.GetType().GetField("dwSettableBaud",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(p);

            _serialPort.Close();
            _currentSerialSettings.UpdateBaudRateCollection(dwSettableBaud);

        }

        #endregion

    }
}
