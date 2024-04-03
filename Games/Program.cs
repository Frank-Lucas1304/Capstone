using A3TTRControl;
using A3TTRControl2;
using A3TTRControl2.mod;
using A3ttrEngine.mod;
using Cgen.Audio;
using Games.mod;
using PianoTiles.mod;
using System.Drawing;
using System.IO.Ports;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
    internal class Program
    {
        static A3ttrGame a3ttrGame = new A3ttrGame();
        static SerialPort _serialPort = new SerialPort("COM4", 19200, Parity.None, 8, StopBits.One);
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
        static void Main(string[] args)
        {
            DisplayPorts();
            _serialPort.Open();
            //COM4

            //launchpad 连接状态
            a3ttrGame.a3ttr_ConnectChangedEvent += A3ttrGame_a3ttr_ConnectChangedEvent;
            
            var rollType = Console.ReadKey(); // I think its to exit

            if (true)
            {
             

            }
        }
        
        private static void A3ttrGame_a3ttr_ConnectChangedEvent(bool connected)
        {
            Console.WriteLine("连接状态:"+ connected);
            if (connected)
            {

                //进入DemoMod
                a3ttrGame.changeGameModel(new Menu(a3ttrGame,_serialPort));
               // a3ttrGame.changeGameModel(new MusicMelody(a3ttrGame,0));


                //a3ttrGame.changeGameModel(new DemoMod(Color.Aqua));
            }
        }
    }
}