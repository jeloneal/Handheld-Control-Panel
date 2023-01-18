﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Handheld_Control_Panel.Classes.Brightness_Management
{
    public class WindowsSettingsBrightnessController
    {
        public static void getBrightness()
        {
            try
            {
                ManagementScope scope;
                SelectQuery query;

                scope = new ManagementScope("root\\WMI");
                query = new SelectQuery("SELECT * FROM WmiMonitorBrightness");

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    using (ManagementObjectCollection objectCollection = searcher.Get())
                    {
                        foreach (ManagementObject mObj in objectCollection)
                        {
                            Console.WriteLine(mObj.ClassPath);
                            foreach (var item in mObj.Properties)
                            {
                                Console.WriteLine(item.Name + " " + item.Value.ToString());
                                if (item.Name == "CurrentBrightness")
                                { Global_Variables.Global_Variables.brightness = Convert.ToInt32(item.Value); }    //Do something with CurrentBrightness
                            }
                        }
                    }
                }
            }
            catch { }


        }
        public static void setBrightness(int intBrightness)
        {

            try
            {
                ManagementClass mclass = new ManagementClass("WmiMonitorBrightnessMethods");
                mclass.Scope = new ManagementScope("\\\\.\\root\\wmi");

                ManagementObjectCollection instances = mclass.GetInstances();   

                foreach(ManagementObject instance in instances)
                {
                    byte brightness = Convert.ToByte(intBrightness);
                    UInt64 timeout = 1;

                    object[] args = new object[] { timeout, brightness };

                    instance.InvokeMethod("WmiSetBrightness", args);
                }

            }
            catch 
            { 
            
            
            
            
            }



   



        }
    }
}
