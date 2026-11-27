using System.ComponentModel;
using System.Collections.ObjectModel;
using ClassAideApp.Data;
using ClassAideApp.Model;
using System.Runtime.CompilerServices;

namespace ClassAideApp.ViewModel;

public class CourseViewModel : INotifyPropertyChanged
{
    private readonly ICourseRepository _repository;
    private string _newCourseCode = string.Empty;
    private string _newCourseName = string.Empty;
    private string _statusMessage = string.Empty;

    public ObservableCollection<Course> Courses {get;} = new();
    public ObservableCollection<Student> Students {get;} = new();

    // getters and setters for each fields
    public string NewCourseCode
    {
        get => _newCourseCode;
        set {_newCourseCode = value; OnPropertyChanged();}
    }

    public string NewCourseName
    {
        get => _newCourseName;
        set {_newCourseName = value; OnPropertyChanged();}
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set {_statusMessage = value; OnPropertyChanged();}
    }

    public CourseViewModel(ICourseRepository repository) => _repository = repository;

    // database query and nonquery methods
    public void LoadCourses()
    {
        try
        {
            var courses = _repository.GetAllCourses();
            Courses.Clear();
            foreach (var c in courses)
            {
                Courses.Add(c);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            StatusMessage = "Couldn't load course";
        }
    }

    public void AddCourse()
    {
        if (new[] {NewCourseCode, NewCourseName}.Any(s => string.IsNullOrWhiteSpace(s)))
        {
            StatusMessage = "Empty Field";
            return;
        }

        try {
            var saved = _repository.AddCourse(new Course(NewCourseCode.Trim(), NewCourseName.Trim()));
            Courses.Add(saved);

            NewCourseCode = string.Empty;
            NewCourseName = string.Empty;
            StatusMessage = $"Added new course {saved.Name} {saved.Code}";
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            StatusMessage = "Couldn't save the course";
        }
    }

    public void DeleteCourse(Course course)
    {
        try
        {
            if(_repository.DeleteCourse(course.ID))
            {
                Courses.Remove(course);
                StatusMessage = $"Deleted {course.Name} {course.Code}";
            }
            else
            {
                StatusMessage = "Course not found";
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            StatusMessage = "Couldn't delete course";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}