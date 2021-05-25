using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Web.Script.Serialization;

namespace DeLauncherForm
{
    public static class OptionsSetter
    {
        public static async Task CheckAndApplyOptions(LaunchOptions options)
        {
            if (options.FixFile && !(File.Exists("d3d8to9.dll") || File.Exists("d3d8x.dll")))
            {
                await LoadContent("p0ls3r/d3d8");
            }
            

            if (options.FixFile && (options.Gentool == GentoolsMode.AutoUpdate || File.Exists("d3d8.dll")))
            {
                if (File.Exists("d3d8to9.dll"))
                    File.Move("d3d8to9.dll", "d3d8x.dll");
            }

            if (options.FixFile && options.Gentool == GentoolsMode.Disable )
            {
                if (File.Exists("d3d8x.dll"))
                    File.Move("d3d8x.dll", "d3d8to9.dll");

                if (File.Exists("d3d8to9.dll"))
                    File.Move("d3d8to9.dll", "d3d8.dll");
            }

            if (!options.FixFile)
            {
                if (File.Exists("d3d8to9.dll"))
                    File.Delete("d3d8to9.dll");
                if (File.Exists("d3d8x.dll"))
                    File.Delete("d3d8x.dll");
                if (File.Exists("d3d8.dll") && options.Gentool == GentoolsMode.Disable)
                    File.Delete("d3d8.dll");
            }

            if (options.DebugFile && File.Exists("dbghelp.dll"))
                File.Delete("dbghelp.dll");

            if (options.ModdedExe && !File.Exists("modded.exe"))
                await LoadContent("p0ls3r/moddedExe");
        }

        public static async Task LoadContent(string repos)
        {
            foreach (var parsedData in GetRepoContent(repos))
            {
                if ((string)parsedData["type"] == "file")
                {
                    var downloadUrl = (string)parsedData["download_url"];
                    var fileName = (string)parsedData["name"]+"temp";

                    await DownLoad(downloadUrl, fileName);
                    File.Move(fileName, fileName.Substring(0, fileName.Length - 4));
                }
            }
        }

        private static IEnumerable<Dictionary<string, object>> GetRepoContent(string repo)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("DeLauncherForm", typeof(EntryPoint).Assembly.GetName().Version.ToString()));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var contentsUrl = $"https://api.github.com/repos/{repo}/contents";

            dynamic contents = new JavaScriptSerializer().DeserializeObject(httpClient.GetStringAsync(contentsUrl).GetAwaiter().GetResult());

            foreach (var content in contents)
            {
                yield return (Dictionary<string, object>)content;
            }
        }

        private static Task DownLoad(string downloadUrl, string fileName)
        {
            var uri = new Uri(downloadUrl);
            using (WebClient client = new WebClient())
            {
                var task = client.DownloadFileTaskAsync(uri, fileName);
                return task;
            }
        }
    }
}
