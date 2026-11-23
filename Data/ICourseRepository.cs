using ClassAideApp.Model;

namespace ClassAideApp.Data;

public interface ICourseRepository
{
    // course related querries
    Task<List<Course>> GetAllCoursesAsync(); // returns a list of courses from the db
    Task AddCourseAsync(String Code, String Name); // adds a course to the db and returns its id
    Task<bool> DeleteCourseAsync(int CourseId); // deletes a course from the db and returns feedback

    // student related querries: SPECIFIC TO THE COURSE
    Task<List<Student>> GetStudentsFromCourseAsync(int CourseId); // returns a list of students from the db
    Task<Student> AddStudentToCourseAsync(int CourseId, String FirstName, String? MiddleName, String LastName); // adds a student to the db and returns its id
    Task<bool> DeleteStudentFromCourseAsync(int CourseId, int StudentId); // deletes a student from the db and returns feedback
}
