﻿using RTSSSharedMemoryNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Handheld_Control_Panel.Classes
{
    public static class OSD_Management
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
            {AppFlags.Direct3D12 },
            {AppFlags.Direct3D12AFR },
            {AppFlags.Direct3D9Ex },
            {AppFlags.Direct3D9 },
            {AppFlags.Direct3D10 },
            {AppFlags.Direct3D11 },
            {AppFlags.Vulkan },
            {AppFlags.OpenGL }

        };

        private static void test2()
        {
            if (RTSS.RTSSRunning())
            {
                var OSD = new OSD("new");

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
        public static unsafe int closeGame()
        {
            int processID = 0;
            if (RTSS.RTSSRunning())
            {
  
                AppFlags appFlag = appFlags[0];
                AppEntry[] appEntries = OSD.GetAppEntries(appFlag);
                              

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
                    processID = app.ProcessId;
                 
                    System.Diagnostics.Process procs = null;

                    try
                    {
                        procs = Process.GetProcessById(processID);

                        

                        if (!procs.HasExited)
                        {
                            procs.CloseMainWindow();
                        }
                    }
                    finally
                    {
                        if (procs != null)
                        {
                            procs.Dispose();
                        }
                    }
                }

       

            }
            return processID;


        }
        public static unsafe string gameRunning()
        {
          
            string gameRunning = "";
            try
            {
      
                if (RTSS.RTSSRunning())
                {
           
                    AppFlags appFlag = appFlags[0];
                    AppEntry[] appEntries = OSD.GetAppEntries(appFlag);
                   

                    foreach (AppFlags af in appFlags)
                    {
                        appEntries = OSD.GetAppEntries(af);
                        if (appEntries.Length > 0) { appFlag = af; break; }
                    }
          
                    foreach (var app in appEntries)
                    {
              
                        string[] gamedir = app.Name.Split('\\');
                        if (gamedir.Length > 0)
                        {
                  
                            string currGameName = gamedir[gamedir.Length - 1];
                            gameRunning = currGameName.Substring(0, currGameName.Length - 4);
                 
                            break;
                        }

                    }
              
                }



               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log_Writer.writeLog(ex.Message, "OSDM01");
                return "";
            }
            return gameRunning;
        }
        public static unsafe int gameRunningProcessID()
        {

            int gameRunning = 0;
            try
            {

                if (RTSS.RTSSRunning())
                {

                    AppFlags appFlag = appFlags[0];
                    AppEntry[] appEntries = OSD.GetAppEntries(appFlag);

                    foreach (AppFlags af in appFlags)
                    {
                        appEntries = OSD.GetAppEntries(af);
                        if (appEntries.Length > 0) { appFlag = af; break; }
                    }

                    foreach (var app in appEntries)
                    {
                        gameRunning = app.ProcessId;
                        break;

                    }

                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log_Writer.writeLog(ex.Message, "OSDM01");
                return 0;
            }
            return gameRunning;
        }
        private static void test()
        {
            if (RTSS.RTSSRunning())
            {
                var OSD = new RTSSSharedMemoryNET.OSD("new");

          
                while (displayOSD)
                {



                    //string[] osdArr = AutoTDP_Management.writeGPUInfo();
                    string total = "";
                  
                    OSD.Update(total);
                    Task.Delay(350);

                }
                OSD.Update("");
                OSD.Dispose();
                osd.Abort();
            }

        }
    }
}
