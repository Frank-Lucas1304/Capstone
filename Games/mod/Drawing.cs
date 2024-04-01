using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A3TTRControl2;
using Midi.Instruments;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Threading.Tasks;
using static Cgen.Logger;
using System.Net.NetworkInformation;
using A3TTRControl;
using static ControlPanel;


namespace A3ttrEngine.mod
{
    internal class Drawing : A3GameModel
    {
        List<(Color c, int x, int y)> gameColors = new List<(Color c, int x, int y)>();
        List<(int x, int y)> LitUpTiles = new List<(int x, int y)>();
        Color col = Color.Black;
        Boolean erase = false;

        // Control Panel Variables
        A3ttrGame consoleObj;

        public Drawing(A3ttrGame consoleObj)
        {
            this.consoleObj = consoleObj;

        }
        public override void init()
        {

            base.Name = "Drawing";
            base.init();

            base.setLed(Color.FromArgb(255, 0, 0), 0, 0);
            base.setLed(Color.FromArgb(255, 135, 0), 0, 1);
            base.setLed(Color.FromArgb(255, 255, 0), 0, 2);
            base.setLed(Color.FromArgb(0, 255, 0), 0, 3);
            base.setLed(Color.FromArgb(0, 255, 255), 0, 4);
            base.setLed(Color.FromArgb(0, 0, 255), 0, 5);
            base.setLed(Color.FromArgb(255, 0, 255), 0, 6);
            base.setLed(Color.FromArgb(255, 255, 255), 0, 7);
            //List<(Color c, int x,int y)> gameColors = new List<(Color c, int x, int y)>();
            gameColors.Add((Color.FromArgb(255, 0, 0), 0, 0));
            gameColors.Add((Color.FromArgb(255, 135, 0), 0, 1));
            gameColors.Add((Color.FromArgb(255, 255, 0), 0, 2));
            gameColors.Add((Color.FromArgb(0, 255, 0), 0, 3));
            gameColors.Add((Color.FromArgb(0, 255, 255), 0, 4));
            gameColors.Add((Color.FromArgb(0, 0, 255), 0, 5));
            gameColors.Add((Color.FromArgb(255, 0, 255), 0, 6));
            gameColors.Add((Color.FromArgb(255, 255, 255), 0, 7));

        }
        public override void update(long time)
        {
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
            if (x == 0)
            {
                foreach ((Color c, int x, int y) i in gameColors)
                {
                    if (i.x == x && i.y == y)
                    {
                        // if user selects new colour, variable col changes to that colour
                        col = i.c;
                    }
                }
            }

            if (action == 1 && type == 1 && x != 0)
            {
                //IF TILE ALREADY LIT UP, ERASE COLOUR
                foreach ((int x, int y) j in LitUpTiles)
                {
                    if (j.x == x && j.y == y)
                    {
                        erase = true;
                        base.clearLed(x, y);
                        LitUpTiles.Remove(j);
                        break;
                    }
                }
                //if tile is not already lit up, light up with selected colour
                if (!erase)
                {
                    //base.clearLed(x, y);
                    base.setLed(col, x, y);
                    LitUpTiles.Add((x, y));
                }
                erase = false;

                /**
                base.clearLed(x, y);
                base.setLed(col, x, y);
                LitUpTiles.Add((x, y));
                **/

            }
            else if (action == 1 && type == 2)
            {
                switch (ControlButtonID(x))
                {
                    case 0: //Scroll Up
                        {
                            // No Purpose Here
                        }
                        break;
                    case 1:
                        { //Scroll Down

                            // No Purpose Here

                        }
                        break;
                    case 2:
                        { // Previous Game
                            // No Purpose Here
                        }
                        break;
                    case 3:
                        { // Next
                            consoleObj.changeGameModel(new MusicMelody(consoleObj, 0)); // Default Song settings
                            
                        }
                        break;
                }
                base.input(action, type, x, y);
            }
        }
    }
}
