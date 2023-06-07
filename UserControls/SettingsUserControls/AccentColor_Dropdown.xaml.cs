﻿using ControlzEx.Standard;
using ControlzEx.Theming;
using Handheld_Control_Panel.Classes;
using Handheld_Control_Panel.Classes.Controller_Management;
using Handheld_Control_Panel.Classes.Display_Management;
using Handheld_Control_Panel.Classes.Global_Variables;
using Handheld_Control_Panel.Classes.TaskSchedulerWin32;
using Handheld_Control_Panel.Classes.UserControl_Management;
using Handheld_Control_Panel.Styles;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Handheld_Control_Panel.UserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class AccentColor_Dropdown : UserControl
    {
        private string windowpage = "";
        private string usercontrol = "";
        private object selectedObject = "";
        public ReadOnlyObservableCollection<Theme> themes = ThemeManager.Current.Themes;
        public AccentColor_Dropdown()
        {
            InitializeComponent();
            setControlValue();
          
        }

        private  void setControlValue()
        {
            foreach (Theme theme in themes)
            {
                if (theme.ToString().Contains("(Light)"))
                {
                    colorRectangle color = new colorRectangle();
                    color.theme = theme.DisplayName;
                    color.brush = theme.ShowcaseBrush;
                    controlList.Items.Add(color);
                    if (color.theme.Contains(Global_Variables.settings.systemAccent))
                    {
                        controlList.SelectedIndex = controlList.Items.Count - 1;
                        colorBorder.Background = color.brush;
                        selectedObject = controlList.SelectedItem;

                    }
                }

            }

            //foreach (colorRectangle lbi in controlList.Items)
            //{
                //if (lbi.theme.Contains(Global_Variables.Global_Variables.settings.systemAccent))
                //{
                   // controlList.SelectedItem = lbi;
                   //selectedObject = lbi;
                   
                //}

           // }


            controlList.Visibility = Visibility.Collapsed;
        }

        private Rectangle accentShape(Theme theme)
        {
            Rectangle accentShape = new Rectangle();
            accentShape.RadiusX = 4;
            accentShape.RadiusY = 4;
            accentShape.Width = 120;
            accentShape.Height = 30;
            accentShape.VerticalAlignment = VerticalAlignment.Center;
            //accentShape.HorizontalAlignment = HorizontalAlignment.Center;
            accentShape.Margin = new Thickness(3, 6, 4, 6);
            accentShape.Fill = theme.ShowcaseBrush;
            accentShape.Tag = theme.DisplayName;
            return accentShape;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Controller_Window_Page_UserControl_Events.userControlControllerInput += handleControllerInputs;
            windowpage = WindowPageUserControl_Management.getWindowPageFromWindowToString(this);
            usercontrol = this.ToString().Replace("Handheld_Control_Panel.Pages.UserControls.","");
         
        }

      
        private void handleControllerInputs(object sender, EventArgs e)
        {
            controllerUserControlInputEventArgs args= (controllerUserControlInputEventArgs)e;
            if (args.WindowPage == windowpage && args.UserControl==usercontrol)
            {
                switch(args.Action)
                {
                    case "A":
                        if (controlList.SelectedItem !=null)
                        {
                            colorRectangle lbi = controlList.SelectedItem as colorRectangle;
                            if (!lbi.theme.ToString().Contains(Global_Variables.settings.systemAccent))
                            {
                                handleListboxChange();
                            }

                            else
                            {

                                button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

                                MainWindow wnd = (MainWindow)Application.Current.MainWindow;
                                wnd.changeUserInstruction("SelectedListBox_Instruction");
                                wnd = null;

                            }
                        }
                      

                        break;
                    case "B":
                        MainWindow wnd2 = (MainWindow)Application.Current.MainWindow;
                        wnd2.changeUserInstruction("HomePage_Instruction");
                        wnd2 = null;
                        if (controlList.Visibility == Visibility.Visible)
                        {
                            

                            if (selectedObject != null)
                            {
                                controlList.SelectedItem = selectedObject;
                            }
                            button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

                        }
  
                        break;
                    default:
                        Classes.UserControl_Management.UserControl_Management.handleUserControl(border, controlList, args.Action);

                        break;
                }

            }
        }
        private void handleListboxChange()
        {
            if (controlList.IsLoaded)
            {
                if (controlList.SelectedItem != null)
                {
                    colorRectangle themeShape = (colorRectangle)controlList.SelectedItem;
                    string theme = themeShape.theme;
                    theme = theme.Substring(0, theme.IndexOf("(")).Trim();
                    ThemeManager.Current.ChangeTheme(Window.GetWindow(this), Global_Variables.settings.SystemTheme + "." + theme);
                    Global_Variables.settings.systemAccent = theme;
                    Global_Variables.settings.Save();
                    selectedObject = controlList.SelectedItem;
                    colorBorder.Background = themeShape.brush;              
                    if (controlList.Visibility == Visibility.Visible)
                    {
                        button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }
                }

            }

        }


        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Controller_Window_Page_UserControl_Events.userControlControllerInput -= handleControllerInputs;
         
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (controlList.Visibility == Visibility.Visible)
            {
                controlList.Visibility = Visibility.Collapsed;
                PackIconUnicons icon = (PackIconUnicons)button.Content;
                icon.RotationAngle = 0;
            }
            else
            {
                controlList.Visibility = Visibility.Visible;
                PackIconUnicons icon = (PackIconUnicons)button.Content;
                icon.RotationAngle = 90;
            }
        }


        private void controlList_TouchUp(object sender, TouchEventArgs e)
        {
            handleListboxChange();
        }

        private void controlList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            handleListboxChange();
        }
    }
    public class colorRectangle
    {
        public string theme { get; set; }
        public Brush brush { get; set; }
    }
}
