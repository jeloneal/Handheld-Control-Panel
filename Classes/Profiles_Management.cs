﻿using MahApps.Metro.IconPacks;
using SharpDX;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Xml;

namespace Handheld_Control_Panel.Classes
{
    public class Profiles_Management: List<Profile>
    {
        
        public Profile activeProfile=null;
        public Profile editingProfile = null;
        public Profile defaultProfile = null;
        public Profiles_Management()
        {
            //populates list
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(Global_Variables.Global_Variables.xmlFile);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("//Configuration/Profiles");

           
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                Profile profile = new Profile();

                profile.LoadProfile(node.SelectSingleNode("ID").InnerText, xmlDocument);
                if (node.SelectSingleNode("DefaultProfile").InnerText == "True") { defaultProfile = profile; activeProfile = profile; activeProfile.ActiveProfile = true;  }
                this.Add(profile);
            }
           // this.OrderBy(o => o.ProfileName);
            xmlDocument = null;          
        }

        public void createProfileForSteamGame(string profileName, string gameID)
        {

            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(Global_Variables.Global_Variables.xmlFile);
            XmlNode xmlNodeTemplate = xmlDocument.SelectSingleNode("//Configuration/ProfileTemplate/Profile");
            XmlNode xmlNodeProfiles = xmlDocument.SelectSingleNode("//Configuration/Profiles");



            XmlNode newNode = xmlDocument.CreateNode(XmlNodeType.Element, "Profile", "");
            newNode.InnerXml = xmlNodeTemplate.InnerXml;
            newNode.SelectSingleNode("ProfileName").InnerText = profileName;
            newNode.SelectSingleNode("ID").InnerText = Global_Variables.Global_Variables.profiles.getNewIDNumberForProfile(xmlDocument);
            newNode.SelectSingleNode("LaunchOptions/GameID").InnerText = gameID;
            newNode.SelectSingleNode("LaunchOptions/AppType").InnerText = "Steam";

            xmlNodeProfiles.AppendChild(newNode);



            xmlDocument.Save(Global_Variables.Global_Variables.xmlFile);

            xmlDocument = null;

        }
        public void createProfileForEpicGame(string profileName, string path, string gameID)
        {

            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(Global_Variables.Global_Variables.xmlFile);
            XmlNode xmlNodeTemplate = xmlDocument.SelectSingleNode("//Configuration/ProfileTemplate/Profile");
            XmlNode xmlNodeProfiles = xmlDocument.SelectSingleNode("//Configuration/Profiles");



            XmlNode newNode = xmlDocument.CreateNode(XmlNodeType.Element, "Profile", "");
            newNode.InnerXml = xmlNodeTemplate.InnerXml;
            newNode.SelectSingleNode("ProfileName").InnerText = profileName;
            newNode.SelectSingleNode("ID").InnerText = Global_Variables.Global_Variables.profiles.getNewIDNumberForProfile(xmlDocument);
            newNode.SelectSingleNode("LaunchOptions/GameID").InnerText = gameID;
            newNode.SelectSingleNode("LaunchOptions/Path").InnerText = path;
            newNode.SelectSingleNode("LaunchOptions/AppType").InnerText = "EpicGames";

            xmlNodeProfiles.AppendChild(newNode);



            xmlDocument.Save(Global_Variables.Global_Variables.xmlFile);

            xmlDocument = null;

        }
        public void syncSteamGameToProfile()
        {
            //gets list of steam games from library.vdf file, then makes profiles for those without one

            Dictionary<string, string> result = Steam_Management.syncSteam_Library();

            if (result.Count > 0)
            {
                System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                xmlDocument.Load(Global_Variables.Global_Variables.xmlFile);
                XmlNode xmlNode = xmlDocument.SelectSingleNode("//Configuration/Profiles");

                foreach (KeyValuePair<string, string> pair in result)
                {
                    XmlNode xmlSelectedNode = xmlNode.SelectSingleNode("Profile/LaunchOptions/GameID[text()='" + pair.Key + "']");
                    if (xmlSelectedNode == null)
                    {
                        Global_Variables.Global_Variables.profiles.createProfileForSteamGame(pair.Value, pair.Key);
                    }
                }

                Global_Variables.Global_Variables.profiles = new Profiles_Management();
                xmlDocument = null;

            }



        }
        public void syncEpicGameToProfile()
        {
            //gets list of steam games from library.vdf file, then makes profiles for those without one

            List<EpicGamesLauncherItem> result = EpicGames_Management.syncEpic_Library();

            if (result.Count > 0)
            {
                System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                xmlDocument.Load(Global_Variables.Global_Variables.xmlFile);
                XmlNode xmlNode = xmlDocument.SelectSingleNode("//Configuration/Profiles");

                foreach (EpicGamesLauncherItem item in result)
                {
                    XmlNode xmlSelectedNode = xmlNode.SelectSingleNode("Profile/LaunchOptions/GameID[text()='" + item.gameID + "']");
                    if (xmlSelectedNode == null)
                    {
                        Global_Variables.Global_Variables.profiles.createProfileForEpicGame(item.gameName, item.installPath, item.gameID);
                    }
                }


                xmlDocument = null;

            }



        }
        public void openProgram(string ID)
        {
            foreach (Profile profile in Global_Variables.Global_Variables.profiles)
            {
                if (profile.ID == ID)
                {
                    //profile.applyProfile(true);
                    if (profile.AppType == "Steam" && profile.GameID != "")
                    {
                        Steam_Management.openSteamGame(profile.GameID);
                    }
                    else
                    {
                        if (File.Exists(profile.Path))
                        {
                            Task.Run(()=> System.Diagnostics.Process.Start(profile.Path));
                        }
                        
                    }
              
                    break;
                }
                
            }

           

        }
        public void setCurrentDefaultProfileToFalse(string ID)
        {
            //changes 
            foreach (Profile profile in this)
            {
                if (profile.ID == ID)
                {
                    profile.DefaultProfile = false;
                }
            }

        }
        public string getProfileNameById(string ID)
        {
            string name = "";
            foreach (Profile profile in this)
            {
                if (profile.ID == ID)
                {
                    name = profile.ProfileName;
                }
            }
            return name;
        }
        public void deleteProfile(Profile profile)
        {
            if (profile != null)
            {
                string ID = profile.ID;

                if (Global_Variables.Global_Variables.profiles.activeProfile != null)
                {
                    if (Global_Variables.Global_Variables.profiles.activeProfile == profile)
                    {
                        Global_Variables.Global_Variables.profiles.activeProfile = null;
                    }
                }
                if (Global_Variables.Global_Variables.profiles.defaultProfile != null)
                {
                    if (Global_Variables.Global_Variables.profiles.defaultProfile == profile)
                    {
                        Global_Variables.Global_Variables.profiles.defaultProfile = null;
                    }
                }
                System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                xmlDocument.Load(Global_Variables.Global_Variables.xmlFile);
                XmlNode xmlNodeProfiles = xmlDocument.SelectSingleNode("//Configuration/Profiles");

                foreach (XmlNode node in xmlNodeProfiles.ChildNodes)
                {
                    if (node.SelectSingleNode("ID").InnerText == ID)
                    {
                        xmlNodeProfiles.RemoveChild(node);
                        break;
                    }

                }

                xmlDocument.Save(Global_Variables.Global_Variables.xmlFile);
                xmlDocument = null;

                this.Remove(profile);
            }
          

        }

        public void addNewProfile(Profile copyProfile)
        {
         
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(Global_Variables.Global_Variables.xmlFile);
            XmlNode xmlNodeTemplate = xmlDocument.SelectSingleNode("//Configuration/ProfileTemplate/Profile");

            XmlNode xmlNodeProfiles = xmlDocument.SelectSingleNode("//Configuration/Profiles");

            if (copyProfile != null)
            {

                foreach (XmlNode node in xmlNodeProfiles.ChildNodes)
                {
                    if (node.SelectSingleNode("ID").InnerText == copyProfile.ID)
                    {
                        xmlNodeTemplate = node;
                        break;
                    }

                }
            }

            string newProfileName = "NewProfile";
            if (copyProfile != null) { newProfileName = copyProfile.ProfileName; }
            int countProfile = 0;
            XmlNodeList xmlNodesByName = xmlNodeProfiles.SelectNodes("Profile/ProfileName[text()='" + newProfileName + "']");

            if (xmlNodesByName.Count > 0)
            {
                while (xmlNodesByName.Count > 0)
                {
                    countProfile++;
                    xmlNodesByName = xmlNodeProfiles.SelectNodes("Profile/ProfileName[text()='" + newProfileName + countProfile.ToString() + "']");

                }
                newProfileName = newProfileName + countProfile.ToString();
            }


            XmlNode newNode = xmlDocument.CreateNode(XmlNodeType.Element, "Profile", "");
            newNode.InnerXml = xmlNodeTemplate.InnerXml;
            newNode.SelectSingleNode("ProfileName").InnerText = newProfileName;
            newNode.SelectSingleNode("ID").InnerText = getNewIDNumberForProfile(xmlDocument);
            newNode.SelectSingleNode("DefaultProfile").InnerText = "False";


            xmlNodeProfiles.AppendChild(newNode);

            Profile profile = new Profile();
            this.Add(profile);
            profile.LoadProfile(newNode.SelectSingleNode("ID").InnerText, xmlDocument);

            xmlDocument.Save(Global_Variables.Global_Variables.xmlFile);

            xmlDocument = null;

        }

        public string getNewIDNumberForProfile(XmlDocument xmlDocument)
        {
            //gets ID for new profiles
            int ID = 0;

            XmlNode xmlNode = xmlDocument.SelectSingleNode("//Configuration/Profiles");
            XmlNode xmlSelectedNode = xmlNode.SelectSingleNode("Profile/ID[text()='" + ID.ToString() + "']");

            while (xmlSelectedNode != null)
            {
                ID = ID + 1;
                xmlSelectedNode = xmlNode.SelectSingleNode("Profile/ID[text()='" + ID.ToString() + "']");
            }
            //ID++;
            return ID.ToString();

        }
    }

    public class Profile
    {
        public string ID { get; set; }
        public bool DefaultProfile
        {
            get { return defaultProfile; }
            set
            {
                defaultProfile = value;
                if (value == true) { VisibilityDefaultProfile = Visibility.Visible; } else { VisibilityDefaultProfile = Visibility.Collapsed; }
            }
        }
        public bool defaultProfile { get; set; }
        public bool activeProfile { get; set; }
        public bool ActiveProfile
        {
            get { return activeProfile; }
            set
            {
                activeProfile = value;
                if (value == true) { VisibilityActiveProfile = Visibility.Visible; } else { VisibilityActiveProfile = Visibility.Collapsed; }
            }
        }
        public Visibility VisibilityActiveProfile { get; set; } = Visibility.Collapsed;
        public Visibility VisibilityDefaultProfile { get; set; } = Visibility.Collapsed;
        public string ProfileName { get; set; } = "";
        public string Exe { get; set; } = "";
        public string Resolution { get; set; } = "";
        public string RefreshRate { get; set; } = "";
        public string Path { get; set; } = "";

        public string appType = "";
        public string AppType
        {
            get { return appType; }
            set
            {
                appType = value;
                switch (value)
                {
                    case "Steam":
                        icon = PackIconSimpleIconsKind.Steam;
                        iconMaterial = PackIconMaterialKind.None;
                        iconVisibility = Visibility.Visible;
                        break;
                    case "EpicGames":
                        icon = PackIconSimpleIconsKind.EpicGames;
                        iconMaterial = PackIconMaterialKind.None;
                        iconVisibility = Visibility.Visible;
                        break;
                    case "Exe":
                        icon = PackIconSimpleIconsKind.None;
                        iconMaterial = PackIconMaterialKind.ApplicationCogOutline;
                        iconMaterialVisibility = Visibility.Visible;
                        break;
                    default:
                        icon = PackIconSimpleIconsKind.None;
                        iconMaterial = PackIconMaterialKind.None;
                  
                        break;
                }

            }
        }

        public string GameID { get; set; } = "";
        public string Offline_TDP1 { get; set; } = "";
        public string Offline_TDP2 { get; set; } = "";
        public string Offline_ActiveCores { get; set; } = "";
        public string Offline_MAXCPU { get; set; } = "";
        public string Offline_FPSLimit { get; set; } = "";
        public string Offline_EPP { get; set; } = "";
        public string Offline_GPUCLK { get; set; } = "";
        public string Online_TDP1 { get; set; } = "";
        public string Online_TDP2 { get; set; } = "";
        public string Online_ActiveCores { get; set; } = "";
        public string Online_MAXCPU { get; set; } = "";
        public string Online_FPSLimit { get; set; } = "";
        public string Online_EPP { get; set; } = "";
        public string Online_GPUCLK { get; set; } = "";

        public PackIconSimpleIconsKind icon {get;set;}
        public PackIconMaterialKind iconMaterial {get;set;}

        public Visibility iconVisibility { get; set; } = Visibility.Collapsed;
        public Visibility iconMaterialVisibility { get; set; } = Visibility.Collapsed;
       
        public void SaveToXML()
        {
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(Global_Variables.Global_Variables.xmlFile);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("//Configuration/Profiles");
            XmlNode xmlSelectedNode = xmlNode.SelectSingleNode("Profile/ID[text()='" + ID + "']");

            if (xmlSelectedNode != null)
            {
                XmlNode parentNode = xmlSelectedNode.ParentNode;

                if (parentNode != null)
                {
                    XmlNode onlineNode = parentNode.SelectSingleNode("Online");
                    onlineNode.SelectSingleNode("TDP1").InnerText = Online_TDP1;
                    onlineNode.SelectSingleNode("TDP2").InnerText = Online_TDP2;
                    onlineNode.SelectSingleNode("ActiveCores").InnerText = Online_ActiveCores;
                    onlineNode.SelectSingleNode("MAXCPU").InnerText = Online_MAXCPU;
                    onlineNode.SelectSingleNode("FPSLimit").InnerText = Online_FPSLimit;
                    onlineNode.SelectSingleNode("EPP").InnerText = Online_EPP;
                    onlineNode.SelectSingleNode("GPUCLK").InnerText = Online_GPUCLK;

                    XmlNode offlineNode = parentNode.SelectSingleNode("Offline");
                    offlineNode.SelectSingleNode("TDP1").InnerText = Offline_TDP1;
                    offlineNode.SelectSingleNode("TDP2").InnerText = Offline_TDP2;
                    offlineNode.SelectSingleNode("ActiveCores").InnerText = Offline_ActiveCores;
                    offlineNode.SelectSingleNode("MAXCPU").InnerText = Offline_MAXCPU;
                    offlineNode.SelectSingleNode("FPSLimit").InnerText = Offline_FPSLimit;
                    offlineNode.SelectSingleNode("EPP").InnerText = Offline_EPP;
                    offlineNode.SelectSingleNode("GPUCLK").InnerText = Offline_GPUCLK;

                    XmlNode LaunchOptions = parentNode.SelectSingleNode("LaunchOptions");
                    LaunchOptions.SelectSingleNode("Resolution").InnerText = Resolution;
                    LaunchOptions.SelectSingleNode("RefreshRate").InnerText = RefreshRate;
                    LaunchOptions.SelectSingleNode("Path").InnerText = Path;
                    LaunchOptions.SelectSingleNode("AppType").InnerText = AppType;
                    LaunchOptions.SelectSingleNode("GameID").InnerText = GameID;


                    parentNode.SelectSingleNode("ProfileName").InnerText = ProfileName;
                    parentNode.SelectSingleNode("Exe").InnerText = Exe;

                    //if ID isnt 0, which is the default profile, and its been saved to be the default profile, then make this ID 0 and change the other profile
                    if (DefaultProfile.ToString() != parentNode.SelectSingleNode("DefaultProfile").InnerText && DefaultProfile)
                    {
                        //check to see if a default profile exists
                        XmlNode xmlCurrentDefault = xmlNode.SelectSingleNode("Profile/DefaultProfile[text()='True']");
                        if (xmlCurrentDefault != null)
                        {
                            //if not null set to false
                            xmlCurrentDefault.InnerText = "False";
                            //get the ID and change status in profiles list
                            string curDefID = xmlCurrentDefault.ParentNode.SelectSingleNode("ID").InnerText;
                            Global_Variables.Global_Variables.profiles.setCurrentDefaultProfileToFalse(curDefID);
                        }
                        
                    }
                    parentNode.SelectSingleNode("DefaultProfile").InnerText = DefaultProfile.ToString();

                }


            }
            xmlDocument.Save(Global_Variables.Global_Variables.xmlFile);

            xmlDocument = null;


        }

        public void LoadProfile(string loadID, XmlDocument xmlDocument=null)
        {
            if (xmlDocument == null)
            {
                xmlDocument = new System.Xml.XmlDocument();
                xmlDocument.Load(Global_Variables.Global_Variables.xmlFile);
            }


            XmlNode xmlNode = xmlDocument.SelectSingleNode("//Configuration/Profiles");
            XmlNode xmlSelectedNode = xmlNode.SelectSingleNode("Profile/ID[text()='" + loadID + "']");

            if (xmlSelectedNode != null)
            {
                XmlNode parentNode = xmlSelectedNode.ParentNode;

                if (parentNode != null)
                {
                    XmlNode onlineNode = parentNode.SelectSingleNode("Online");
                    Online_TDP1 = onlineNode.SelectSingleNode("TDP1").InnerText;
                    Online_TDP2 = onlineNode.SelectSingleNode("TDP2").InnerText;
                    Online_ActiveCores = onlineNode.SelectSingleNode("ActiveCores").InnerText;
                    Online_MAXCPU = onlineNode.SelectSingleNode("MAXCPU").InnerText;
                    Online_FPSLimit = onlineNode.SelectSingleNode("FPSLimit").InnerText;
                    Online_EPP = onlineNode.SelectSingleNode("EPP").InnerText;
                    Online_GPUCLK = onlineNode.SelectSingleNode("GPUCLK").InnerText;

                    XmlNode offlineNode = parentNode.SelectSingleNode("Offline");
                    Offline_TDP1 = offlineNode.SelectSingleNode("TDP1").InnerText;
                    Offline_TDP2 = offlineNode.SelectSingleNode("TDP2").InnerText;
                    Offline_ActiveCores = offlineNode.SelectSingleNode("ActiveCores").InnerText;
                    Offline_MAXCPU = offlineNode.SelectSingleNode("MAXCPU").InnerText;
                    Offline_FPSLimit = offlineNode.SelectSingleNode("FPSLimit").InnerText;
                    Offline_EPP = offlineNode.SelectSingleNode("EPP").InnerText;
                    Offline_GPUCLK = offlineNode.SelectSingleNode("GPUCLK").InnerText;

                    XmlNode LaunchOptions = parentNode.SelectSingleNode("LaunchOptions");
                    Resolution = LaunchOptions.SelectSingleNode("Resolution").InnerText;
                    RefreshRate = LaunchOptions.SelectSingleNode("RefreshRate").InnerText;
                    Path = LaunchOptions.SelectSingleNode("Path").InnerText;
                    AppType = LaunchOptions.SelectSingleNode("AppType").InnerText;
                    GameID = LaunchOptions.SelectSingleNode("GameID").InnerText;

                    ProfileName = parentNode.SelectSingleNode("ProfileName").InnerText;
                    Exe = parentNode.SelectSingleNode("Exe").InnerText;
                    if (parentNode.SelectSingleNode("DefaultProfile").InnerText == "True") { DefaultProfile = true; } else { DefaultProfile = false; }
                    ID = loadID;
                    

                }


            }
            
            xmlDocument = null;

        }

        public void applyProfile(bool changeDisplay=false)
        {
            //remove active profile flag for current
            if (Global_Variables.Global_Variables.profiles.activeProfile != null) 
            { 
                Global_Variables.Global_Variables.profiles.activeProfile.ActiveProfile = false;

            }
            //apply active profile flag
            Global_Variables.Global_Variables.profiles.activeProfile = this;
            ActiveProfile = true;
            string powerStatus = SystemParameters.PowerLineStatus.ToString();

            if (changeDisplay)
            {
                if (Resolution != "")
                {
                    Display_Management.Display_Management.SetDisplayResolution(Resolution);
                }
                if (RefreshRate != "")
                {
                    Display_Management.Display_Management.SetDisplayRefreshRate(RefreshRate);
                }
            }

            switch (powerStatus)
            {
                case "Online":
                    if (Online_ActiveCores != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => CoreParking_Management.CoreParking_Management.changeActiveCores(Convert.ToInt32(Online_ActiveCores))); }
                    if (Online_EPP != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => EPP_Management.EPP_Management.changeEPP(Convert.ToInt32(Online_EPP))); }
                    if (Online_FPSLimit != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => RTSS.setRTSSFPSLimit(Convert.ToInt32(Online_FPSLimit))); }
                    if (Online_GPUCLK != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => GPUCLK_Management.GPUCLK_Management.changeAMDGPUClock(Convert.ToInt32(Online_EPP))); }
                    if (Online_MAXCPU != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => MaxProcFreq_Management.MaxProcFreq_Management.changeCPUMaxFrequency(Convert.ToInt32(Online_EPP))); }
                    if (Online_TDP1 != "" && Online_TDP2 == "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => TDP_Management.TDP_Management.changeTDP(Convert.ToInt32(Online_TDP1), Convert.ToInt32(Online_TDP1))); }
                    if (Online_TDP1 != "" && Online_TDP2 != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => TDP_Management.TDP_Management.changeTDP(Convert.ToInt32(Online_TDP1), Convert.ToInt32(Online_TDP2))); }
                
                    break;
                case "Offline":
                    if (Offline_ActiveCores != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => CoreParking_Management.CoreParking_Management.changeActiveCores(Convert.ToInt32(Offline_ActiveCores))); }
                    if (Offline_EPP != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => EPP_Management.EPP_Management.changeEPP(Convert.ToInt32(Offline_EPP))); }
                    if (Offline_FPSLimit != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => RTSS.setRTSSFPSLimit(Convert.ToInt32(Offline_FPSLimit))); }
                    if (Offline_GPUCLK != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => GPUCLK_Management.GPUCLK_Management.changeAMDGPUClock(Convert.ToInt32(Offline_EPP))); }
                    if (Offline_MAXCPU != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => MaxProcFreq_Management.MaxProcFreq_Management.changeCPUMaxFrequency(Convert.ToInt32(Offline_EPP))); }
                    if (Offline_TDP1 != "" && Offline_TDP2 == "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => TDP_Management.TDP_Management.changeTDP(Convert.ToInt32(Offline_TDP1), Convert.ToInt32(Offline_TDP1))); }
                    if (Offline_TDP1 != "" && Offline_TDP2 != "") { Classes.Task_Scheduler.Task_Scheduler.runTask(() => TDP_Management.TDP_Management.changeTDP(Convert.ToInt32(Offline_TDP1), Convert.ToInt32(Offline_TDP2))); }

                    break;

                default: break;
            }


            Notification_Management.Show(Application.Current.Resources["Notification_ProfileApplied"] + ": " + ProfileName,"");

        }
    }
}
