using A3TTRControl2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Games.Scrap
{
    public class A3moleGameMod : A3GameModel
    {
        public Color color;
        List<Mole> molelist = new List<Mole>();
        long times = 0;
        bool isstart = false;
        long nexttime = 0;
        Random r = new Random();
        public A3moleGameMod()
        {
            // color = c;
            //speed = s;
        }

        public override void init()
        {
            base.Name = "打地鼠游戏测试";

            Console.WriteLine();
            a3ttrSoundlist.Add("BGM", new A3ttrSound(Environment.CurrentDirectory + "\\sound\\bgm_test.wav"));
            a3ttrSoundlist.Add("cx", new A3ttrSound(Environment.CurrentDirectory + "\\sound\\test1.wav"));
            a3ttrSoundlist.Add("Click", new A3ttrSound(Environment.CurrentDirectory + "\\sound\\click.wav"));
            loadAnimation("red", Environment.CurrentDirectory + "\\animation\\gradient.ttr");
            loadAnimation("green", Environment.CurrentDirectory + "\\animation\\green.ttr");
            base.init();
            a3ttrSoundlist["BGM"].Play(false, true);
            nexttime += r.Next(300, 1000);

        }
        public override void update(long time)
        {
            times = times + time;
            if (times >= nexttime)
            {
                int keeptime = r.Next(1000, 3000);
                int fadetime = r.Next(500, 1000);
                int x = r.Next(0, 7);
                int y = r.Next(0, 7);
                Console.WriteLine(molelist.Count());
                if (molelist.Count(c => c.cleartime >= times && c.x == x && c.y == y) == 0)
                {
                    molelist.Add(new Mole(times, times + keeptime + fadetime, 0, x, y));
                    setFadeLed(Color.Yellow, x, y, keeptime, fadetime);
                    a3ttrSoundlist["cx"].Play(true);
                    nexttime += r.Next(200, 800);
                }

            }

            base.update(time);
        }
        public override void input(int action, int type, int x, int y)
        {
            if (action == 1 && type == 1)
            {

                if (molelist.Count(c => c.cleartime >= times && c.x == x && c.y == y) > 0) // checks if taget was hit
                {
                    // Positive Feedback Effects
                    a3ttrSoundlist["Click"].Play(true); // Audio feedback
                    StartAnimation("green", 1.5, 0.03); // Visual Feedback --> whole board pulsates
                    clearFadeLed(x, y); // Clears default target fadeout since target was hit before it dies out
                    setLed(Color.Green, x, y); // Changes target color to green --> positive visual feedback for correct input
                    BackMsgEvent("打中得分:+1"); // Sends message back saying to add + 1 to score
                }
                else
                {
                    // Negative Feedback Effects
                    StartAnimation("red", 1.5, 0.03);
                    setLed(Color.Red, x, y);
                    BackMsgEvent("打错得分:-1");
                }

            }
            else if (action == 2 && type == 1)
            {
                clearLed(x, y);
            }
            base.input(action, type, x, y);
        }
    }
    public class Mole
    {
        public long ctime { get; set; }
        public long cleartime { get; set; }
        public int point { get; set; }

        public int x { get; set; }
        public int y { get; set; }

        public Mole(long ctime, long cleartime, int point, int x, int y)
        {
            this.ctime = ctime;
            this.cleartime = cleartime;
            this.point = point;
            this.x = x;
            this.y = y;
        }
    }
    //Could first start with a for loop, or can just do like next
}
