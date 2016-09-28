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
            /*   SerialPortManager sp = new SerialPortManager();
            sp.StartListening();
            long counter_prev = 0;


            while (!Console.KeyAvailable)
            {
                if (sp.receivedCounter != counter_prev)
                {
                    counter_prev = sp.receivedCounter;
                    Console.WriteLine(sp.getMessageAsString());
                }

           } 
            Console.ReadKey();
            Console.WriteLine("muhaha");


            Console.ReadKey();
            sp.StopListening();
            return 0;
            */


            SerialInterface serial = new SerialInterface();

            Console.ReadKey();
            serial.ClosePort();
        }
    }
}
