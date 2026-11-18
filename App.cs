using Avalonia;
using Avalonia.Themes.Simple;

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
            base.OnFrameworkInitializationCompleted();
        }
    }
}
