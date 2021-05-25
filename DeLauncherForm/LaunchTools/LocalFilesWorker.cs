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
        private static string scriptPath = "Data\\Scripts\\";
        private static string[] scbFiles = new string[] { scriptPath + "MultiplayerScripts", scriptPath + "SkirmishScripts", };
        private static string[] iniFiles = new string[] { scriptPath + "Scripts"};

        public static void SetGameFiles(FormConfiguration conf)
        {
            RenameROTRFiles();

            foreach (var exceptionFile in conf.Patch.ExceptionFiles)
            {
                if (File.Exists(exceptionFile + ".big"))
                    File.Move(exceptionFile + ".big", exceptionFile + ".gib");
            }

            RenameScriptFiles();
            RenameWindowFiles();
        }

        public static void SetGameFilesBack()
        {
            RenameROTRFilesBack();
            RenameScriptFilesBack();
            RenameWindowFilesBack();
            ConvertBigsToGibs();
        }

        //Переименовать все big файлы известных патчей в gib
        public static void ConvertBigsToGibs()
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
                        //если файл оказался известным патчем переименовываем его или удаляем                        
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

        public static void ActivateFileByName(string FileName)
        {
            if (!File.Exists(FileName.Substring(0, FileName.Length - 3)))
                File.Move(FileName, FileName.Substring(0, FileName.Length - 3) + "big");
        }

        //Получить имена файлов последней версии патча в директории
        public static IEnumerable<string> GetLatestPatchFileNames(Patch patch)
        {
            if (patch is Vanilla && File.Exists("!!Rotr_Intrnl_INI.gib") && File.Exists("!!Rotr_Intrnl_Eng.gib"))
            {
                yield return "!!Rotr_Intrnl_INI.gib";
                yield return "!!Rotr_Intrnl_Eng.gib";
                yield break;
            }

            var number = 0;
            var previousPatchFile = "";

            foreach (var patchFile in GetPatchFileNames(patch))
            {
                var tempNumber = GetVersionNumberFromPatchName(patchFile);
                if (tempNumber > number)
                {
                    previousPatchFile = patchFile;
                    number = tempNumber;
                }
            }

            yield return previousPatchFile;
        }

        //Определить существует ли файл патча в директории
        public static bool CheckPatchFileExist(Patch patch)
        {           
            //отдельный кейс для ваниллы
            if (patch is Vanilla && File.Exists("!!Rotr_Intrnl_INI.gib") && File.Exists("!!Rotr_Intrnl_Eng.gib"))
            {
                return true;
            }
            else
                if (patch is Vanilla)
                return false;

            foreach (var patchFile in GetPatchFileNames(patch))
                if (CheckFileNameForNameMatch(patchFile, patch))
                    return true;

            return false;
        }

        //Получить номер последней версии патча в директории
        public static int GetCurrentVersionNumber(Patch patch)
        {
            //отдельный кейс для ваниллы
            if (patch is Vanilla && File.Exists("!!Rotr_Intrnl_INI.gib") && File.Exists("!!Rotr_Intrnl_Eng.gib"))
            {
                return 18720;
            }
            else if (patch is Vanilla)
                return 0;



            var number = 0;

            foreach (var patchFile in GetPatchFileNames(patch))
            {
                var tempNumber = GetVersionNumberFromPatchName(patchFile);
                if (tempNumber > number)
                    number = tempNumber; 
            }

            return number;
        }

        //Удалить все устаревшие файлы патчей
        public static void DeleteAllOldPatchFiles(Patch patch)
        {
            //отдельный кейс для ваниллы
            if (patch is Vanilla)
                return;

            foreach (var f in GetPatchFileNames(patch))
            {
                File.Delete(f);
            }                     
        }

        public static void DeleteAllPatchFiles(Patch patch)
        {
            //отдельный кейс для ваниллы
            if (patch is Vanilla)
                return;

            foreach (var patchFile in Directory.GetFiles(Directory.GetCurrentDirectory(), "*gib").Concat(Directory.GetFiles(Directory.GetCurrentDirectory(), "*big")))
            {
                if (patchFile.Contains("HP") || patchFile.Contains("BP"))
                File.Delete(patchFile);
            }
        }

        //Получить коллекцию имён файлов патчей
        public static IEnumerable<string> GetPatchFileNames(Patch patch)
        {
            var filesGib = Directory.GetFiles(Directory.GetCurrentDirectory(), "*gib");

            //проходим по всем файлам гиб директории
            foreach (var gibFile in filesGib)
            {
                var fileAttributes = gibFile.Split('\\');

                //отделяем имя файла без полного адреса в ОС
                var fileGib = fileAttributes[fileAttributes.Length - 1];

                if (CheckFileNameForNameMatch(fileGib, patch))
                {
                    yield return (fileGib);
                }
            }
        }

        public static IEnumerable<string> GetPatchFileNames()
        {
            var filesGib = Directory.GetFiles(Directory.GetCurrentDirectory(), "*gib");

            //проходим по всем файлам гиб директории
            foreach (var gibFile in filesGib)
            {
                var fileAttributes = gibFile.Split('\\');

                //отделяем имя файла без полного адреса в ОС
                var fileGib = fileAttributes[fileAttributes.Length - 1];

                if (fileGib.Contains("HP") || fileGib.Contains("BP"))
                  yield return fileGib;
            }
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

        private static void RenameROTRFiles()
        {
            Switch(rotrFiles, "gib", "big");
        }

        private static void RenameROTRFilesBack()
        {
            Switch(rotrFiles, "big", "gib");
        }

        private static void RenameScriptFiles()
        {
            Switch(scbFiles, "scb", "nope");
            Switch(iniFiles, "ini", "nope");
        }

        private static void RenameScriptFilesBack()
        {
            Switch(scbFiles, "nope", "scb");
            Switch(iniFiles, "nope", "ini");
        }

        private static void Switch(string[] source, string from, string to)
        {
            foreach (var file in source)
            {
                if (File.Exists(file + "." + from) && !File.Exists(file + "." + to))
                    File.Move(file + "." + from, file + "." + to);
                else
                    if (File.Exists(file + "." + to))
                      File.Delete(file + "." + from);
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
                //Сохранить ZH скрин, если он уже сохранён, то просто удалить текущий(Final)
                if (!File.Exists("Install_Final_zh.bmp"))
                    File.Move("Install_Final.bmp", "Install_Final_zh.bmp");
                else
                    File.Delete("Install_Final.bmp");

               //Установить ROTR скрин
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

            if (File.Exists("Install_Final.bmp") && File.Exists("Install_Final_zh.bmp"))
            {
                //Сохранить ROTR скрин, если он уже есть, то просто удалить текущий(Final)
                if (!File.Exists("Install_Final_rotr.bmp"))
                    File.Move("Install_Final.bmp", "Install_Final_rotr.bmp");
                else
                    File.Delete("Install_Final.bmp");

                //Установить ZH скрин
                File.Move("Install_Final_zh.bmp", "Install_Final.bmp");
            }
        }

        //Определить что файл относится к файлу патча
        private static bool CheckFileNameForNameMatch(string fileName, Patch patch)
        {
            foreach (var actualVersionFiles in patch.PatchTags)
                if (fileName.Contains(actualVersionFiles))
                    return true;
            return false;
        }
    }
}
