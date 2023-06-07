﻿using Handheld_Control_Panel.Classes;
using Handheld_Control_Panel.Classes.Controller_Management;
using Handheld_Control_Panel.UserControls;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using MahApps.Metro;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlzEx.Theming;
using System.Windows.Controls.Primitives;
using Handheld_Control_Panel.Classes.Global_Variables;
using Handheld_Control_Panel.UserControls;
using System.Windows.Threading;
using Handheld_Control_Panel.Classes.UserControl_Management;

namespace Handheld_Control_Panel.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class ProfileEditPage : Page
    {
        private string windowpage;
        private List<UserControl> userControls = new List<UserControl>();
  
        private int highlightedUserControl = -1;
        private int selectedUserControl = -1;
        public ProfileEditPage()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, Global_Variables.settings.SystemTheme + "." + Global_Variables.settings.systemAccent);
           

            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            wnd.changeUserInstruction("ProfileEditPage_Instruction");
            wnd = null;
        }
       
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //get unique window page combo from getwindow to string
            windowpage = WindowPageUserControl_Management.getWindowPageFromWindowToString(this);
            //subscribe to controller input events
            Controller_Window_Page_UserControl_Events.pageControllerInput += handleControllerInputs;
            Profile_Events.separateChargerBatteryEvent += Profile_Events_separateChargerBatteryEvent;

            //do this to account for the separate profile for charger/battery
            collapseUncollapseOnlineValues(Global_Variables.profiles.editingProfile.SeparateChargerBattery);
            if (Global_Variables.profiles.editingProfile.SeparateChargerBattery)
            {
                batteryLabel.Visibility = Visibility.Visible;
                chargerLabel.Visibility = Visibility.Visible;
            }
            else
            {
                chargerLabel.Visibility = Visibility.Collapsed;
                batteryLabel.Visibility = Visibility.Collapsed;
            }
            UserControl_Management.getUserControlsOnPage(userControls, stackPanel);


        }

        private void Profile_Events_separateChargerBatteryEvent(object? sender, EventArgs e)
        {
            userControls.Clear();
            collapseUncollapseOnlineValues(Global_Variables.profiles.editingProfile.SeparateChargerBattery);
            if (Global_Variables.profiles.editingProfile.SeparateChargerBattery)
            {
                batteryLabel.Visibility = Visibility.Visible;
                chargerLabel.Visibility = Visibility.Visible;
            }
            else
            {
                chargerLabel.Visibility = Visibility.Collapsed;
                batteryLabel.Visibility = Visibility.Collapsed;
            }
            UserControl_Management.getUserControlsOnPage(userControls, stackPanel);
            if (userControls.Count < highlightedUserControl || userControls.Count < selectedUserControl)
            {
                highlightedUserControl = 0;
                selectedUserControl = -1;
            }
        }

        private void collapseUncollapseOnlineValues(bool SeparateChargerBattery)
        {
            foreach (object child in stackPanel.Children)
            {
                if (child is UserControl)
                {
                    UserControl uc = (UserControl)child;
                    if (child.ToString().Contains(".Online"))
                    {
                        if (SeparateChargerBattery)
                        {
                            uc.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            uc.Visibility = Visibility.Collapsed;
                        }
                    }
                }

            }
        }

        //
        private void handleControllerInputs(object sender, EventArgs e)
        {
            //get action from custom event args for controller
            controllerPageInputEventArgs args = (controllerPageInputEventArgs)e;
            string action = args.Action;

            if (args.WindowPage == windowpage)
            {
                int[] intReturn;
                MainWindow wnd;
                switch (args.Action)
                {
                    
                    case "B":
                        if (selectedUserControl < 0)
                        {
                            Global_Variables.profiles.loadProfile(Global_Variables.profiles.editingProfile.ProfileName);
                            wnd = (MainWindow)Application.Current.MainWindow;
                            wnd.navigateFrame("ProfilesPage");
                            wnd = null;
                        }
                        else
                        {
                            intReturn = WindowPageUserControl_Management.globalHandlePageControllerInput(windowpage, action, userControls, highlightedUserControl, selectedUserControl, stackPanel);

                            highlightedUserControl = intReturn[0];
                            selectedUserControl = intReturn[1];
                        }
                        break;
                    case "Start":
                        Notification_Management.ShowInWindow(Application.Current.Resources["Usercontrol_ProfileSaved"].ToString(), Notification.Wpf.NotificationType.Success);
                        Global_Variables.profiles.SaveToXML(Global_Variables.profiles.editingProfile);

                        Global_Variables.profiles.loadProfile(Global_Variables.profiles.editingProfile.ProfileName);
                        wnd = (MainWindow)Application.Current.MainWindow;
                        wnd.navigateFrame("ProfilesPage");
                        wnd = null;
                        //runSaveMessage();
                        break;


                    default:
                        //global method handles the event tracking and returns what the index of the highlighted and selected usercontrolshould be
                        intReturn = WindowPageUserControl_Management.globalHandlePageControllerInput(windowpage, action, userControls, highlightedUserControl, selectedUserControl, stackPanel);

                        highlightedUserControl = intReturn[0];
                        selectedUserControl = intReturn[1];

                        break;


                }
              
   
            }

        }
      
      

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Controller_Window_Page_UserControl_Events.pageControllerInput -= handleControllerInputs;
            //make sure to load profile to clear any unsaved changes to the profile
            Global_Variables.profiles.loadProfile(Global_Variables.profiles.editingProfile.ProfileName);
            if (Global_Variables.profiles.activeProfile != null)
            {
                if (Global_Variables.profiles.editingProfile == Global_Variables.profiles.activeProfile)
                {
                    Global_Variables.profiles.activeProfile.applyProfile(false,false);
                }
            }
        }
    }
}
