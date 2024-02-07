using A3TTRControl2;

using System.Drawing;

using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace PianoTiles.mod
{
    public class Game1 : A3GameModel
    {
        List<Target> gameTargets = new List<Target>();
        Target animationDisplay = new Target((0, 0), (7, 0), (1, 0), 7);
        // we are going to have to manage memory better, we can't just have ever growing list --> later
        bool isAnimationOn = true;
        int animationState = 0;
        bool levelUp = false;
        bool gameOver = false;
        int feedback = 7;
        /*TIME VARIABLES*/
        long times = 0;
            //Music Synchronisation Variables
        int offset = 0;
        static int bpm = 278;
        int targetSpeed = bpm;
            //Animation
        int keeptime = bpm;
        int fadetime = bpm;

        /*GAME PARAMETERS*/
        int points = 0;
        int lives = 10;

        int targetNum = 4;
        int level = 1;
        double speed_incr = 0.20;
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

            animationDisplay.on(false);
            a3ttrSoundlist.Add("BGM", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\demosong.wav"));
            a3ttrSoundlist.Add("levelUp", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\levelUp.wav"));
            a3ttrSoundlist.Add("feedback", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\feedback.wav"));
            a3ttrSoundlist["BGM"].Play();
            a3ttrSoundlist.Add("gameover", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\gameover.wav"));
            loadAnimation("levelUp", System.Environment.CurrentDirectory + "\\animation\\gradient2.ttr");

            loadAnimation("gameover", System.Environment.CurrentDirectory + "\\animation\\gameover.ttr");

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

            base.init();

        }
        /// <summary>
        /// 更新事件，mod逻辑处理
        /// </summary>
        /// <param name="time">距离上次更新的时间(毫秒)</param>
        public override void update(long time)
        {

            times += time;
            if (isAnimationOn & times >= bpm)//switch constraint to times%(speed*0.5)<(speed*0.5 -1)
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
                            setFadeLed(Color.White, 4, 4, keeptime * 3, fadetime*2 );
                            ++animationState;
                            animationDisplay.direction = (-1, 0);
                            animationDisplay.startPos = endPos;
                            animationDisplay.endPos = (0, 7);
                            break;
                        case 2:
                            setFadeLed(Color.White, 3, 4, keeptime * 3, fadetime*2) ;
                            ++animationState;
                            animationDisplay.direction = (0, -1);
                            animationDisplay.startPos = endPos;
                            animationDisplay.endPos = (0, 0);
                            break;
                        case 3:
                            setFadeLed(Color.White, 3, 3, keeptime * 3, fadetime*2);
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
                if (times >= offset)
                {
                    if (levelUp)
                    {   //CLEAR ANIMATION

                        /*RANDOMIZING TARGETS*/
                        for (int i = gameTargets.Count - 1; i >= 0; i--)
                        {
                            var k = random.Next(i + 1);
                            var value = gameTargets[k];
                            gameTargets[k] = gameTargets[i];
                            gameTargets[i] = value;
                        }
                        levelUp = false;

                        a3ttrSoundlist["BGM"].Play();
                    }
                    if (gameOver)
                    {


                        Environment.Exit(0);

                    }
                    offset = targetSpeed; // updating time condition so that target movement will hit their end position on beat
                    // SENDING A NEW TARGET IN A RANDOM 
                    // MOVE EACH TARGET TO THE NEXT POSITION
                    for (int i = 0; i < targetNum; i++)
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
            
            base.update(time);// instead of time  put 2
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
                        clearFadeLed(x, y);
                        clearLed(x, y);
                        setFadeLed(Color.Green, x, y, keeptime, fadetime);
                        points++;


                        /*TIMING ADJUSTMENTS
                        Ensures that the targets will always hits at the same time as the bpm of the song
                        Since the length of the trajectory is 4, all the time param need to be adjusted so that the last note is always in sync with music*/
                        offset = (int)(targetSpeed * speed_incr * 4);
                        targetSpeed = (bpm * 4 - offset) / 4; // by 4 because of their length
                        keeptime = targetSpeed;
                        fadetime = targetSpeed;

                        /*Slowly increasing speed*/
                        speed_incr *= 1.1;

                        Console.WriteLine("Your points are now: " + points);


                        if (!levelUp && points != 0 & points % 15 == 0)
                        {
                            if (points < gameTargets.Count())
                                targetNum += 2;
                            level += 1;
                            Console.WriteLine("level up " + level);
                            Console.WriteLine($"{points} POINTS!");
                            
                            offset += 10 * bpm; // little break
                            speed_incr = 0.20;
                            a3ttrSoundlist["BGM"].Stop();
                            a3ttrSoundlist["levelUp"].Play();
                            // StartAnimation("green", 1.5, 0.03); // Visual Feedback --> whole board pulsates
                            Console.WriteLine("BREAK");
                            StartAnimation("levelUp", 1, 1);

                            levelUp = true;

                        }

                    }


                }

                // ADDING EXTRA DIRECTIONS AS THE GAME GOES ON
                if (points != 0 & points % feedback == 0)
                {
                    feedback = random.Next(0, 2)==1 ? 7 : 9;
                    a3ttrSoundlist["feedback"].Play();

                }

            }
            else if (action == 2 && type == 1)
            {
                // WHEN USER LIFTS OFF BUTTON, BUTTON GOES BACK TO ORIGINAL COLOUR

                //清除按钮led灯光
            }

        }
        public void NextPos(Target target)
        {

            (int x, int y) currPos = target.currPos;
            (int x, int y) direction = target.direction;
            int distance = target.distance();

            switch (target.status)
            {   /*the other cases are just in case*/
                case "inactive":

                    if (distance != 0)
                    {
                        //ISSUE AT START OF THE GAME HERE
                        Color color = (distance == 1) ? Color.Aqua : Color.Magenta;
                        if (distance == 2) {
                            color = Color.BlueViolet;
                        }
                        setFadeLed(color, currPos.x, currPos.y, keeptime, fadetime*2);
                        target.currPos = (currPos.x + direction.x, currPos.y + direction.y); //moves target to next position
                                                                                             //
                    }
                    else
                    {   /* TARGET IS ACTIVATED */
                        //I CHANGED THIS SO IT FADES BECAUSE OTHERWISE THE 4 BUTTONS ARE LIT UP WHITE THE WHOLE TIME

                        setLed(Color.White, target.currPos.x, target.currPos.y);
                        //base.setFadeLed(Color.White, target.currPos.x, target.currPos.y, keeptime, fadetime);
                        target.status = "active"; // Activate it

                    }
                    break;

                case "active":
                    /* MISSED TARGET
                    When the target enters here it is because the user did not hit the target in time. Therefore the target status is set to "missed" and a visual feedback is ouputed*/
                    --Target.inactiveTargets;

                    target.status = "missed";
                    clearLed(target.endPos.x, target.endPos.y);
                    setFadeLed(Color.Red, target.endPos.x, target.endPos.y, keeptime, fadetime);
                    lives--;
                    Console.WriteLine("You now have " + lives + " lives!");
                    
                    if (lives < 1)
                    {
                        //ONCE THE USER GETS TO ZERO LIVES, THE GAME IS OVER
                        Console.WriteLine("GAME OVER :(");
                        Console.WriteLine("Your score is " + points);
                        a3ttrSoundlist["BGM"].Stop();
                        offset = 20 * bpm;
                        gameOver = true;
                        StartAnimation("gameover", 0.5, 1);
                        a3ttrSoundlist["gameover"].Play();

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