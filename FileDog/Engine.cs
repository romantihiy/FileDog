using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;

namespace FileDog
{
    public static class Engine
    {
        public static MainWindow mainWindow;
        public static Database state;
        public static Login login;
        public static MainPage mainPage;
        public static FirstStartStack firstStartStack;

        public static void Init(MainWindow _mainWindow = null)
        {
            if (_mainWindow != null)
                mainWindow = _mainWindow;
            mainPage = new MainPage();
            login = new Login();
            if (System.IO.File.Exists("data.xml"))
            {
                if (!CheckIndexer())
                {
                    MessageBox.Show("Процесс логирования не был запущен!", "Внимание");
                    Restart();
                }
                state = LoadDatabase();
                //Test();
                ShowChanges(state.changes);
                if (state.settings.pass != "None")
                    SetPage(login);
                else
                    SetPage(mainPage);
            }
            else
            {
                state = new Database();
                firstStartStack = new FirstStartStack(state);
            }
        }

        public static void SaveDatabase(Database database, 
            string path = "data.xml", bool restart = true)
        {
            XmlSerializer formatter = new 
                XmlSerializer(typeof(Database));
            using (FileStream fs = new FileStream(path, 
                FileMode.Create))
            {
                formatter.Serialize(fs, database);
            }
            if (restart) Restart();
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
            public List<string> paths = new List<string>();
            public List<File> changes = new List<File>();
            public Settings settings = new Settings();
            public string hash;
        }

        public class File
        {
            public string oldName;
            public string newName = null;
            public DateTime date;
            public ChangeType changeType;
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
            public bool IncludeSubfolders = true;
            public string pass = "";
        }

        public static void SetPage(object page)
        {
            mainWindow.Frame.Content = page;
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
                SetPage(welcome);
            }
            public void NextPage(object sender, 
                object nextpage = null)
            {
                if (nextpage != null)
                {
                    SetPage(nextpage);
                }
                else if (
                    mainWindow.Frame.Content is FirstStart.Welcome)
                    mainWindow.Frame.Content = firstStartStack.paths;
                else
                {
                    welcome = null;
                    paths = null;
                    senders = null;
                    SaveDatabase(state);
                    Init();
                    return;
                }
                senders.Push(sender); 
            }
            public void PreviousPage()
            {
                SetPage(senders.Pop());
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
                SetPage(mainPage);
            }
        }
        public static void ShowChanges(List<File> changes)
        {
            mainPage.ClearPanel();
            foreach (var file in changes)
            {
                mainPage.PastePath(file);
            }
        }
        private static void Test()
        {
            for (int i = 0; i < 5; ++i)
            {
                var file = new File
                {
                    oldName = "Desktop\\Корзина",
                    newName = "Desktop\\Не корзина",
                    date = new DateTime(2021, 5, 8, 21, 0, 0),
                    changeType = ChangeType.Renamed
                };
                state.changes.Add(file);
            }
        }
        public static void Restart()
        {
            var processes = Process.GetProcessesByName("FileIndexer");
            foreach (var proc in processes)
                proc.Kill();
            //Thread.Sleep(1000);
            Process fileIndexer = new Process();
            fileIndexer.StartInfo.FileName = "FileIndexer.exe";
            fileIndexer.StartInfo.CreateNoWindow = true;
            fileIndexer.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            fileIndexer.Start();
        }
        public static bool CheckIndexer()
        {
            if (Process.GetProcessesByName("FileIndexer").Length != 0)
                return true;
            return false;
        }
    }
}
