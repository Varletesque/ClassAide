using Models;
using Data;
using MySqlConnector;

namespace Repository;

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
        return false;
    }
}
