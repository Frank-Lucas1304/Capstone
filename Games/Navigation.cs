using A3TTRControl;
using A3TTRControl2;
using A3TTRControl2.mod;
using A3ttrEngine.mod;
using Cgen.Audio;
using PianoTiles.mod;
using System.Drawing;



//Communication with arduino
using System;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace ConsoleApp1
{
    internal class Navigation
    {

        //static A3ttrGame a3ttrGame = new A3ttrGame();
        static bool _continue;
        static SerialPort _serialPort;
        static string _serialPortName;


        public static void DisplayPorts()
        {
            string[] ports = SerialPort.GetPortNames();

            Console.WriteLine("The following serial ports were found:");

            // Display each port name to the console.
            foreach (string port in ports)
            {
                Console.WriteLine(port);
            }
        }
        public static void Main(string[] args)
        {
            DisplayPorts();




            //_serialPort = new SerialPort("COM", 9600);
            //_serialPort.Close();
            //a3ttrGame.a3ttr_ConnectChangedEvent += A3ttrGame_a3ttr_ConnectChangedEvent;
            string name;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            

        }

        
    }
    
}
