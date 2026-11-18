using System.IO;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

public class MainWindow : Window
{
    public MainWindow() 
    {

        Title = "Welcome to ClassAide";
        Width = 400;
        Height = 600;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Background = Brushes.Black;

        Content = new TextBlock
        {
            Text = "Meow",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center

        };

    }
}

