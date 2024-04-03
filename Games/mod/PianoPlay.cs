using A3TTRControl;
using A3TTRControl2;
using Games.mod;
using System.Drawing;
using static ControlPanel;


namespace A3ttrEngine.mod
{
    public class PianoPlay : A3GameModel
    {
        static int bpm = 278;
        int targetSpeed = bpm;
        //Animation
        int keeptime = bpm;
        int fadetime = bpm;
        List<Color> colors = new List<Color> { Color.Red, Color.White, Color.Violet, Color.Green, Color.Lavender };
        int pointer = 0;
        int maxPointer = 4;
        A3ttrGame consoleObj;
        bool setup = true;
        public PianoPlay(A3ttrGame consoleObj)
        {
            this.consoleObj = consoleObj;
        }
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
        public void colorLaunchpad()
        {
            setLed(Color.White, 0, 7);
            setLed(Color.White, 1, 7);
            setLed(Color.White, 2, 7);
            setLed(Color.White, 3, 7);
            setLed(Color.White, 4, 7);
            setLed(Color.White, 5, 7);
            setLed(Color.White, 6, 7);

            setLed(Color.BlueViolet, 1, 6);
            setLed(Color.BlueViolet, 2, 6);
            setLed(Color.BlueViolet, 4, 6);
            setLed(Color.BlueViolet, 5, 6);
            setLed(Color.BlueViolet, 6, 6);

            setLed(Color.White, 0, 5);
            setLed(Color.White, 1, 5);
            setLed(Color.White, 2, 5);
            setLed(Color.White, 3, 5);
            setLed(Color.White, 4, 5);
            setLed(Color.White, 5, 5);
            setLed(Color.White, 6, 5);

            setLed(Color.BlueViolet, 1, 4);
            setLed(Color.BlueViolet, 2, 4);
            setLed(Color.BlueViolet, 4, 4);
            setLed(Color.BlueViolet, 5, 4);
            setLed(Color.BlueViolet, 6, 4);

            setLed(Color.White, 0, 3);
            setLed(Color.White, 1, 3);
            setLed(Color.White, 2, 3);
            setLed(Color.White, 3, 3);
            setLed(Color.White, 4, 3);
            setLed(Color.White, 5, 3);
            setLed(Color.White, 6, 3);

            setLed(Color.BlueViolet, 1, 2);
            setLed(Color.BlueViolet, 2, 2);
            setLed(Color.BlueViolet, 4, 2);
            setLed(Color.BlueViolet, 5, 2);
            setLed(Color.BlueViolet, 6, 2);

            setLed(Color.White, 0, 1);
            setLed(Color.White, 1, 1);
            setLed(Color.White, 2, 1);
            setLed(Color.White, 3, 1);
            setLed(Color.White, 4, 1);
            setLed(Color.White, 5, 1);
            setLed(Color.White, 6, 1);

            setLed(Color.BlueViolet, 1, 0);
            setLed(Color.BlueViolet, 2, 0);
            setLed(Color.BlueViolet, 4, 0);
            setLed(Color.BlueViolet, 5, 0);
            setLed(Color.BlueViolet, 6, 0);

        }

        public override void init()
        {
            base.Name = "PianoPlay";


            loadPianoWaveFiles();

            base.init();

        }
        public override void update(long time)
        {
            if (setup)
            {
                Console.WriteLine("In");
                setup = false;
                colorLaunchpad();
            }

        }
        public override void input(int action, int type, int x, int y)
        {
            if (action == 1 && type == 1)
            {
                try
                {
                    a3ttrSoundlist[$"{x}-{y}"].Play();
                    
                }
                catch
                {

                }
            }
            else if (action == 1 && type == 2)
            {

                switch (ControlButtonID(x))
                {
                    case 0: //Scroll Up
                        {
                            // No purpose 
                        }
                        break;
                    case 1:
                        { // Scroll Down
                          // No purpose
                        }
                        break;
                    case 2:
                        { // Previous Game
                            a3ttranimationlist.Clear();
                            a3ttrSoundlist.Clear();
                            consoleObj.changeGameModel(new MusicMelody(consoleObj, 0));
                        }
                        break;
                    case 3:
                        { // Next
                          // No purpose
                        }
                        break;
                    case 4:
                        { //Select
                          // No purpose
                        }
                        break;
                    case 5:
                        { // Pause or Play
                          // No purpose
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

            base.input(action, type, x, y);
        }
    }
}