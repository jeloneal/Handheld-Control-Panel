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
using MahApps.Metro.Controls;

namespace Handheld_Control_Panel.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class ProfilesPage : Page
    {
        private string windowpage;

        public ProfilesPage()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, Global_Variables.settings.SystemTheme + "." + Global_Variables.settings.systemAccent);

            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            wnd.changeUserInstruction("ProfilePage_Instruction");
            wnd = null;
        }
      
      
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //get unique window page combo from getwindow to string
            windowpage = WindowPageUserControl_Management.getWindowPageFromWindowToString(this);
            //subscribe to controller input events
            Controller_Window_Page_UserControl_Events.pageControllerInput += handleControllerInputs;


            controlList.ItemsSource = Global_Variables.profiles;

            if (controlList.Items.Count > 0)
            {
                if (Global_Variables.profiles.editingProfile != null)
                {
                    controlList.SelectedItem = Global_Variables.profiles.editingProfile;
                    Global_Variables.profiles.editingProfile = null;
                }
                else
                {
                    if (Global_Variables.profiles.activeProfile != null)
                    {
                        controlList.SelectedItem = Global_Variables.profiles.activeProfile;
                    }
                    else
                    {
                        controlList.SelectedIndex = 0;
                    }

                   
                }
            }

        }

        private void handleControllerInputs(object sender, EventArgs e)
        {
            //get action from custom event args for controller
            controllerPageInputEventArgs args = (controllerPageInputEventArgs)e;
            string action = args.Action;

            if (args.WindowPage == windowpage)
            {
                //global method handles the event tracking and returns what the index of the highlighted and selected usercontrolshould be
                if (controlList.SelectedItem != null)
                {
                    Profile_Main profile = controlList.SelectedItem as Profile_Main;
                    int index = controlList.SelectedIndex;
                    switch (action)
                    {
                        case "A":
                            Global_Variables.profiles.editingProfile = (Profile_Main)controlList.SelectedItem;
                            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
                            wnd.navigateFrame("ProfileEditPage");
                            break;

                        case "Delete_Profile":
                            Global_Variables.profiles.deleteProfile(profile);


                            controlList.Items.Refresh();
                            if (controlList.Items.Count > 0) { if (index > 0) { controlList.SelectedIndex = index - 1; } else { controlList.SelectedIndex = 0; } };

                            break;
                        case "X":
                            Notification_Management.ShowYesNoPrompt(Application.Current.Resources["Prompt_DeleteSelectedProfile"].ToString(), Notification.Wpf.NotificationType.Warning, "Delete_Profile");
                            break;
                                                  
                        case "Y":
                            profile.applyProfile(false,true);
                            Global_Variables.profileAutoApplied = false;
                            controlList.Items.Refresh();
                            break;
                        case "Up":
                            if (index > 0) { controlList.SelectedIndex = index - 1;  } else { controlList.SelectedIndex = controlList.Items.Count - 1; }
                            controlList.ScrollIntoView(controlList.SelectedItem);
                            break;
                        case "Down":
                            if (index < controlList.Items.Count - 1) { controlList.SelectedIndex = index + 1;  } else { controlList.SelectedIndex = 0; }
                            controlList.ScrollIntoView(controlList.SelectedItem);
                            break;
                        case "Start":
                            Global_Variables.profiles.createProfile();
                            controlList.Items.Refresh();
                            break;
                        case "Back":
                            Global_Variables.profiles.createProfile(null, profile);
                            controlList.Items.Refresh();
                            break;
                        default: break;

                    }

                }
                else
                {
                    if (action == "Start")
                    {
                        Global_Variables.profiles.createProfile();
                        controlList.Items.Refresh();
                    }
                    if (action == "Up" || action == "Down")
                    {
                        if (controlList.Items.Count > 0) { controlList.SelectedIndex = 0; controlList.ScrollIntoView(controlList.SelectedItem); }

                    }
                }


              

            }

        }
      


        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Controller_Window_Page_UserControl_Events.pageControllerInput -= handleControllerInputs;
        }
    }
}
