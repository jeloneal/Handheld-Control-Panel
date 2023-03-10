﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Handheld_Control_Panel.Classes.Global_Variables;
using SharpDX;
using SharpDX.XInput;
using Linearstar.Windows.RawInput;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WindowsInput;
using Nefarius.Utilities.DeviceManagement.PnP;
using Nefarius.Utilities.DeviceManagement.Extensions;
using Windows.Devices.Usb;
using System.Timers;

namespace Handheld_Control_Panel.Classes.Controller_Management
{
    public static class Controller_Management
    {
        public static Controller? controller;
        public static Gamepad currentGamePad;
        public static Gamepad previousGamePad;

        public static bool suspendEventsForGamepadHotKeyProgramming = false;

        public static DispatcherTimer timerController = new DispatcherTimer(DispatcherPriority.Send);

        public static buttonEvents buttonEvents = new buttonEvents();


        public static void getDefaultControllerDeviceInformation()
        {
            if (Properties.Settings.Default.GUID == "" || Properties.Settings.Default.instanceID == "")
            {
                var instance = 0;
                //Get the VID and PID from instance path, should look like this VID_045E&PID_028E, we need this to search in the Devcon interface GUID
                string strVIDPID = "VID_045E&PID_028E";


                // enumerate all devices that export the GUID_DEVINTERFACE_USB_DEVICE interface
                while (Devcon.FindByInterfaceGuid(DeviceInterfaceIds.UsbDevice, out var path,
                           out var instanceId, instance++))
                {

                    var usbDevice = PnPDevice
                        .GetDeviceByInterfaceId(path)
                        .ToUsbPnPDevice();

                   

                    //We want the device that has our VID and PID value, the variable strDevInstPth that should look like this  VID_045E&PID_028E
                    if (path.Contains(strVIDPID))
                    {
                        
                        Guid guid = usbDevice.GetProperty<Guid>(DevicePropertyKey.Device_ClassGuid);
                        Properties.Settings.Default.GUID = guid.ToString();
                        Properties.Settings.Default.instanceID = usbDevice.InstanceId;
                        Properties.Settings.Default.Save();
                        break;
                    }
                }
            }
          
        }

        public static void hideController()
        {
            if (Global_Variables.Global_Variables.hidHide.IsInstalled)
            {
                if (Global_Variables.Global_Variables.hidHide.IsInstalled)
                {

                }
            }
        }

       
        public static bool toggleEnableDisableController()
        {
            bool deviceEnable;

            if (controller != null)
            {
                //apply ! so that when the controller is connected the bool tells to disable and vice versa
                deviceEnable = !controller.IsConnected;
            }
            else
            {
                deviceEnable = true;
            }
            if (Properties.Settings.Default.GUID != "" && Properties.Settings.Default.instanceID != "")
            {
                var instance = 0;

                

                Guid guid = new Guid(Properties.Settings.Default.GUID);
                string instanceID = Properties.Settings.Default.instanceID;

                enabledevice.DeviceHelper.SetDeviceEnabled(guid, instanceID, deviceEnable);


                if (!deviceEnable)
                {
                    Task.Delay(2000);
                    powerCycleController();
                }


                return deviceEnable;
            }
            else
            {
                return !deviceEnable;
            }
      

        }

        public static void powerCycleController()
        {
            var instance = 0;
   
            while (Devcon.FindByInterfaceGuid(DeviceInterfaceIds.UsbDevice, out var path,
                       out var instanceId, instance++))
            {

                var usbDevice = PnPDevice
                    .GetDeviceByInterfaceId(path)
                    .ToUsbPnPDevice();
                               
                //We want the device that has our VID and PID value, the variable strDevInstPth that should look like this  VID_045E&PID_028E
                if (usbDevice.InstanceId == Properties.Settings.Default.instanceID)
                {
                   
                    //Apply power port cycle to finish disable
                    usbDevice.CyclePort();
                }
            }
        }

        public static void start_Controller_Management()
        {
            //create background thread to handle controller input
            getController();
            timerController.Interval = TimeSpan.FromMilliseconds(40);
            timerController.Tick += controller_Tick;
            timerController.Start();

        }
    
        private static void controller_Tick(object sender, EventArgs e)
        {
            //start timer to read and compare controller inputs
            //Controller input handler

            if (controller == null)
            {
                getController();
                if (controller.IsConnected == false)
                {
                    getController();
                }
            }
            else if (!controller.IsConnected)
            {
                getController();
            }


            if (controller != null)
            {
                if (controller.IsConnected)
                {
                    //var watch = System.Diagnostics.Stopwatch.StartNew();
                    currentGamePad = controller.GetState().Gamepad;

                    ushort btnShort = ((ushort)currentGamePad.Buttons);


                    if (!suspendEventsForGamepadHotKeyProgramming)
                    {
                        //check if controller combo is in controller hot key dictionary

                        if (Global_Variables.Global_Variables.controllerHotKeyDictionary != null)
                        {
                            if (Global_Variables.Global_Variables.controllerHotKeyDictionary.ContainsKey(btnShort))
                            {

                                if (((ushort)previousGamePad.Buttons) != btnShort)
                                {
                                    ActionParameter action = Global_Variables.Global_Variables.controllerHotKeyDictionary[btnShort];
                                    QuickAction_Management.runHotKeyAction(action);
                                }
                            }
                        }

                        if (!MouseMode_Management.MouseMode_Management.status_MouseMode())
                        {


                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.Back) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.Back))
                            {
                                buttonEvents.raiseControllerInput("Back");
                            }
                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.Start) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.Start))
                            {
                                buttonEvents.raiseControllerInput("Start");
                            }
                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.Back) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.Back))
                            {
                                buttonEvents.raiseControllerInput("Back");
                            }
                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.A) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.A))
                            {
                                buttonEvents.raiseControllerInput("A");
                            }
                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.X) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.X))
                            {
                                buttonEvents.raiseControllerInput("X");
                            }
                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.Y) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.Y))
                            {
                                buttonEvents.raiseControllerInput("Y");
                            }
                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.DPadUp) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.DPadUp))
                            {
                                buttonEvents.raiseControllerInput("Up");
                            }
                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.DPadDown) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.DPadDown))
                            {
                                buttonEvents.raiseControllerInput("Down");
                            }
                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.DPadRight) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.DPadRight))
                            {
                                buttonEvents.raiseControllerInput("Right");
                            }
                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft))
                            {
                                buttonEvents.raiseControllerInput("Left");
                            }
                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.B) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.B))
                            {
                                buttonEvents.raiseControllerInput("B");
                            }

                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder))
                            {
                                buttonEvents.raiseControllerInput("LB");
                            }
                            if (currentGamePad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder) && !previousGamePad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder))
                            {
                                buttonEvents.raiseControllerInput("RB");
                            }

                            if (currentGamePad.LeftThumbX > 12000 && previousGamePad.LeftThumbX <= 12000)
                            {
                                buttonEvents.raiseControllerInput("Right");
                            }
                            if (currentGamePad.LeftThumbX < -12000 && previousGamePad.LeftThumbX >= -12000)
                            {
                                buttonEvents.raiseControllerInput("Left");
                            }
                            if (currentGamePad.LeftThumbY > 12000 && previousGamePad.LeftThumbY <= 12000)
                            {
                                buttonEvents.raiseControllerInput("Up");
                            }
                            if (currentGamePad.LeftThumbY < -12000 && previousGamePad.LeftThumbY >= -12000)
                            {
                                buttonEvents.raiseControllerInput("Down");
                            }
                        }



                    }
                    //watch.Stop();
                    //Debug.WriteLine($"Total Execution Time: {watch.ElapsedMilliseconds} ms");
                    //doSomeWork();

                    previousGamePad = currentGamePad;
                }

            }

        }

        public static void doSomeWork()
        {
           

        }


        private static void getController()
        {
            int controllerNum = 1;
            //get controller used, loop controller number if less than 5, so if controller is connected make num = 5 to get out of while loop
            while (controllerNum < 6)
            {
                switch (controllerNum)
                {
                    default:
                        break;
                    case 1:
                        controller = new Controller(UserIndex.One);
                        break;
                    case 2:
                        controller = new Controller(UserIndex.Two);
                        break;
                    case 3:
                        controller = new Controller(UserIndex.Three);
                        break;
                    case 4:
                        controller = new Controller(UserIndex.Four);
                        break;
                    case 5:
                        timerController.Interval = TimeSpan.FromMilliseconds(1500);
                        controllerNum = 6;

                        if (Global_Variables.Global_Variables.controllerConnected == true)
                        {
                            Global_Variables.Global_Variables.controllerConnected = false;
                            buttonEvents.raiseControllerStatusChanged();
                        }
                        break;
                }
                if (controller == null)
                {
                    controllerNum++;
                }
                else
                {
                    if (controller.IsConnected)
                    {
                        controllerNum = 6;
                        timerController.Interval = TimeSpan.FromMilliseconds(20);
                        if (Global_Variables.Global_Variables.controllerConnected == false)
                        {
                            Global_Variables.Global_Variables.controllerConnected = true;
                            buttonEvents.raiseControllerStatusChanged();
                        }
                    }
                    else
                    {
                        controllerNum++;
                    }
                }
            }
        }

    }
  
    public class buttonEvents
    {

        public event EventHandler controllerStatusChangedEvent;
        public void raiseControllerStatusChanged()
        {
            controllerStatusChangedEvent?.Invoke(this, EventArgs.Empty);
        }

   
        public event EventHandler<controllerInputEventArgs> controllerInput;
        
        public void raiseControllerInput(string action)
        {
           
            controllerInput?.Invoke(this, new controllerInputEventArgs(action));
        }
      



        public event EventHandler openAppEvent;
        public void RaiseOpenAppEvent()
        {
            openAppEvent?.Invoke(this, EventArgs.Empty);
        }
    }
    public class controllerInputEventArgs : EventArgs
    {
        public string Action { get; set; }
        public controllerInputEventArgs(string action)
        {
            this.Action = action;
        }
    }

    public class controllerPageInputEventArgs : EventArgs
    {
        public string Action { get; set; }
        public string WindowPage { get; set; }
        public controllerPageInputEventArgs(string action, string windowpage)
        {
            this.Action = action;
            this.WindowPage = windowpage;
        }
    }

    public class controllerUserControlInputEventArgs : EventArgs
    {
        public string Action { get; set; }
        public string WindowPage { get; set; }
        public string UserControl { get; set; }
        public controllerUserControlInputEventArgs(string action, string windowpage, string usercontrol)
        {
            this.Action = action;
            this.WindowPage = windowpage;
            this.UserControl = usercontrol;
        }
    }

    public static class Controller_Window_Page_UserControl_Events
    {
        public static event EventHandler<controllerPageInputEventArgs> pageControllerInput;
        public static event EventHandler<controllerUserControlInputEventArgs> userControlControllerInput;


        public static void raisePageControllerInputEvent(string action, string windowpage)
        {
            pageControllerInput?.Invoke(null, new controllerPageInputEventArgs(action, windowpage));
        }
        public static void raiseUserControlControllerInputEvent(string action, string windowpage, string usercontrol)
        {
            userControlControllerInput?.Invoke(null, new controllerUserControlInputEventArgs(action, windowpage, usercontrol));
        }
    }
}
