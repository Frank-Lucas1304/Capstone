using A3TTRControl2;
using Midi.Devices;
using OpenTK.Graphics.ES20;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PianoTiles.mod
{
    public class Fluidity : A3GameModel
    {

        long times = 0;// time vs TimeSpan
        List<Target> gameTargets = new List<Target>();

        bool once = true;
        private int state; //Sets value automatically to 0 if not assigned later in the code
        int red = 0x0;
        int blue = 0x0;
        int green = 0x0;
        public Fluidity()
        {


        }
        /// <summary>
        /// 初始化（提前加载资源）
        /// </summary>
        public override void init()
        {
            base.Name = "PianoTiles";
            a3ttrSoundlist.Add("BGM", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\demosong.wav"));
            base.init();
            gameTargets.Add(new Target((3, 3), (0, 0), (-1, -1), 3)); //A
                                                                      //usertime = new TimeSpan(0, 0, 0);
            a3ttrSoundlist["BGM"].Play();

        }
        /// <summary>
        /// 更新事件，mod逻辑处理
        /// </summary>
        /// <param name="time">距离上次更新的时间(毫秒)</param>
        public override void update(long time)
        {
            // Color Experiments and time variable conditions
            times += time;
            if (times%100<50)
            {

                if (red < 255)
                {
                    red += 1;
                }
                else
                {

                    if (green < 255)
                        green += 1;

                }



            }
            setLed(System.Drawing.Color.FromArgb(red, green, red), 1, 1);
            setLed(System.Drawing.Color.FromArgb(20, green, 20), 1, 2);
            setLed(System.Drawing.Color.FromArgb(20, green, 20), 1, 2);
            base.update(time);
            
        }



        /// <summary>
        /// Launchpad按压事件
        /// </summary>
        /// <param name="action">操作类型(2:up,1:down)</param>
        /// <param name="type">按键类型(1:key,2:cc)</param>
        /// <param name="x">按键x坐标</param>
        /// <param name="y">按键Y坐标</param>
        /// 
        public override void input(int action, int type, int x, int y)
        {

            if (action == 1 && type == 1)
            {

                Console.WriteLine(times);

            }
            else if (action == 2 && type == 1)
            {
                // WHEN USER LIFTS OFF BUTTON, BUTTON GOES BACK TO ORIGINAL COLOUR
                base.clearLed(x, y);
                //清除按钮led灯光
            }

        }
        public class Target
        {
            public static int colorSpeed = 0;

            public static int inactiveTargets = 0;
            public long ctime { get; set; }
            public long speed { get; set; } // speed and time window are the same
            public string status { get; set; }
            public int length { get; set; }
            public (int x, int y) endPos { get; set; }
            public (int x, int y) startPos { get; set; }
            public (int x, int y) currPos { get; set; }
            public (int x, int y) direction { get; set; }
            public Color color { get; set; }
            public void update(int times)
            {
                // This method will be responsible for updating the color of the led

                switch (status)
                {
                    case "inactive":
                        if (times % colorSpeed <= colorSpeed - 1)
                        {
                            color = Color.FromArgb(0, 0, 0);
                        }


                        if (times % speed <= speed - 1)
                        {

                        }


                        break;
                }
            }
            public int distance()
            {
                int xDiff = Math.Abs(currPos.x - endPos.x);
                int yDiff = Math.Abs(currPos.y - endPos.y);

                return xDiff > yDiff ? xDiff : yDiff;
            }
            public void on(bool state = true)
            {
                this.status = "inactive";
                this.currPos = this.startPos;
                if (state)
                    ++inactiveTargets;
            }
            public Target((int, int) startPos, (int, int) endPos, (int, int) direction, int length)
            {
                this.startPos = startPos;
                this.endPos = endPos;
                this.direction = direction;
                this.length = length;
                this.status = "missed";
            }
        }
    }


}



