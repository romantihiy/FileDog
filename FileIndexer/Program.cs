using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileIndexer
{
    public class Program
    {
        static object obj = new object();
        static string masterpath;
        static void Main(string[] args)
        {
            Init();
            Console.Read();
        }
        static void Init()
        {
            masterpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Local\\FileIndexer\\data.xml";
            masterpath = masterpath.Replace("\\Roaming", "");
            if (!System.IO.File.Exists(masterpath)) return;
            Database database = LoadDatabase(masterpath);
            foreach (string path in database.paths)
            {
                var watcher = new FileSystemWatcher(path);
                if (database.settings.Deleted)
                    watcher.Deleted += Watcher_Deleted;
                if (database.settings.Created)
                    watcher.Created += Watcher_Created;
                if (database.settings.Changed)
                    watcher.Changed += Watcher_Changed;
                if (database.settings.Renamed)
                    watcher.Renamed += Watcher_Renamed;
                watcher.IncludeSubdirectories = database.settings.IncludeSubfolders;
                watcher.EnableRaisingEvents = true;
            }
        }
        static private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            RecordEntry(ChangeType.Renamed, e.OldFullPath, e.FullPath);
        }
        static private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            RecordEntry(ChangeType.Changed, e.FullPath, e.FullPath);
        }
        static private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            RecordEntry(ChangeType.Created, "", e.FullPath);
        }
        static private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            RecordEntry(ChangeType.Deleted, e.FullPath, "");
        }

        static private void RecordEntry(ChangeType fileEvent,
            string oldPath, string newPath)
        {
            lock (obj)
            {
                var file = new File
                {
                    changeType = fileEvent,
                    date = DateTime.Now,
                    oldName = oldPath,
                    newName = newPath
                };
                var database = LoadDatabase(masterpath);
                if (database.changes.Count > 250) // Максимум записей
                    database.changes.RemoveAt(database.changes.Count - 1);
                database.changes.Insert(0, file);
                SaveDatabase(database, masterpath);
            }
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
        public static void SaveDatabase(Database database,
            string path = "data.xml")
        {
            XmlSerializer formatter = new
                XmlSerializer(typeof(Database));
            using (FileStream fs = new FileStream(path,
                FileMode.Create))
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
    }
}
