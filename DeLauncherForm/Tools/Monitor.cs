using System.Management;

namespace DeLauncherForm
{
    public class Monitor
    {
        public bool IsArrived = false;
        ManagementEventWatcher stopWatch;
        private string ProcessName;

        public void StartMonitoring()
        {
            stopWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += stopWatch_EventArrived;
            stopWatch.Start();
        }

        private void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var processName = (string)e.NewEvent.Properties["ProcessName"].Value;
            if (processName.Contains(ProcessName))
                IsArrived = true;
        }

        public Monitor(string processName)
        {
            ProcessName = processName;  
        }
    }
}
