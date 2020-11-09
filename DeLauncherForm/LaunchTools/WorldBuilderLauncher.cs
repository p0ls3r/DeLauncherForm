using System.Threading;
using System.Diagnostics;

namespace DeLauncherForm
{
    public static class WorldBuilderLauncher
    {
        public static void LaunchWorldBuilder(FormConfiguration conf)
        {
            GameLauncher.SetRotrFiles();

            var processID = Process.Start(EntryPoint.WorldBuilderFile).Id;

            var mon = new Monitor(processID);

            mon.StartMonitoring();

            while (!mon.IsArrived)
            {
                Thread.Sleep(5000);
            }

            GameLauncher.SetRotrFilesBack();
        }

    }
}
