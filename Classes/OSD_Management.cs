﻿using RTSSSharedMemoryNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Handheld_Control_Panel.Classes
{
    class OSD_Management
    {

        private static Thread osd;
        public static bool displayOSD = false;
        public static void startOSDThread()
        {
            displayOSD = true;
            Thread osd = new Thread(() => { test(); });
            osd.Start();
        }

        public static void stopOSDThread() { displayOSD = false; }

        private static List<AppFlags> appFlags = new List<AppFlags>()
        {
            {AppFlags.OpenGL },
            {AppFlags.Direct3D9Ex },
            {AppFlags.Direct3D9 },
            {AppFlags.Direct3D10 },
            {AppFlags.Direct3D11 },
            {AppFlags.Direct3D12 },
            {AppFlags.Direct3D12AFR },
            {AppFlags.OpenGL },
            {AppFlags.Vulkan }
        };

        private static void test2()
        {
            if (RTSS.RTSSRunning())
            {
                var OSD = new RTSSSharedMemoryNET.OSD("new");

                AppFlags appFlag = appFlags[0];
                var appEntries = OSD.GetAppEntries(appFlag); ;
                while (displayOSD)
                {

                    appEntries = OSD.GetAppEntries(appFlag);

                    while (appEntries.Length == 0)
                    {
                        foreach (AppFlags af in appFlags)
                        {
                            appEntries = OSD.GetAppEntries(af);
                            if (appEntries.Length > 0) { appFlag = af; break; }
                        }

                    }


                    foreach (var app in appEntries)
                    {
                        string[] osdArr = { app.InstantaneousFrameTime.ToString(), app.InstantaneousFrames.ToString() };
                        OSD.Update(("Time: " + DateTime.Now + " FPS: " + app.InstantaneousFrames.ToString()) + " Avg FPS: " + app.StatFramerateAvg + " min fps: " + app.StatFramerateMin);
                        Debug.WriteLine(app.Name);


                    }
                    Task.Delay(350);

                }
                OSD.Update("");
                OSD.Dispose();
                osd.Abort();
            }

        }
        private static void test()
        {
            if (RTSS.RTSSRunning())
            {
                var OSD = new RTSSSharedMemoryNET.OSD("new");

          
                while (displayOSD)
                {



                    string[] osdArr = AutoTDP_Management.writeGPUInfo();
                    foreach (string osd in osdArr)
                    {
                        OSD.Update(osd);
                    }
                    Task.Delay(350);

                }
                OSD.Update("");
                OSD.Dispose();
                osd.Abort();
            }

        }
    }
}
