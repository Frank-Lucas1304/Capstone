using A3TTRControl;
using A3TTRControl2;
using Midi.Instruments;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static CXO2.Charting.Event;


namespace A3ttrEngine.mod
{
    /// <summary>
    /// 这是一个演示
    /// </summary>
    public class MusicMelody : A3GameModel
    {

        Target[,] buttonGrid = new Target[8,8];

        List<String> noteList = new List<String>() { "C3", " C3", " D3", " C3", " F3", " E3", "...", " C3", " C3", " D3", " C3", " F3", " E3", "...", " C3", " C3", " C2", " A3", " F3", " E3", " D3", " B3", " B3", " A3", " F3", " G3", " F3" };
        int note_pos = 0;
        int lives = 3;
        int level = 3;

        long quitDelay = 1000;
        bool quitGame = false;

        long times = 0;
        bool launchpadSetUp = true;
        public MusicMelody( )
        {

        }
        /// <summary>
        /// 初始化（提前加载资源）
        /// </summary>
        /// 
        
        public override void init()
        {
            base.Name = "MusicMelody";
            
           
            Console.WriteLine("In");
            a3ttrSoundlist.Add("GameOver", new A3ttrSound(System.Environment.CurrentDirectory + "\\sound\\GameOver.wav"));
            
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++) {
                    buttonGrid[x,y] = new Target((x, y));
                }
            }
            /*gameTargets.Add(new Target((5, 7), "A"));
            gameTargets.Add(new Target((0, 1), "A"));
            gameTargets.Add(new Target((1, 0), "A"));
            gameTargets.Add(new Target((2, 2), "A"));
            gameTargets.Add(new Target((2, 3), "A"));
            gameTargets.Add(new Target((3, 2), "A"));
            gameTargets.Add(new Target((4, 4), "A"));*/
            loadAnimation("gameover", System.Environment.CurrentDirectory + "\\animation\\gameover.ttr");
            //names of piano tile sounds correspond to their coordinates on the launchapd

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
            base.init();

        }
        /// <summary>
        /// 更新事件，mod逻辑处理
        /// </summary>
        /// <param name="time">距离上次更新的时间(毫秒)</param>
        /// 



        public override void update(long time)
        {

            if (quitGame)
            {
                if (times++ >= quitDelay) // Force quit delay as well as time increment
                {
                    Console.WriteLine("Exit", times);
                    Environment.Exit(0);
                }

            }
            else
            {
                if (launchpadSetUp)
                {
                    Target.launchpad = a3ttrPadCell; // to be able to update the board from the target instances
                    Target.a3ttrSoundlist = a3ttrSoundlist;
                    launchpadSetUp = false;
                }
                
                if (note_pos < level)
                {
                    (int x, int y) = KeyMapping(noteList[note_pos]);
                    buttonGrid[x, y].Animate(time, ref note_pos);

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

            if (action == 1 && type == 1)
            {
                (int x, int y) pos;
                if (note_pos>= level && note_pos < 2*level)
                {
                    pos = KeyMapping(noteList[note_pos]);
                    bool isTargetHit = buttonGrid[pos.x, pos.y].hit(x, y);

                    if (isTargetHit)
                    {   
                        Console.WriteLine(note_pos - level);
                        setLed(Color.Green, x, y);
                        
                        note_pos += 1;
                    }
                    else
                    {
                        setFadeLed(Color.Red, x, y,100,100); 
                        lives--;
                        if (lives ==0)
                            GameOver(); //Executes when liv
                        note_pos = 0; //Reshow sequence
                      

                    }
                    // Not all notes are there
                    try {
                        a3ttrSoundlist[$"{x}-{y}"].Play(); // to play correct and wrong note
                    }catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Check Keys, not all of them have sounds linked to them");
                    }
                }

                // Increase in sequence length
                if (note_pos == 2 * level)
                {
                    GameCompleted();
                    level += 1;
                    note_pos = 0;
                }

                ClearBoard();//NOT CODED YET --> Will clear entire board of color --> maybe add in the update function



            }
            else if (action == 2 && type == 1)
            {

            }
            base.input(action, type, x, y);
        }
        public void GameCompleted()
        {
            if (noteList.Count==level)
            {
                quitGame = true;
                Console.WriteLine("Sequence Completed");
            }
        }
        public void GameOver()
        {
            quitGame = true;
            StartAnimation("gameover", 1, 1);
            Console.WriteLine("Sequence length: " + (level-1));
            Console.WriteLine("Game Over");
        }
        public void ClearBoard() { 
            //Play an empty animation
        }

        static (int, int) KeyMapping(string key)
        {
            //length of string is
            int x = 0;
            int y = 0;
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
            Console.WriteLine("Half"+(x, y));
            return (x, y);
        }

    }
    class Target {

        public static A3ttrPadCell[,] launchpad;
        public static Dictionary<string, A3ttrSound> a3ttrSoundlist;

        /*Sequence Display
         pos 1 is gradient1 duration
         pos 2 is gradient2 duration
         pos 3 delay for next note
        */
        int[] timing = new int[3] {100, 200, 200};
        const int keeptime = 50;
        const int fadetime = 50;

        (int R, int G, int B) black = (0, 0, 0);
        (int R, int G, int B) purple = (50, 0, 50);
        (int R, int G, int B) white = (255,255,255);

        public long times { get; set; }
        public int status { get; set; }
        public int length { get; set; }
        public (int x, int y) pos { get; set; }

        public string key { get; set; }
        public (int R,int G, int B) currColor { get; set; }
        public (int R,int G, int B) init_color { get; set; }
        public (int R,int G, int B) gradColor { get; set; }
        public Target((int, int) pos)
        {
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
        public void Animate(long time,ref int note_pos) { //did you mean times?

            if (status <= 2)
            {
                gradient(timing[status] - times);
                setLed(Color.FromArgb(currColor.R, currColor.G, currColor.B), pos.x, pos.y);

            }

            if (times>= timing[status])
            {
                ++status;
                switch (status)
                {
                    case 1:
                        gradColor = white;
                        try{
                            // In case not located in list
                            a3ttrSoundlist[$"{pos.x}-{pos.y}"].Play();
                        }catch (Exception e) { }
                        break;
                    case 2:
                        gradColor = black;
                        //setFadeLed(Color.White, pos.x, pos.y, keeptime, fadetime);
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
        public void setFadeLed(Color c, int x, int y, int keeptime, int fadetime)
        {

            launchpad[x, y].fadeLedlist.Add(new A3ttrFadeled(fadetime, keeptime, c));
        }
        public void setLed(Color c, int x, int y)
        {

            launchpad[x, y].ledColor = c;
        }

        public void gradient(long timeleft)
        {   if (timeleft <= 0)
            {
                timeleft = 1;
            }
            (int R, int G, int B) c;
            c.R = (int)Math.Ceiling((decimal)(gradColor.R - currColor.R) / timeleft)+currColor.R;
            c.G = (int)Math.Ceiling((decimal)(gradColor.G - currColor.G) / timeleft)+currColor.G;
            c.B = (int)Math.Ceiling((decimal)(gradColor.B - currColor.B) / timeleft)+currColor.B;
            
            currColor = c;
        }
        public bool hit(int x, int y) {
            return ((pos.y == y) & (pos.x == x));


        }

    }
}

