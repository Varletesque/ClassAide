using Avalonia;

// main entry point
namespace ClassAideApp
{
    class Program
    {
        [STAThread]
        public static void Main(String[] args) =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .StartWithClassicDesktopLifetime(args);
    }
}
