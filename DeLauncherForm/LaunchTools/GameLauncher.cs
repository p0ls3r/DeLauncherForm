using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeLauncherForm
{
    static class GameLauncher
    {        
        public static async Task PrepareWithUpdate(FormConfiguration conf)
        {
            SetRotrFiles();
            LocalFilesWorker.RemoveOldVersions(conf, null);
            await ReposWorker.LoadActualPatch(conf);
        }

        public static void Launch(FormConfiguration conf)
        {
            StartGame(conf);

            var mon = new Monitor(EntryPoint.GameProcessTag);
            mon.StartMonitoring();

            while (!mon.IsArrived)
            {
                Thread.Sleep(5000);
            }

            SetRotrFilesBack();
        }

        public static void PrepareWithoutUpdate(FormConfiguration conf, PatchInfo exceptionFile)
        {
            SetRotrFiles();
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

        private static void StartGame(FormConfiguration conf)
        {            
            var parameters = "";

            if (conf.Windowed)
                parameters += "-win ";
            if (conf.QuickStart)
                parameters += "-quickstart";

            Process.Start(EntryPoint.GameFile, parameters);
        }
    }
}
