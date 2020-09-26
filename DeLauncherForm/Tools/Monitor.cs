using System.Management;
using System.Collections.Generic;

namespace DeLauncherForm
{
    public class Monitor
    {
        public bool IsArrived = false;
        ManagementEventWatcher stopWatch;
        private List<string> ProcessName;

        public void StartMonitoring()
        {
            stopWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += stopWatch_EventArrived;
            stopWatch.Start();
        }

        private void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var processName = (string)e.NewEvent.Properties["ProcessName"].Value;
            if (ProcessName.Contains(processName))
                IsArrived = true;
        }

        public Monitor(List<string> processName)
        {
            ProcessName = processName;  
        }
    }
}
