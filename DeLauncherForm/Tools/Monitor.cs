using System.Management;
using System.Collections.Generic;

namespace DeLauncherForm
{
    public class Monitor
    {
        public bool IsArrived = false;
        ManagementEventWatcher stopWatch;
        private int targetProcessID;

        public void StartMonitoring()
        {
            stopWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += stopWatch_EventArrived;
            stopWatch.Start();
        }

        private void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var processId = (uint)e.NewEvent.Properties["ProcessId"].Value;
            
            if (processId == targetProcessID)
                IsArrived = true;
        }

        public Monitor(int id)
        {
            targetProcessID = id;
        }
    }
}
