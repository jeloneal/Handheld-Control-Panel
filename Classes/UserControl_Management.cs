﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

using System.Windows.Media;

namespace Handheld_Control_Panel.Classes.UserControl_Management
{
    public static class UserControl_Management
    {
        public static void setupControl(object control)
        {
            if (control is Slider)
            {
                Slider slider = (Slider)control;
                slider.IsSnapToTickEnabled = true;

                switch (slider.Tag)
                {
                    
                    case "Slider_AMDRSRSharpness-TickChange":
                        slider.Minimum = 0;
                        slider.Maximum = 100;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 10;
                        slider.Value = ADLX_Management.GetRSRSharpness();
                        Debug.WriteLine(ADLX_Management.GetRSRSharpness());
                        break;

                     case "Slider_MaxGPUCLK":
                        slider.Minimum = 400;
                        slider.Maximum = 3200;
                        slider.TickFrequency = 50;
                        slider.SmallChange = 50;
                        slider.LargeChange = 100;
                        slider.Value = Global_Variables.Global_Variables.settings.maxGPUCLK;
                        break;
                    case "Slider_MouseSensitivity":
                        slider.Minimum = 5;
                        slider.Maximum = 35;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;

                        break;
                    case "Slider_Deadzone":
                        slider.Minimum = 0;
                        slider.Maximum = 20;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        slider.Value = Math.Round(Global_Variables.Global_Variables.settings.joystickDeadzone*100 / 32768, 0);
                        break;

                    case "Slider_ProfileOnlineTDP":
                        slider.Minimum = Global_Variables.Global_Variables.settings.minTDP;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxTDP;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        
                        break;
                    case "Slider_ProfileOfflineTDP":
                        slider.Minimum = Global_Variables.Global_Variables.settings.minTDP;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxTDP;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        break;
                    case "Slider_ProfileOnlineTDP2":
                        slider.Minimum = Global_Variables.Global_Variables.settings.minTDP;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxTDP;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        break;
                    case "Slider_ProfileOfflineTDP2":
                        slider.Minimum = Global_Variables.Global_Variables.settings.minTDP;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxTDP;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        break;
                    case "Slider_ProfileOnlineEPP":
                        slider.Minimum = 0;
                        slider.Maximum = 100;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 10;
                        break;
                    case "Slider_ProfileOfflineEPP":
                        slider.Minimum = 0;
                        slider.Maximum = 100;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 10;
                        break;
                    case "Slider_ProfileOnlineGPUCLK":
                        slider.Minimum = 400;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxGPUCLK;
                        slider.TickFrequency = 50;
                        slider.SmallChange = 50;
                        slider.LargeChange = 100;
                        slider.Value = slider.Minimum;
                        break;
                    case "Slider_ProfileOfflineGPUCLK":
                        slider.Minimum = 400;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxGPUCLK;
                        slider.TickFrequency = 50;
                        slider.SmallChange = 50;
                        slider.LargeChange = 100;
                        slider.Value = slider.Minimum;
                        break;
                    case "Slider_ProfileOnlineMaxCPU":
                        slider.Minimum = 500;
                        slider.Maximum = 5000;
                        slider.TickFrequency = 100;
                        slider.SmallChange = 100;
                        slider.LargeChange = 500;
                        break;
                    case "Slider_ProfileOfflineMaxCPU":
                        slider.Minimum = 500;
                        slider.Maximum = 5000;
                        slider.TickFrequency = 100;
                        slider.SmallChange = 100;
                        slider.LargeChange = 500;
                        break;
                    case "Slider_ProfileOnlineActiveCores":
                        slider.Minimum = 1;
                        slider.Maximum = Global_Variables.Global_Variables.maxCpuCores;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 2;
                        break;
                    case "Slider_ProfileOfflineActiveCores":
                        slider.Minimum = 1;
                        slider.Maximum = Global_Variables.Global_Variables.maxCpuCores;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 2;
                        break;
                    case "Slider_ProfileOfflineFPS":
                        slider.Minimum = 0;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxRTSSFPSLimit;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        break;
                    case "Slider_ProfileOnlineFPS":
                        slider.Minimum = 0;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxRTSSFPSLimit;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        break;


                    case "Slider_TDP-TickChange":
                        slider.Minimum = Global_Variables.Global_Variables.settings.minTDP;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxTDP;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        slider.Value = Global_Variables.Global_Variables.ReadPL1;
                        break;
                    case "Slider_GPUCLK-TickChange":
                        slider.Minimum = 400;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxGPUCLK;
                        slider.TickFrequency = 50;
                        slider.SmallChange = 50;
                        slider.LargeChange = 100;
                        slider.Value = slider.Minimum;
                        if (Global_Variables.Global_Variables.gpuclk != 0)
                        {
                            slider.Value = Global_Variables.Global_Variables.gpuclk;
                        }
                        
                        break;
                 
                    case "Slider_MinTDP":
                        slider.Minimum = 3;
                        slider.Maximum = 25;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        slider.Value = Global_Variables.Global_Variables.settings.minTDP;
                        break;
                    case "Slider_MaxTDP":
                        slider.Minimum = 5;
                        slider.Maximum = 85;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        slider.Value = Global_Variables.Global_Variables.settings.maxTDP;
                        break;
                    case "Slider_TDP2-TickChange":
                        slider.Minimum = Global_Variables.Global_Variables.settings.minTDP;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxTDP;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        slider.Value = Global_Variables.Global_Variables.ReadPL2;
                        break;
                    case "Slider_Volume":
                        slider.Minimum = 0;
                        slider.Maximum = 100;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 5;
                        slider.LargeChange = 10;
                        slider.Value = Global_Variables.Global_Variables.volume;
                        break;
                    case "Slider_EPP":
                        slider.Minimum = 0;
                        slider.Maximum = 100;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 5;
                        slider.LargeChange = 10;
                        slider.Value = Global_Variables.Global_Variables.EPP;
                        break;
                    case "Slider_Fan-TickChange":
                        if (Global_Variables.Global_Variables.Device.MinFanSpeedPercentage != 0)
                        {
                            slider.Minimum = Global_Variables.Global_Variables.Device.MinFanSpeedPercentage-1;
                        }
                        else
                        {
                            slider.Minimum = 0;
                        }
                       
                        slider.Maximum = 100;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 5;
                        slider.LargeChange = 10;
                        
                        
                        break;
                    case "Slider_CoreParking":
                        slider.Minimum = 1;
                        slider.Maximum = Global_Variables.Global_Variables.maxCpuCores;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 2;
                        slider.Value = Global_Variables.Global_Variables.CPUActiveCores;
                        break;
                    case "Slider_Brightness":
                        slider.Minimum = 0;
                        slider.Maximum = 100;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 5;
                        slider.LargeChange = 10;
                        slider.Value = Global_Variables.Global_Variables.Brightness;
                        break;
                    case "Slider_FPSLimit":
                        slider.Minimum = 0;
                        slider.Maximum = Global_Variables.Global_Variables.settings.maxRTSSFPSLimit;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 5;
                        slider.Value = Global_Variables.Global_Variables.FPSLimit;
                        break;
                    case "Slider_MaxFPS":
                        slider.Minimum = 5;
                        slider.Maximum = 200;
                        slider.TickFrequency = 1;
                        slider.SmallChange = 1;
                        slider.LargeChange = 10;
                        slider.Value = Global_Variables.Global_Variables.settings.maxRTSSFPSLimit;
                        break;

                    case "Slider_MaxCPU":
                        slider.Minimum = 1000;
                        slider.Maximum = 5000;
                        slider.TickFrequency = 100;
                        slider.SmallChange = 100;
                        slider.LargeChange = 500;
                        if (Global_Variables.Global_Variables.CPUMaxFrequency == 0)
                        {
                            slider.Value = slider.Maximum;
                        }
                        else
                        {
                            slider.Value = Global_Variables.Global_Variables.CPUMaxFrequency;
                        }
                   

                        break;
                    default:break;
                }









                   
            }



        }

        public static void handleHidingInstructionTextBlocksWithControllerIcons(StackPanel spMain)
        {
            foreach (DependencyObject child in spMain.Children)
            {
                if (child is Button)
                {
                    Button button = (Button)child;

                    StackPanel sp = (StackPanel)button.Content;

                    foreach (DependencyObject tb in sp.Children)
                    {
                        if (tb is TextBlock)
                        {
                            TextBlock textBlock = (TextBlock)tb;
                            if (textBlock.FontFamily.ToString() == "Segoe MDL2 Assets")
                            {
                                if (!Controller_Management.Controller_Management.controller.IsConnected)
                                {
                                    textBlock.Visibility = Visibility.Collapsed;
                                }

                            }
                        }
                    }
                }
            }


        }

        public static void handleUserControl(Border border, object control, string action)
        {

            ResourceDictionary res = (ResourceDictionary)Application.LoadComponent(new Uri("Styles/ControlStyle.xaml", UriKind.RelativeOrAbsolute));


            switch (action )
            {
                case "Highlight":
                    border.Style = (Style)res["userControlBorderHighlight"];
                    border.Tag = "Highlight";
                    break;
                case "Select":
                    border.Style = (Style)res["userControlBorderHighlight"];
                    border.Tag = "Highlight";
                    break;
                case "Unhighlight":
                    border.Style = (Style)res["userControlBorder"];
                    border.Tag = "";
                    break;

                default: 
                    if (control is Slider)
                    {
                        Slider slider = (Slider)control;
                        if (slider.Visibility == Visibility.Visible)
                        {
                            double originalValue = slider.Value;
                            switch (action)
                            {

                                case "Right":
                                    slider.Value = slider.Value + slider.SmallChange;
                                    break;
                                case "Left":
                                    slider.Value = slider.Value - slider.SmallChange;
                                    break;

                                
                                default: break;
                            }
                            if (originalValue != slider.Value && !slider.Tag.ToString().Contains("-TickChange"))
                            {
                                Slider_ValueChanged(slider, null);
                            }
                        }
                      
                    }
                    if (control is Button)
                    {
                        Button button = (Button)control;
                        if (action == "A")
                        {
                            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                        }
                    }
                    if (control is ToggleSwitch)
                    {
                        if (action == "A")
                        {
                            ToggleSwitch toggleSwitch = (ToggleSwitch)control;
                            toggleSwitch.IsOn = !toggleSwitch.IsOn;
                        }
              

                    }
                    if (control is ListBox)
                    {
                        ListBox listBox = (ListBox)control;
                        switch(action)
                        {
                            case "Left":
                                if (listBox.SelectedIndex > 0) 
                                { 
                                    listBox.SelectedIndex = listBox.SelectedIndex - 1; 
                                    listBox.ScrollIntoView(listBox.SelectedItem); 
                                }
                                break;

                            case "Up":
                                if (listBox.SelectedIndex > 0) 
                                { 
                                    listBox.SelectedIndex = listBox.SelectedIndex - 1; 
                                    listBox.ScrollIntoView(listBox.SelectedItem); 
                                }
                                break;
                            case "Right":
                                if (listBox.SelectedIndex < (listBox.Items.Count -1)) 
                                { 
                                    listBox.SelectedIndex = listBox.SelectedIndex + 1;
                                    listBox.ScrollIntoView(listBox.SelectedItem);
                                }
                                break;
                            case "Down":
                                if (listBox.SelectedIndex < (listBox.Items.Count - 1)) 
                                { 
                                    listBox.SelectedIndex = listBox.SelectedIndex + 1;
                                    listBox.ScrollIntoView(listBox.SelectedItem);
                                }
                                break;
                           
                            default: break;

                        }
                        

                    }
                    if (control is Hyperlink)
                    {
                        Hyperlink hyperlink = (Hyperlink)control;
                        if (action == "A")
                        {
                            if (hyperlink.Tag == null)
                            {
                                System.Diagnostics.Process.Start(new ProcessStartInfo(hyperlink.NavigateUri.AbsoluteUri) { UseShellExecute = true });
                                
                            }
                            else if (hyperlink.Tag.ToString() == "Page")
                            {
                                Global_Variables.Global_Variables.mainWindow.navigateFrame(hyperlink.NavigateUri.ToString());
                            }
                           

                            

                        }
                    }
                    break;
            }
        }

        public static void Slider_ValueChanged(Object Slider, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (Slider)Slider;
            string sliderTag = slider.Tag.ToString();
            double sliderValue = slider.Value;
            switch (sliderTag)
            {
                case "Slider_Fan-TickChange":
                    if (Global_Variables.Global_Variables.Device.FanCapable & Global_Variables.Global_Variables.fanControlEnabled)
                    {
                        if (sliderValue == 0 || sliderValue == slider.Minimum)
                        {
                            Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.Fan_Management.Fan_Management.setFanSpeed(0));
                        }
                        else
                        {
                            if (sliderValue <= Global_Variables.Global_Variables.Device.MinFanSpeedPercentage)
                            {
                                Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.Fan_Management.Fan_Management.setFanSpeed(Global_Variables.Global_Variables.Device.MinFanSpeedPercentage));
                            }
                            else
                            {
                                Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.Fan_Management.Fan_Management.setFanSpeed((int)sliderValue));
                            }
                        }


                        
                    }
                    break;
                case "Slider_AMDRSRSharpness-TickChange":
                    ADLX_Management.SetRSRSharpness((int)sliderValue);
                    break;
                case "Slider_MouseSensitivity":
                    Global_Variables.Global_Variables.mousemodes.editingMouseMode.MouseSensitivity = sliderValue;

                    break;
                case "Slider_ProfileOnlineTDP":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Online_TDP1 = sliderValue.ToString();
                    break;
                case "Slider_ProfileOfflineTDP":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Offline_TDP1 = sliderValue.ToString();
                    break;
                case "Slider_ProfileOnlineTDP2":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Online_TDP2 = sliderValue.ToString();
                    break;
                case "Slider_ProfileOfflineTDP2":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Offline_TDP2 = sliderValue.ToString();
                    break;
                case "Slider_ProfileOnlineEPP":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Online_EPP = sliderValue.ToString();
                    break;
                case "Slider_ProfileOfflineEPP":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Offline_EPP = sliderValue.ToString();
                    break;
                case "Slider_ProfileOnlineGPUCLK":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Online_GPUCLK = sliderValue.ToString();
                    break;
                case "Slider_ProfileOfflineGPUCLK":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Offline_GPUCLK = sliderValue.ToString();
                    break;
                case "Slider_ProfileOnlineFPS":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Online_FPSLimit = sliderValue.ToString();
                    break;
                case "Slider_ProfileOfflineFPS":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Offline_FPSLimit = sliderValue.ToString();
                    break;
                case "Slider_ProfileOnlineMaxCPU":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Online_MAXCPU = sliderValue.ToString();
                    break;
                case "Slider_ProfileOfflineMaxCPU":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Offline_MAXCPU = sliderValue.ToString();
                    break;
                case "Slider_ProfileOnlineActiveCores":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Online_ActiveCores = sliderValue.ToString();
                    break;
                case "Slider_ProfileOfflineActiveCores":
                    Global_Variables.Global_Variables.profiles.editingProfile.profile_Parameters.Offline_ActiveCores = sliderValue.ToString();
                    break;
 
                case "Slider_MaxGPUCLK":
                    Global_Variables.Global_Variables.settings.maxGPUCLK = (int)sliderValue;
                    Global_Variables.Global_Variables.settings.Save();
                    break;
                case "Slider_MinTDP":
                    Global_Variables.Global_Variables.settings.minTDP = (int)sliderValue;
                    Global_Variables.Global_Variables.settings.Save();
                    break;
                case "Slider_Deadzone":
                    Global_Variables.Global_Variables.settings.joystickDeadzone = Math.Round((sliderValue/100)*32768,0);
                    Global_Variables.Global_Variables.settings.Save();
                    break;
                case "Slider_MaxTDP":
                    Global_Variables.Global_Variables.settings.maxTDP = (int)sliderValue;
                    Global_Variables.Global_Variables.settings.Save();
                    break;
                case "Slider_MaxFPS":
                    Global_Variables.Global_Variables.settings.maxRTSSFPSLimit = (int)sliderValue;
                    Global_Variables.Global_Variables.settings.Save();
                    break;
                case "Slider_TDP-TickChange":
                    if (Global_Variables.Global_Variables.settings.combineTDP)
                    {
                        Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.TDP_Management.TDP_Management.changeTDP((int)sliderValue, (int)sliderValue));

                    }
                    else
                    {
                        Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.TDP_Management.TDP_Management.changeTDP((int)sliderValue, (int)Global_Variables.Global_Variables.ReadPL2));
                    }
                    break;
                case "Slider_TDP2-TickChange":
                    Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.TDP_Management.TDP_Management.changeTDP((int)Global_Variables.Global_Variables.ReadPL1, (int)sliderValue));
                    break;
                case "Slider_GPUCLK-TickChange":
                    Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.GPUCLK_Management.GPUCLK_Management.changeAMDGPUClock((int)sliderValue)); ;
                    break;
                case "Slider_Volume":
                    Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.Volume_Management.AudioManager.SetMasterVolume((int)sliderValue));
                    break;
                case "Slider_Brightness":
                    Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.Brightness_Management.WindowsSettingsBrightnessController.setBrightness((int)sliderValue));
                    break;
                case "Slider_EPP":
                    Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.EPP_Management.EPP_Management.changeEPP((int)sliderValue));
                    break;
                case "Slider_FPSLimit":
                    Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.RTSS.setRTSSFPSLimit((int)sliderValue));
                    break;
                case "Slider_CoreParking":
                    Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.CoreParking_Management.CoreParking_Management.changeActiveCores((int)sliderValue));
                    break;
                case "Slider_MaxCPU":
                    if (sliderValue == slider.Maximum)
                    {
                        Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.MaxProcFreq_Management.MaxProcFreq_Management.changeCPUMaxFrequency(0));
                    }
                    else
                    {
                        Classes.Task_Scheduler.Task_Scheduler.runTask(() => Classes.MaxProcFreq_Management.MaxProcFreq_Management.changeCPUMaxFrequency((int)sliderValue));
                    }
                    
                    break;
                default: break;


            }
        }

        #region set slider thumbsize
        private static DependencyObject GetElementFromParent(DependencyObject parent, string childname)
        {
            //Internet routine for finding thumb of slider
            //Use element parent for thumb size control on slider
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement childframeworkelement && childframeworkelement.Name == childname)
                    return child;

                var FindRes = GetElementFromParent(child, childname);
                if (FindRes != null)
                    return FindRes;
            }
            return null;
        }
            

        public static void setThumbSize(Slider slider)
        {
            //set thumb size, internet routine
            var SliderThumb = GetElementFromParent(slider as DependencyObject, "HorizontalThumb"); //Make sure to put the right name for your slider layout options are: ("VerticalThumb", "HorizontalThumb")
            if (SliderThumb != null)
            {

                if (SliderThumb is Thumb thumb)
                {
               
                    thumb.Width = 22;
                    thumb.Height = 22;
                }
                else { }
            }
            else { }
        }
        #endregion

        public static void getUserControlsOnPage(List<UserControl> userControls, StackPanel stackPanel)
        {
            foreach (object child in stackPanel.Children)
            {
                if (child is UserControl)
                {
                    UserControl uc = (UserControl)child;
                    if (!child.ToString().Contains(".Divider") && uc.Visibility != Visibility.Collapsed)
                    {
                        userControls.Add((UserControl)child);
                    }
                }

            }
        }

        public static void handleListBoxIndexChange(ListBox lb, int change)
        {
            int selectedIndex = lb.SelectedIndex;
            int upperIndex = lb.Items.Count - 1;
            if (change < 0)
            {
                if (selectedIndex >= -change)
                {
                    lb.SelectedIndex = selectedIndex + change;
                    lb.ScrollIntoView(lb.SelectedItem);
                }
                else if (selectedIndex != 0)
                {
                    lb.SelectedIndex = 0;
                    lb.ScrollIntoView(lb.SelectedItem);
                }

            }
            if (change > 0)
            {
                if ((upperIndex - selectedIndex) >= change)
                {
                    lb.SelectedIndex = selectedIndex + change;
                    lb.ScrollIntoView(lb.SelectedItem);
                }
                else if (selectedIndex != upperIndex)
                {
                    lb.SelectedIndex = upperIndex;
                    lb.ScrollIntoView(lb.SelectedItem);
                }


            }

        }
    }
}
