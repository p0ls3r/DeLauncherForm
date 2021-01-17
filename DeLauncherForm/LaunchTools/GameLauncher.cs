using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DeLauncherForm
{
    static class GameLauncher
    {               
        public static void Launch(FormConfiguration conf, LaunchOptions options, bool worldBuilderLaunch)
        {
            LocalFilesWorker.SetROTRFiles(conf);

            var id = StartExe(conf, options, worldBuilderLaunch);

            var mon = new Monitor(id);
            mon.StartMonitoring();

            while (!mon.IsArrived)
            {
                Thread.Sleep(5000);
            }

            LocalFilesWorker.SetROTRFilesBack();
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

        private static int StartExe(FormConfiguration conf, LaunchOptions options, bool worldBuilderLaunch)
        {
            Process process;

            if (worldBuilderLaunch)
            {
                process = Process.Start(EntryPoint.WorldBuilderFile);
                return process.Id;
            }

            var parameters = "";

            if (conf.Windowed)
                parameters += "-win ";
            if (conf.QuickStart)
                parameters += "-quickstart";            

            if(!options.ModdedExe)
                process = Process.Start(EntryPoint.GameFile, parameters);
            else
                process = Process.Start(EntryPoint.ModdedGameFile, parameters);


            return process.Id;
        }
    }
}
