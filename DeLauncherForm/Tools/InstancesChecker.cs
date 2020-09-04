using System;
using System.Diagnostics;

namespace DeLauncherForm
{
    static class InstancesChecker
    {
        public static bool AlreadyRunning()
        {
            Process[] processes = Process.GetProcesses();
            Process currentProc = Process.GetCurrentProcess();
            foreach (Process process in processes)
            {
                if (currentProc.ProcessName == process.ProcessName && currentProc.Id != process.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
