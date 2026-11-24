
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
        Width = 400;
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
            RowDefinitions = new RowDefinitions("Auto,*")
        };

        // Build the header and place it in row 0.
        var header = BuildHeader();
        Grid.SetRow(header, 0);          // assigns the Grid.Row attached property
        rootGrid.Children.Add(header);   // attach it to the Grid

        // ── Empty state (what shows when there are 0 classes) ────────────
        // Vertical StackPanel, centered horizontally + vertically in its slot.
        var emptyState = new StackPanel
        {
            Orientation = Orientation.Vertical,                  // stack top-to-bottom
            Spacing = 8,                                         // 8px between children
            HorizontalAlignment = HorizontalAlignment.Center,    // center block horizontally
            VerticalAlignment = VerticalAlignment.Center,        // center block vertically
            Children =
            {
                // Primary line: big, bold, light-colored.
                new TextBlock
                {
                    Text = "No classes yet",
                    FontSize = 22,
                    FontWeight = FontWeight.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.Parse("#F0F0F0"))
                },
                // Secondary hint line: smaller, gray.
                new TextBlock
                {
                    Text = "Click + ADD to create your first class",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.Parse("#A0A0A0"))
                }
            }
        };

        // ── Cards panel (the container that will hold class cards) ───────
        // Starts invisible; shown on first click of + Add.
        _cardsPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Spacing = 12,         // 12px gap between cards
            IsVisible = false     // hidden until there's at least one card
        };

        // ── emptyHost: a full-size Grid that fills the scroll area ───────
        // Its only job is to give emptyState and cardsPanel a stretched parent
        // so "Center" alignment actually has space to center *within*.
        var emptyHost = new Grid
        {
            RowDefinitions = new RowDefinitions("*"),      // single row, fills height
            ColumnDefinitions = new ColumnDefinitions("*") // single column, fills width
        };

        // Expose the empty state via a field so the click handler can hide it.
        _emptyState = emptyState;

        // Both the empty state and the cards panel live in the SAME grid cell.
        // They visually overlap, but IsVisible decides which one shows.
        emptyHost.Children.Add(emptyState);
        emptyHost.Children.Add(_cardsPanel);

        // ── ScrollViewer: lets the content grow and scroll ───────────────
        // Auto = only show a scrollbar when content overflows.
        var scroll = new ScrollViewer
        {
            Content = emptyHost,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto
        };

        // ── mainContent: the big area under the header ───────────────────
        // Background fill + 24px padding so content doesn't touch edges.
        var mainContent = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#252526")),
            Padding = new Thickness(24),
            Child = scroll
        };

        // Place mainContent in row 1 (the *-sized row → fills remaining height).
        Grid.SetRow(mainContent, 1);
        rootGrid.Children.Add(mainContent);

        // Hand the whole tree back to the constructor, which sets Content.
        return rootGrid;
    }

    // Builds the top header bar: title on left, actions on right.
    private Control BuildHeader()
    {
        // Two-column grid:
        //   col 0 = "Auto"  → hugs the title text
        //   col 1 = "*"     → fills remaining width (where the buttons live)
        // 16px left/right margin so contents don't touch the window edges.
        var headerGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,*"),
            Margin = new Thickness(16, 0, 16, 0)
        };

        // Left side: the title text.
        var title = new TextBlock
        {
            Text = "MY CLASSES",
            FontWeight = FontWeight.Bold,
            VerticalAlignment = VerticalAlignment.Top,        // anchored to top...
            Margin = new Thickness(0, 12, 0, 0),              // ...plus 12px top margin
            Foreground = new SolidColorBrush(Color.Parse("#F0F0F0"))
        };

        // Search box — dark fill, light text, gray hint text when empty.
        var searchBox = new TextBox
        {
            Watermark = "Search...",
            Width = 180,
            VerticalAlignment = VerticalAlignment.Center,
            Background = new SolidColorBrush(Color.Parse("#3A3A3D")),
            Foreground = new SolidColorBrush(Color.Parse("#F0F0F0")),
            WatermarkForeground = new SolidColorBrush(Color.Parse("#A0A0A0"))
        };

        // "Import" button — neutral dark style.
        var importBtn = new Button
        {
            Content = "Import",
            VerticalAlignment = VerticalAlignment.Center,
            Background = new SolidColorBrush(Color.Parse("#3A3A3D")),
            Foreground = new SolidColorBrush(Color.Parse("#F0F0F0"))
        };
        importBtn.Click += OnImportClicked;

        // "+ Add" button — green accent, the primary action.
        var addBtn = new Button
        {
            Content = "+ Add",
            FontWeight = FontWeight.Bold,
            VerticalAlignment = VerticalAlignment.Center,
            Background = new SolidColorBrush(Color.Parse("#2E7D32")),
            Foreground = Brushes.White
        };
        addBtn.Click += OnAddClicked;

        // Group the three controls horizontally with 8px gaps.
        // Right-aligned so the group hugs the right edge of its column.
        // Bottom-aligned (with 12px bottom margin) so it sits lower in the header.
        var actionsPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 8,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = new Thickness(0, 0, 0, 12),
            Children = { searchBox, importBtn, addBtn }
        };

        // Place title in col 0 and actions in col 1.
        Grid.SetColumn(title, 0);
        headerGrid.Children.Add(title);

        Grid.SetColumn(actionsPanel, 1);
        headerGrid.Children.Add(actionsPanel);

        // Outer Border gives the header a full-width gray background + fixed height.
        return new Border
        {
            Background = new SolidColorBrush(Color.Parse("#2D2D30")),
            Height = 90,
            Child = headerGrid
        };
    }

    // Builds ONE card. Placeholder content — becomes the real class card later.
    private Control BuildCard()
    {
        return new Border
        {
            Background = new SolidColorBrush(Color.Parse("#3A3A3D")),
            CornerRadius = new CornerRadius(8),        // rounded corners
            Padding = new Thickness(16),               // inner spacing
            MinHeight = 60,                            // at least 60px, grows with content
            Child = new TextBlock
            {
                Text = "maow",
                FontSize = 14,
                Foreground = new SolidColorBrush(Color.Parse("#F0F0F0"))
            }
        };
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
        // Temporary placeholder — writes to the debug output (not console).
        // In VS Code, view it in the "Debug Console" panel while running with F5.
        System.Diagnostics.Debug.WriteLine("Import clicked!");
    }
}
