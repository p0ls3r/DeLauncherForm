using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace DeLauncherForm
{
    static class LocalFilesWorker
    {
        private static string[] rotrFiles = new string[] { "!!Rotr_Intrnl_AI", "!Rotr_Textures", "!!Rotr_Intrnl_Main", "!Rotr_Audio", "!Rotr_Maps", "!Rotr_Voice", "!!Rotr_Patch",
        "!Rotr_Blckr","!Rotr_Music","!Rotr_W3D","!Rotr_2D","!Rotr_English","!Rotr_Terrain","!Rotr_Window"};
  
        public static void SetROTRFiles(FormConfiguration conf)
        {
            Switch("gib", "big");

            foreach (var exceptionFile in conf.Patch.ExceptionFiles)
            {
                if (File.Exists(exceptionFile + ".big"))
                    File.Move(exceptionFile + ".big", exceptionFile + ".gib");
            }

            RenameScriptFiles();
            RenameWindowFiles();
        }

        public static void SetROTRFilesBack()
        {
            Switch("big", "gib");
            RenameScriptFilesBack();
            RenameWindowFilesBack();
            RemoveBigs();
        }

        public static void RemoveBigs()
        {
            var filesBig = Directory.GetFiles(Directory.GetCurrentDirectory(), "*big");

            //проходим по всем биг файлам
            foreach (var bigFile in filesBig)
            {
                var fileAttributes = bigFile.Split('\\');

                //отделяем имя файла без полного адреса в ОС
                var fileBig = fileAttributes[fileAttributes.Length - 1];

                if (fileBig[0] == '!')
                {
                    //проходим по списку имен патчей
                    foreach (var patchFile in EntryPoint.KnownPatchTags)
                    {
                        //если файл оказался известным патчем и НЕ является актуальным из actualPatch переименовываем его или удаляем                        
                        if (fileBig.Contains(patchFile))
                        {
                            var file = fileBig.Substring(0, fileBig.Length - 4);
                            if (File.Exists(file + ".gib"))
                                File.Delete(fileBig);
                            else
                                File.Move(fileBig, file + ".gib");
                        }
                    }
                }
            }
        }

        public static void RemoveOldVersions(FormConfiguration conf, PatchInfo actualPatch)
        {
            if (conf.Patch is None)
                return;

            var filesBig = Directory.GetFiles(Directory.GetCurrentDirectory(), "*big");

            //проходим по всем биг файлам
            foreach (var bigFile in filesBig)
            {
                var fileAttributes = bigFile.Split('\\');

                //отделяем имя файла без полного адреса в ОС
                var fileBig = fileAttributes[fileAttributes.Length - 1];

                //если файл оказался актуальной версией, пропускаем итерацию                
                if (actualPatch != null && CheckFileNameForNameMatch(fileBig, actualPatch))
                    continue;

                if (fileBig[0] == '!')
                {
                    //проходим по списку имен патчей
                    foreach (var patchFile in EntryPoint.KnownPatchTags)
                    {
                        //если файл оказался известным патчем и НЕ является актуальным из actualPatch переименовываем его или удаляем                        
                        if (fileBig.Contains(patchFile))
                        {
                            var file = fileBig.Substring(0, fileBig.Length - 4);
                            if (File.Exists(file + ".gib"))
                                File.Delete(fileBig);
                            else
                                File.Move(fileBig, file + ".gib");
                        }
                    }
                }
            }
        }     

        //рефакторинг, сделать метод универсальным
        public static bool GetCurrentVersionNumberFromGib(PatchInfo actualPatch, bool deleteOldVersions)
        {
            //отдельный кейс для ваниллы
            if (actualPatch.Patch is Vanilla && File.Exists("!!Rotr_Intrnl_INI.gib") && File.Exists("!!Rotr_Intrnl_Eng.gib"))
            {
                File.Move("!!Rotr_Intrnl_INI.gib", "!!Rotr_Intrnl_INI.big");
                File.Move("!!Rotr_Intrnl_Eng.gib", "!!Rotr_Intrnl_Eng.big");
                return true;
            }
            else
                if (actualPatch.Patch is Vanilla)
                return false;

            //кейс для других патчей
            var filesGib = Directory.GetFiles(Directory.GetCurrentDirectory(), "*gib");
            //проходим по всем файлам гиб директории
            foreach (var gibFile in filesGib)
            {
                var fileAttributes = gibFile.Split('\\');

                //отделяем имя файла без полного адреса в ОС
                var fileGib = fileAttributes[fileAttributes.Length - 1];

                if (CheckFileNameForNameMatch(fileGib, actualPatch))
                {
                    var number = GetVersionNumberFromPatchName(fileGib);
                    if (number == actualPatch.Patch.PatchVersion)
                    {
                        var file = fileGib.Substring(0, fileGib.Length - 4);
                        if (!File.Exists(file + ".big"))
                            File.Move(fileGib, file + ".big");
                        return true;
                    }
                    else
                        if (deleteOldVersions && File.Exists(fileGib))
                            File.Delete(fileGib);
                }                
            }           
            return false;
        }
        
        public static void ClearTempFiles()
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*temp");
            foreach (var file in files)
                File.Delete(file);
        }

        public static int GetVersionNumberFromPatchName(string name)
        {
            var nameParts = name.Split('.');
            var number = "0";
            int b;
            foreach (var part in nameParts)
            {
                if (int.TryParse(part, out b))
                    number += part;
            }

            return int.Parse(number);
        }

        private static void RenameScriptFiles()
        {
            var path = "Data\\Scripts\\";
            if (File.Exists(path + "MultiplayerScripts.scb"))
                File.Move(path + "MultiplayerScripts.scb", path + "MultiplayerScripts.nope");

            if (File.Exists(path + "Scripts.ini"))
                File.Move(path + "Scripts.ini", path + "Scripts.nope");

            if (File.Exists(path + "SkirmishScripts.scb"))
                File.Move(path + "SkirmishScripts.scb", path + "SkirmishScripts.nope");
        }

        private static void RenameScriptFilesBack()
        {
            var path = "Data\\Scripts\\";
            if (File.Exists(path + "MultiplayerScripts.nope"))
                File.Move(path + "MultiplayerScripts.nope", path + "MultiplayerScripts.scb");

            if (File.Exists(path + "Scripts.nope"))
                File.Move(path + "Scripts.nope", path + "Scripts.ini");

            if (File.Exists(path + "SkirmishScripts.nope"))
                File.Move(path + "SkirmishScripts.nope", path + "SkirmishScripts.scb");

            //можно удалить через некоторое время 01.09.20, фикс опечатки
            if (File.Exists(path + "SkirmishScripts.ini"))
                File.Move(path + "SkirmishScripts.ini", path + "SkirmishScripts.scb");
        }

        private static void Switch(string from, string to)
        {
            foreach (var file in rotrFiles)
            {
                if (File.Exists(file + "." + from))
                    File.Move(file + "." + from, file + "." + to);
            }            
        }
        private static void RenameWindowFiles()
        {
            if (File.Exists("00000000.016") && File.Exists("00000000.256") && File.Exists("00000000.016_") && File.Exists("00000000.256_"))
            {
                File.Move("00000000.016", "00000000.016_temp");
                File.Move("00000000.256", "00000000.256_temp");

                File.Move("00000000.016_", "00000000.016");
                File.Move("00000000.256_", "00000000.256");
            }

            if (File.Exists("Install_Final.bmp") && File.Exists("Install_Final_rotr.bmp"))
            {
                File.Move("Install_Final.bmp", "Install_Final.bmp_temp");
                File.Move("Install_Final_rotr.bmp", "Install_Final.bmp");
            }
        }

        private static void RenameWindowFilesBack()
        {
            if (File.Exists("00000000.016_temp") && File.Exists("00000000.256_temp") && File.Exists("00000000.016") && File.Exists("00000000.256"))
            {
                File.Move("00000000.016", "00000000.016_");
                File.Move("00000000.256", "00000000.256_");

                File.Move("00000000.016_temp", "00000000.016");
                File.Move("00000000.256_temp", "00000000.256");
            }
            if (File.Exists("Install_Final.bmp") && File.Exists("Install_Final.bmp_temp"))
            {
                File.Move("Install_Final.bmp", "Install_Final_rotr.bmp");
                File.Move("Install_Final.bmp_temp", "Install_Final.bmp");                
            }
        }

        private static bool CheckFileNameForNameMatch(string fileName, PatchInfo actualPatch)
        {
            foreach (var actualVersionFiles in actualPatch.Patch.PatchTags)
                if (fileName.Contains(actualVersionFiles))
                    return true;
            return false;
        }
    }
}
