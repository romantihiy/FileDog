using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileIndexerStarter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string masterpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Local\\FileIndexer\\FileIndexer.exe";
            masterpath = masterpath.Replace("\\Roaming", "");
            if (File.Exists(masterpath))
            {
                Process view = new Process();
                view.StartInfo.FileName = masterpath;
                view.StartInfo.CreateNoWindow = true;
                view.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                view.Start();
                Close();
            }
        }
    }
}
