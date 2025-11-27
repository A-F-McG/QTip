using Microsoft.Data.Sqlite;

namespace QTipBackend.src
{
    public class Database
    {
        public static void Initialise(string connectionString)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var createTables = @"
            CREATE TABLE IF NOT EXISTS Submissions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TokenizedText TEXT
            );

            CREATE TABLE IF NOT EXISTS ClassificationRecords (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SubmissionId INTEGER,
                Token TEXT,
                OriginalValue BLOB,
                PiiType TEXT
            );
        ";

            using var cmd = new SqliteCommand(createTables, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
