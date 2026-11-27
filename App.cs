using Avalonia;
using Avalonia.Themes.Simple;
using Avalonia.Controls.ApplicationLifetimes;
using ClassAideApp.Repository;
using ClassAideApp.Data;
using ClassAideApp.ViewModel;

namespace ClassAideApp;
class App : Application
{
    public override void Initialize()
    {
        Styles.Add(new SimpleTheme());
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            string dbPath = Path.Combine(AppContext.BaseDirectory, "classaide.db");
            string connString = $"Data Source={dbPath};Foreign Keys=True";

            var repo = new CourseRepository(connString);
            var vm = new CourseViewModel(repo);
         
            desktop.MainWindow = new MainWindow();
            
            
            /*
            MANUAL TESTING OF VIEW MODEL AND REPO FUNCTIONALITY
            */

            Console.WriteLine($"Inital count: {vm.Courses.Count}");

            // add a course to repo
            vm.NewCourseCode = "TESTCODE101";
            vm.NewCourseName = "Testing Name";
            vm.AddCourse();

            // checking results
            Console.WriteLine(vm.StatusMessage); // Should be "Added new course..."
            vm.LoadCourses();
            Console.WriteLine($"Current count: {vm.Courses.Count}");

            // clean up database
            Console.WriteLine("Deleting courses");
            foreach (var c in vm.Courses.ToList())
            {
                vm.DeleteCourse(c);
                Console.WriteLine(vm.StatusMessage);
            }
            Console.WriteLine($"Current count: {vm.Courses.Count}");
        }
        base.OnFrameworkInitializationCompleted();
    }
}