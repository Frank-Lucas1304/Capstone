using A3TTRControl;
using A3TTRControl2;
using Midi.Instruments;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Cgen.Logger;
using static CXO2.Charting.Event;

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
        public PianoPlay()
        {
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
            colorLaunchpad();
        }
        public override void input(int action, int type, int x, int y)
        {
            if (action == 1 && type == 1)
            {
                (int x, int y) pos;

                //light up the key that you just pressed 
                try
                {
                    a3ttrSoundlist[$"{x}-{y}"].Play(); // to play correct and wrong note
                    setFadeLed(Color.Green, x, y, keeptime, fadetime);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Check Keys, not all of them have sounds linked to them");
                }
            }
            else if (action == 2 && type == 1)
            {

            }
            base.input(action, type, x, y);

        }
    }
}