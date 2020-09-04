using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DeLauncherForm
{
    static class ReposWorker
    {    
        public static event Action<int> DownloadStatusChanged;
        
        public static string CurrentFileName { get; private set; }

        private static Stopwatch sw = new Stopwatch();        
        static void webClient_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {            
            var percentage = e.ProgressPercentage;
                       
            DownloadStatusChanged(percentage);
        }

        public static PatchInfo GetLatestPatchInfo(FormConfiguration conf)
        {
            //особый кейс
            if (conf.Patch is Vanilla)
                return new PatchInfo(new Vanilla());            

            int versionNumber = 0;

            foreach (var parsedData in GetRepoContent(conf))
            {
                var fileName = (string)parsedData["name"];
                if (fileName[fileName.Length - 1] == 'g' && fileName[fileName.Length - 2] == 'i' && fileName[fileName.Length - 3] == 'b' && fileName[fileName.Length - 4] == '.')
                    versionNumber = LocalFilesWorker.GetVersionNumberFromPatchName(fileName);
            }

            if (conf.Patch is BPatch)
                return new PatchInfo(new BPatch(versionNumber));

            if (conf.Patch is HPatch)
                return new PatchInfo(new HPatch(versionNumber));

            return new PatchInfo(new None());
        }

        public static async Task LoadActualPatch(FormConfiguration conf)
        {
            if (conf.Patch is None)
                return;            

            foreach (var parsedData in GetRepoContent(conf))
            {
                if ((string)parsedData["type"] == "file")
                {
                    var downloadUrl = (string)parsedData["download_url"];
                    var fileName = (string)parsedData["name"];

                    //если запрашиваем не ваннилу, а БП или ХП, то скачиваем с пометкой темп и создаём !!!!
                    if (!(conf.Patch is Vanilla) && ((fileName[0] == 'H' && fileName[1] == 'P') || (fileName[0] == 'B' && fileName[1] == 'P')))
                    {
                        await DownLoad(downloadUrl, fileName);
                        sw.Reset();

                        if (File.Exists("!!!!" + fileName))
                            File.Delete("!!!!" + fileName);

                        File.Move(fileName + "temp", "!!!!" + fileName);
                    }
                    //иначе скачиваем без !!!!
                    else
                    {
                        await DownLoad(downloadUrl, fileName);
                        sw.Reset();

                        if (File.Exists(fileName))
                            File.Delete(fileName);

                        File.Move(fileName + "temp", fileName);
                    }
                }
            }
        }

        private static IEnumerable<Dictionary<string, object>> GetRepoContent(FormConfiguration conf)
        {
            var repo = SetRepository(conf);

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
            CurrentFileName = fileName;
            using (WebClient client = new WebClient())
            {                
                client.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
                sw.Start();
                var task = client.DownloadFileTaskAsync(uri, fileName + "temp");
                return task;
            }
        }
        private static string SetRepository(FormConfiguration conf)
        {
            return conf.Patch.Repository;
        }
    }
}
