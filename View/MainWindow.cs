
using Avalonia;                      // Core Avalonia stuff (AppBuilder, etc.)
using Avalonia.Controls;             // Window, Grid, Border, Button, TextBox, TextBlock, StackPanel, ScrollViewer
using Avalonia.Layout;               // Orientation, HorizontalAlignment, VerticalAlignment
using Avalonia.Media;                // Brushes, SolidColorBrush, FontWeight, Color
using Avalonia.Controls.Primitives;  // ScrollBarVisibility lives here
using Avalonia.Interactivity;        // RoutedEventArgs for Click handlers



public class MainWindow : Window
{
    // Fields for controls we need to reach from click handlers.
    // The "?" means "may be null" — they're null until BuildLayout() runs.
    private StackPanel? _cardsPanel;   // Holds the list of cards
    private StackPanel? _emptyState;   // "No classes yet" block — hidden once cards exist

    public MainWindow()
    {
        // Window title (shows in the OS title bar).
        Title = "ClassAide";

        // Initial window size in pixels.
        Width = 1200;
        Height = 600;

        // Base window background — shows anywhere children don't paint over.
        Background = new SolidColorBrush(Color.Parse("#1E1E1E"));

        // The whole UI tree. A Window has exactly one Content.
        Content = BuildLayout();
    }

    // Builds the full screen: header on top, main area below.
    private Control BuildLayout()
    {
        // Root Grid splits the window vertically into two rows.
        // "Auto,*" = row 0 hugs the header; row 1 takes whatever's left.
        var rootGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("300,*")
        };

        var sectionPanel = BuildClassListPanel();
        Grid.SetColumn(sectionPanel, 0);
        rootGrid.Children.Add(sectionPanel);

        var sessionPanel = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#1E1E1E")),
            Child = new TextBlock
            {
                Text = "Select a class from the left to begin",
                  Foreground = new SolidColorBrush(Color.Parse("#A0A0A0")),
                  HorizontalAlignment = HorizontalAlignment.Center,
                  VerticalAlignment = VerticalAlignment.Center,
                  FontSize = 16
            }
        };
        Grid.SetColumn(sessionPanel, 1);
        rootGrid.Children.Add(sessionPanel);

        // Hand the whole tree back to the constructor, which sets Content.
        return rootGrid;
    }


    // Builds the top header bar: title on left, actions on right.
private Control BuildHeader()
{
    // Stack elements vertically instead of horizontally to fit 300px width.
    var headerStack = new StackPanel
    {
        Orientation = Orientation.Vertical,
        Spacing = 12,
        Margin = new Thickness(16)
    };

    // Title
    headerStack.Children.Add(new TextBlock
    {
        Text = "MY CLASSES",
        FontWeight = FontWeight.Bold,
        FontSize = 18,
        Foreground = new SolidColorBrush(Color.Parse("#F0F0F0"))
    });

    // A horizontal row for the buttons (since they are short)
    var buttonRow = new StackPanel
    {
        Orientation = Orientation.Horizontal,
        Spacing = 8
    };

    var addBtn = new Button
    {
        Content = "+ ADD",
        FontWeight = FontWeight.Bold,
        Background = new SolidColorBrush(Color.Parse("#2E7D32")),
        Foreground = Brushes.White
    };
    addBtn.Click += OnAddClicked;

    var importBtn = new Button
    {
        Content = "Import",
        Background = new SolidColorBrush(Color.Parse("#3A3A3D")),
        Foreground = new SolidColorBrush(Color.Parse("#F0F0F0"))
    };
    importBtn.Click += OnImportClicked;

    buttonRow.Children.Add(addBtn);
    buttonRow.Children.Add(importBtn);

    headerStack.Children.Add(buttonRow);


    return headerStack;
}
private Control BuildSearchBox()
{
    var searchBox = new TextBox
    {
        PlaceholderText = "Search...",
        Margin = new Thickness(16, 8, 16, 32),
        Background = new SolidColorBrush(Color.Parse("#3A3A3D")),
        Foreground = new SolidColorBrush(Color.Parse("#F0F0F0")),
        PlaceholderForeground = new SolidColorBrush(Color.Parse("#A0A0A0")),
        CornerRadius = new CornerRadius(6),
        Padding = new Thickness(10, 8)
    };
    searchBox.TextChanged += OnSearchTextChanged;
    return searchBox;
}

    // Builds ONE card. Placeholder content — becomes the real class card later.
    private Control BuildCard(string course = "CS103", string section = "Software Dev", int count = 34)
    {

     var cardGrid = new Grid
     {
       ColumnDefinitions = new ColumnDefinitions("*,Auto")  
     };

     var infoStack = new StackPanel
     {
         Orientation = Orientation.Vertical,
         Spacing = 4
     };

     infoStack.Children.Add(new TextBlock
     {
         Text = "CS103 | Software Dev",
         FontWeight = FontWeight.Bold,
         FontSize = 14,
         Foreground = new SolidColorBrush(Color.Parse("#F0F0F0"))
     });

      infoStack.Children.Add(new TextBlock
     {
         Text = "34 students",
         FontSize = 12,
         Foreground = new SolidColorBrush(Color.Parse("#A0A0A0"))
     });

     Grid.SetColumn(infoStack, 0);
     cardGrid.Children.Add(infoStack);
     
        var chevron = new TextBlock
        {
            Text = "›",
            FontSize = 20,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = new SolidColorBrush(Color.Parse("#A0A0A0"))
        };
    Grid.SetColumn(chevron, 1);
    cardGrid.Children.Add(chevron);

      return new Border
    {
        Background = new SolidColorBrush(Color.Parse("#3A3A3D")),
        CornerRadius = new CornerRadius(8),
        Padding = new Thickness(16),
        Margin = new Thickness(0, 0, 0, 8), // Spacing between cards
        Child = cardGrid,
        Tag = $"{course} {section}"

    };

}

private Control BuildClassListPanel()
{
    // Left panel has 3 rows: Header, Search, Scrollable content
    var leftGrid = new Grid
    {
        RowDefinitions = new RowDefinitions("Auto,Auto,*"),
        Background = new SolidColorBrush(Color.Parse("#2D2D30"))
    };

    // Row 0: Header
    var header = BuildHeader();
    Grid.SetRow(header, 0);
    leftGrid.Children.Add(header);

    // Row 1: Search Box
    var searchBox = BuildSearchBox();
    Grid.SetRow(searchBox, 1);
    leftGrid.Children.Add(searchBox);

    // Row 2: Scrollable Content
    _emptyState = new StackPanel
    {
        Orientation = Orientation.Vertical,
        Spacing = 8,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
        Children =
        {
            new TextBlock
            {
                Text = "No classes yet",
                FontSize = 22,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.Parse("#F0F0F0"))
            },
            new TextBlock
            {
                Text = "Click + ADD to create your first class",
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.Parse("#A0A0A0"))
            }
        }
    };

    _cardsPanel = new StackPanel
    {
        Orientation = Orientation.Vertical,
        Spacing = 12,
        IsVisible = false,
        Margin = new Thickness(16, 0, 16, 0)
    };

    var contentHost = new Grid();
    contentHost.Children.Add(_emptyState);
    contentHost.Children.Add(_cardsPanel);

    var scroll = new ScrollViewer
    {
        Content = contentHost,
        VerticalScrollBarVisibility = ScrollBarVisibility.Auto
    };
    Grid.SetRow(scroll, 2);
    leftGrid.Children.Add(scroll);

    return leftGrid;
}

    // Runs when "+ Add" is clicked.
    private void OnAddClicked(object? sender, RoutedEventArgs e)
    {
        // Safety check: if any field is somehow null, don't crash.
        if (_cardsPanel == null || _emptyState == null) return;

        // Add a new card to the panel's children list.
        _cardsPanel.Children.Add(BuildCard());

        // Show cards, hide empty state.
        _cardsPanel.IsVisible = true;
        _emptyState.IsVisible = false;
    }

    // Runs when "Import" is clicked.
    private void OnImportClicked(object? sender, RoutedEventArgs e)
    {
        // Temporary placeholder — writes to the debug output (not console)
        Console.WriteLine("Import clicked!");
    }

    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_cardsPanel == null) return;
        if (sender is not TextBox searchBox) return;

        var query = searchBox.Text?.Trim().ToLowerInvariant() ?? "";

        foreach (var child in _cardsPanel.Children)
        {
            if (child is Border card && card.Tag is string searchText)
            {
                card.IsVisible = string.IsNullOrEmpty(query) ||
                                searchText.ToLowerInvariant().Contains(query);
            }
        }
    }
}
