using A3TTRControl2;
using Midi.Devices;
using OpenTK.Graphics.ES20;
using OpenTK.Input;
using OpenTK.Platform.Windows;
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


            base.Name = "PianoTiles";
           
            base.init();
            //gameTargets.Add(new Target((3, 3), (0, 0), (-1, -1), 3)); //A
                                                                      //usertime = new TimeSpan(0, 0, 0);


        }
        /// <summary>
        /// 更新事件，mod逻辑处理
        /// </summary>
        /// <param name="time">距离上次更新的时间(毫秒)</param>
        double incr = 0;
        int num = 0;
        public override void update(long time)
        {
           
            //Color Testing 
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++) {
                    double val = 0 + incr;
                    /*if (i < 4)
                    {
                        Color color1 = Color.FromArgb(252, 4, 4);
                        setLed(color1,i, j);
                    }
                    else
                    {
                        Color color1 = Color.FromArgb(255, 7, 4);
                        setLed(color1, i, j);
                    }*/
                    
                    if (val<=1)
                    {
                        num += 1;
                        int R = Target.Gamma(252, val);
                        int G = Target.Gamma(4, val);
                        int B = Target.Gamma(4, val);
                        Console.WriteLine($"{num}: ({R},{G},{B}), {val}, {i},{j}");
                        Console.WriteLine(val);
                        Color color1 = Color.FromArgb(R, G, B);
                        setLed(color1, j, i);
                    }
                    else
                    {
                        if (val <= 2)
                        {
                            int R = (int)(252 * (val - 1.0));
                            int G = (int)(4 * (val - 1.0));
                            int B = (int)(4 * (val - 1.0));
                            Console.WriteLine($"{num}: ({R},{G},{B}), {val}, {i},{j}");
                            Color color1 = Color.FromArgb(R, G, B);
                            setLed(color1, j, i);
                        }
                        else
                            break;

                    }
                    incr += 2.0 / 64;
                    

                }
                //incr += 1.0/64;

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
            public static int Brightness(int pigment,double opacity)
            {    
                return (int)(opacity * pigment);

            }
            public void CircleAnimation()
            {

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
            public void brightness(int opacity) {
                /*
                number of brightness levels = 252/4 = 63
                this function find the min and max pigment values and defines its brightness from there
                */
                /*int R = color.R / 4;
                int G = color.G / 4;
                int B = color.B / 4;
                int[] values = new int[3]{R, G, B};

                int min = 260;
                int max = 0;
                foreach (int pigment in values)
                {
                    if (max<pigment)
                        max = pigment;
                    if (min>pigment & min!=0) 
                        min = pigment;
                }
                if (min == 260)
                    min = max;
                
                /* 
                lowest brightness before completely dark would be pigment of 4 and max 63
                Total number of levels = min-4 + 63-max.
                The minimum has to be a non zero value */
                //NORMALIZED VALUES
                int R = color.R / 255;
                int G = color.G / 255;
                int B = color.B / 255;
                Math.Pow(R, 2.2);
                Math.Pow(R, 2.2);
                Math.Pow(R, 2.2);
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



