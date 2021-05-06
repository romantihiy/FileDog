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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FileDog.FirstStart
{
    /// <summary>
    /// Логика взаимодействия для Paths.xaml
    /// </summary>
    public partial class Paths : Page
    {
        public Engine.Database state;
        public Paths(Engine.Database database)
        {
            state = database;
            InitializeComponent();
        }

        private void AddPath(object sender, MouseButtonEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = dialog.FileName;
                var test = new EditPath(path);
                test.ShowDialog();
                MessageBox.Show(test.directory.includeSubfolders.ToString());
            }
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            Engine.PreviousPage();
        }
    }
}
