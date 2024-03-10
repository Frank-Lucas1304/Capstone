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
using Midi.Devices;
using Midi.Enums;
using Midi.Instruments;

namespace PianoTiles.mod
{
    public class Game1Hard : A3GameModel
    {
        List<Target> gameTargets = new List<Target>();
        static double[] targetProbability = new double[12] {0,0,0,0,0,0,0,0,0,0,0,0};
        Target animationDisplay = new Target((0, 0), (7, 0), (1, 0), 7);
        //Music Synchronisation Variables
        int offset = 0;
        static int bpm = 278;
        int targetSpeed = bpm;
        //Animation
        int keeptime = bpm;
        int fadetime = bpm;

        // we are going to have to manage memory better, we can't just have ever growing list --> later
        bool isAnimationOn = true;
        int animationState = 0;
        bool gameOver = false;
        int gameOverCount = 0;

        bool firstRun = true;
        Color ledColor = Color.Cyan; //Colour when you touch a key
        long times = 0;// time vs TimeSpan
        int speed = 380;
        //int keeptime = 1000;
        //int fadetime = 200;
        int points = 0;
        int lives = 3;
        //int level = 4;
        int speed_incr = 8;
        int maxTargetsAtTheTime = 1; //This variable sends x at the exact same time, not staggered
        int count = 0; //counter to keep track of when 2 targets should be sent out at the same time
        Random random = new Random();

        private int state; //Sets value automatically to 0 if not assigned later in the code

        //Nice gradient purple colours
        System.Drawing.Color color2 = System.Drawing.Color.FromArgb(200, 0, 200); //1
        System.Drawing.Color color1 = System.Drawing.Color.FromArgb(150, 0, 255); //2
        System.Drawing.Color color3 = System.Drawing.Color.FromArgb(50, 0, 255); //3
        public Game1Hard()
        {


        }
        /// <summary>
        /// 初始化（提前加载资源）
        /// </summary>
        public override void init()
        {
            base.Name = "PianoTiles";
            a3ttrSoundlist.Add("BGM", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\demosong.wav"));
            a3ttrSoundlist.Add("levelUp", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\levelUp.wav"));
            a3ttrSoundlist.Add("feedback", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\feedback.wav"));
            a3ttrSoundlist["BGM"].Play();
            a3ttrSoundlist.Add("gameover", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\gameover.wav"));
            a3ttrSoundlist.Add("Buzzer", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\WrongBuzzer.wav"));

            loadAnimation("levelUp", System.Environment.CurrentDirectory + "\\animation\\gradient2.ttr");

            loadAnimation("gameover", System.Environment.CurrentDirectory + "\\animation\\gameover.ttr");

            //Target.launchpad = a3ttrPadCell;

            base.init();
            animationDisplay.on(false);

            //LIGHT UP OUTSIDE PERIMETER
            /**
            Color outside_color = Color.Magenta;
            foreach (int value in Enumerable.Range(0, 7)) 
            {
                base.setLed(outside_color, 0, value);
                base.setLed(outside_color, 7, value);
                base.setLed(outside_color, value, 0);
                base.setLed(outside_color, value, 7);

            }**/
            
            //4 CORNERS CAN ONLY GO IN DIAGONAL DIRECTION
            
            gameTargets.Add(new Target((0, 0), (3, 3), (1, 1), 3)); //A
            gameTargets.Add(new Target((7,0), (4,3), (-1, 1), 3));  //D
            gameTargets.Add(new Target((7,7), (4,4), (-1, -1), 3));  //G
            gameTargets.Add(new Target((0,7), (3,4), (1, -1), 3));  //J

            //I FIND ITS TOO HARD WHEN THE DIRECTION IS DIFFERENT THAN 3, BUT COULD TRY OUT
            /**
            gameTargets.Add(new Target((0, 0), (4, 4), (1, 1), 4)); //A
            gameTargets.Add(new Target((7, 0), (3, 4), (-1, 1), 4));  //D
            gameTargets.Add(new Target((7, 7), (3, 3), (-1, -1), 4));  //G
            gameTargets.Add(new Target((0, 7), (4, 3), (1, -1), 4));  //J
            **/
            //STARTING ON LEFT SIDE
            foreach (int i in Enumerable.Range(1, 6)) {
                //horizontals
                gameTargets.Add(new Target((0, i), (3, i), (1, 0), 3));
                if (i < 4)
                {
                    //downward diagonal direction
                    gameTargets.Add(new Target((0, i), (3, i + 3), (1, 1), 3));
                }
                else 
                {
                    //upward diagonal direction
                    gameTargets.Add(new Target((0, i), (3, i - 3), (1, -1), 3));
                }
            }
            //STARTING ON RIGHT SIDE
            foreach (int i in Enumerable.Range(1, 6))
            {
                //horizontals
                gameTargets.Add(new Target((7, i), (4, i), (-1, 0), 3));
                if (i < 4)
                {
                    //downward diagonal direction
                    gameTargets.Add(new Target((7, i), (4, i + 3), (-1, 1), 3));
                }
                else
                {
                    //upward diagonal direction
                    gameTargets.Add(new Target((7, i), (4, i - 3), (-1, -1), 3));
                }
            }
            //STARTING ON TOP
            foreach (int i in Enumerable.Range(1, 6))
            {
                //verticals
                gameTargets.Add(new Target((i, 0), (i, 3), (0, 1), 3));
                if (i < 4)
                {
                    //right diagonal direction
                    gameTargets.Add(new Target((i, 0), (i + 3, 3), (1, 1), 3));
                }
                else
                {
                    //left diagonal direction
                    gameTargets.Add(new Target((i, 0), (i - 3, 3), (-1, 1), 3));
                }
            }
            //STARTING ON BOTTOM
            foreach (int i in Enumerable.Range(1, 6))
            {
                //verticals
                gameTargets.Add(new Target((i, 7), (i, 4), (0, -1), 3));
                if (i < 4)
                {
                    //right diagonal direction
                    gameTargets.Add(new Target((i, 7), (i + 3, 4), (1, -1), 3));
                }
                else
                {
                    //left diagonal direction
                    gameTargets.Add(new Target((i, 7), (i - 3, 4), (-1, -1), 3));
                }
            }



        }
        /// <summary>
        /// 更新事件，mod逻辑处理
        /// </summary>
        /// <param name="time">距离上次更新的时间(毫秒)</param>
        public override void update(long time)
        {
            Note a = new Note("A");
            a.PitchInOctave(1);
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
                    if (gameOver)
                    {
                        //needs to wait longer, so adding in a count
                        gameOverCount += 1;
                        if (gameOverCount > 8) {
                            Environment.Exit(0);
                        }
                        

                    }
                    // SENDING A NEW TARGET IN A RANDOM 
                    // MOVE EACH TARGET TO THE NEXT POSITION
                    if (gameOverCount == 0) {
                        for (int i = 0; i < gameTargets.Count; i++)
                        {

                            Target target = gameTargets.ElementAt(i);

                            if (target.status == "missed" | target.status == "hit")
                            {
                                if (random.Next(0, 2) == 1 && Target.inactiveTargets < maxTargetsAtTheTime)
                                {
                                    if (maxTargetsAtTheTime > 1)
                                    {
                                        while (animationDisplay.endPos == target.endPos)
                                        {
                                            Console.WriteLine("################################################################################");

                                            if (i < gameTargets.Count - 2)
                                            {
                                                target = gameTargets.ElementAt(i + 2);
                                            }
                                            else
                                            {
                                                target = gameTargets.ElementAt(i - 2);
                                            }

                                        }
                                    }
                                    target.on();

                                }
                            }
                            NextPos(target); // need to figure out when to add targets
                            /**
                            count++;
                            if(points > 5 && points <10) {
                                //2 directions every third time
                                if (maxTargetsAtTheTime == 1 && count > 1)
                                {
                                    maxTargetsAtTheTime = 2;
                                    count = 0;
                                }
                                else
                                {
                                    maxTargetsAtTheTime = 1;
                                }
                            }
                            if (points >= 10)
                            {
                                //2 directions every second time
                                if (maxTargetsAtTheTime == 1)
                                {
                                    maxTargetsAtTheTime = 2;
                                }
                                else
                                {
                                    maxTargetsAtTheTime = 1;
                                }
                            }**/


                        }
                        times = 0;
                    }
                    
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
                    //level += 2;
                    Console.WriteLine($"{points} POINTS!");
                    for (int i = gameTargets.Count - 1; i >= 0; i--)
                    {
                        var k = random.Next(i + 1);
                        var value = gameTargets[k];
                        gameTargets[k] = gameTargets[i];
                        gameTargets[i] = value;
                    }
                    a3ttrSoundlist["feedback"].Play();


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
                        if (distance == 3)
                        {
                            //SETTING SUPER FADED WHITE PATH SO THAT EACH SQUARE WILL DISAPPEAR BEFORE IT LIGHTS UP PURPLE
                            setFadeLed(Color.FromArgb(25, 25, 25), target.startPos.x + target.direction.x, target.startPos.y + target.direction.y, keeptime / 4, fadetime);
                            setFadeLed(Color.FromArgb(25, 25, 25), target.startPos.x + 2 * target.direction.x, target.startPos.y + 2 * target.direction.y, keeptime/2, fadetime);
                            setFadeLed(Color.FromArgb(25, 25, 25), target.endPos.x, target.endPos.y, keeptime, fadetime);
                        }


                        Color color = (distance == 1) ? color3 : color2;
                        if (distance == 2) {
                            color = color1;
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
                    a3ttrSoundlist["Buzzer"].Play();
                    target.status = "missed";
                    base.clearLed(target.endPos.x, target.endPos.y);
                    base.setFadeLed(Color.Red, target.endPos.x, target.endPos.y, keeptime, fadetime);
                    lives--;
                    Console.WriteLine("You now have " + lives + " lives!");
                    if (lives < 1)
                    {
                        //ONCE THE USER GETS TO ZERO LIVES, THE GAME IS OVER
                        a3ttrSoundlist["gameover"].Play();
                        Console.WriteLine("GAME OVER :(");
                        Console.WriteLine("Your score is " + points);
                        a3ttrSoundlist["BGM"].Stop();
                        offset = 20 * bpm;
                        gameOver = true;
                        StartAnimation("gameover", 0.5, 1);
                        //a3ttrSoundlist["gameover"].Play();
                        //Environment.Exit(0);
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