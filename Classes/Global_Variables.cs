﻿using Handheld_Control_Panel.Classes.Controller_Management;
using Nefarius.Drivers.HidHide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using GameLib;
using GameLib.Core;

namespace Handheld_Control_Panel.Classes.Global_Variables
{
    public static class Global_Variables
    {
       public static MainWindow mainWindow = null;

        public static string xmlFile = AppDomain.CurrentDomain.BaseDirectory + "Resources\\Profiles.xml";

        //Processor global
        public static string cpuType = "";
        public static string MCHBAR = "";
        public static string processorName = "";

        //SETTINGS
        public static Settings settings;

        //TDP global
        #region TDP

        public static bool autoTDP = false;
        public static double ReadPL1
        {
            get
            {
               return readPL1 ;
            } 
            set
            {
                readPL1 = value;
                raiseValueChanged("TDP1");
            }
        }
        public static double ReadPL2
        {
            get
            {
                return readPL2;
            }
            set
            {
                readPL2 = value;
                raiseValueChanged("TDP2");
            }
        }

        public static double readPL1 = 0;
        public static double readPL2 = 0;
        public static double SetPL1 = 0;
        public static double SetPL2 = 0;



        #endregion
        //AMD GPU CLOCK
        public static int gpuclk = 0;

        public static int GPUCLK
        {
            set
            {
                gpuclk = value;
                raiseValueChanged("GPUCLK");
            }
            get
            {
                return gpuclk;
            }
        }

        //Shut down boolean to stop threads
        public static bool useRoutineThread = true;


      
        #region brightness
        public static int Brightness 
        {
            get
            {
                return brightness;
            }

            set
            {
                brightness = value;
                raiseValueChanged("Brightness");
            }
        }
    

        public static int brightness { get; set; } = -1;
        #endregion

        #region volume
        public static int Volume
        {
            get
            {
                return volume;
            }

            set
            {
                volume = value;
                raiseValueChanged("Volume"); 
            }
        }
        

        public static int volume { get; set; } = 0;
        #endregion

        #region mutevolume
        public static bool Mute
        {
            get
            {
                return mute;
            }

            set
            {
                mute = value;
                raiseValueChanged("VolumeMute");
            }
        }
      
        public static bool mute { get; set; } = false;
        #endregion


        //hidhide
        public static HidHideControlService hidHide;

        //mouse mode
        public static bool mouseModeEnabled = false;
        public static bool MouseModeEnabled
        {
            get
            {
                return mouseModeEnabled;
            }
            set
            {
                mouseModeEnabled = value;
                raiseValueChanged("MouseModeEnabled");
            }

        }

        //cpu settings
        public static int cpuMaxFrequency = 0;
        public static int CPUMaxFrequency
        {
            get
            {
                return cpuMaxFrequency;
            }
            set
            {
                cpuMaxFrequency = value;
                raiseValueChanged("CPUMaxFrequency");
            }
        }



        public static int cpuActiveCores = 0;
        public static int CPUActiveCores
        {
            get
            {
                return cpuActiveCores;
            }
            set
            {
                cpuActiveCores = value;
                raiseValueChanged("ActiveCores");
            }
        }

        public static int maxCpuCores = 1;
        public static int baseCPUSpeed = 1100;


        #region EPP
        public static int EPP
        {
            get
            {
                return ePP;
            }
            set
            {
                ePP = value;
                raiseValueChanged("EPP");
            }
        }


        public static int ePP = 0;
        #endregion
        #region fpslimit
        public static int fpsLimit = 0;
        public static int FPSLimit
        {
            get
            {
                return fpsLimit;
            }

            set
            {
                fpsLimit = value;
                raiseValueChanged("FPSLimit");
            }
        }
        #endregion
        //Profile 
        public static bool profileAutoApplied = false;
      
        public static Profiles_Management profiles;
        public static MouseMode_Management mousemodes;
        public static Action_Management hotKeys;
        public static CustomizeHome_Management homePageItems;
        //Power
        public static string powerStatus = SystemParameters.PowerLineStatus.ToString();
        public static int batteryLevel = -1;

        //controller status
        public static bool controllerConnected = false;

        //controller keyboard shortcuts
        public static Dictionary<ushort,ActionParameter> controllerHotKeyDictionary= new Dictionary<ushort, ActionParameter>();
        public static Dictionary<string, ActionParameter> KBHotKeyDictionary = new Dictionary<string, ActionParameter>();
        
        //kill controller loop thread
        public static bool killControllerThread = false;

        # region display settings
        public static string Resolution
        {
            get
            {
                return resolution;
            }
            set
            {
                resolution= value;
                
                raiseValueChanged("Resolution");
            }
        }
        public static string RefreshRate
        {
            get
            {
                return refreshRate;
            }
            set
            {
                refreshRate = value;
                raiseValueChanged("RefreshRate");
            }
        }
        public static string Scaling = "Default";


        public static string resolution = "";
        public static string refreshRate = "";

        public static List<string> resolutions = new List<string>();
        public static Dictionary<string, List<string>> resolution_refreshrates = new Dictionary<string, List<string>>();

        public static List<string> scalings = new List<string>();
        public static List<string> FPSLimits = new List<string>();
        public static List<string> FanModes = new List<string>();

        #endregion

        //amd power slide
        public static int AMDPowerSlide;

        //fan controls
        public static HandheldDevice Device;
        public static bool fanControlEnabled = false;
        public static bool softwareAutoFanControlEnabled = false;
        public static double fanSpeed = 0;

        public static double FanSpeed
        {
            get
            {
                return fanSpeed;
            }
            set
            {
                fanSpeed = value;
                raiseValueChanged("FanSpeed");
            }
        }


        //cpu values
        public static double cpuTemp = 0;

        //language pack
        public static ResourceDictionary languageDict = new ResourceDictionary();




        public static event EventHandler<valueChangedEventArgs> valueChanged;

        public static void raiseValueChanged(string parameter)
        {

            valueChanged?.Invoke(null, new valueChangedEventArgs(parameter));
        }
    }
    public class valueChangedEventArgs : EventArgs
    {
        public string Parameter { get; set; }
        public valueChangedEventArgs(string parameter)
        {
           
            this.Parameter = parameter;
        }
    }
    public struct ActionParameter
    {
        public string Action;
        public string Parameter;
    }
}
