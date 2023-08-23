namespace Unitversal;

/// <summary>
/// Stores the primary state of the app.
/// </summary>
public static class AppState
{
    /// <summary>
    /// Location of the app as an absolute path.
    /// </summary>
    readonly static string CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
    /// <summary>
    /// Connection to the setting file using the Windows ini API.
    /// </summary>
    public static IniFile SettingsFile = new IniFile(CurrentDirectory + "Unitversal");
    /// <summary>
    /// Path to the database file.
    /// </summary>
    public static string DatabasePath = CurrentDirectory + "db.db";
    /// <summary>
    /// Enable or disable the window from resize by dragging the edges or corners.
    /// </summary>
    public static bool Resize = true;
    /// <summary>
    /// <see cref="string"/> of the interpretation of the user query in <see cref="MainWindow.SearchBox"/>.
    /// </summary>
    public static string Interpretation;
    /// <summary>
    /// Tracks whether the interpret label tooltip is shown.
    /// </summary>
    public static bool InterpretToolTipShown = false;
    /// <summary>
    /// Tracks the widget that was right clicked.
    /// </summary>
    public static string RightClickLocation;
    /// <summary>
    /// Tracks whether the sort menu is shown.
    /// </summary>
    public static bool SortShown = false;
    /// <summary>
    /// Tracks whether the text in <see cref="MainWindow.SearchBox"/> is greater than its width.
    /// </summary>
    public static bool EntryMaxed = false;
    /// <summary>
    /// Tracks whether the search box query contains recognizable units from database.
    /// </summary>
    public static bool Recognized = false;
    /// <summary>
    /// The theme of the app.
    /// </summary>
    public static string Theme;
    /// <summary>
    /// The background color of all widgets.
    /// </summary>
    public static Color BackgroundColor;
    /// <summary>
    /// The foreground color of all widgets.
    /// </summary>
    public static Color ForegroundColor;
    /// <summary>
    /// The menu highlight color.
    /// </summary>
    public static Color MenuHighlight;
    /// <summary>
    /// Pen color according to <see cref="AppState.Theme"/>.
    /// </summary>
    public static Pen SystemColorPen;
    /// <summary>
    /// Tracks whether the user is exploring units.
    /// </summary>
    public static bool Explore = false;
    /// <summary>
    /// Tracks whether the sort menu is shown.
    /// </summary>
    public static bool ExploreSortShown = false;
    /// <summary>
    /// Tracks whether <see cref="MainWindow.CategoryView"/> is on a second menu.
    /// </summary>
    public static string SecondMenu = "";
    /// <summary>
    /// Tracks the first unit selected <see cref="MainWindow.TemperatureView"/>.
    /// </summary>
    public static string FirstUnit = "";
    /// <summary>
    /// Tracks the current view in explorer.
    /// </summary>
    public static string CurrentView = "";
}
/// <summary>
/// Partially stores a previous state of the app.
/// </summary>
public static class PreviousState
{
    //Search box
    public static BigDecimal Magnitude;
    public static string QueryType;
    public static List<string> Unit1BestMatches = new List<string>();
    public static List<string> Unit2BestMatches = new List<string>();
    //Sort menu
    public static string SortOrder;
    public static string SortBy;
    //Explorer
    public static string SecondMenu;
    public static string FirstUnit;
}
/// <summary>
/// Stores the settings of the app which will be stored in a file.
/// </summary>
public static class Settings
{
    public static bool RememberPosition = false;
    public static bool RememberSize = false;
    public static bool UpdateCurrencies = false;
    public static Point WindowPosition = new Point(0, 0);
    public static Size WindowSize = new Size(400, 350);
    public static bool Maximized = false;
    public static string SortOrder = "ASCENDING";
    public static string SortBy = "UNIT";
    public static int SignificantFigures = Calculate.SignificantFigures;
    public static string DecimalSeparator = BigDecimal.DecimalSeparator;
    public static string IntegerGroupSeparator = BigDecimal.IntegerGroupSeparator;
    public static int IntegerGroupSize = BigDecimal.IntegerGroupSize;
    public static string DecimalGroupSeparator = BigDecimal.DecimalGroupSeparator;
    public static int DecimalGroupSize = BigDecimal.DecimalGroupSize;
    public static BigDecimal LargeMagnitude = Calculate.LargeMagnitude;
    public static BigDecimal SmallMagnitude = Calculate.SmallMagnitude;
    public static string Theme = "SYSTEM";
}
