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
        static bool terminateLoop = false;
        static A3ttrGame a3ttrGame = new A3ttrGame();
        static void Test(string[] args)
        {
            while (!terminateLoop)
            {
                //I dont get what this does if it returns void
                a3ttrGame.a3ttr_ConnectChangedEvent += A3ttrGame_a3ttr_ConnectChangedEvent;
                //Stall the program from terminating
                var command = Console.ReadLine();

                if (command == "exit")
                {
                    terminateLoop = true;
                }

            }

            Console.WriteLine("Quit Game");
        }

        private static void A3ttrGame_a3ttr_ConnectChangedEvent(bool connected)
        {
            Console.WriteLine("连接状态:"+ connected);
            if (connected)
            {

                //进入DemoMod
                a3ttrGame.changeGameModel(new MusicMelody());
                //a3ttrGame.changeGameModel(new DemoMod(Color.Aqua));
            }
        }
    }
}