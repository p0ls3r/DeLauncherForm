using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DeLauncherForm
{
    static class GameLauncher
    {               
        public static void Launch(FormConfiguration conf, LaunchOptions options)
        {
            SetRotrFiles();

            StartGame(conf, options);

            var mon = new Monitor(EntryPoint.GameProcessTags);
            mon.StartMonitoring();

            while (!mon.IsArrived)
            {
                Thread.Sleep(5000);
            }

            SetRotrFilesBack();
        }

        public static async Task PrepareWithUpdate(FormConfiguration conf)
        {            
            LocalFilesWorker.RemoveOldVersions(conf, null);
            await ReposWorker.LoadActualPatch(conf);
        }
        public static void PrepareWithoutUpdate(FormConfiguration conf, PatchInfo exceptionFile)
        {            
            LocalFilesWorker.RemoveOldVersions(conf, exceptionFile);
        }

        public static void SetRotrFiles()
        {
            LocalFilesWorker.SetROTRFiles();
        }

        public static void SetRotrFilesBack()
        {
            LocalFilesWorker.SetROTRFilesBack();
        }

        private static void StartGame(FormConfiguration conf, LaunchOptions options)
        {            
            var parameters = "";

            if (conf.Windowed)
                parameters += "-win ";
            if (conf.QuickStart)
                parameters += "-quickstart";

            if(!options.ModdedExe)
                Process.Start(EntryPoint.GameFile, parameters);
            else
                Process.Start(EntryPoint.ModdedGameFile, parameters);
        }
    }
}
