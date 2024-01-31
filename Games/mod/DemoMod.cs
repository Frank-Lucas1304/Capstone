using A3TTRControl2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace A3ttrEngine.mod
{
    /// <summary>
    /// 这是一个演示
    /// </summary>
    public class DemoMod:A3GameModel
    {
       private Color ledColor;
        public DemoMod(Color ledcolor)
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
            if (action==1&&type==1)
            {
                //设置按钮led灯光

                base.setLed(ledColor, x, y);
               
            }
            else if (action == 2 && type == 1)
            {
                //清除按钮led灯光
                base.clearLed(x, y);
            }
            base.input(action, type, x, y);
        }
    }
}
