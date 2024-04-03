using A3TTRControl2;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Games.Scrap
{
    /// <summary>
    /// 这是一个演示
    /// </summary>
    public class DemoMod : A3GameModel
    {
        private System.Drawing.Color ledColor;
        public DemoMod(System.Drawing.Color ledcolor)
        {
            ledColor = ledcolor;




        }
        /// <summary>
        /// 初始化（提前加载资源）
        /// </summary>
        public override void init()
        {
            base.Name = "LED测试";
            base.init();
            /**
            System.Drawing.Color color1 = System.Drawing.Color.FromArgb(10, 0, 40);
            base.setLed(color1, 1, 1);
            System.Drawing.Color color2 = System.Drawing.Color.FromArgb(30, 0, 60);
            base.setLed(color2, 1, 2);
            System.Drawing.Color color3 = System.Drawing.Color.FromArgb(80, 0, 100);
            base.setLed(color3, 1, 3);
            System.Drawing.Color color4 = System.Drawing.Color.FromArgb(100, 100, 100);
            base.setLed(color4, 1, 4);
            **/


        }
        /// <summary>
        /// 更新事件，mod逻辑处理
        /// </summary>
        /// <param name="time">距离上次更新的时间(毫秒)</param>
        public override void update(long time)
        {
            //input(0, 1, 0, 0);
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
                //设置按钮led灯光
                //Console.WriteLine("press");
                //Console.WriteLine("Lmao");
                setLed(ledColor, x, y);

            }
            else if (action == 2 && type == 1)
            {
                setLed(System.Drawing.Color.RosyBrown, x, y);
                //Console.WriteLine("inside");


                //Console.WriteLine("release");
            }
            else if (action == 2 && type == 2)
            {
                Console.WriteLine($"{x},{y}");

                //清除按钮led灯光
            }
            Console.WriteLine($"{action}{type}{x}{y}");
            base.input(action, type, x, y);
        }
    }
}
