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
        public const string WorldBuilderFile = "WorldBuilder.exe";
        public const string HPLogURL = "https://docs.google.com/document/d/1ZMlVFDPf4SDD5Y6vYatOCtaudBBl32gdWg-YrswvnGo/edit?usp=sharing";
        public const string BPLogURL = "https://docs.google.com/document/d/1iN2Zbl7i46RHSk-X9ewuYlaN8GrM_W0t2FHsP3KhMf0/edit?usp=sharing";

        public const string HPLink = "alanblack166/Hanpatch";        
        public const string BPLink = "Knjaz136/BPatch";
        public const string VanillaLink = "p0ls3r/ROTR187";

        public const double Volume1 = 0.25;
        public const double Volume2 = 0.2;

        [System.STAThreadAttribute()]
        public static void Main()
        {            
            try
            {
                Tools.RotrInstallChecker.CheckROTRInstallation();

                CheckDbgCrash();                
                var conf = XmlData.ReadConfiguration();
                var opt = XmlData.ReadOptions();                

                if (!InstancesChecker.AlreadyRunning())
                {
                    SoundsExtractor.ExtractSounds();
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
