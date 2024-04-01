using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ControlPanel
{
    public static int ControlButtonID(int x)
    {
        return x - 49;
    }
    public static bool isScrollUp(int x, int y) {
        return (x == 49 && y == 57);
        }
    public static bool iScrollDownPressed(int x, int y) {
        return (x == 50 && y == 57);
    }
    public static bool PreviousPressed(int x, int y) {
        return (x == 51 && y == 57);
    }
    public static bool NextPressed(int x, int y) {
        return (x == 52 && y == 57);
    }
    public static bool SelectPressed(int x, int y) {
        return (x == 53 && y == 57);

    }
    public static bool PlayPausePressed(int x, int y) {

        return (x == 54 && y == 57);
    }
    public static bool HomePressed(int x, int y) {
        return (x == 55 && y == 57);

    }

    public static bool ReturnPressed(int x, int y) {

        return (x == 55 && y == 57);
    }


}