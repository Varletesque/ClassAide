// main entry point
namespace Application
{
    class Program
    {
        static void Main(String[] args)
        {
            var application = new Application();
            application.init();

            var window = new Window();
            window.init();

            application.attachWindow(window);
            application.run();
        }
    }
}
