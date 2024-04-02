using A3TTRControl2;
using Midi.Instruments;
using System.Drawing;
using Midi.Instruments;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using A3TTRControl;
using A3ttrEngine.mod;
using System.Net.NetworkInformation;
using static ControlPanel;
using Games.mod;



namespace PianoTiles.mod
{
    public class Game1 : A3GameModel
    {
        string songSelect;
        string currentSong = "";

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
        static int bpm0 = 321;
        static int bpm = bpm0; //originally 278
        int targetSpeed = bpm;
            //Animation
        int keeptime = bpm + 10;
        int fadetime = bpm;

        /*GAME PARAMETERS*/
        int points = 0;
        int lives = 10;

        int targetNum = 4;
        int level = 1;
        double speed_incr = 0.06; //originally 0.06
        //int speed_incr = 8;
        const int maxTargetsAtTheTime = 1; //This variable sends x at the exact same time, not staggered
        Random random = new Random();
        (int,int) prevTargetStart = (100,100);
        (int, int) prevTargetEnd = (100, 100);

        private int state; //Sets value automatically to 0 if not assigned later in the code


        //TIMER FOR LEVEL UP ANIMATION
        int levelUpDuration = 1000; // 1 second in milliseconds
        int levelUpTimer = 0;

        //Nice gradient purple colours
        System.Drawing.Color color2 = System.Drawing.Color.FromArgb(200, 0, 200); //1
        System.Drawing.Color color1 = System.Drawing.Color.FromArgb(150, 0, 255); //2
        System.Drawing.Color color3 = System.Drawing.Color.FromArgb(50, 0, 255); //3

        int counter_target_press = 0;

        List<int> inactive_list = new List<int>(); // JUST FOR ME TO KEEP TRACK OF WHERE THINGS ARE GOING WRONG

        // Control Panel Variables
        A3ttrGame consoleObj;
        int songID;
        public Game1(A3ttrGame consoleObj, int songID)
        {
            this.consoleObj = consoleObj;
            this.songID = songID;
            songSelect = selectSong(songID);
        }
        public string selectSong(int songID)
        {
            switch (songID)
            {
                case 0:
                    return "BGM";
                case 1:
                    return "EDance";
                case 2:
                    return "StayinAlive";
                default:
                    return null;
                    

            }
        }
        /// <summary>
        /// 初始化（提前加载资源）
        /// </summary>
        public override void init()
        {
            base.Name = "PianoTiles";
            List<string> songList = new List<string>();
            songList.Add("BGM0"); // bpm 108
            songList.Add("BGM1"); // bpm 119
            songList.Add("BGM2"); // bpm 129
            songList.Add("BGM3"); // bpm 140
            songList.Add("BGM4"); // bpm 151
            songList.Add("BGM5"); // bpm 162
            //songList.Add("PianoSong");  //bpm 140
            //songList.Add("Pickles"); //bpm 150
            songList.Add("EDance0"); //bpm 123
            songList.Add("EDance1");
            songList.Add("EDance2");
            songList.Add("EDance3");
            songList.Add("EDance4");
            songList.Add("EDance5");

            songList.Add("StayinAlive0");
            songList.Add("StayinAlive1");
            songList.Add("StayinAlive2");
            songList.Add("StayinAlive3");
            songList.Add("StayinAlive4");
            songList.Add("StayinAlive5");

            int songToPlay = 0;
            string song = songList[songToPlay];
            song = songSelect + "0";
            if (songSelect == "StayinAlive") {
                bpm0 = 327;
                bpm = 327;
            }
            Console.WriteLine(songSelect);

            animationDisplay.on(false);// what is this??
            a3ttrSoundlist.Add("BGM0", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\demosong.wav"));
            a3ttrSoundlist.Add("BGM1", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\demosong1_1.wav"));
            a3ttrSoundlist.Add("BGM2", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\demosong1_2.wav"));
            a3ttrSoundlist.Add("BGM3", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\demosong1_3.wav"));
            a3ttrSoundlist.Add("BGM4", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\demosong1_4.wav"));
            a3ttrSoundlist.Add("BGM5", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\demosong1_5.wav"));
            //a3ttrSoundlist.Add("PianoSong", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoSong.wav"));
            a3ttrSoundlist.Add("EDance0", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\edance1_0.wav"));
            a3ttrSoundlist.Add("EDance1", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\edance1_1.wav"));
            a3ttrSoundlist.Add("EDance2", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\edance1_2.wav"));
            a3ttrSoundlist.Add("EDance3", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\edance1_3.wav"));
            a3ttrSoundlist.Add("EDance4", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\edance1_4.wav"));
            a3ttrSoundlist.Add("EDance5", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\edance1_5.wav"));

            a3ttrSoundlist.Add("StayinAlive0", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\stayinalive1_0.wav"));
            a3ttrSoundlist.Add("StayinAlive1", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\stayinalive1_1.wav"));
            a3ttrSoundlist.Add("StayinAlive2", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\stayinalive1_2.wav"));
            a3ttrSoundlist.Add("StayinAlive3", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\stayinalive1_3.wav"));
            a3ttrSoundlist.Add("StayinAlive4", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\stayinalive1_4.wav"));
            a3ttrSoundlist.Add("StayinAlive5", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\stayinalive1_5.wav"));

            a3ttrSoundlist.Add("levelUp", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\levelUp.wav"));
            a3ttrSoundlist.Add("feedback", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\feedback.wav"));
            a3ttrSoundlist[song].Play();
            a3ttrSoundlist.Add("gameover", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\gameover.wav"));
            a3ttrSoundlist.Add("Buzzer", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\WrongBuzzer.wav"));
            loadAnimation("levelUp", System.Environment.CurrentDirectory + "\\animation\\gradient2.ttr");

            loadAnimation("gameover", System.Environment.CurrentDirectory + "\\animation\\gameover.ttr");

            Target.launchpad = a3ttrPadCell;

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
                isAnimationOn = false;
            }
            else
            {              
                if (times >= offset)
                {
                    if (levelUp)
                    {
                        //CLEAR ANIMATION
                        for (int x = 0; x < 8; x++)
                        {
                            for (int y = 0; y < 8; y++)
                            {
                                base.clearLed(x, y);
                                base.clearFadeLed(x, y);
                            }
                        }


                        /*RANDOMIZING TARGETS*/
                        for (int i = gameTargets.Count - 1; i >= 0; i--)
                        {
                            Random random = new Random(Guid.NewGuid().GetHashCode());
                            var k = random.Next(i + 1);
                            var value = gameTargets[k];
                            gameTargets[k] = gameTargets[i];
                            gameTargets[i] = value;
                        }
                        levelUp = false;

                        //a3ttrSoundlist["BGM"].Play();
                    }
                    if (gameOver)
                    {

                        //Chord("G");
                        //NoteOnOffMessage(IDeviceBase device, Channel channel, Pitch pitch, int velocity, float time, Clock clock, float duration);
                        Console.WriteLine("GAME OVER :(");
                        Console.WriteLine("Your score is " + points);
                        Console.WriteLine(string.Join(", ", inactive_list));
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
                            Random random = new Random(Guid.NewGuid().GetHashCode());
                            if (random.Next(0, 2) == 1 && Target.inactiveTargets < maxTargetsAtTheTime) //&& Target.inactiveTargets < maxTargetsAtTheTime

                            {   if (!(target.startPos == prevTargetStart && target.endPos == prevTargetEnd)) 
                                {
                                    // if the new generated target is not the previous target
                                    // to avoid repition of the same target in a row

                                    target.on();
                                    inactive_list.Add(Target.inactiveTargets); // JUST FOR ME TO KEEP TRACK OF WHERE THINGS ARE GOING WRONG
                                    // updates previous target
                                    prevTargetStart = target.startPos;
                                    prevTargetEnd = target.endPos;
                                }
                                

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
                        //Target.inactiveTargets = 0;
                        inactive_list.Add(Target.inactiveTargets); // JUST FOR ME TO KEEP TRACK OF WHERE THINGS ARE GOING WRONG
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
                            // PLAY SPED UP VERSION OF SONG
                            int oldlevel = level - 2;
                            string oldsong = songSelect + oldlevel.ToString();
                            /**
                            if (level == 2)
                            {
                                oldsong = "BGM";
                            }**/
                            int newlevel = level - 1;
                            string newsong = songSelect + newlevel.ToString();
                            
                            if (level > 6) {
                                oldsong = songSelect+"5";
                                newsong = songSelect + "5";
                                
                            }
                            currentSong = newsong;

                            //display points
                            Console.WriteLine("level up " + level);
                            Console.WriteLine($"{points} POINTS!");
                            
                            offset += 10 * bpm; // little break
                            speed_incr = 0.20;
                            a3ttrSoundlist[oldsong].Pause();
                            //a3ttrSoundlist[song].Pause();
                            a3ttrSoundlist["levelUp"].Play();
                            //StartAnimation("green", 1.5, 0.03); // Visual Feedback --> whole board pulsates
                            Console.WriteLine("BREAK");
                            //StartAnimation("levelUp", 1, 1);
                            HappyFace(Color.Black, Color.Magenta);
                            a3ttrSoundlist[currentSong].Play();
                            bpm = bpm0 * (1 + (level - 1) / 10); //adjust bpm
                            targetSpeed = bpm;
                            speed_incr += 0.01;

                            levelUp = true;

                        }

                    }


                }

                // PLAYING FEEDBACK
                if (points != 0 & points % feedback == 0)
                {
                    feedback = random.Next(0, 2)==1 ? 7 : 9;
                    a3ttrSoundlist["feedback"].Play();

                }

            }
            else if (action == 1 && type == 2)
            {

                switch (ControlButtonID(x))
                {
                    case 0: //Scroll Up
                        {
                            if (songID < 2)
                            {
                                consoleObj.changeGameModel(new Game1(consoleObj, songID + 1));

                            }



                        }
                        break;
                    case 1:
                        { //Scroll Down

                            if (0 < songID)
                            {
                                consoleObj.changeGameModel(new Game1(consoleObj, songID - 1));

                            }
                        }
                        break;
                    case 2:
                        { // Previous Game

                            consoleObj.changeGameModel(new Drawing(consoleObj));
                        }
                        break;
                    case 3:
                        { // Next
                            consoleObj.changeGameModel(new PianoPlay(consoleObj));
                        }
                        break;
                    case 4:
                        { //Select
                            // No purpose
                        }
                        break;
                    case 5:
                        { // Pause or Play
                            /*
                            pauseGame = !pauseGame;
                            if (pauseGame)
                            {
                                StartAnimation("pause", 1, 1);
                                times = 0;

                            }
                            else
                            {
                                // reseting sequence
                                StartAnimation("countDown", 1, 1);
                                countDownActivated = true;
                                times = 0;

                            }*/
                        }
                        break;
                    case 6:
                        {
                            consoleObj.changeGameModel(new Menu(consoleObj));
                        }
                        break;
                    case 7:
                        {
                            consoleObj.changeGameModel(new Menu(consoleObj));
                        }
                        break;

                }
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
                        if (distance == 3) {
                            //SETTING SUPER FADED WHITE PATH SO THAT EACH SQUARE WILL DISAPPEAR BEFORE IT LIGHTS UP PURPLE
                            setFadeLed(Color.FromArgb(25, 25, 25), target.startPos.x + target.direction.x, target.startPos.y + target.direction.y, keeptime/2, fadetime);
                            setFadeLed(Color.FromArgb(25, 25, 25), target.startPos.x + 2 * target.direction.x, target.startPos.y + 2 * target.direction.y, keeptime, fadetime);
                            setFadeLed(Color.FromArgb(25, 25, 25), target.endPos.x, target.endPos.y, keeptime * 2, fadetime);
                        }
                        
                        //ISSUE AT START OF THE GAME HERE
                        Color color = (distance == 1) ? color3 : color2;
                        if (distance == 2) {
                            color = color1;
                            //setFadeLed(Color.FromArgb(10, 10, 10), target.startPos.x, target.startPos.y, keeptime, fadetime * 2);
                            //setFadeLed(Color.FromArgb(10, 10, 10), target.startPos.x + 2*target.direction.x, 2*target.startPos.y + target.direction.y, keeptime, fadetime * 2);
                            
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
                    if (counter_target_press > 0)
                    {
                        // THIS COUNTER GIVES USER EXTRA TIME TO PRESS BUTTON
                        /* MISSED TARGET
                        When the target enters here it is because the user did not hit the target in time. Therefore the target status is set to "missed" and a visual feedback is ouputed*/
                        --Target.inactiveTargets;
                        inactive_list.Add(Target.inactiveTargets); // JUST FOR ME TO KEEP TRACK OF WHERE THINGS ARE GOING WRONG
                        //Target.inactiveTargets = 0;
                        //Target.inactiveTarget = false;
                        a3ttrSoundlist["Buzzer"].Play();
                        target.status = "missed";
                        clearLed(target.endPos.x, target.endPos.y);
                        setFadeLed(Color.Red, target.endPos.x, target.endPos.y, keeptime, fadetime);
                        lives--;
                        Console.WriteLine("You now have " + lives + " lives!");
                        counter_target_press = 0;
                    }
                    else { 
                        counter_target_press += 1;
                    }
                    
                    
                    
                    if (lives < 1)
                    {
                        //ONCE THE USER GETS TO ZERO LIVES, THE GAME IS OVER
                        Console.WriteLine("GAME OVER :(");
                        Console.WriteLine("Your score is " + points);
                        a3ttrSoundlist[currentSong].Stop();
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
            public static A3ttrPadCell[,] launchpad;

            public static int colorSpeed = 0;

            public static int inactiveTargets = 0;
            public static bool inactiveTarget = false;
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
                status = "inactive";
                currPos = startPos;
                inactiveTarget = true;
                if (state)
                    ++inactiveTargets;
                    //inactiveTargets = 1;
            }
            public Target((int,int) startPos,(int, int) endPos, (int, int) direction, int length)
            {
                this.startPos = startPos;
                this.endPos = endPos;
                this.direction = direction;
                this.length = length;
                this.status = "missed";
            }
            public void setFadeLed(Color c, int x, int y, int keeptime, int fadetime) {

                launchpad[x, y].fadeLedlist.Add(new A3ttrFadeled(fadetime, keeptime, c));
            }
        }
        public void HappyFace(Color color1, Color color2) {
            Color[,] happyFace = {
            { color1, color1, color1, color1, color1, color1, color1, color1 },
            { color1, color1, color1, color1, color1, color2, color1, color1 },
            { color1, color2, color2, color1, color1, color1, color2, color1 },
            { color1, color1, color1, color1, color1, color1, color2, color1 },
            { color1, color1, color1, color1, color1, color1, color2, color1 },
            { color1, color2, color2, color1, color1, color1, color2, color1 },
            { color1, color1, color1, color1, color1, color2, color1, color1 },
            { color1, color1, color1, color1, color1, color1, color1, color1 }
            };
            // Set LEDs to form the happy face
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    base.setLed(happyFace[x, y], x, y);
                }
            }
        }
    }
}