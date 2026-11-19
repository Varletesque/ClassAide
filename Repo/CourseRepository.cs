using MySqlConnector;

public record Course(int id, String Code, String Name)
{
    public override string ToString() => $"{Code} - {Name}";
}

// temporary Student record to prevent interface error
public record Student(int id, String FirstName, String? MiddleName, String LastName);

public interface ICourseRepository
{
    // course related querries
    Task<List<Course>> GetCoursesAsync(); // returns a list of courses from the db
    Task<int> AddCourseAsync(String Code, String Name); // adds a course to the db and returns its id
    Task<bool> DeleteCourseAsync(int CourseId); // deletes a course from the db and returns feedback

    // student related querries: SPECIFIC TO THE COURSE
    Task<List<Student>> GetStudentsFromCourseAsync(int CourseId); // returns a list of students from the db
    Task<Student> AddStudentToCourseAsync(int CourseId, String FirstName, String? MiddleName, String LastName); // adds a student to the db and returns its id
    Task<bool> DeleteStudentFromCourseAsync(int CourseId, int StudentId); // deletes a student from the db and returns feedback
}

public class CourseRepository : ICourseRepository
{
    public async Task<List<Course>> GetCoursesAsync()
    {
        return null;
    }

    public async Task<int> AddCourseAsync(String Code, String Name)
    {
        return 0;
    }

    public async Task<bool> DeleteCourseAsync(int CourseId)
    {
        return false;
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
        return null;
    }
}
