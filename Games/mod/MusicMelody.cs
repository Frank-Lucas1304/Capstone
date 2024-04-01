using A3TTRControl2;
using NAudio.Wave;
using System.Drawing;
using NAudio.Wave.SampleProviders;

using static ControlPanel;
using System.Linq.Expressions;
using PianoTiles.mod;
using A3TTRControl;
using System.ComponentModel;
using OpenTK.Input;
using System.ComponentModel.Design;


namespace A3ttrEngine.mod
{
    /// <summary>
    /// 这是一个演示
    /// </summary>
    public class MusicMelody : A3GameModel
    {

        Target[,] buttonGrid = new Target[8, 8];
        Queue<Target> animatedButtons = new Queue<Target>();
        Queue<Circle> positiveFeedbackEffects = new Queue<Circle>();
        static string[] happy = new string[] { "C3", "F3", "F3", "F3", "C3", "C3", "F3", "C3", "F3" };

        //static string[] auClairDeLaLune = new string[] { };
        //static string[] baaBaaBlackSheep = new string[] { "C3", "C3", "G3", "G3", "A3", "A3", "A3", "A3", "G3" };
        static Partition happySong = new Partition(new string[] { "HC3", "HF3", "HF3", "QF3", "HC3", "QC3", "HF3", "HC3", "HF3" }, 156,6);
        static Partition happyBirthday = new Partition(new string[] { "EC3", "EC3", "QD3", "QC3", "QF3", "HE3", "EC3", "EC3", "QD3", "QC3", "QF3", "HE3", "C3", "C3", "C2", "A3", "F3", "E3", "D3", "B3", "B3", "A3", "F3", "G3", "F3", }, 156,6);
        //static List<Note> noteList = happySong.noteList;

        static List<Partition> songOptions = new List<Partition>() { happyBirthday, happySong };
        static List<Note> noteList = null;



        int note_pos = 0;

        bool isInvalidInput = false;
        (int x, int y) invalidPos;
        int lives = 3;
        int level = 6;

        (int R, int G, int B) black = (0, 0, 0);
        (int R, int G, int B) light = (255, 0, 255);
        (int R, int G, int B) mid = (120, 50, 120);
        (int R, int G, int B) neutral = (10, 10, 10);

        long betweenLevelDelay;
        long quitDelay = 1200;
        long countDownDelay = 4000;
        long pauseAnimationDelay = 3000;
        bool quitGame = false;
        bool pauseGame = false;
        bool countDownActivated = false;

        long times = 0;
        bool launchpadSetUp = true;


        // Control Panel Variables
        A3ttrGame consoleObj;

        int songID = 0;



        public MusicMelody(A3ttrGame consoleObj, int songID)
        {
            this.consoleObj = consoleObj;

            // default Settings
            this.songID = songID;
            noteList = songOptions[songID].noteList;
            level = songOptions[songID].initLevel;
                 
        }

        /// <summary>
        /// 初始化（提前加载资源）
        /// </summary>
        /// 
        public void loadPianoWaveFiles()
        {

            a3ttrSoundlist.Add("5-7", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - A.wav"));
            a3ttrSoundlist.Add("6-6", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - A#.wav"));
            a3ttrSoundlist.Add("6-7", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - B.wav"));
            a3ttrSoundlist.Add("0-7", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - C.wav"));
            a3ttrSoundlist.Add("1-6", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - C#.wav"));
            a3ttrSoundlist.Add("1-7", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - D.wav"));
            a3ttrSoundlist.Add("2-6", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - D#.wav"));
            a3ttrSoundlist.Add("2-7", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - E.wav"));
            a3ttrSoundlist.Add("3-7", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - F.wav"));
            a3ttrSoundlist.Add("4-6", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - F#.wav"));
            a3ttrSoundlist.Add("4-7", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - G.wav"));
            a3ttrSoundlist.Add("5-6", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\0 - G#.wav"));
            //2nd Octave
            a3ttrSoundlist.Add("5-5", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - A.wav"));
            a3ttrSoundlist.Add("6-4", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - A#.wav"));
            a3ttrSoundlist.Add("6-5", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - B.wav"));
            a3ttrSoundlist.Add("0-5", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - C.wav"));
            a3ttrSoundlist.Add("1-4", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - C#.wav"));
            a3ttrSoundlist.Add("1-5", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - D.wav"));
            a3ttrSoundlist.Add("2-4", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - D#.wav"));
            a3ttrSoundlist.Add("2-5", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - E.wav"));
            a3ttrSoundlist.Add("3-5", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - F.wav"));
            a3ttrSoundlist.Add("4-4", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - F#.wav"));
            a3ttrSoundlist.Add("4-5", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - G.wav"));
            a3ttrSoundlist.Add("5-4", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\1 - G#.wav"));
            //3rd octave
            a3ttrSoundlist.Add("5-3", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - A.wav"));
            a3ttrSoundlist.Add("6-2", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - A#.wav"));
            a3ttrSoundlist.Add("6-3", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - B.wav"));
            a3ttrSoundlist.Add("0-3", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - C.wav"));
            a3ttrSoundlist.Add("1-2", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - C#.wav"));
            a3ttrSoundlist.Add("1-3", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - D.wav"));
            a3ttrSoundlist.Add("2-2", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - D#.wav"));
            a3ttrSoundlist.Add("2-3", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - E.wav"));
            a3ttrSoundlist.Add("3-3", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - F.wav"));
            a3ttrSoundlist.Add("4-2", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - F#.wav"));
            a3ttrSoundlist.Add("4-3", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - G.wav"));
            a3ttrSoundlist.Add("5-2", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\2 - G#.wav"));
            //4th octave
            a3ttrSoundlist.Add("5-1", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - A.wav"));
            a3ttrSoundlist.Add("6-0", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - A#.wav"));
            a3ttrSoundlist.Add("6-1", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - B.wav"));
            a3ttrSoundlist.Add("0-1", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - C.wav"));
            a3ttrSoundlist.Add("1-0", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - C#.wav"));
            a3ttrSoundlist.Add("1-1", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - D.wav"));
            a3ttrSoundlist.Add("2-0", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - D#.wav"));
            a3ttrSoundlist.Add("2-1", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - E.wav"));
            a3ttrSoundlist.Add("3-1", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - F.wav"));
            a3ttrSoundlist.Add("4-0", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - F#.wav"));
            a3ttrSoundlist.Add("4-1", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - G.wav"));
            a3ttrSoundlist.Add("5-0", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\PianoTileSounds\\3 - G#.wav"));
        }
        public override void init()
        {
            base.Name = "MusicMelody";
            
            loadAnimation("pause", System.Environment.CurrentDirectory + "\\animation\\pause.ttr");
            
            loadAnimation("countDown", System.Environment.CurrentDirectory + "\\animation\\countDown.ttr");

            a3ttrSoundlist.Add("GameOver", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\GameOver.wav"));

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    buttonGrid[x, y] = new Target((x, y));
                }
            }
            loadAnimation("gameover", System.Environment.CurrentDirectory + "\\animation\\gameover.ttr");
            //names of piano tile sounds correspond to their coordinates on the launchapd
            loadPianoWaveFiles();

            base.init();

        }
        /// <summary>
        /// 更新事件，mod逻辑处理
        /// </summary>
        /// <param name="time">距离上次更新的时间(毫秒)</param>
        public override void update(long time)
        {
            if (launchpadSetUp)
            {
                Target.launchpad = a3ttrPadCell; // to be able to update the board from the target instances
                Target.a3ttrSoundlist = a3ttrSoundlist;
                launchpadSetUp = false;

            }
            if (noteList != null)
            {
                if (quitGame)
                {
                    times += time;
                    if (times++ >= quitDelay) // Force quit delay as well as time increment
                    {
                        Console.WriteLine("Exit", times);
                        Environment.Exit(0);
                    }
                }
                else
                {
                    if (!pauseGame && !countDownActivated)
                    {
                        if (note_pos < level & note_pos < noteList.Count)
                        {   // Display Sequence
                            times += time;
                            if (times >= betweenLevelDelay)
                            {
                                (int x, int y) = KeyMapping(noteList[note_pos].key);
                                buttonGrid[x, y].Display(time, ref note_pos, noteList[note_pos].duration);
                                betweenLevelDelay = 0;
                                times = 0;
                            }
                        }
                        else
                        {
                            //Optimisation: using Animation Curve/ function or different colors
                            (int R, int G, int B)[] color_list = new (int R, int G, int B)[5] { black, neutral, mid, light, black };
                            int[] timing = new int[5] { 0, 100, 100, 100, 100 };

                            // Displays all circle animations
                            foreach (Circle circle in positiveFeedbackEffects)
                            {
                                circle.Animate(animatedButtons, time, 60, buttonGrid, color_list, timing);
                            }
                            //Reducing size of queue if needed
                            if (positiveFeedbackEffects.Count > 0)
                            {
                                if (positiveFeedbackEffects.Peek().status == 1)
                                {
                                    positiveFeedbackEffects.Dequeue();
                                }
                            }
                            else
                            {
                                // Making sure all animation are done before moving on
                                if (animatedButtons.Count == 0)
                                { 
                                    if (note_pos == 2 * level)
                                    {
                                        int size = noteList.Count;
                                        if (size == level)
                                        {
                                            GameCompleted();
                                        }
                                        else
                                        {
                                            // Increasing level and displaying longer sequence
                                            level += level + 2 < size ? 2 : 1;
                                            note_pos = 0;
                                            times = 0;
                                        }
                                    }
                                    else
                                    {
                                        // Checking if invalid input
                                        if (isInvalidInput)
                                        {
                                            isInvalidInput = !isInvalidInput;
                                            if (lives == 0)
                                            {
                                                GameOver();
                                            }
                                            note_pos = 0;
                                            times = 0;
                                        }
                                    }

                                }
                            }

                            // Reducing Queue Size when required
                            if (animatedButtons.Count > 0)
                            {
                                if (animatedButtons.Peek().animation_sequence.Count == 0)
                                {
                                    animatedButtons.Dequeue();
                                }
                            }

                            foreach (Target button in animatedButtons)
                            {
                                button.AnimateTarget(time);
                            }

                            // Reduces Size of Queue as Animations are completed
                        }
                    }
                    else
                    {
                        if (countDownActivated)
                        {
                            times += time;
                            if (times >= countDownDelay)
                            {
                                countDownActivated = false;
                                times = 0;
                            }
                        }
                        else
                        {
                            times += time;
                            if (times >= pauseAnimationDelay)
                            {

                                StartAnimation("pause", 1, 1);
                                times = 0;
                            }
                        }
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
        public override void input(int action, int type, int x, int y)
        {
            //if game is paused we dont want to be able to register input
            if (action == 1 && type == 1 && !pauseGame && !countDownActivated)
            {
                (int x, int y) pos;
                if (note_pos >= level && note_pos < 2 * level)
                {
                    pos = KeyMapping(noteList[note_pos - level].key);
                    bool isTargetHit = buttonGrid[pos.x, pos.y].hit(x, y);
                    if (isTargetHit)
                    {
                        note_pos += 1;
                        if (note_pos == 2 * level)
                        {
                            positiveFeedbackEffects.Enqueue(new Circle(pos));
                        }
                        else
                        {
                            (int R, int G, int B)[] color_list = new (int R, int G, int B)[5] { black, neutral, mid, light, black };
                            int[] timing = new int[5] { 0, 100, 100, 100, 100 };
                            buttonGrid[x, y].animation_sequence.Enqueue(new Effect(color_list, timing));
                            //To avoid animating same item multiple times
                            if (!animatedButtons.Contains(buttonGrid[x, y]))
                                animatedButtons.Enqueue(buttonGrid[x, y]);
                        }

                    }
                    else
                    {
                        // Avoids incrementing lives and stalling game before displaying Game Over Sequence
                        if (!isInvalidInput)
                        {
                            (int R, int G, int B) red = (255, 0, 0);
                            (int R, int G, int B) black = (0, 0, 0);
                            (int R, int G, int B)[] color_list = { red, red, black };
                            int[] timing = new int[] { 0, 200, 100 };
                            buttonGrid[x, y].animation_sequence = new Queue<Effect>();
                            buttonGrid[x, y].animation_sequence.Enqueue(new Effect(color_list, timing));
                            if (!animatedButtons.Contains(buttonGrid[x, y]))
                                animatedButtons.Enqueue(buttonGrid[x, y]);
                            lives--;
                            isInvalidInput = true;
                        }
                    }
                    // Not all notes are there
                    try
                    {
                        a3ttrSoundlist[$"{x}-{y}"].Play(); // to play correct and wrong note
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Check Keys, not all of them have sounds linked to them");
                    }
                }
            }
            else if (action == 1 && type == 2)
            {
                if (!(note_pos >= level)) // display sequence was already completed before user tried to switch song
                {
                    Console.WriteLine("Switch activate");

                    // This code removes previous state of the last target activatio in the display sequence
                    (int x, int y) displayTarget = KeyMapping(noteList[note_pos].key); // last button in display sequence
                    buttonGrid[displayTarget.x, displayTarget.y].reset(); // resets last button into initial state
                    setLed(Color.Black, displayTarget.x, displayTarget.y); // reseting color of button 
                    note_pos = 0;

                }
                switch (ControlButtonID(x)) {
                    case 0: //Scroll Up
                        {
                            if (songID < songOptions.Count-1)
                            {
                                Console.WriteLine("Switch song +=1");
                                // Reseting Game Settings
                                songID += 1;
                                note_pos = 0;
                                times = 0;
                                level = songOptions[songID].initLevel;
                                noteList = songOptions[songID].noteList;
                            }

                        }
                        break;
                    case 1: { //Scroll Down

                            if (0 < songID)
                            {
                                Console.WriteLine("Switch song -=1");
                                // Reseting Game Settings
                                songID -= 1;
                                note_pos = 0;
                                times = 0;
                                level = songOptions[songID].initLevel;
                                noteList = songOptions[songID].noteList;
                            }

                        }
                        break; 
                    case 2: { // Previous Game

                            consoleObj.changeGameModel(new Drawing(consoleObj));
                        }
                        break;
                    case 3: { // Next
                                consoleObj.changeGameModel(new PianoPlay());
                        }
                        break;
                    case 4: { //Select
                            // No purpose
                        } 
                        break;
                    case 5: { // Pause or Play
                            pauseGame = !pauseGame;
                            if (pauseGame){
                                StartAnimation("pause",1,1);
                                times = 0;

                            }
                            else {
                                // reseting sequence
                                StartAnimation("countDown", 1, 1);
                                countDownActivated = true;
                                times = 0;

                            }
                        } 
                        break;
                    case 6: { 
                        } 
                        break;
                    case 7: { 
                        } break;
                
                }
            }
            base.input(action, type, x, y);
        }
        public void GameCompleted()
        {
            quitGame = true;
            Console.WriteLine("Sequence Completed");
        }
        public void GameOver()
        {
            quitGame = true;
            StartAnimation("gameover", 1, 1);
            Console.WriteLine("Animation Length");
            Console.WriteLine("Sequence length: " + (level - 1));
            Console.WriteLine("Game Over");
        }
        public void ClearBoard()
        {
            //Play an empty animation
        }

        static (int, int) KeyMapping(string key)
        {
            int x = 0, y = 0;

            //
            char letter = key[0];
            int octave = key[1] - 48;

            if (key.Length < 3)
            {

                y = 7 - octave * 2;

                switch (letter)
                {

                    case 'A':
                        x = 5;
                        break;
                    case 'B':
                        x = 6;
                        break;
                    case 'C':
                        x = 0;
                        break;
                    case 'D':
                        x = 1;
                        break;
                    case 'E':
                        x = 2;
                        break;
                    case 'F':
                        x = 3;
                        break;
                    case 'G':
                        x = 4;
                        break;
                }
            }
            else
            { // Sharp notes
                octave = key[2] - 48;

                y = 6 - octave * 2;
                switch (letter)
                {
                    case 'A':
                        x = 6;
                        break;
                    case 'C':
                        x = 1;
                        break;
                    case 'D':
                        x = 2;
                        break;
                    case 'F':
                        x = 4;
                        break;
                    case 'G':
                        x = 5;
                        break;
                }
            }
            return (x, y);
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

        (int R, int G, int B) black = (0, 0, 0);
        (int R, int G, int B) purple = (50, 0, 50);
        (int R, int G, int B) white = (255, 255, 255);

        public long times { get; set; }
        public int display_status { get; set; }
        //public int animation_status { get; set; }
        public int length { get; set; }
        public (int x, int y) pos { get; set; }
        public string key { get; set; }
        public (int R, int G, int B) currColor { get; set; }
        public (int R, int G, int B) gradColor { get; set; }

        public Queue<Effect> animation_sequence = new Queue<Effect>();

        public Target((int, int) pos, string key = null)
        {
            this.key = key;
            this.times = 0;
            this.pos = pos;
            //this.animation_status = 0;
            //INSERT AN ERROR IF KEY DOESNT EXIST

            //Starting effect
            reset();
        }
        public void reset()
        {
            this.currColor = (0, 0, 0);
            this.gradColor = purple;


            this.display_status = 0;
            this.gradient(timing[display_status]);
        }

        public void Display(long time, ref int note_pos,int duration)
        { //did you mean times
            if (timing[1]!=duration)
                { timing[1] = duration; }
            if (display_status <= 2)
            {
                gradient(timing[display_status] - times);
                setLed(Color.FromArgb(currColor.R, currColor.G, currColor.B));

            }

            if (times >= timing[display_status])
            {
                ++display_status;
                switch (display_status)
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

        public void AnimateTarget(long time)
        {
            int size = animation_sequence.Count();
            if (size > 0)
            {

                (int R, int G, int B) temp = (0, 0, 0);

                foreach (Effect effect in animation_sequence)
                {
                    if (effect.ChangeCurrColor(time))
                    {

                        temp.R += effect.currColor.R;
                        temp.G += effect.currColor.G;
                        temp.B += effect.currColor.B;
                    }
                }
                if (animation_sequence.Peek().color_sequence.Length == 0)
                {
                    animation_sequence.Dequeue();

                }
                currColor = (temp.R / size, temp.G / size, temp.B / size);

                setLed(Color.FromArgb(currColor.R, currColor.G, currColor.B));
            }
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
            // Need Math.Sign and Math.Abs because when Math.Ceiling curves negative values. Therefore when the difference is smaller than 0 it considers it as 0 and no change occurs. 
            c.R = Math.Sign(gradColor.R - currColor.R) * (int)Math.Ceiling((decimal)Math.Abs(gradColor.R - currColor.R) / timeleft) + currColor.R;
            c.G = Math.Sign(gradColor.G - currColor.G) * (int)Math.Ceiling((decimal)Math.Abs(gradColor.G - currColor.G) / timeleft) + currColor.G;
            c.B = Math.Sign(gradColor.B - currColor.B) * (int)Math.Ceiling((decimal)Math.Abs(gradColor.B - currColor.B) / timeleft) + currColor.B;


            //Console.WriteLine(currColor.ToString() + " " + gradColor.ToString() + " " + timeleft.ToString());
            currColor = c;
        }
        public bool hit(int x, int y)
        {
            return ((pos.y == y) & (pos.x == x));
        }

    }

    class Circle
    {
        public Random random_radii = new Random();
        public long times { get; set; }
        public long speed { get; set; }
        public int radius { get; set; }

        int max_radius { get; set; }
        public int status { get; set; }

        public (int x, int y) origin;

        public Circle((int x, int y) origin)
        {
            radius = 0;
            status = 0;
            max_radius = random_radii.Next(3, 4);
            this.origin = origin;
        }
        public void Animate(Queue<Target> animatedButtons, long time, int speed, Target[,] Grid, (int, int, int)[] color_list, int[] timing)
        {
            //Make this two for loops, implement it so that you store 
            if ((status != 1) && ((speed - times) >= 0))
            {
                if (radius != 0)
                {
                    int iterations = 360 / (45 / radius);
                    for (int i = 0; i < iterations + 1; i++)
                    {   //Angle tolerance was determined through testing 
                        for (int tolerance = -1; tolerance < 1; tolerance++)
                        {
                            /* Idea is that your taking the square that encapsulates the circle. The angle from one of its corner is to its origin is 45 degrees
                            You then divide it by the number of blocks making up its height*/
                            double angle = (45 / radius) * i - tolerance;
                            double rad = angle * Math.PI / 180;
                            // Finding x and y components relative to the origin
                            double dy = Math.Round(radius * Math.Sin(rad));
                            double dx = Math.Round(radius * Math.Cos(rad));
                            int x = origin.x + (int)dx;
                            int y = origin.y + (int)dy;

                            double err = Math.Pow(dx, 2) + Math.Pow(dy, 2) - Math.Pow(radius, 2);
                            int bound = radius - 1;
                            /*The bound condition was discovered through robust testing dont ask why it is like that it just works*/
                            if ((-bound - 1 < err) & (err <= bound) & 0 <= y & y < 8 & 0 <= x & x < 8)
                            {

                                Grid[x, y].animation_sequence.Enqueue(new Effect(color_list, timing));
                                //To avoid animating same item multiple times
                                if (!animatedButtons.Contains(Grid[x, y]))
                                    animatedButtons.Enqueue(Grid[x, y]);


                            }
                        }
                    }
                }
                else
                {
                    //Animation for pressed button
                    Grid[origin.x, origin.y].animation_sequence.Enqueue(new Effect(color_list, timing));
                    //To avoid animating same item multiple times
                    if (!animatedButtons.Contains(Grid[origin.x, origin.y]))
                        animatedButtons.Enqueue(Grid[origin.x, origin.y]);
                }

                times += time;
            }
            else
            {
                radius += 1;
                times = 0;
            }
            if (radius >= (max_radius * 2 + 1))
            {
                radius = 0;
                status = 1;
                times = 0;
            }

        }

    }


    class Effect
    {
        public long times;
        public int[] timing_sequence { get; set; }
        public int status { get; set; }
        public (int R, int G, int B)[] color_sequence { get; set; }
        public (int R, int G, int B) currColor { get; set; }
        public (int R, int G, int B) gradColor { get; set; }
        public Effect((int R, int G, int B)[] color_list, int[] timing_list)
        {
            color_sequence = color_list;
            timing_sequence = timing_list;
            status = 0;
            times = 0;
        }
        public bool ChangeCurrColor(long time)
        {

            if (status >= 0 & color_sequence.Length != 0)
            {
                if (status < timing_sequence.Length)
                {
                    gradient(timing_sequence[status] - times);

                }

                if (times >= timing_sequence[status])
                {
                    //DO I ALSO NEE TO MAKE SURE COLOR IS THE SAME?
                    ++status;

                    if (status < timing_sequence.Length)
                        gradColor = color_sequence[status];
                    else
                    {
                        // Resetting values

                        color_sequence = new (int R, int G, int B)[0];
                        timing_sequence = new int[0];
                        status = 0;
                        times = 0;
                        return false;
                    }
                    times = 0;

                }
                times += time;
            }
            // Need to check also color_sequence to see if currColor is a valid input
            return true;//Discard Effect
        }

        public void gradient(long timeleft)
        {
            if (timeleft <= 0)
            {
                timeleft = 1;
            }
            (int R, int G, int B) c;

            // Need Math.Sign and Math.Abs because when Math.Ceiling curves negative values. Therefore when the difference is smaller than 0 it considers it as 0 and no change occurs.
            c.R = Math.Sign(gradColor.R - currColor.R) * (int)Math.Ceiling((decimal)Math.Abs(gradColor.R - currColor.R) / timeleft) + currColor.R;
            c.G = Math.Sign(gradColor.G - currColor.G) * (int)Math.Ceiling((decimal)Math.Abs(gradColor.G - currColor.G) / timeleft) + currColor.G;
            c.B = Math.Sign(gradColor.B - currColor.B) * (int)Math.Ceiling((decimal)Math.Abs(gradColor.B - currColor.B) / timeleft) + currColor.B;

            currColor = c;

        }
    }

    class Partition
    {
        public int bpm { get; set; }
        public List<Note> noteList { get; set; }

        public int initLevel { get; set; }
        public Partition(string[] partition, int bpm, int initLevel)
        {
            // Initialising bpm for song --> this will determine display pace
            this.bpm = bpm;
            // This parameter will be used to implement starting difficulty
            this.initLevel = initLevel;

            noteList = new List<Note>();

            foreach (string name in partition)
            {
                // The type is to know the length of each note
                char type = name[0];
                string key = name.Substring(1, name.Length - 1);
                noteList.Add(new Note(type, key, bpm));

            }

            this.initLevel = initLevel;
        }
    }
    class Note
    {

        public int duration { get; set; }
        public string key { get; }
        public char type { get; }
        public Note(char type, string key, int bpm)
        {   
            this.type = type;
            this.key = key;
            float beatPerSecond = 60.0f / bpm;
            switch (type)
            {
                case 'H': // Half Note
                    duration  = (int)(beatPerSecond * 2 * 1000);


                    break;

                case 'Q': // Quarter Note
                    duration  = (int)(beatPerSecond * 1000);


                    break;

                case 'E': // 1/8th Note
                    duration = (int)(beatPerSecond * 0.5f*1000);
                    break;


            }
           
        }
    }
}


