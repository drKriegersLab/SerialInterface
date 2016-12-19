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
    class Program
    {
        
        public static void Main()
        {

            SerialPortInterface serial = new SerialPortInterface();

            Console.ReadKey();

            // send signal that indicate the stream of strip RGB codes is begining
            serial.sendIndicatorOfRGBstream();

            // send RGB codes' stream

            // first 10 is red
            for (byte i = 0; i < 10; i += 2)
            {
                serial.sendRGBcodes(i, Convert.ToByte(i + 1), 50, 0, 0, 50, 0, 0);
            }
            
            // second 10 is green
            for (byte i = 10; i< 20; i += 2)
            { 
                serial.sendRGBcodes(i, Convert.ToByte(i + 1), 0, 50, 0, 0, 50, 0);
            }

            // third 10 is blue
            for (byte i = 20; i < 30; i += 2)
            {
                serial.sendRGBcodes(i, Convert.ToByte(i + 1), 0, 0, 50, 0, 0, 50);
            }

            
            Console.ReadKey();
            serial.ClosePort();
        }
    }
}
