﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Handheld_Control_Panel.Classes;
using Handheld_Control_Panel.Classes.Controller_Management;
using Linearstar.Windows.RawInput;
using static Vanara.Interop.KnownShellItemPropertyKeys;

namespace Handheld_Control_Panel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    
    public partial class App : Application
    {
        public App()
        {
            //start up
            Controller_Management.start_Controller_Management();

          
        }

       

    }
}
