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
    public partial class ActionEditPage : Page
    {
        private string windowpage;
        private List<UserControl> userControls = new List<UserControl>();

        private int highlightedUserControl = -1;
        private int selectedUserControl = -1;
        public ActionEditPage()
        {
            InitializeComponent();
            ThemeManager.Current.ChangeTheme(this, Global_Variables.settings.SystemTheme + "." + Global_Variables.settings.systemAccent);
           

            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            wnd.changeUserInstruction("ActionEditPage_Instruction");
            wnd = null;
        }
       
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //get unique window page combo from getwindow to string
            windowpage = WindowPageUserControl_Management.getWindowPageFromWindowToString(this);
            //subscribe to controller input events
            Controller_Window_Page_UserControl_Events.pageControllerInput += handleControllerInputs;
            UserControl_Management.getUserControlsOnPage(userControls, stackPanel);


        }

      
        //
        private void handleControllerInputs(object sender, EventArgs e)
        {
            //get action from custom event args for controller
            controllerPageInputEventArgs args = (controllerPageInputEventArgs)e;
            string action = args.Action;

            if (args.WindowPage == windowpage)
            {
                MainWindow wnd;
                int[] intReturn;
                switch (args.Action)
                {
                    case "B":
                        if (Global_Variables.mainWindow.disable_B_ToClose)
                        {
                            intReturn = WindowPageUserControl_Management.globalHandlePageControllerInput(windowpage, action, userControls, highlightedUserControl, selectedUserControl, stackPanel);

                            highlightedUserControl = intReturn[0];
                            selectedUserControl = intReturn[1];
                        }
                        else
                        {
                            XML_Management.Load_Action(Global_Variables.hotKeys.editingHotkey.ID);
                            wnd = (MainWindow)Application.Current.MainWindow;
                            wnd.navigateFrame("ActionPage");
                            wnd = null;
                        }
    
                        break;
                    case "Start":
                        XML_Management.Save_Action(Global_Variables.hotKeys.editingHotkey);
                        if (Global_Variables.hotKeys.editingHotkey.Type == "Controller")
                        {
                            Global_Variables.hotKeys.generateGlobalControllerHotKeyList();
                        }
                        if (Global_Variables.hotKeys.editingHotkey.Type == "Keyboard")
                        {
                            Global_Variables.hotKeys.generateGlobalKeyboardHotKeyList();
                        }
                        Notification_Management.ShowInWindow(Application.Current.Resources["Usercontrol_HotKeySaved"].ToString(), Notification.Wpf.NotificationType.Success);

                        wnd = (MainWindow)Application.Current.MainWindow;
                        wnd.navigateFrame("ActionPage");
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
            XML_Management.Load_Action(Global_Variables.hotKeys.editingHotkey.ID);
        }
    }
}
