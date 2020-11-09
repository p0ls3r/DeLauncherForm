using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;

namespace DeLauncherForm
{
    class ConnectionChecker
    {
        public enum ConnectionStatus
        {
            NotConnected,
            Connected
        }

        public static async Task<ConnectionStatus> CheckConnection(string url)
        {
            System.Net.Http.HttpClient client = new HttpClient();

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                return ConnectionStatus.Connected;
            }
            catch
            {
                return ConnectionStatus.NotConnected;
            }
        }
    }
}
