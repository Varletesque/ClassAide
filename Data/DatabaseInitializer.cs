using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace ClassAideApp.Data;

public class DatabaseInitializer
{
    public static async Task InitializeAsync(string connString, string schemaFilename)
    {
        string path = Path.Combine(AppContext.BaseDirectory, schemaFilename);
        string schemaScript = await File.ReadAllTextAsync(path);

        await using var conn = new SqliteConnection(connString);
        await conn.OpenAsync();

        await using var cmd = new SqliteCommand(schemaScript, conn);
        await cmd.ExecuteNonQueryAsync();
    }
}