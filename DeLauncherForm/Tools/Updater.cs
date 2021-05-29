using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeLauncherForm
{
    static class Updater
    {
        const string repo = "p0ls3r/LauncherUpdater";
        const string tempPrefix = "temp";

        public static int GetLatestVersionNumber()
        {
            Console.WriteLine("Checking version...");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("DeLauncher", typeof(EntryPoint).Assembly.GetName().Version.ToString()));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var contentsUrl = $"https://api.github.com/repos/{repo}/contents";

            var contentsJson = httpClient.GetStringAsync(contentsUrl).GetAwaiter().GetResult();

            dynamic contents = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(contentsJson);

            foreach (var data in contents)
            {
                var parsedData = (Dictionary<string, object>)data;

                var fileName = (string)data["name"];

                if (fileName == EntryPoint.VersionFileName)
                {
                    var downloadUrl = (string)data["download_url"];
                    var myWebClient = new WebClient();
                    var name = tempPrefix + fileName;

                    using (WebClient client = new WebClient())
                        client.DownloadFile(downloadUrl, name);

                    var newVersionNumber = XmlData.GetVersionFromXml(name);

                    File.Delete(name);

                    return newVersionNumber;
                }
            }
            throw new ApplicationException("version file not found!");
        }

        public static void DownloadUpdate()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("DeLauncher", typeof(EntryPoint).Assembly.GetName().Version.ToString()));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var contentsUrl = $"https://api.github.com/repos/{repo}/contents";

            var contentsJson = httpClient.GetStringAsync(contentsUrl).GetAwaiter().GetResult();

            dynamic contents = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(contentsJson);

            using (WebClient client = new WebClient())
            {
                foreach (var data in contents)
                {
                    var parsedData = (Dictionary<string, object>)data;

                    if ((string)parsedData["type"] == "file")
                    {
                        var downloadUrl = (string)parsedData["download_url"];
                        Console.WriteLine($"Download: {downloadUrl}");
                        var fileName = (string)parsedData["name"];

                        if (fileName == EntryPoint.VersionFileName)
                            client.DownloadFile(new Uri(downloadUrl), EntryPoint.LauncherFolder + fileName);
                        else
                            client.DownloadFile(new Uri(downloadUrl), fileName);
                    }
                }
            }
        }
    }
}
