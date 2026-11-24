using Avalonia;                    // Core Avalonia stuff (AppBuilder, etc.)
using Avalonia.Controls;           // Window, Grid, Border, Button, TextBox, TextBlock, StackPanel, ScrollViewer
using Avalonia.Layout;             // Orientation, HorizontalAlignment, VerticalAlignment
using Avalonia.Media;              // Brushes, SolidColorBrush, FontWeight, Color
using Avalonia.Controls.Primitives; // ScrollBarVisibility lives here
using Avalonia.Interactivity;

public class MainWindow : Window
{
    public MainWindow()
    {
        // Title shown in the OS window title bar.
        Title = "ClassAide";

        // Initial window size in pixels.
        Width = 400;
        Height = 600;

        // Window background — dark theme base. Used anywhere the layout doesn't paint over it.
        Background = new SolidColorBrush(Color.Parse("#1E1E1E"));

        // Put the whole UI tree into the window. A Window has exactly one Content.
        Content = BuildLayout();
    }

    // Builds the overall screen layout: header on top, main content below.
    private Control BuildLayout()
    {
        // Root Grid that splits the window into two vertical rows.
        // "Auto,*" = row 0 sizes to content (header), row 1 takes the rest (main area).
        var rootGrid = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,*")
        };

        // Ask BuildHeader() to build the top bar, put it in row 0, and add it to root.
        var header = BuildHeader();
        Grid.SetRow(header, 0);           // tell the Grid this child belongs to row 0
        rootGrid.Children.Add(header);    // actually attach it to the Grid

        // Empty-state block: shown when there are no classes.
        // Vertical StackPanel, centered both ways, with two text lines.
        var emptyState = new StackPanel
        {
            Orientation = Orientation.Vertical,                    // stack children top-to-bottom
            Spacing = 8,                                           // 8px gap between children
            HorizontalAlignment = HorizontalAlignment.Center,      // center the block horizontally
            VerticalAlignment = VerticalAlignment.Center,          // center the block vertically
            Children =
            {
                // Primary line: bold, big, light-colored for dark mode.
                new TextBlock
                {
                    Text = "No classes yet",
                    FontSize = 22,
                    FontWeight = FontWeight.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.Parse("#F0F0F0"))
                },
                // Secondary line: gray hint underneath.
                new TextBlock
                {
                    Text = "Click + ADD to create your first class",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.Parse("#A0A0A0"))
                }
            }
        };

        // A Grid with a single *-sized row and column. Its only job is to FILL the scroll area
        // so the emptyState (which is Center-aligned) can be centered in the visible space.
        var emptyHost = new Grid
        {
            RowDefinitions = new RowDefinitions("*"),
            ColumnDefinitions = new ColumnDefinitions("*")
        };

        // Add the empty state into the host grid (one child, fills the cell).
        emptyHost.Children.Add(emptyState);

        // ScrollViewer wraps the grid so future content (many cards) can scroll.
        // Auto = show the scrollbar only when content overflows; hide it otherwise.
        var scroll = new ScrollViewer
        {
            Content = emptyHost,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto
        };

        // The main content area (row 1) — a Border giving it a dark background + padding.
        // Padding insets *inside* the border, so the content doesn't touch the edges.
        // Child = scroll because a Border holds exactly one child.
        var mainContent = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#252526")),
            Padding = new Thickness(24),
            Child = scroll
        };

        // Put mainContent into row 1 (the *-sized row → it fills the remaining height).
        Grid.SetRow(mainContent, 1);
        rootGrid.Children.Add(mainContent);

        // Hand the finished root Grid back to the constructor (which assigns it to Content).
        return rootGrid;
    }

    // Builds the header bar: title on the left, search + buttons on the right.
    private Control BuildHeader()
    {
        // Header grid: two columns.
        // "Auto,*" = title column hugs its text, actions column takes whatever's left.
        // Margin (16,0,16,0) = 16px gap on left and right inside the bar.
        var headerGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,*"),
            Margin = new Thickness(16, 0, 16, 0)
        };

        // Left side: the app title. Bold, light color, centered vertically in the 60px bar.
        var title = new TextBlock
        {
            Text = "MY CLASSES",
            FontWeight = FontWeight.Bold,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 12, 0 ,0),
            Foreground = new SolidColorBrush(Color.Parse("#F0F0F0"))
        };

        // Search box on the right side of the header.
        // Watermark = gray hint text shown when the box is empty.
        // WatermarkForeground = the color of that hint text.
        var searchBox = new TextBox
        {
            Watermark = "Search...",
            Width = 180,
            VerticalAlignment = VerticalAlignment.Center,
            Background = new SolidColorBrush(Color.Parse("#3A3A3D")),
            Foreground = new SolidColorBrush(Color.Parse("#F0F0F0")),
            WatermarkForeground = new SolidColorBrush(Color.Parse("#A0A0A0"))
        };

        // Import button — neutral dark style, same as the search box fill.
        var importBtn = new Button
        {
            Content = "Import",
            VerticalAlignment = VerticalAlignment.Center,
            Background = new SolidColorBrush(Color.Parse("#3A3A3D")),
            Foreground = new SolidColorBrush(Color.Parse("#F0F0F0"))
        };
        importBtn.Click += OnImportClicked; 

        // "+ Add" button — green accent, the primary action of the screen.
        var addBtn = new Button
        {
            Content = "+ Add",
            FontWeight = FontWeight.Bold,
            VerticalAlignment = VerticalAlignment.Center,
            Background = new SolidColorBrush(Color.Parse("#2E7D32")),
            Foreground = Brushes.White
        };

        addBtn.Click += OnAddClicked; 

        // Group the three controls in a horizontal row with 8px spacing.
        // HorizontalAlignment.Right pushes the whole group to the right edge of its column.
        var actionsPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 8,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = new Thickness (0, 0, 0, 12),

            Children = { searchBox, importBtn, addBtn }
        };


        // Put the title in column 0 (the Auto column → hugs the text).
        Grid.SetColumn(title, 0);
        headerGrid.Children.Add(title);

        // Put the button group in column 1 (the * column → fills remaining space, right-aligned).
        Grid.SetColumn(actionsPanel, 1);
        headerGrid.Children.Add(actionsPanel);

        // Wrap the whole header grid in a Border so the gray background fills the full width —
        // even the margin area outside the grid's 16px inset.
        return new Border
        {
            Background = new SolidColorBrush(Color.Parse("#2D2D30")),
            Height = 90,
            Child = headerGrid
        };
    }

        private void OnAddClicked(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("+ Add clicked!");
    }

        private void OnImportClicked(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Import clicked!");
    }
}