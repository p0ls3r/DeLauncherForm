using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace DeLauncherForm
{
    static class IPChecker
    {
        public static bool IsCurrentUserBanned()
        {
            var strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            foreach (var ip in addr)
                foreach (var bannedIp in EntryPoint.BannedAdresses)
                    if (ip.Equals(bannedIp))
                        return true;
            return false;

        }
    }
}
