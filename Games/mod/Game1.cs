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
    public class Game1 : A3GameModel
    {
        List<Target> gameTargets = new List<Target>();
        static double[] targetProbability = new double[12] {0,0,0,0,0,0,0,0,0,0,0,0};
        Target animationDisplay = new Target((0, 0), (7, 0), (1, 0), 7);
        // we are going to have to manage memory better, we can't just have ever growing list --> later
        bool isAnimationOn = true;
        int animationState = 0;

        bool firstRun = true;
        Color ledColor = Color.Cyan; //Colour when you touch a key
        long times = 0;// time vs TimeSpan
        int speed = 380;
        int keeptime = 1000;
        int fadetime = 200;
        int points = 0;
        int lives = 3;
        int level = 4;
        int speed_incr = 10;
        const int maxTargetsAtTheTime = 1; //This variable sends x at the exact same time, not staggered
        Random random = new Random();

        private int state; //Sets value automatically to 0 if not assigned later in the code
        public Game1()
        {


        }
        /// <summary>
        /// 初始化（提前加载资源）
        /// </summary>
        public override void init()
        {
            base.Name = "PianoTiles";
            base.init();
            animationDisplay.on(false);

            gameTargets.Add(new Target((3, 3), (0, 0), (-1, -1), 3)); //A
            gameTargets.Add(new Target((4, 3), (7, 0), (1, -1), 3));  //D
            gameTargets.Add(new Target((4, 4), (7, 7), (1, 1), 3));  //G
            gameTargets.Add(new Target((3, 4), (0, 7), (-1, 1), 3));  //J

            gameTargets.Add(new Target((3, 3), (3, 0), (0, -1), 3));  //B
            gameTargets.Add(new Target((4, 3), (7, 3), (1, 0), 3));  //E
            gameTargets.Add(new Target((4, 4), (4, 7), (0, 1), 3));  //H
            gameTargets.Add(new Target((3, 3), (0, 3), (-1, 0), 3));  //L

            gameTargets.Add(new Target((4, 3), (4, 0), (0, -1), 3));  //C
            gameTargets.Add(new Target((4, 4), (7, 4), (1, 0), 3));  //F
            gameTargets.Add(new Target((3, 4), (3, 7), (0, 1), 3));  //I
            gameTargets.Add(new Target((3, 4), (0, 4), (-1, 0), 3));  //K


        }
        /// <summary>
        /// 更新事件，mod逻辑处理
        /// </summary>
        /// <param name="time">距离上次更新的时间(毫秒)</param>
        public override void update(long time)
        {
            //usertime = usertime.Add(new TimeSpan(0, 0, 0, 0, (int)time));

            times += time;
            if (isAnimationOn & times >= speed*0.5)//switch constraint to times%(speed*0.5)<(speed*0.5 -1)
            {
                (int x, int y) = animationDisplay.currPos;
                (int x, int y) endPos = animationDisplay.endPos;
                setFadeLed(Color.White, x, y, keeptime * 3, fadetime*2);// Overides all colors from the target functions
                if (x == endPos.x & y == endPos.y)
                {   switch(animationState) {
                        case 0:
                            setFadeLed(Color.White,4,3,keeptime * 3, fadetime*2);   
                            ++animationState;
                            animationDisplay.direction = (0, 1);
                            animationDisplay.startPos = endPos;
                            animationDisplay.endPos = (7, 7);
                            break;
                        case 1:
                            setFadeLed(Color.White, 4, 4, keeptime * 3, fadetime * 2);
                            ++animationState;
                            animationDisplay.direction = (-1, 0);
                            animationDisplay.startPos = endPos;
                            animationDisplay.endPos = (0, 7);
                            break;
                        case 2:
                            setFadeLed(Color.White, 3, 4, keeptime * 3, fadetime * 2);
                            ++animationState;
                            animationDisplay.direction = (0, -1);
                            animationDisplay.startPos = endPos;
                            animationDisplay.endPos = (0, 0);
                            break;
                        case 3:
                            setFadeLed(Color.White, 3, 3, keeptime * 3, fadetime * 2);
                            animationState = 0;
                            animationDisplay.direction = (1,0);
                            animationDisplay.startPos = endPos;
                            animationDisplay.endPos = (7, 0);
                            break;
                    }
                }
                NextPos(animationDisplay);
                times = 0;
            }
            else
            { 
                if (times >= speed)
                {
                    // SENDING A NEW TARGET IN A RANDOM 
                    // MOVE EACH TARGET TO THE NEXT POSITION
                    for (int i = 0; i < level; i++)
                    {

                        Target target = gameTargets.ElementAt(i);

                        if (target.status == "missed" | target.status == "hit")
                        {   
                            if (random.Next(0, 2) == 1 && Target.inactiveTargets < maxTargetsAtTheTime)
                            {   
                                target.on();

                            }
                        }
                        NextPos(target); // need to figure out when to add targets
                   
                    }
                    times = 0;
                }
            }
            
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
                if (isAnimationOn)
                {
                    isAnimationOn = false;
                    times = 0;
                }
                   
                // CHECK IF BUTTON PRESSED IS A TARGET BUTTON
                foreach (Target target in gameTargets)
                {
                    if (x == target.endPos.x && y == target.endPos.y && target.status == "active")
                    {
                        target.status = "hit";
                        --Target.inactiveTargets;

                        base.clearLed(x, y);
                        setFadeLed(Color.Green, x, y, keeptime, fadetime);
                        points++;
                        speed = speed - speed_incr;
                        keeptime = keeptime - speed_incr;
                        fadetime = fadetime - speed_incr;
                        Console.WriteLine("Your points are now: " + points);

                    }


                }

                // ADDING EXTRA DIRECTIONS AS THE GAME GOES ON
                if (points < gameTargets.Count() & points != 0 & points % 5 == 0)
                {
                    level += 2;
                    Console.WriteLine($"{points} POINTS!");
                    for (int i = gameTargets.Count - 1; i >= 0; i--)
                    {
                        var k = random.Next(i + 1);
                        var value = gameTargets[k];
                        gameTargets[k] = gameTargets[i];
                        gameTargets[i] = value;
                    }


                }



            }
            else if (action == 2 && type == 1)
            {
                // WHEN USER LIFTS OFF BUTTON, BUTTON GOES BACK TO ORIGINAL COLOUR
                base.clearLed(x, y);
                //清除按钮led灯光
            }

        }
       
        public void NextPos(Target target)
        {

            (int x, int y) currPos = target.currPos;
            (int x, int y) direction = target.direction;
            Console.WriteLine(direction);
            int distance = target.distance();

            switch (target.status)
            {   /*the other cases are just in case*/
                case "inactive":

                    if (distance != 0)
                    {
                        

                        Color color = (distance == 1) ? Color.Aqua : Color.Magenta;
                        if (distance == 2) {
                            color = Color.BlueViolet;
                        }
                        base.setFadeLed(color, currPos.x, currPos.y, keeptime, fadetime);
                        target.currPos = (currPos.x + direction.x, currPos.y + direction.y); //moves target to next position
                                                                                             //
                    }
                    else
                    {   /* TARGET IS ACTIVATED */
                        //I CHANGED THIS SO IT FADES BECAUSE OTHERWISE THE 4 BUTTONS ARE LIT UP WHITE THE WHOLE TIME
                        Console.WriteLine(target.currPos);
                        base.setLed(Color.White, target.currPos.x, target.currPos.y);
                        //base.setFadeLed(Color.White, target.currPos.x, target.currPos.y, keeptime, fadetime);
                        target.status = "active"; // Activate it

                    }
                    break;

                case "active":
                    /* MISSED TARGET
                    When the target enters here it is because the user did not hit the target in time. Therefore the target status is set to "missed" and a visual feedback is ouputed*/
                    --Target.inactiveTargets;

                    target.status = "missed";
                    base.clearLed(target.endPos.x, target.endPos.y);
                    base.setFadeLed(Color.Red, target.endPos.x, target.endPos.y, keeptime, fadetime);
                    lives--;
                    Console.WriteLine("You now have " + lives + " lives!");
                    if (lives < 1)
                    {
                        //ONCE THE USER GETS TO ZERO LIVES, THE GAME IS OVER
                        Console.WriteLine("GAME OVER :(");
                        Console.WriteLine("Your score is " + points);
                        Environment.Exit(0);
                    }


                    break;
                default:
                    /*Targets that are still in gameTargets list but out of use --> status is either "missed" or "hit" */
                    break;
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
                if (times % colorSpeed < colorSpeed - 1)
                {   
                    
                }
                
            }
            public int distance()
            {
                int xDiff = Math.Abs(currPos.x - endPos.x);
                int yDiff = Math.Abs(currPos.y - endPos.y);
                return xDiff>yDiff?xDiff:yDiff ;
            }
            public void on(bool state = true)
            {   
                this.status = "inactive";
                this.currPos = this.startPos;
                if (state)
                    ++inactiveTargets;
            }
            public Target((int,int) startPos,(int, int) endPos, (int, int) direction, int length)
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