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
using Newtonsoft.Json.Linq;

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
        public static void lol(string[] args)
        {
            DisplayPorts();
            // Need to make a command class just in case thing change and I could easily update format without worrying too much
            Console.WriteLine(extractAllData(0b0000100101000000, new byte[4]{2,1,6,6}));



            //_serialPort = new SerialPort("COM", 9600);
            //_serialPort.Close();
            //a3ttrGame.a3ttr_ConnectChangedEvent += A3ttrGame_a3ttr_ConnectChangedEvent;
            string name;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;


        }
        public static byte extractData(uint command,int commandMask,byte shiftData)
        {
            return (byte)((command & commandMask)>>shiftData);
        }
        public static byte[] extractAllData(uint command, byte[] commandFormat)
        {
            
            if (commandFormat.Length == 4)
            {
                byte[] answer = new byte[8];
                int shift = 0;
                uint zero = 0;
                byte temp = (byte)(~zero);

                byte _parityShift, _stateShift, _modeShift, _optionShift;
                _parityShift = _stateShift = _modeShift = _optionShift = 0;

                int _parityMask, _stateMask, _modeMask, _optionMask, _musicMask;
                _stateMask = _modeMask = _optionMask = _musicMask = 0;

                for (int i = commandFormat.Length - 1; 0 <= i; i--)
                {
                    shift += commandFormat[i];
                    switch (i)
                    {
                        
                        case 0:
                            _parityShift = (byte)(shift);
                            break;
                        case 1:
                            _stateShift = (byte)shift;
                            break;
                        case 2:
                            _modeShift = (byte)shift;
                            break;
                        case 3:
                            _optionShift = (byte)shift;
                            break;

                    }
                }
                _parityMask = (temp >> 7) << _parityShift;
                _stateMask = (temp >> (8 - commandFormat[0])) << _stateShift;
                _modeMask = (temp >> (8 - commandFormat[1])) << _modeShift;
                _optionMask = (temp >> (8 - commandFormat[2])) << _optionShift;
                _musicMask = temp >> (8 - commandFormat[3]);

                /*
                //need to check if it works
                Console.WriteLine(_parityShift);
                Console.WriteLine(_stateShift);
                Console.WriteLine(_modeShift);
                Console.WriteLine(_optionShift);

                // Are Mask Accurate?
                Console.WriteLine(Convert.ToString(_parityMask, 2));
                Console.WriteLine("0"+Convert.ToString(_stateMask, 2));
                Console.WriteLine("000"+Convert.ToString(_modeMask, 2));
                Console.WriteLine("0000"+Convert.ToString(_optionMask, 2));
                Console.WriteLine("0000000000"+Convert.ToString(_musicMask, 2));
                */


                answer[0]=extractData(command, _stateMask, _stateShift);
                answer[1]=extractData(command, _modeMask, _modeShift);
                answer[2]=extractData(command, _optionMask, _optionShift);
                answer[3]=extractData(command, _musicMask, 0);

                return answer;
            }
            else
            {
                throw new Exception("commandFormat is currently 4! Update it if necessary or else adjust code to make the length of commandFormat 4");
            }

        }

    } 
    
}
