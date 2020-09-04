using System.Threading;
using System.Diagnostics;

namespace DeLauncherForm
{
    public static class WorldBuilderLauncher
    {
        public static void LaunchWorldBuilder()
        {
            GameLauncher.SetRotrFiles();

            Process.Start(EntryPoint.WorldBuilderFile);

            var mon = new Monitor(EntryPoint.BuilderProcessTag);

            mon.StartMonitoring();

            while (!mon.IsArrived)
            {
                Thread.Sleep(5000);
            }

            GameLauncher.SetRotrFilesBack();
        }

    }
}
