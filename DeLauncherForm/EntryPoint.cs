using System;
using System.Windows.Forms;

namespace DeLauncherForm
{    
    class EntryPoint
    {
        public const int DefaultVersionNumber = -1;
        public const string LauncherFolder = @".LauncherFolder/";

        public const string configName = ".LauncherFolder/DeLauncherCfg.xml";
        public const string VersionFileName = "Uversion.xml";
        public static string[] KnownPatchTags  = { "BP", "HP", "PFB", "BR", "Hanpatch", "!!Rotr_Intrnl_Eng", "!!Rotr_Intrnl_INI" };
        public const string GameFile  = "generals.exe";
        public const string GameProcessTag  = "generals";
        public const string WorldBuilderFile  = "WorldBuilder.exe";
        public const string BuilderProcessTag  = "WorldBuilder";

        public const string HPLink = "alanblack166/Hanpatch";
        public const string BPLink = "Knjaz136/BPatch";
        public const string VanillaLink = "p0ls3r/ROTR187";


        [System.STAThreadAttribute()]
        public static void Main()
        {            
            try
            {
                var conf = ConfigurationReader.ReadConfiguration();
                if (!InstancesChecker.AlreadyRunning())
                {
                    DeLauncherForm.App app = new DeLauncherForm.App();
                    app.Run(new MainWindow(conf));
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
    }
}
