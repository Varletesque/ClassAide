using System.ComponentModel;
using System.Collections.ObjectModel;
using ClassAideApp.Repository;
using ClassAideApp.Model;
using System.Runtime.CompilerServices;

namespace ClassAideApp.ViewModel;

public class CourseViewModel : INotifyPropertyChanged
{
    private readonly CourseRepository _repository;
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

    public CourseViewModel(CourseRepository repository) => _repository = repository;

    // database query and nonquery methods
    public async Task LoadCoursesAsync()
    {
        var courses = await _repository.GetAllCoursesAsync();
        Courses.Clear();
        foreach (var c in courses)
        {
            Courses.Add(c);
        }
    }

    public async Task AddCourseAsync()
    {
        if (new[] {NewCourseCode, NewCourseName}.Any(s => string.IsNullOrWhiteSpace(s)))
        {
            StatusMessage = "Empty Field";
            return;
        }

        var draft = new Course(NewCourseCode.Trim(), NewCourseName.Trim());
        var savedCourse = await _repository.AddCourseAsync(draft);
        Courses.Add(savedCourse);

        NewCourseCode = string.Empty;
        NewCourseName = string.Empty;
        StatusMessage = $"Added new course {savedCourse}";
    }

    public async Task DeleteCourseAsync(Course course)
    {
        await _repository.DeleteCourseAsync(course.ID);
        Courses.Remove(course);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}