using SQLiteTutorials2.Services;



namespace SQLiteTutorials2
{
    public partial class App : Application
    {
        static DatabaseService _database; // Singleton database service
        public static DatabaseService Database // Public property to access the database service
        {
            get // Lazy initialization
            {
                if (_database == null) // If not initialized
                { 
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tasks.db3"); // Database path
                    _database = new DatabaseService(dbPath); // Initialize database service
                }
                return _database; // Return the database service

            }
        }


        public App() // Application constructor
        {
            InitializeComponent(); // Initialize components
            MainPage = new AppShell(); // Set the main page
        }

        
    }
}