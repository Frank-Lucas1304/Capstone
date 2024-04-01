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
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Games.mod
{
    /// <summary>
    /// 这是一个演示
    /// </summary>
    public class Test : A3GameModel
    {
        A3ttrGame currentGame;
        public int state = 0;
        public Test(A3ttrGame consoleGame)
        {
            currentGame = consoleGame;




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
            if (state == 1)
            {
                Console.WriteLine("IN");
            }
            else
            {
                if (state == 2)
                {
                    Console.WriteLine("OUT");
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
                Console.WriteLine(x + " " + y);
                if (x == 0 && y == 0)
                {
                    state = 1;
                    currentGame.changeGameModel(new MusicMelody(currentGame,0));
                }
                else
                {
                    currentGame.changeGameModel(new Game1(currentGame, 0));
                }

            }
            else if (action == 2 && type == 2)
            {


                //清除按钮led灯光
            }
            base.input(action, type, x, y);
        }
    }
}
