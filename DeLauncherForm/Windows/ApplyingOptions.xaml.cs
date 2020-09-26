using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeLauncherForm.Windows
{
    public partial class ApplyingOptions : Window
    {
        public ApplyingOptions(FormConfiguration conf)
        {
            InitializeComponent();

            if (conf.Lang == DeLauncherForm.Language.Rus)
            {
                DonwloadMessage1.Text = "Идёт применение настроек";
                DonwloadMessage2.Text = "Пожалуйста ждите";
            }
        }
    }
}
