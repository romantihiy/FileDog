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
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public Engine.Database database;
        public Paths paths;
        public Settings settings;
        public Main(Engine.Database _database)
        {
            InitializeComponent();
            database = _database;
            settings = new Settings(database);
            paths = new Paths(database);
            Frame.Content = settings;
        }

        private void OpenSettings(object sender, MouseButtonEventArgs e)
        {
            Frame.Content = settings;
        }

        private void OpenPaths(object sender, MouseButtonEventArgs e)
        {
            Frame.Content = paths;
        }

        private void Save(object sender, MouseButtonEventArgs e)
        {
            database.settings.Changed = settings.Changed.IsChecked.Value;
            database.settings.Created = settings.Created.IsChecked.Value;
            database.settings.Deleted = settings.Deleted.IsChecked.Value;
            database.settings.Renamed = settings.Renamed.IsChecked.Value;
            database.settings.IncludeSubfolders = settings.Subfolders.IsChecked.Value;
            if (settings.TextBox.Text == "None")
                database.settings.pass = "None";
            else if (settings.TextBox.Text != "")
                database.settings.pass = 
                    Engine.GetHash(settings.TextBox.Text);
            database.paths = paths.paths;
            Engine.SaveDatabase(database);
            Engine.SetPage(Engine.mainPage);
        }

        private void Cancel(object sender, MouseButtonEventArgs e)
        {
            Engine.SetPage(Engine.mainPage);
        }
    }
}
