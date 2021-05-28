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

namespace FileDog.Options
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Engine.Database database;
        public Settings(Engine.Database _database)
        {
            InitializeComponent();
            database = _database;
            Changed.IsChecked = database.settings.Changed;
            Created.IsChecked = database.settings.Created;
            Deleted.IsChecked = database.settings.Deleted;
            Renamed.IsChecked = database.settings.Renamed;
            Subfolders.IsChecked = database.settings.IncludeSubfolders;
        }
    }
}
