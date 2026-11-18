using Avalonia;
using Avalonia.Controls;

public class MainWindow : Window
{
    public MainWindow()
    {

        Title = "Welcome to ClassAide";
        Width = 400; Height = 600;

        var label = new TextBlock {Text = "Avalonia Window Test"};

        Content = new StackPanel
        {
            Margin = new Thickness(16),
            Spacing = 8,
            Children = {label}
        };
    }
}
