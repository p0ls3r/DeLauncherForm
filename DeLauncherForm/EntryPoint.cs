using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace DeLauncherForm
{
    class EntryPoint
    {
        public const int DefaultVersionNumber = -1;
        public const string LauncherFolder = @".LauncherFolder/";

        public const string configName = ".LauncherFolder/DeLauncherCfg.xml";
        public const string optionsName = ".LauncherFolder/DeLauncherOpt.xml";
        public const string VersionFileName = "Uversion.xml";
        public static string[] KnownPatchTags = { "BP", "HP", "PFB", "BR", "Hanpatch", "!!Rotr_Intrnl_Eng", "!!Rotr_Intrnl_INI" };

        public const string GameFile = "generals.exe";
        public const string ModdedGameFile = "modded.exe";

        //public const string GameFile = "Kat.exe";
        //public const string ModdedGameFile = "Kat.exe";

        //public static List<string> GameProcessTags = new List<string> { "Kat.exe" };
        public static List<string> GameProcessTags = new List<string> { "generals.exe", "Generals.exe", "generals", "Generals", "modded.exe", "modded" };
        public const string WorldBuilderFile  = "WorldBuilder.exe";
        public static List<string> BuilderProcessTags  = new List<string> { "WorldBuilder.exe", "worldBuilder.exe", "worldbuilder.exe", "WorldBuilder", "worldBuilder", "worldbuilder" };

        public const string HPLink = "alanblack166/Hanpatch";
        public const string BPLink = "Knjaz136/BPatch";
        public const string VanillaLink = "p0ls3r/ROTR187";


        [System.STAThreadAttribute()]
        public static void Main()
        {            
            try
            {
                var conf = XMLReader.ReadConfiguration();
                var opt = XMLReader.ReadOptions();

                CheckDbgCrash();

                if (!InstancesChecker.AlreadyRunning())
                {
                    DeLauncherForm.App app = new DeLauncherForm.App();
                    app.Run(new MainWindow(conf, opt));
                }
                else
                {                    
                    DeLauncherForm.App app = new DeLauncherForm.App();
                    var window = new AbortWindow(conf)
                    {
                        WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen
                    };
                    app.Run(window);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred/Произошла непредвиденная ошибка: " + ex.Message);                
            }            
        }

        public static void CheckDbgCrash()
        {
            if (File.Exists("dbghelp.dll"))
                File.Delete("dbghelp.dll");
        }
    }
}
