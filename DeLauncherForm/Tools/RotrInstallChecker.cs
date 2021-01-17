using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace DeLauncherForm.Tools
{
    public static class RotrInstallChecker
    {        
        public static void CheckROTRInstallation()
        {
            //Tests
            if (File.Exists("GodMode13.debug")) return;

            //1.87
            if (!File.Exists("!!Rotr_Intrnl_Main.gib") && !File.Exists("!!Rotr_Intrnl_Main.big"))
                ErrorMessage("ROTR 1.87 PB 2.0 not detected. Move the Delauncher.exe to the game with mod folder, or install/reinstall ROTR mod \r" +
                    "ROTR 1.87 PB 2.0 не обнаружен. Переместите файл Delauncher.exe в папку с игрой и модом или установите/переустановите ROTR мод", "Installation error");

            if ((File.Exists("!!Rotr_Intrnl_Main.gib") && ComputeMD5Checksum("!!Rotr_Intrnl_Main.gib") != "A66352911B951B82952EA3C1C0F9F2E9") ||
                (File.Exists("!!Rotr_Intrnl_Main.big") && ComputeMD5Checksum("!!Rotr_Intrnl_Main.big") != "A66352911B951B82952EA3C1C0F9F2E9"))
                ErrorMessage("ROTR 1.87 PB 2.0 not detected. Move the Delauncher.exe to the game with mod folder, or install/reinstall ROTR mod \r" +
                    "ROTR 1.87 PB 2.0 не обнаружен. Переместите файл Delauncher.exe в папку с игрой и модом или установите/переустановите ROTR мод", "Installation error");

            //1.86
            if (!File.Exists("!!Rotr_Patch.gib") && !File.Exists("!!Rotr_Patch.big"))
                ErrorMessage("ROTR 1.86 not detected. Move the Delauncher.exe to the game with mod folder, or install/reinstall ROTR mod \r" +
                    "ROTR 1.86 не обнаружен. Переместите файл Delauncher.exe в папку с игрой и модом или установите/переустановите ROTR мод", "Installation error");

            //1.85
            if (!File.Exists("!Rotr_Textures.gib") && !File.Exists("!Rotr_Textures.big"))
                ErrorMessage("ROTR 1.85 not detected. Move the Delauncher.exe to the game with mod folder, or install/reinstall ROTR mod \r" +
                    "ROTR 1.85 не обнаружен. Переместите файл Delauncher.exe в папку с игрой и модом или установите/переустановите ROTR мод", "Installation error");

            //ZH
            if (!File.Exists("TerrainZH.big"))
                ErrorMessage("C&C Generals Zero Hour not detected. Move the Delauncher.exe to the game folder, or install/reinstall the game and ROTR mod \r" +
                    "C&C Generals Zero Hour не обнаружен. Переместите файл Delauncher.exe в папку с игрой или установите/переустановите игру и ROTR мод", "Installation error");
        }

        private static void ErrorMessage(string message, string caption)
        {
            var buttonsError = MessageBoxButtons.OK;
            var result = MessageBox.Show(message, caption, buttonsError);

            if (result == DialogResult.OK)
            {
                SelfDestroy();
            }
        }

        private static void SelfDestroy()
        {
            string BatFileName = Guid.NewGuid().ToString() + ".bat"; // батник с уникальным именем
            string ExutetableFileName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);// наш exe файл
            Directory.Delete(EntryPoint.LauncherFolder, true);

            using (StreamWriter SW = new StreamWriter(BatFileName))
            {
                SW.WriteLine(String.Format("del \"{0}\" \r\n del \"{1}\"", ExutetableFileName, BatFileName));// удалить себя, удалить батник                    
                SW.Flush();
                SW.Close();
            }
            Process.Start(new ProcessStartInfo() { UseShellExecute = false, FileName = BatFileName, CreateNoWindow = true });// запустить без создания окна
            Environment.Exit(0);
        }

        private static string ComputeMD5Checksum(string path)
        {
            using (FileStream fs = System.IO.File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                return result;
            }
        }
    }    
}



