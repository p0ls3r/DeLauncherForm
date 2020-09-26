using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.ComponentModel;
using System.IO.Compression;
using System.Threading.Tasks;

namespace DeLauncherForm
{
    static class GentoolsUpdater
    {
        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetFileVersionInfoSize(string lptstrFilename, out int lpdwHandle);
        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetFileVersionInfo(string lptstrFilename, int dwHandle, int dwLen, byte[] lpData);
        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool VerQueryValue(byte[] pBlock, string lpSubBlock, out IntPtr lplpBuffer, out int puLen);

        static string genToolZipFileName = "";
        static string gentoolFileName = "d3d8.dll";
        public static async Task CheckAndUpdateGentools(LaunchOptions options)
        {
            if (options.Gentool == GentoolsMode.Current)
                return;

            if (options.Gentool == GentoolsMode.Disable)
            {
                if (File.Exists(gentoolFileName))
                    File.Delete(gentoolFileName);
                return;
            }

            genToolZipFileName = GetGentoolDownloadName();

            if (GetCurrentGentoolVersion() < GetGentoolLatestVersion(genToolZipFileName))
                await Download();
        }

        public static async Task Download()
        {
            genToolZipFileName = GetGentoolDownloadName();
            string gtURL = "http://www.gentool.net/download/" + genToolZipFileName;
            await DownloadGentool(gtURL);
        }

        public static bool isGentoolInstalled(string gentoolPath)
        {
            try
            {
                var size = GetFileVersionInfoSize(gentoolPath, out _);
                if (size == 0) { throw new Win32Exception(); };
                var bytes = new byte[size];
                bool success = GetFileVersionInfo(gentoolPath, 0, size, bytes);
                if (!success) { throw new Win32Exception(); }

                VerQueryValue(bytes, @"\StringFileInfo\040904E4\ProductName", out IntPtr ptr, out _);
                return Marshal.PtrToStringUni(ptr) == "GenTool";
            }
            catch
            {
                return false;
            }
        }

        public static bool isGentoolOutdated(string gentoolPath)
        {
            try
            {
                var size = GetFileVersionInfoSize(gentoolPath, out _);
                if (size == 0)
                    throw new Win32Exception();

                var bytes = new byte[size];
                var success = GetFileVersionInfo(gentoolPath, 0, size, bytes);
                if (!success)
                    throw new Win32Exception();

                // 040904E4 US English + CP_USASCII
                VerQueryValue(bytes, @"\StringFileInfo\040904E4\ProductVersion", out IntPtr ptr, out _);
                var currentVersion = int.Parse(Marshal.PtrToStringUni(ptr));

                var latestVersion = GetGentoolLatestVersion(genToolZipFileName);

                return latestVersion > currentVersion;
            }
            catch
            {
                return true;
            }
        }

        private static int GetGentoolLatestVersion(string unparsedName)
        {
            var parsedVersion = unparsedName.Substring(0, unparsedName.Length - 4).Substring(9);
            var versionNumbers = parsedVersion.Split('.');

            var stringVersion = "";

            foreach (var element in versionNumbers)
                stringVersion += element;

            return int.Parse(stringVersion);
        }


        private static string GetGentoolDownloadName()
        {
            WebRequest request = WebRequest.Create("http://www.gentool.net/download/patch");
            WebResponse response = request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("GenTool_v"))
                        {
                            return line;
                        }
                    }
                }
            }

            throw new ApplicationException("Unable to get Gentools version!");
        }

        public static int GetCurrentGentoolVersion()
        {
            try
            {
                var size = GetFileVersionInfoSize(gentoolFileName, out _);
                if (size == 0) { throw new Win32Exception(); };
                var bytes = new byte[size];
                bool success = GetFileVersionInfo(gentoolFileName, 0, size, bytes);
                if (!success) { throw new Win32Exception(); }

                // 040904E4 US English + CP_USASCII
                VerQueryValue(bytes, @"\StringFileInfo\040904E4\ProductVersion", out IntPtr ptr, out _);
                return int.Parse(Marshal.PtrToStringUni(ptr));
            }
            catch 
            {
                return -1;
            }
        }


        public static Task DownloadGentool(string url)
        {
            WebClient gtwc = new WebClient();
            gtwc.DownloadFileCompleted += new AsyncCompletedEventHandler(gtwc_DownloadCompleted);

            return gtwc.DownloadFileTaskAsync(new Uri(url), genToolZipFileName);
        }

        public static void gtwc_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string zipPath = genToolZipFileName;

            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries.Where(a => a.FullName.Contains("d3d8.dll")))
                {
                    entry.ExtractToFile(entry.FullName, true);
                }
            }

            File.Delete(genToolZipFileName);

            Console.WriteLine("Downloaded and Extracted");
        }
    }
}
