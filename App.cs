using Avalonia;
using Avalonia.Themes.Simple;
using Avalonia.Controls.ApplicationLifetimes;
using ClassAideApp.Repository;
using ClassAideApp.Data;

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
            
            desktop.MainWindow = new MainWindow();

            try
            {
                await DatabaseInitializer.InitializeAsync(connString, "classaide_schema.sql");

                //await repo.AddCourseAsync("TESTCODE_01", "TESTNAME_A");
                //await repo.AddCourseAsync("TESTCODE_02", "TESTNAME_B");

                var courses = repo.GetAllCourses();

                Console.WriteLine($"SUCCESS! Found {courses.Count} course(s):");
                foreach (var c in courses)
                {
                    Console.WriteLine($"-> ID: {c.ID} | Code: {c.Code} | Name: {c.Name}");
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"DATABASE FAILED: {ex.Message}");
            }
        }
        base.OnFrameworkInitializationCompleted();
    }
}