﻿using A3TTRControl;
using A3TTRControl2;
using Midi.Devices;
using Midi.Enums;
using Midi.Messages;
using OpenTK.Graphics.ES20;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static A3TTRControl.a3Interface;

namespace PianoTiles.mod
{
    public class Fluidity : A3GameModel
    {
        LaunchpadDevice lol = new LaunchpadDevice("LPMiniMK3 MIDI 0", "MIDIIN2 (LPMiniMK3 MIDI) 1");
        long times = 0;// time vs TimeSpan
        bool once = true;
        private int state; //Sets value automatically to 0 if not assigned later in the code
        public Fluidity()
        {


        }
        /// <summary>
        /// 初始化（提前加载资源）
        /// </summary>
        public override void init()
        {
            base.Name = "PianoTiles";
            base.init();

            //usertime = new TimeSpan(0, 0, 0);






        }
        /// <summary>
        /// 更新事件，mod逻辑处理
        /// </summary>
        /// <param name="time">距离上次更新的时间(毫秒)</param>
        public override void update(long time)
        {

            Console.WriteLine();
            //usertime = usertime.Add(new TimeSpan(0, 0, 0, 0, (int)time));
            if (times == 0)
            {

                //Color colorP = a3ttrHelper.getColorP(a3ttrHelper.TransformHexToRGB(item.c.Remove(0, 1)), Color.Black, 1.0 - Opacity);
            }
            if (times >= 100 && once)
            {
                //Color test = Color.FromArgb((int)(0), 255, 0, 0);
                //setLed(test, 0, 1);
                once = false;
            }
            times += time;
            
            
            base.update(time);
        }

        static void move() { 
        
        
        
        }

        /// <summary>
        /// Launchpad按压事件
        /// </summary>
        /// <param name="action">操作类型(2:up,1:down)</param>
        /// <param name="type">按键类型(1:key,2:cc)</param>
        /// <param name="x">按键x坐标</param>
        /// <param name="y">按键Y坐标</param>
        /// 
        public override void input(int action, int type, int x, int y)
        {

            if (action == 1 && type == 1)
            {
               


            }
            else if (action == 2 && type == 1)
            {
                // WHEN USER LIFTS OFF BUTTON, BUTTON GOES BACK TO ORIGINAL COLOUR
                base.clearLed(x, y);
                //清除按钮led灯光
            }

        }





    }
}



