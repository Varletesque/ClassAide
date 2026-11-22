using ClassAideApp.Model;
using ClassAideApp.Data;

using MySqlConnector;

namespace ClassAideApp.Repository;

public class CourseRepository : ICourseRepository
{
    private readonly String _connString;

    CourseRepository(String connString) => _connString = connString;

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        var tasks = new List<Course>(); // container for returned tasks

        // open a connection to db
        await using var conn = new MySqlConnection(_connString);
        await conn.OpenAsync();

        // impose query
        const string query = "SELECT `id`, `code`, `name` FROM `courses`;"; // sql statement
        await using var cmd = new MySqlCommand(query, conn); // build sql statement
        await using var reader = await cmd.ExecuteReaderAsync(); // this returns a list of columns/rows

        // get row index of each column
        int idOrd = reader.GetOrdinal("id");
        int codeOrd = reader.GetOrdinal("code");
        int nameOrd = reader.GetOrdinal("name");

        // read every row and add it on task
        while (await reader.ReadAsync())
        {
            tasks.Add(new Course
            (
                reader.GetInt32(idOrd), // id
                reader.GetString(codeOrd), // code
                reader.GetString(nameOrd) // name
            ));
        }
        return tasks;
    }

    public async Task<int> AddCourseAsync(String Code, String Name)
    {
        // open a connection to db
        await using var conn = new MySqlConnection(_connString);
        await conn.OpenAsync();

        // impose query
        const string query = "INSERT INTO `courses` (`code`, `name`) VALUES (@code, @name);SELECT LAST_INSERT_ID();";
        await using var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@code", Code);
        cmd.Parameters.AddWithValue("@name", Name);

        var newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        return newId;
    }

    public async Task<bool> DeleteCourseAsync(int CourseId)
    {
        // open a connection to db
        await using var conn = new MySqlConnection(_connString);
        await conn.OpenAsync();

        // impose query
        const string query = "DELETE FROM `courses` WHERE `id` = @id";
        await using var cmd = new MySqlCommand(query, conn);
        await cmd.ExecuteNonQueryAsync();

        // TODO: Get boolean check if qury was a success
        return false; // temp
    }

    public async Task<List<Student>> GetStudentsFromCourseAsync(int CourseId)
    {
        return null;
    }

    public async Task<Student> AddStudentToCourseAsync(int CourseId, String FirstName, String? MiddleName, String LastName)
    {
        return null;
    }

    public async Task<bool> DeleteStudentFromCourseAsync(int CourseId, int studentId)
    {
        return false;
    }
}
