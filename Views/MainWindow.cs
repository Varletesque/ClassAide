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

            // Main content row (row 1)
            var mainContent = new Border { Background = Brushes.Black };
            Grid.SetRow(mainContent, 1);
            rootGrid.Children.Add(mainContent);

             return rootGrid;
        }

        private Control BuildHeader()
        {
            var headerGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("Auto,*"),
                Margin = new Thickness(16, 0, 16, 0)
            };

            var title = new TextBlock { Text = "MY CLASSES", FontWeight = FontWeight.Bold, VerticalAlignment = VerticalAlignment.Center };

            var searchBox   = new TextBox { Watermark = "Search...", Width = 180, VerticalAlignment = VerticalAlignment.Center };
            var importBtn   = new Button  { Content = "Import", VerticalAlignment = VerticalAlignment.Center };
            var addBtn      = new Button  { Content = "+ Add", FontWeight = FontWeight.Bold, VerticalAlignment = VerticalAlignment.Center };

            var actionsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Children = { searchBox, importBtn, addBtn }
            };

        Grid.SetColumn(title, 0);
        headerGrid.Children.Add(title);

        Grid.SetColumn(actionsPanel, 1);
        headerGrid.Children.Add(actionsPanel);

        return new Border
        {
            Background = Brushes.LightGray,
            Height = 60,
            Child = headerGrid
        };
    }
}