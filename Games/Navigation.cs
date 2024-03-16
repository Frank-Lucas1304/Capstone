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

namespace ConsoleApp1
{
    internal class Navigation
    {

        static A3ttrGame a3ttrGame = new A3ttrGame();
        static bool _continue;
        static SerialPort _serialPort;


        public static void Main(string[] args)
        {
            a3ttrGame.a3ttr_ConnectChangedEvent += A3ttrGame_a3ttr_ConnectChangedEvent;
            string name;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
           
        }

        private static void A3ttrGame_a3ttr_ConnectChangedEvent(bool connected)
        {
            Console.WriteLine("连接状态:" + connected);
            if (connected)
            {

                //进入DemoMod
                a3ttrGame.changeGameModel(new MusicMelody());
                //a3ttrGame.changeGameModel(new DemoMod(Color.Aqua));
            }
        }
    }
    
}
