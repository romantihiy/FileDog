using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace FileDog
{
    public static class Engine
    {
        public static MainWindow mainWindow;
        public static Database state;
        public static Login login;
        public static FirstStartStack firstStartStack;
        public static bool firstStart = false;

        public static void Init(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
            login = new Login();
            if (System.IO.File.Exists("data.xml"))
            {
                state = LoadDatabase();
                mainWindow.Frame.Content = login;
            }
            else
            {
                firstStart = true;
                state = new Database();
                firstStartStack = new FirstStartStack(state);
                mainWindow.Frame.Content = firstStartStack.welcome;
            }
            //new AddPath().Show();
        }

        public static void SaveDatabase(Database database, 
            string path = "data.xml")
        {
            XmlSerializer formatter = new 
                XmlSerializer(typeof(Database));
            using (FileStream fs = new FileStream(path, 
                FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, database);
            }
        }

        public static Database LoadDatabase(string path = "data.xml")
        {
            Database database;
            XmlSerializer formatter = new
                XmlSerializer(typeof(Database));
            using (FileStream fs = new FileStream(path, 
                FileMode.OpenOrCreate))
            {
                database = (Database)formatter.Deserialize(fs);
            }
            return database;
        }

        public class Database
        {
            public List<Directory> paths = new List<Directory>();
            public List<File> files = new List<File>();
            public Settings settings = new Settings();
        }

        public class File
        {
            public string oldName;
            public string newName;
            public DateTime date;
            public ChangeType changeType;
        }

        public class Directory
        {
            public Directory(string dirname = "")
            {
                path = dirname;
            } 
            public string path;
            public bool includeSubfolders = false;
        }

        public enum ChangeType
        {
            Changed,
            Created,
            Deleted,
            Renamed
        }

        public class Settings
        {
            public bool Changed = true;
            public bool Created = true;
            public bool Deleted = true;
            public bool Renamed = true;
            public string pass = "";
        }

        public class FirstStartStack
        {
            public FirstStart.Welcome welcome;
            public FirstStart.Paths paths;
            public Stack<object> senders;

            public FirstStartStack(Database state)
            {
                welcome = new FirstStart.Welcome(state);
                paths = new FirstStart.Paths(state);
                senders = new Stack<object>();
            }
            public void NextPage(object sender, 
                object nextpage = null)
            {
                if (nextpage != null)
                {
                    mainWindow.Frame.Content = nextpage;
                }
                else if (
                    mainWindow.Frame.Content is FirstStart.Welcome)
                    mainWindow.Frame.Content = firstStartStack.paths;
                senders.Push(sender); 
            }
            public void PreviousPage()
            {
                mainWindow.Frame.Content = senders.Pop();
            }
            public object GetParent() { return senders.Peek(); }
        }
        public static string GetHash(string value)
        {
            if (value == "") return "None";
            var md5 = MD5.Create();
            var hash = BitConverter.ToString(md5.ComputeHash(
                Encoding.UTF8.GetBytes(value))).
                    Replace("-", "").ToLowerInvariant();
            return hash;
        }
        public static void CheckPass(string pass)
        {
            string hash = GetHash(pass);
            if (hash == state.settings.pass)
            {
                MessageBox.Show("Пароль верный");
            }
        }
    }
}
