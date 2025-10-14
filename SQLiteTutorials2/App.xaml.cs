using SQLiteTutorials2.Services;



namespace SQLiteTutorials2
{
    public partial class App : Application
    {
        static DatabaseService _database;
        public static DatabaseService Database
        {
            get
            {
                if (_database == null)
                { 
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tasks.db3");
                    _database = new DatabaseService(dbPath);
                }
                return _database;
            
            }
        }


        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        
    }
}