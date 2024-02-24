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
        List<Test> gameTargets = new List<Test>();

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
            Test x = new Test();


            gameTargets.Add(x);

            base.Name = "PianoTiles";
           
            base.init();
            //gameTargets.Add(new Target((3, 3), (0, 0), (-1, -1), 3)); //A
                                                                      //usertime = new TimeSpan(0, 0, 0);


        }
        /// <summary>
        /// 更新事件，mod逻辑处理
        /// </summary>
        /// <param name="time">距离上次更新的时间(毫秒)</param>
        public override void update(long time)
        {
            int incr = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++) {
                    int val = 0 + incr;
                    
                    if (i < 4)
                    {
                        Color color1 = Color.FromArgb(252, 4, 4);
                        setLed(color1,i, j);
                    }
                    else
                    {
                        Color color1 = Color.FromArgb(255, 7, 4);
                        setLed(color1, i, j);
                    }
                    if (incr <= 255)
                    {
                        Color color1 = Color.FromArgb(val, 0, 0);
                        //setLed(color1, j, i);
                    }
                    else
                    {
                        Console.WriteLine($"{i} {j}");
                        break;
                    }
                    incr += 1;

                }
                incr += 1;

            }
            Test.launchpad = a3ttrPadCell;// got it to work
            // Color Experiments and time variable conditions


        /*
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
            gameTargets[0].setLed();
            setLed(System.Drawing.Color.FromArgb(red, green, red), 1, 1);
        */
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
            public void Brightness(int opacity)
            {
                
            }
        }
        class Test
        {
            public static A3ttrPadCell[,] launchpad;
            public Test()
            {

            }
            public void setLed()
            {
                launchpad[0, 1].ledColor = Color.FromArgb(255,255,255);
            }
        }
    }


}



