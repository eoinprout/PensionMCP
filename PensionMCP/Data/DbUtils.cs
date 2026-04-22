namespace PensionMCP.Data
{
    public static class DbUtils
    {
        /// <summary>
        /// Returns the full path to the SQLite DB
        /// </summary>
        /// <returns>String, Full path including filename</returns>
        /// <remarks>
        /// When running as an extenstion of Claude Desktop which is a sandboxed application
        /// the path reported may NOT in fact be the actual location of the file.
        /// Windows my intercept the and the actual folder may be 
        /// [User]\AppData\Local\Packages\Claude_[Unique ID]
        /// 
        /// This is transparent to the application but is confusing if searching for the DB
        /// file in windows explorer.
        /// </remarks>
        public static string GetDatabasePath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dataDirectory = Path.Combine(appDataPath, "PensionMCP");
            return Path.Combine(dataDirectory, "pensionmcp.db");
        }

        public static string BuildConnectionString()
        {
            var databasePath = GetDatabasePath();
            Directory.CreateDirectory(Path.GetDirectoryName(databasePath)!);
            return $"Data Source={databasePath}";
        }
    }
}
