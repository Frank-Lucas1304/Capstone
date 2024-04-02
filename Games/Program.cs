using A3TTRControl;
using A3TTRControl2;
using A3TTRControl2.mod;
using A3ttrEngine.mod;
using Cgen.Audio;
using PianoTiles.mod;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
    internal class Program
    {
        static A3ttrGame a3ttrGame = new A3ttrGame();
        static void Main(string[] args)
        {

            
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
                a3ttrGame.changeGameModel(new PianoPlay(a3ttrGame));
               // a3ttrGame.changeGameModel(new MusicMelody(a3ttrGame,0));


                //a3ttrGame.changeGameModel(new DemoMod(Color.Aqua));
            }
        }
    }
}