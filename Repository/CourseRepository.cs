using ClassAideApp.Model;
using ClassAideApp.Data;

using Microsoft.Data.Sqlite;

namespace ClassAideApp.Repository;

public class CourseRepository : ICourseRepository
{
    private readonly String _connString;

    public CourseRepository(String connString) => _connString = connString;

    public List<Course> GetAllCourses()
    {
        var courses = new List<Course>(); // container for returned courses

        // connect to sqlite db
        using var conn = new SqliteConnection(_connString);
        conn.Open();
        
        // build and send the command
        using var cmd = new SqliteCommand("SELECT `id`, `code`, `name` FROM courses;", conn);
        using var reader = cmd.ExecuteReader(); // returns a reader to read the results

        // read data
        int idOrd = reader.GetOrdinal("id");
        int idCode = reader.GetOrdinal("code");
        int idName = reader.GetOrdinal("name");

        while (reader.Read())
        {            
            courses.Add(new Course(                
                reader.GetString(idCode),
                reader.GetString(idName),
                reader.GetInt32(idOrd)
            ));
        }
        return courses;
    }

    public Course AddCourse(Course course)
    {
        // connect to sqlite db
        using var conn = new SqliteConnection(_connString);
        conn.Open();
        
        // build and send the command
        using var cmd = new SqliteCommand("""
            INSERT INTO courses (`code`, `name`)
            VALUES ($code, $name);
            SELECT last_insert_rowid();
        """, conn);
        cmd.Parameters.AddWithValue("$code", course.Code);
        cmd.Parameters.AddWithValue("$name", course.Name);
        var result = cmd.ExecuteScalar();
        int newId = Convert.ToInt32(result);

        return course with {ID = newId};
    }

    public bool DeleteCourse(int CourseId)
    {
        // connect to sqlite db
        using var conn = new SqliteConnection(_connString);
        conn.Open();

        // build and send the command
        using var cmd = new SqliteCommand("DELETE FROM `courses` WHERE id = $id", conn);
        cmd.Parameters.AddWithValue("$id", CourseId);
        return cmd.ExecuteNonQuery() > 0; // returns true if there are rows affected
    }

    public List<Student> GetStudentsFromCourse(int CourseId)
    {
        var students = new List<Student>();
        using var conn = new SqliteConnection(_connString);
        conn.Open();

        using var cmd = new SqliteCommand("""
            SELECT s.* 
            FROM students s 
            INNER JOIN course_students cs ON s.id = cs.student_id
            WHERE cs.course_id = $CourseId;
        """, conn);
        cmd.Parameters.AddWithValue("$CourseId", CourseId);
        using var reader = cmd.ExecuteReader();

        int idOrd = reader.GetOrdinal("id");
        int firstOrd = reader.GetOrdinal("first_name");
        int middleOrd = reader.GetOrdinal("middle_name");
        int lastOrd = reader.GetOrdinal("last_name");
        
        while (reader.Read())
        {
            students.Add(new Student(
                reader.GetInt32(idOrd),
                reader.GetString(firstOrd),
                reader.IsDBNull(middleOrd) ? null : reader.GetString(middleOrd),
                reader.GetString(lastOrd)
            ));
        }
        return students;
    }

    public Student AddStudentToCourse(int CourseId, String FirstName, String? MiddleName, String LastName)
    {
        using var conn = new SqliteConnection(_connString);
        conn.Open();

        // begin a transaction to rollback when errors occur
        using var tx = conn.BeginTransaction();

        using var cmd = new SqliteCommand("""
            INSERT INTO students (`first_name`, `middle_name`, `last_name`)
            VALUES ($FirstName, $MiddleName, $LastName);
            RETURNING id;
        """, conn, tx);
        cmd.Parameters.AddWithValue("$FirstName", FirstName);
        // if Middlename is Null, get the actualy null value for the db
        cmd.Parameters.AddWithValue("$MiddleName", (object?)MiddleName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$LastName", LastName);
        var result = cmd.ExecuteScalar();
        int newStudentId = Convert.ToInt32(result);
        
        using var cmdChild = new SqliteCommand("""
            INSERT INTO course_students (course_id, student_id)
            VALUES ($CourseId, $StudentId);
        """, conn, tx);
        cmdChild.Parameters.AddWithValue("$CourseId", CourseId);
        cmdChild.Parameters.AddWithValue("$StudentId", newStudentId);
        cmdChild.ExecuteNonQuery();
        
        tx.Commit(); // success!

        return new Student(
            newStudentId,
            FirstName,
            MiddleName,
            LastName
        );
    }

    public bool DeleteStudentFromCourse(int CourseId, int StudentId)
    {
        using var conn = new SqliteConnection(_connString);
        conn.Open();

        using var cmd = new SqliteCommand("""
            DELETE FROM course_students
            WHERE course_id = $CourseId AND student_id = $StudentId
        """, conn);
        cmd.Parameters.AddWithValue("$CourseId", CourseId);
        cmd.Parameters.AddWithValue("$StudentId", StudentId);
        return cmd.ExecuteNonQuery() > 0; // returns true if there are rows affected
    }
}