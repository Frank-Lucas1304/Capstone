using A3TTRControl;
using A3TTRControl2;
using A3ttrEngine.mod;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using PianoTiles.mod;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ControlPanel;


namespace Games.mod
{
    /// <summary>
    /// 这是一个演示
    /// </summary>
    public class Menu : A3GameModel
    {
        A3ttrGame currentGame;
        public int state = 0;
        int consoleState = 0;
        int menuItem = 0;
        int menuID = 0;
        int gameID = 0;
        int songID = 0;
        SerialPort _serialport;
        public Menu(A3ttrGame consoleGame, SerialPort _serialport)
        {
            currentGame = consoleGame;
            this._serialport = _serialport;



        }
        /// <summary>
        /// 初始化（提前加载资源）
        /// </summary>
        public override void init()
        {
            base.Name = "Control";
            base.init();
        }
        /// <summary>
        /// 更新事件，mod逻辑处理
        /// </summary>
        /// <param name="time">距离上次更新的时间(毫秒)</param>
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
            if (action == 1 && type == 1)
            {

            }
            else if (action == 1 && type == 2)
            {
                switch (ControlButtonID(x))
                {
                    case 0:
                        _serialport.Write("1");
                        if (menuItem > 0)
                        {
                            menuItem -= 1;
                        }
                        break;
                    case 1:
                        _serialport.Write("2");
                        if (menuItem < 3)
                        {
                            
                            menuItem += 1;
                            
                        }
                        break;
                    case 4:
                        _serialport.Write("3");
                        if (menuID == 0)
                        {
                            gameID = menuItem;
                            if (menuItem != 0 || menuItem != 2)
                            {
                                Console.WriteLine(gameID);
                                if (gameID == 1)
                                {
                                    currentGame.changeGameModel(new Drawing(currentGame, _serialport));

                                }
                                if (gameID == 3)
                                {
                                    currentGame.changeGameModel(new PianoPlay(currentGame, _serialport));

                                }
                            }
                            menuID = 1;
                            menuItem = 0;
                        }
                        else
                        {
                            songID=menuItem;
                            if (gameID == 0)
                            {
                                currentGame.changeGameModel(new Game1(currentGame,songID, _serialport));

                            }
                            else
                            {
                                currentGame.changeGameModel(new MusicMelody(currentGame,songID, _serialport));

                            }
                        }
                        break;
                    case 6:
                        _serialport.Write("4");

                        break;
                    case 7:
                        _serialport.Write("4");

                        break;


                }

                //清除按钮led灯光
            }
            base.input(action, type, x, y);
        }
    }
}

