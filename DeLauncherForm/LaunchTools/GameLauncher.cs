using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DeLauncherForm
{
    static class GameLauncher
    {
        public static void Launch(FormConfiguration conf, LaunchOptions options, bool worldBuilderLaunch)
        {
            LocalFilesWorker.SetGameFiles(conf);
            if (options.DeleteOldVersions)
                LocalFilesWorker.DeleteAllOldPatchFiles(conf.Patch);

            if (conf.Patch is BPatch && IPChecker.IsCurrentUserBanned())
            {
                LocalFilesWorker.DeleteAllPatchFiles(conf.Patch);
                MessageBox.Show(" Sorry, but infantry is countered by antiInfantry, DeLauncher counters BP Bruce now. \r Nonsense: Russian sanctions against an American, lol) \r If you are not Bruce, sorry, send a private message to DeL, i'll fix that");
                return;
            }

            var id = StartExe(conf, options, worldBuilderLaunch);

            var mon = new Monitor(id);
            mon.StartMonitoring();

            while (!mon.IsArrived)
            {
                Thread.Sleep(5000);
            }

            LocalFilesWorker.SetGameFilesBack();
        }

        public static async Task PrepareWithUpdate(FormConfiguration conf)
        {
            await ReposWorker.LoadActualPatch(conf.Patch);            
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
                parameters += "-quickstart ";
            if (conf.particleEdit)
                parameters += "-particleEdit ";
            if (conf.scriptDebug)
                parameters += "-scriptDebug";


            if (!options.ModdedExe)
                process = Process.Start(EntryPoint.GameFile, parameters);
            else
                process = Process.Start(EntryPoint.ModdedGameFile, parameters);


            return process.Id;
        }
    }
}
