using A3TTRControl;
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
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PianoTiles.mod
{
    public class Fluidity : A3GameModel
    {         

        long times = 0;// time vs TimeSpan
        Target[,] buttonGrid = new Target[8, 8];

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

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    buttonGrid[x, y] = new Target((x, y));
                }
            }
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
        bool launchpadSetUp = true;
        public override void update(long time)
        {
            if (launchpadSetUp)
            {
                Target.launchpad = a3ttrPadCell; // to be able to update the board from the target instances
                Target.a3ttrSoundlist = a3ttrSoundlist;
                launchpadSetUp = false;
            }

            //Color Testing 
            /*
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
                    }
                    
                    if (val<=1)
                    {
                        num += 1;


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
                
            }*/
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

        public void CircleAnimation(int radius, (int x, int y) origin, (int x, int y) pos)
        {
            // Each Square is in contact with 8 other squares
            int deltaX = origin.x - pos.x;
            int deltaY = origin.y - pos.y;

            double err = Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2) - Math.Pow(radius,2);
            if (err < 0)
            {
                if (pos.y > 0 & pos.y <= origin.y)
                    CircleAnimation(radius, origin, (pos.x, pos.y - 1));
                if (pos.x > 0 & pos.x <= origin.x)
                    CircleAnimation(radius, origin, (pos.x - 1, pos.y));
                if (pos.y < 7 & pos.y >= origin.y)
                    CircleAnimation(radius, origin, (pos.x, pos.y + 1));
                if (pos.x < 7 & pos.x >= origin.x)
                    CircleAnimation(radius, origin, (pos.x + 1, pos.y));
                buttonGrid[pos.x, pos.y].setLed(Color.Red);

            }
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
                CircleAnimation(4, (2, 2), (2, 2));

            }
            else if (action == 2 && type == 1)
            {
                // WHEN USER LIFTS OFF BUTTON, BUTTON GOES BACK TO ORIGINAL COLOUR
                base.clearLed(x, y);
                //清除按钮led灯光
            }

        }

        class Target
        {
            public static int[] duration = new int[3] { 100, 200, 200 };
            public static A3ttrPadCell[,] launchpad;
            public static Dictionary<string, A3ttrSound> a3ttrSoundlist;

            /*Sequence Display
             pos 1 is gradient1 duration
             pos 2 is gradient2 duration
             pos 3 delay for next note
            */

            // Performing Shallow Copy
            int[] timing = (int[])duration.Clone();

            //int[] timing = new int[3] {100, 200, 200};
            const int keeptime = 50;
            const int fadetime = 50;

            (int R, int G, int B) black = (0, 0, 0);
            (int R, int G, int B) purple = (50, 0, 50);
            (int R, int G, int B) white = (255, 255, 255);

            public long times { get; set; }
            public int status { get; set; }
            public int length { get; set; }
            public (int x, int y) pos { get; set; }

            public string key { get; set; }
            public (int R, int G, int B) currColor { get; set; }
            public (int R, int G, int B) init_color { get; set; }
            public (int R, int G, int B) gradColor { get; set; }
            public Target((int, int) pos, string key = null)
            {
                this.key = key;
                this.times = 0;
                this.pos = pos;
                //INSERT AN ERROR IF KEY DOESNT EXIST

                //Starting effect
                reset();

            }
            public void reset()
            {
                this.currColor = (0, 0, 0);
                this.gradColor = purple;


                this.status = 0;
                this.gradient(timing[status]);
            }
            public void Animate(long time, ref int note_pos)
            { //did you mean times
                if (status <= 2)
                {
                    gradient(timing[status] - times);
                    setLed(Color.FromArgb(currColor.R, currColor.G, currColor.B));

                }

                if (times >= timing[status])
                {
                    ++status;
                    switch (status)
                    {
                        case 1:
                            gradColor = white;
                            try
                            {
                                // In case not located in list
                                a3ttrSoundlist[$"{pos.x}-{pos.y}"].Play();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        case 2:
                            gradColor = black;
                            break;
                        case 3:

                            Console.WriteLine("In");
                            note_pos += 1;
                            reset();
                            break;
                    }
                    times = 0;
                }
                times += time;
            }

            public void setFadeLed(Color c, int keeptime, int fadetime)
            {

                launchpad[pos.x, pos.y].fadeLedlist.Add(new A3ttrFadeled(fadetime, keeptime, c));
            }
            public void setLed(Color c)
            {

                launchpad[pos.x, pos.y].ledColor = c;
            }

            public void gradient(long timeleft)
            {
                if (timeleft <= 0)
                {
                    timeleft = 1;
                }
                (int R, int G, int B) c;
                c.R = (int)Math.Ceiling((decimal)(gradColor.R - currColor.R) / timeleft) + currColor.R;
                c.G = (int)Math.Ceiling((decimal)(gradColor.G - currColor.G) / timeleft) + currColor.G;
                c.B = (int)Math.Ceiling((decimal)(gradColor.B - currColor.B) / timeleft) + currColor.B;

                currColor = c;
            }

            public bool hit(int x, int y)
            {
                return ((pos.y == y) & (pos.x == x));
            }

            /*public void CircleAnimation(int radius)
            {
                CircleAnimation(radius, this.pos, this.pos);
            }
            /*public void CircleAnimation(int radius, (int x, int y) origin, (int x, int y) curr_pos) {

                int x = pos.x + 1;
                int y = pos.y + 1;
                bool inCircle = Math.Pow(origin.Item1)
                CircleAnimation(radius, pos,(x,y));

            }*/
            public void brightness(double opacity) {
                int R = (int)(currColor.R * opacity);
                int G = (int)(currColor.G * opacity);
                int B = (int)(currColor.B * opacity);
                currColor = (R, G, B);
                
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



