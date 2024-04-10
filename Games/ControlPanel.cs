using A3TTRControl;
using A3TTRControl2;
using A3ttrEngine.mod;
using PianoTiles.mod;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ControlPanel
{
    public const int TotalNumberOfGame = 4;
    public static int ControlButtonID(int x)
    {
        return x - 49;
    }
    /* Method Does not work
    public static A3GameModel GameID(A3ttrGame consoleObj,int currGameID,int songID = 0)
    {
        switch(currGameID)
        {
            case 0:
                return new Game1();
            case 1:
                return new MusicMelody(consoleObj, songID);
            case 2:
                return new Drawing();
            case 3:
                return new PianoPlay();
            default:
                return null;
        }
    }*/


}