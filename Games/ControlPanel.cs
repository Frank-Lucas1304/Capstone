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
 

}