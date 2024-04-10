using A3TTRControl;
using A3TTRControl2;
using A3TTRControl2.mod;
using A3ttrEngine.mod;
using Cgen.Audio;
using Games.mod;
using PianoTiles.mod;
using System.Drawing;
using System.Management;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design;

namespace ConsoleApp1
{
    internal class Program
    {
        static A3ttrGame a3ttrGame = new A3ttrGame();
        static SerialPort _serialPort;
      
        public static string[] getAvailablePorts()
        {
            List<string> portList = new List<string>();
           
            try { 
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
                {
                    Console.WriteLine("A list of the available COM ports:");
                    var portnames = SerialPort.GetPortNames();
                    var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());

                    portList = portnames.Select(n => n + " - " + ports.FirstOrDefault(s => s.Contains(n))).ToList();

                    foreach (string s in portList)
                    {
                        Console.WriteLine(s);
                    }
                    return portnames;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Capstone Project Team J LaunchPad Setup\n");
                Console.WriteLine("No COM ports are currently available.\n\t1. Please make sure your device is properly plugged in.\n\t2. Make sure no other program is currently using the port.\n");
                Console.WriteLine("When ready enter one of the following command in the command line:\n\t<update>: Update list of available ports.\n\t<exit>\t: Quit Program.");

                Console.Write("--------------------------------------------------------------------------------------\n");

                return null ;
            }

      
          
        }
        static void CommandList() {
            Console.WriteLine("List of possible commands with there description.\n\t<COM#>  : Connect to port COM#.\n\t<update>: Update list of available ports.\n\t<exit>  : Quit Program.");
            Console.Write("--------------------------------------------------------------------------------------\n");
        }

        static string[] refreshDisplay() {
            Console.Clear();
            string[] portsList = getAvailablePorts();
            if (portsList == null) {
                return null;
            }
            else
            {
                CommandList();
            }

            return portsList;
        }

        static void Main(string[] args)
        {
            string selectedPort = null;
            string inputCommand = null;
            string[] portsList = refreshDisplay();
            bool quitLoop = false;
            // Get short form PortNames: COM#
            do {
                Console.Write("Enter command: ");
                
                inputCommand = Console.ReadLine();

                if (inputCommand.ToLower() == "exit")
                {
                    Environment.Exit(0);
                } else if (portsList!=null && inputCommand.Length >= 4 && inputCommand.ToUpper().Substring(0, 3) == "COM")
                {
                    foreach (string portName in portsList)
                    {
                        if (portName == inputCommand.ToUpper())
                        {

                            _serialPort = new SerialPort(portName, 19200, Parity.None, 8, StopBits.One);

                            Console.WriteLine("Successfull Connection");
                            quitLoop = true;
                            break;

                        }
                    }
                    Console.WriteLine("Portname not in list try again.");
                }
                else if (inputCommand.ToLower() == "update")
                {
                    Console.WriteLine("\n\tUpdating prompt...");
                    Thread.Sleep(2000);
                    portsList=refreshDisplay();
                }
                else
                {
                    Console.WriteLine("Invalid Command Input, Try again.");
                }
            } while (!quitLoop);
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

            }
        }
    }
}