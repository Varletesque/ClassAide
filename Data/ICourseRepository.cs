using ClassAideApp.Model;

namespace ClassAideApp.Data;

public interface ICourseRepository
{
    // course related querries
    List<Course> GetAllCourses(); // returns a list of courses from the db
    Course AddCourse(Course course); // adds a course to the db and returns its id
    void DeleteCourse(int CourseId); // deletes a course from the db and returns feedback

    // student related querries: SPECIFIC TO THE COURSE
    List<Student> GetStudentsFromCourse(int CourseId); // returns a list of students from the db
    Student AddStudentToCourse(int CourseId, String FirstName, String? MiddleName, String LastName); // adds a student to the db and returns its id
    bool DeleteStudentFromCourse(int CourseId, int StudentId); // deletes a student from the db and returns feedback
}
