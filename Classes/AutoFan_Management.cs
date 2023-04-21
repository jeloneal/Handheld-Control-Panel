﻿using HidSharp.Reports;
using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Management.Update;

namespace Handheld_Control_Panel.Classes
{
    public static class AutoFan_Management
    {
        private static Thread autoFan;
        private static double[] dataXvalues;
        private static double[] dataYvalues;
        private static bool tempControlled;
        private static int currentFanSpeedPercentage;
        private static int targetFanSpeedPercentage;
        private static int newFanSpeedPercentage;
        private static Computer computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = false,
            IsMemoryEnabled = false,
            IsMotherboardEnabled = false,
            IsControllerEnabled = false,
            IsNetworkEnabled = false,
            IsStorageEnabled = false,
            IsBatteryEnabled = false,
            IsPsuEnabled = false
        };
        public static void startAutoFan()
        {
            if (Global_Variables.Global_Variables.Device.FanCapable)
            {
                Global_Variables.Global_Variables.softwareAutoFanControlEnabled = true;
                Fan_Management.Fan_Management.setFanControlManual();
                //get the property variable whether you want it temperature or package power controlled, true means temp, false means package power
                tempControlled = Properties.Settings.Default.fanAutoModeTemp;
                if (tempControlled )
                {
                    autoFan = new Thread(() => { mainAutoFanLoop_Temperature(); });
                }
                else
                {
                    autoFan = new Thread(() => { mainAutoFanLoop_PackagePower(); });
                }
                
                loadXandYvalues();
                computer.Open();
                autoFan.Start();
            }


        }
        private static void loadXandYvalues()
        {
            string[] dataYstring;
            if (tempControlled)
            {
                dataXvalues= new double[11] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
                dataYvalues = Fan_Functions.getTemperatureYValues();
            }
            else
            {
                dataXvalues = Fan_Functions.getPowerXValues();
                dataYvalues = Fan_Functions.getPowerYValues();
            }
        }
        private static void mainAutoFanLoop_Temperature()
        {
            while (Global_Variables.Global_Variables.softwareAutoFanControlEnabled && Properties.Settings.Default.fanAutoModeTemp == true)
            {
                computer.Accept(new UpdateVisitor());
                getLibre_cpuTemperature();

                getTargetFanSpeedPercentage();
                getNewTargetFanSpeedPercentage();

                Fan_Management.Fan_Management.setFanSpeed(newFanSpeedPercentage);
                currentFanSpeedPercentage = newFanSpeedPercentage;


                Thread.Sleep(1000);
            }



        }
        private static void mainAutoFanLoop_PackagePower()
        {
            
            while (Global_Variables.Global_Variables.softwareAutoFanControlEnabled && Properties.Settings.Default.fanAutoModeTemp == false)
            {
                computer.Accept(new UpdateVisitor());
                getLibre_packagepower();

                getTargetFanSpeedPercentage();
                getNewTargetFanSpeedPercentage();

                
                currentFanSpeedPercentage = newFanSpeedPercentage;

                Thread.Sleep(1000);
            }



        }

        private static List<float?> packagePower = new List<float?>();
        private static float? avg_packagePower = 0;

        private static void getLibre_packagepower()
        {
            foreach (Hardware hardware in computer.Hardware)
            {
                ISensor package = hardware.Sensors.FirstOrDefault(c => c.Name == "Package");
                if (package != null)
                {
                    packagePower.Add(package.Value);

                    if (packagePower.Count > 3)
                    {
                        packagePower.RemoveAt(0);
                    }
                    avg_packagePower = packagePower.Average();
                    break;
                }
            }
                      
        }

        private static List<float?> cpuTemperature = new List<float?>();
        private static float? avg_cpuTemperature = 0;

        private static void getLibre_cpuTemperature()
        {
            
            foreach (Hardware hardware in computer.Hardware)
            {
                ISensor temperature = hardware.Sensors.FirstOrDefault(c => c.Name == "Core (Tctl/Tdie)");
                if (temperature != null)
                {
                    cpuTemperature.Add(temperature.Value);

                    if (cpuTemperature.Count > 3)
                    {
                        cpuTemperature.RemoveAt(0);
                    }
                    avg_cpuTemperature = cpuTemperature.Average();
                    break;
                }
                else
                {
                    foreach (IHardware subhardware in hardware.SubHardware)
                    {
                        Debug.WriteLine("\tSubhardware: {0}", subhardware.Name);

                        ISensor temperature2 = subhardware.Sensors.FirstOrDefault(c => c.Name == "Core (Tctl/Tdie)");
                        if (temperature2 != null)
                        {
                            cpuTemperature.Add(temperature2.Value);

                            if (cpuTemperature.Count > 3)
                            {
                                cpuTemperature.RemoveAt(0);
                            }
                            avg_cpuTemperature = cpuTemperature.Average();
                            break;
                        }
                    }
                }
            }

        }
        
        private static void getTargetFanSpeedPercentage()
        {
            int index = 0;
            if (tempControlled)
            {
                if (avg_cpuTemperature > dataXvalues[dataXvalues.Length - 1])
                {
                    targetFanSpeedPercentage = (int)Math.Round(dataYvalues[dataXvalues.Length - 1], 0);

                }
                else
                {
                    foreach (double d in dataXvalues)
                    {
                        if (avg_cpuTemperature <= d)
                        {
                            targetFanSpeedPercentage = (int)Math.Round(dataYvalues[index], 0);
                            break;
                        }
                    
                        index++;
                    }
                }


            }
            else
            {
                if (avg_packagePower > dataXvalues[dataXvalues.Length - 1])
                {
                    targetFanSpeedPercentage = (int)Math.Round(dataYvalues[dataXvalues.Length - 1], 0);

                }
                else
                {
                    foreach (double d in dataXvalues)
                    {
                        if (avg_packagePower <= d)
                        {
                            targetFanSpeedPercentage = (int)Math.Round(dataYvalues[index], 0);
                            break;
                        }
                     
                        index++;
                    }
                }
            }

        }

        private static void getNewTargetFanSpeedPercentage()
        {
            if (targetFanSpeedPercentage != 0)
            {
                if (currentFanSpeedPercentage < targetFanSpeedPercentage)
                {
                    newFanSpeedPercentage = Math.Min(100, currentFanSpeedPercentage + Math.Min(3, targetFanSpeedPercentage - currentFanSpeedPercentage));

                }
                if (currentFanSpeedPercentage > targetFanSpeedPercentage)
                {
                    newFanSpeedPercentage = Math.Min(100, currentFanSpeedPercentage + Math.Max(-3, targetFanSpeedPercentage - currentFanSpeedPercentage));

                }
                if (currentFanSpeedPercentage == targetFanSpeedPercentage)
                {
                    newFanSpeedPercentage = targetFanSpeedPercentage;
                }

                if (newFanSpeedPercentage < Global_Variables.Global_Variables.Device.MinFanSpeedPercentage)
                {
                    newFanSpeedPercentage = Global_Variables.Global_Variables.Device.MinFanSpeedPercentage;
                }
            }
            else
            {
                newFanSpeedPercentage = 0;
            }
            Debug.WriteLine(newFanSpeedPercentage);

        }

    }
    
    public static class Fan_Functions
    {

        public static double[] getTemperatureYValues()
        {
            double[] returnValues = new double[0];
            string[] dataString; 


            if (Properties.Settings.Default.fanCurveTemperature != "")
            {
                dataString = Properties.Settings.Default.fanCurveTemperature.Split(',').ToArray();

                if (dataString.Length == 11)
                {
                    for (int i = 0; i < dataString.Length; i++)
                    {
                        if (dataString[i] != "")
                        {
                            Array.Resize(ref returnValues, i + 1);
                            returnValues[i] = Convert.ToDouble(dataString[i]);
                        }
                    }
                }
            }
            else if (Global_Variables.Global_Variables.Device.fanCurveTemperature != "")
            {
                dataString = Global_Variables.Global_Variables.Device.fanCurveTemperature.Split(',').ToArray();

                if (dataString.Length > 0)
                {
                    for (int i = 0; i < dataString.Length; i++)
                    {
                        if (dataString[i] != "")
                        {
                            Array.Resize(ref returnValues, i + 1);
                            returnValues[i] = Convert.ToDouble(dataString[i]);
                            Debug.WriteLine(returnValues[i]);
                        }
                    }
                }
            }

            return returnValues;
        }

        public static double[] getPowerYValues()
        {
            double[] returnValues = new double[0];
            string[] dataString;


            if (Properties.Settings.Default.fanCurvePackagePower != "")
            {
                dataString = Properties.Settings.Default.fanCurvePackagePower.Split(',').ToArray();

                if (dataString.Length > 0)
                {
                    for (int i = 0; i < dataString.Length; i++)
                    {
                        if (dataString[i] != "")
                        {
                            Array.Resize(ref returnValues, i + 1);
                            returnValues[i] = Convert.ToDouble(dataString[i]);
                        }
                    }
                }
            }
            else if (Global_Variables.Global_Variables.Device.fanCurvePackagePower != "")
            {
                dataString = Global_Variables.Global_Variables.Device.fanCurvePackagePower.Split(',').ToArray();

                if (dataString.Length > 0)
                {
                    for (int i = 0; i < dataString.Length; i++)
                    {
                        if (dataString[i] != "")
                        {
                            Array.Resize(ref returnValues, i + 1);
                            returnValues[i] = Convert.ToDouble(dataString[i]);
                        }
                    }
                }
            }

            return returnValues;
        }

        public static double[] getPowerXValues()
        {
            double[] returnValues = new double[0];
            string[] dataString;


            if (Properties.Settings.Default.fanCurvePackagePower != "")
            {
                dataString = Properties.Settings.Default.fanCurvePackagePower.Split(',').ToArray();

                if (dataString.Length > 0)
                {
                    for (int i = 0; i < dataString.Length; i++)
                    {
                        if (dataString[i] != "")
                        {
                            Array.Resize(ref returnValues, i + 1);
                            returnValues[i] = i*5 + 5;
                        }
                    }
                }
            }
            else if (Global_Variables.Global_Variables.Device.fanCurvePackagePower != "")
            {
                dataString = Global_Variables.Global_Variables.Device.fanCurvePackagePower.Split(',').ToArray();

                if (dataString.Length > 0)
                {
                    for (int i = 0; i < dataString.Length; i++)
                    {
                        if (dataString[i] != "")
                        {
                            Array.Resize(ref returnValues, i + 1);
                            returnValues[i] = i * 5 + 5;
                        }
                    }
                }
            }

            return returnValues;
        }
    }
}
