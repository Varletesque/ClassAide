using ClassAideApp.Model;
using ClassAideApp.Data;

using Microsoft.Data.Sqlite;

namespace ClassAideApp.Repository;

public class CourseRepository : ICourseRepository
{
    private readonly String _connString;

    public CourseRepository(String connString) => _connString = connString;

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        var courses = new List<Course>(); // container for returned tasks

        // connect to sql server
        await using var conn = new SqliteConnection(_connString);
        await conn.OpenAsync();
        
        // build and send the command
        await using var cmd = new SqliteCommand("SELECT `id`, `code`, `name` FROM courses;", conn);
        await using var reader = await cmd.ExecuteReaderAsync(); // returns a reader to read the results

        int idOrd = reader.GetOrdinal("id");
        int idCode = reader.GetOrdinal("code");
        int idName = reader.GetOrdinal("name");

        while (await reader.ReadAsync())
        {            
            courses.Add(new Course(
                reader.GetInt32(idOrd),
                reader.GetString(idCode),
                reader.GetString(idName)
            ));
        }
        return courses;
    }

    public async Task AddCourseAsync(String Code, String Name)
    {
        // connect to sql server
        await using var conn = new SqliteConnection(_connString);
        await conn.OpenAsync();
        
        // build and send the command
        await using var cmd = new SqliteCommand("INSERT INTO courses (`code`, `name`) VALUES ($code, $name)", conn);
        cmd.Parameters.AddWithValue("$code", Code);
        cmd.Parameters.AddWithValue("$name", Name);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> DeleteCourseAsync(int CourseId)
    {
        // TODO: Get boolean check if qury was a success
        return false; // temp
    }

    public async Task<List<Student>> GetStudentsFromCourseAsync(int CourseId)
    {
        return null;
    }

    public async Task<Student> AddStudentToCourseAsync(int CourseId, String FirstName, String? MiddleName, String LastName)
    {
        return null!;
    }

    public async Task<bool> DeleteStudentFromCourseAsync(int CourseId, int studentId)
    {
        return false;
    }
}
