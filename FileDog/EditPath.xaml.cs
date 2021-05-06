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

namespace FileDog
{
    /// <summary>
    /// Логика взаимодействия для EditPath.xaml
    /// </summary>
    public partial class EditPath : Window
    {
        public Engine.Directory directory;
        public bool delete = false;
        public bool ok = false;
        public EditPath(string filename)
        {
            InitializeComponent();
            Filename.Content = filename;
            directory = new Engine.Directory(filename);
        }

        private void Delete(object sender, MouseButtonEventArgs e)
        {
            delete = true;
            Close();
        }

        private void OK(object sender, MouseButtonEventArgs e)
        {
            directory.includeSubfolders = IncludeSubfolders.
                IsChecked.Value;
            ok = true;
            Close();
        }
    }
}
