using Avalonia;

// main entry point
namespace ClassAide
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
