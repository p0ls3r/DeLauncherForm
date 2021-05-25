using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Lng = DeLauncherForm.Language;

namespace DeLauncherForm
{
    public class ImageHandler
    {
        private Dictionary<string, BitmapImage> ImagesRepository = new Dictionary<string, BitmapImage>();       

        public ImageHandler()
        {
            #region Options

            AddImageByEvidences(false, Lng.Eng, "sounds", "SOUNDS.png");
            AddImageByEvidences(false, Lng.Rus, "sounds", "SOUNDS PYC.png");
            AddImageByEvidences(false, Lng.Eng, "apply", "APPLY.png");
            AddImageByEvidences(false, Lng.Rus, "apply", "APPLY PYC.png");
            AddImageByEvidences(false, Lng.Eng, "currentversion", "CURRENTVERSION.png");
            AddImageByEvidences(false, Lng.Rus, "currentversion", "CURRENTVERSION PYC.png");
            AddImageByEvidences(false, Lng.Eng, "deleteold", "DELETEOLD.png");
            AddImageByEvidences(false, Lng.Rus, "deleteold", "DELETEOLD PYC.png");
            AddImageByEvidences(false, Lng.Eng, "disable", "DISABLEGENTOOLS.png");
            AddImageByEvidences(false, Lng.Rus, "disable", "DISABLEGENTOOLS PYC.png");
            AddImageByEvidences(false, Lng.Eng, "fixfile", "FIXFILE.png");
            AddImageByEvidences(false, Lng.Rus, "fixfile", "FIXFILE PYC.png");
            AddImageByEvidences(false, Lng.Eng, "generals", "GENERALS.png");
            AddImageByEvidences(false, Lng.Rus, "generals", "GENERALS PYC.png");
            AddImageByEvidences(false, Lng.Eng, "modded", "MODDED.png");
            AddImageByEvidences(false, Lng.Rus, "modded", "MODDED PYC.png");
            AddImageByEvidences(false, Lng.Eng, "latestversion", "LATESTVERSION.png");
            AddImageByEvidences(false, Lng.Rus, "latestversion", "LATESTVERSION PYC.png");

            AddImageByEvidences(true, Lng.Eng, "sounds", "SOUNDS 2.png");
            AddImageByEvidences(true, Lng.Rus, "sounds", "SOUNDS PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "apply", "APPLY 2.png");
            AddImageByEvidences(true, Lng.Rus, "apply", "APPLY PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "currentversion", "CURRENTVERSION 2.png");
            AddImageByEvidences(true, Lng.Rus, "currentversion", "CURRENTVERSION PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "deleteold", "DELETEOLD 2.png");
            AddImageByEvidences(true, Lng.Rus, "deleteold", "DELETEOLD PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "disable", "DISABLEGENTOOLS 2.png");
            AddImageByEvidences(true, Lng.Rus, "disable", "DISABLEGENTOOLS PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "fixfile", "FIXFILE 2.png");
            AddImageByEvidences(true, Lng.Rus, "fixfile", "FIXFILE PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "generals", "GENERALS 2.png");
            AddImageByEvidences(true, Lng.Rus, "generals", "GENERALS PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "modded", "MODDED 2.png");
            AddImageByEvidences(true, Lng.Rus, "modded", "MODDED PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "latestversion", "LATESTVERSION 2.png");
            AddImageByEvidences(true, Lng.Rus, "latestversion", "LATESTVERSION PYC 2.png");

            #endregion

            #region MainPanel

            AddImageByEvidences(true, Lng.Eng, "worldbuilder", "WORLDBUILDER 2.png");
            AddImageByEvidences(false, Lng.Eng, "worldbuilder", "WORLDBUILDER.png");
            AddImageByEvidences(true, Lng.Eng, "changelog", "CHANGELOG 2.png");
            AddImageByEvidences(false, Lng.Eng, "changelog", "CHANGELOG.png");

            AddImageByEvidences(false, Lng.Eng, "manual", "MANUAL.png");
            AddImageByEvidences(false, Lng.Rus, "manual", "MANUAL PYC.png");
            AddImageByEvidences(false, Lng.Eng, "BP", "BALANCEPATCH.png");
            AddImageByEvidences(false, Lng.Rus, "BP", "BALANCEPATCH PYC.png");
            AddImageByEvidences(false, Lng.Eng, "exit", "EXIT.png");
            AddImageByEvidences(false, Lng.Rus, "exit", "EXIT PYC.png");
            AddImageByEvidences(false, Lng.Eng, "HP", "HANPATCH.png");
            AddImageByEvidences(false, Lng.Rus, "HP", "HANPATCH PYC.png");
            AddImageByEvidences(false, Lng.Eng, "launch", "LAUNCH.png");
            AddImageByEvidences(false, Lng.Rus, "launch", "LAUNCH PYC.png");
            AddImageByEvidences(false, Lng.Eng, "options", "OPTIONS.png");
            AddImageByEvidences(false, Lng.Rus, "options", "OPTIONS PYC.png");
            AddImageByEvidences(false, Lng.Eng, "vanilla", "ORIGINAL ROTR.png");
            AddImageByEvidences(false, Lng.Rus, "vanilla", "ORIGINAL ROTR PYC.png");
            AddImageByEvidences(false, Lng.Eng, "quickstart", "QUICKSTART.png");
            AddImageByEvidences(false, Lng.Rus, "quickstart", "QUICKSTART PYC.png");
            AddImageByEvidences(false, Lng.Eng, "windowed", "WINDOWED.png");
            AddImageByEvidences(false, Lng.Rus, "windowed", "WINDOWED PYC.png");

            AddImageByEvidences(true, Lng.Eng, "manual", "MANUAL 2.png");
            AddImageByEvidences(true, Lng.Rus, "manual", "MANUAL PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "BP", "BALANCEPATCH 2.png");
            AddImageByEvidences(true, Lng.Rus, "BP", "BALANCEPATCH PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "exit", "EXIT 2.png");
            AddImageByEvidences(true, Lng.Rus, "exit", "EXIT PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "HP", "HANPATCH 2.png");
            AddImageByEvidences(true, Lng.Rus, "HP", "HANPATCH PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "launch", "LAUNCH 2.png");
            AddImageByEvidences(true, Lng.Rus, "launch", "LAUNCH PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "options", "OPTIONS 2.png");
            AddImageByEvidences(true, Lng.Rus, "options", "OPTIONS PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "vanilla", "ORIGINAL ROTR 2.png");
            AddImageByEvidences(true, Lng.Rus, "vanilla", "ORIGINAL ROTR PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "quickstart", "QUICKSTART 2.png");
            AddImageByEvidences(true, Lng.Rus, "quickstart", "QUICKSTART PYC 2.png");
            AddImageByEvidences(true, Lng.Eng, "windowed", "WINDOWED 2.png");
            AddImageByEvidences(true, Lng.Rus, "windowed", "WINDOWED PYC 2.png");

            #endregion
        }
        public BitmapImage GetImage(bool pressed, Language lang, string name)
        {
            return ImagesRepository[pressed.ToString() + lang.ToString() + name];
        }

        private void AddImageByEvidences(bool pressed, Language lang, string name, string URL)
        {
            if (pressed)
                ImagesRepository.Add(pressed.ToString() + lang.ToString() + name, new BitmapImage(new Uri("/Windows/Resources/PressedButtons/" + URL, UriKind.Relative)));
            else
                ImagesRepository.Add(pressed.ToString() + lang.ToString() + name, new BitmapImage(new Uri("/Windows/Resources/UnpressedButtons/" + URL, UriKind.Relative)));
        }
    }
}
