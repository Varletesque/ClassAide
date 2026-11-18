using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Metadata;

public class MainWindow : Window
{
    public MainWindow()
    {

        Title = "Welcome to ClassAide";
        Width = 400; 
        Height = 600;
        Content = BuildLayout();

       /* var label = new TextBlock {Text = "Avalonia Window Test"};

        Content = new StackPanel
        {
            Margin = new Thickness(16),
            Spacing = 8,
            Children = {label}
        };
    */

    } 
    private Control BuildLayout() 
        {
            var rootGrid = new Grid
            {
                RowDefinitions = new RowDefinitions("Auto,*")
            };

            // Header row (row 0)
            var header = BuildHeader();
            Grid.SetRow(header, 0);
            rootGrid.Children.Add(header);

            // Main content row (row 1) - we'll build this in a later step
            var mainContent = new Border { Background = Brushes.Black };
            Grid.SetRow(mainContent, 1);
            rootGrid.Children.Add(mainContent);

             return rootGrid;
        }

    private Control BuildHeader()
    {
        return new Border
        {
            Background = Brushes.LightGray,
            Height = 60,

            Child = new TextBlock
            {
                Text = "Header goes here",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(16, 0, 0, 0)
            }
        };
    }
}
