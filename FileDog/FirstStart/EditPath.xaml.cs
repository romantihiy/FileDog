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

namespace FileDog.FirstStart
{
    /// <summary>
    /// Логика взаимодействия для EditPath.xaml
    /// </summary>
    public partial class EditPath : Page
    {
        public Result result;
        public string path;
        public EditPath(string filename)
        {
            InitializeComponent();
            Filename.Content = filename;
            path = filename;
        }

        public enum Result
        {
            Save,
            Delete,
            Cancel
        }

        private void Save(object sender, MouseButtonEventArgs e)
        {
            result = Result.Save;
            Engine.firstStartStack.paths.UpdatePanel(this);
            Engine.firstStartStack.PreviousPage();
        }
        private void Delete(object sender, MouseButtonEventArgs e)
        {
            result = Result.Delete;
            Engine.firstStartStack.paths.UpdatePanel(this);
            Engine.firstStartStack.PreviousPage();
        }
        private void Cancel(object sender, MouseButtonEventArgs e)
        {
            result = Result.Cancel;
            Engine.firstStartStack.paths.UpdatePanel(this);
            Engine.firstStartStack.PreviousPage();
        }
    }
}
