﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

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

        static void webClient_DownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            var archiveName = CurrentFileName + "temp";

            using (ZipArchive archive = ZipFile.OpenRead(archiveName))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName[0] == '!' && entry.FullName[1] == '!' && entry.FullName[2] == '!' && entry.FullName[3] == '!')
                        entry.ExtractToFile(entry.FullName, true);
                    else
                        entry.ExtractToFile("!!!!" + entry.FullName, true);
                }
            }

            if (File.Exists(archiveName))
                File.Delete(archiveName);          
        }

        //Получить последнюю номер последней версии патча из репозитория
        public static int GetLatestPatchNumber(Patch patch)
        {
            //особый кейс
            if (patch is Vanilla)
                return new Vanilla().PatchVersion;            

            int versionNumber = 0;

            foreach (var parsedData in GetRepoContent(patch))
            {
                var fileName = (string)parsedData["name"];
                if ((fileName[fileName.Length - 1] == 'g' && fileName[fileName.Length - 2] == 'i' && fileName[fileName.Length - 3] == 'b' && fileName[fileName.Length - 4] == '.') ||
                    (fileName[fileName.Length - 1] == 'p' && fileName[fileName.Length - 2] == 'i' && fileName[fileName.Length - 3] == 'z' && fileName[fileName.Length - 4] == '.'))
                    versionNumber = LocalFilesWorker.GetVersionNumberFromPatchName(fileName);
            }

            return versionNumber;
        }

        public static async Task LoadActualPatch(Patch patch)
        {
            if (patch is None)
                return;            

            foreach (var parsedData in GetRepoContent(patch))
            {
                if ((string)parsedData["type"] == "file")
                {
                    var downloadUrl = (string)parsedData["download_url"];
                    var fileName = (string)parsedData["name"];
                     
                    await DownLoad(downloadUrl, fileName);
                    sw.Reset();

                    if (File.Exists(fileName + "temp") && !File.Exists(fileName))
                        File.Move(fileName + "temp", fileName);
                    else File.Delete(fileName + "temp");
                }
            }
        }

        private static IEnumerable<Dictionary<string, object>> GetRepoContent(Patch patch)
        {
            var repo = patch.Repository;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("DeLauncherForm", typeof(EntryPoint).Assembly.GetName().Version.ToString()));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var contentsUrl = $"https://api.github.com/repos/{repo}/contents";

            dynamic contents = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(httpClient.GetStringAsync(contentsUrl).GetAwaiter().GetResult());

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
                client.DownloadProgressChanged += webClient_DownloadProgressChanged;
                if ((fileName[fileName.Length - 1] == 'p' && fileName[fileName.Length - 2] == 'i' && fileName[fileName.Length - 3] == 'z'))
                    client.DownloadFileCompleted += webClient_DownloadComplete;

                sw.Start();
                var task = client.DownloadFileTaskAsync(uri, fileName + "temp");
                return task;
            }
        }
    }
}
