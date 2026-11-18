using Avalonia;
using Avalonia.Themes.Simple;
using Avalonia.Controls.ApplicationLifetimes;

namespace ClassAide
{
    class App : Application
    {
        public override void Initialize()
        {
            Styles.Add(new SimpleTheme());
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = new MainWindow();
            base.OnFrameworkInitializationCompleted();
        }
    }
}
