using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Numerics;

namespace Unitversal
{
    //
    //————————————————————App Settings————————————————————
    //
    /// <summary>
    /// Stores information from the unit's database entry.
    /// </summary>
    public class Entry
    {
        public string Unit;
        public string Type;
        public string SI;
        public string Description;
        public List<string> AlternateNames;
        public List<string> Symbols;
        public List<string> Abbreviations;
        public List<string> AmericanSpelling;
        public List<string> Plurals;
    }
    /// <summary>
    /// Stores the primary state of the app.
    /// </summary>
    public static class AppState
    {
        //Database
        public static bool AlternateExist;
        public static bool InfoExist;
        /// <summary>
        /// <see langword="Dictionary"/> of all unit names and alternate names as keys linked to their primary unit names as values.
        /// </summary>
        public static Dictionary<string,string> UnitList = new Dictionary<string, string>();
        /// <summary>
        /// <see langword="HashSet"/> of all plural unit names.
        /// </summary>
        public static HashSet<string> UnitListPlural = new HashSet<string>();
        /// <summary>
        /// <see langword="ListViewItem"/>[] of all unit names excluding plurals.
        /// </summary>
        public static ListViewItem[] UnitListNoPlural;
        /// <summary>
        /// Unit information stored as an <see cref="Entry"/> inside a <see langword="Dictionary"/> and organized by unit type inside the main <see langword="Dictionary"/>.
        /// </summary>
        public static Dictionary<string, Dictionary<string, Entry>> UnitCache = new Dictionary<string, Dictionary<string, Entry>>();
        /// <summary>
        /// <see langword="Dictionary"/> of all inexact SI equivalent values of units as a string.
        /// </summary>
        public static Dictionary<string, string> InexactValues;
        /// <summary>
        /// <see langword="HashSet"/> of special units that require specific equations for conversion.
        /// </summary>
        public static HashSet<string> SpecialUnits = new HashSet<string>
        {
            { "Celsius" },
            { "Day Per Foot" },
            { "Day Per Kilometre" },
            { "Day Per Metre" },
            { "Day Per Mile" },
            { "Delisle" },
            { "Fahrenheit" },
            { "Hour Per Foot" },
            { "Hour Per Kilometre" },
            { "Hour Per Metre" },
            { "Hour Per Mile" },
            { "Kelvin" },
            { "Litre Per 100 Kilometre" },
            { "Litre Per Kilometre" },
            { "Litre Per Metre" },
            { "Minute Per Foot" },
            { "Minute Per Kilometre" },
            { "Minute Per Metre" },
            { "Minute Per Mile" },
            { "Newton (Temperature)" },
            { "Rankine" },
            { "Réaumur" },
            { "Rømer" },
            { "Second Per Foot" },
            { "Second Per Kilometre" },
            { "Second Per Metre" },
            { "Second Per Mile" },
            { "US Gallon Per Mile" }
        };
        /// <summary>
        /// Connection to the setting file using the Windows ini API.
        /// </summary>
        public static IniFile SettingsFile = new IniFile();
        /// <summary>
        /// Path to the database file.
        /// </summary>
        public static string DatabasePath = "db.db";
        /// <summary>
        /// Enable or disable the window from resize by dragging the edges or corners.
        /// </summary>
        public static bool Resize = true;
        /// <summary>
        /// Get information about unit from interpretation text instead of <see cref="MainWindow"/>.SearchView.
        /// </summary>
        public static bool InterpretInfo = false;
        /// <summary>
        /// Text of the interpretation of the user <see cref="MainWindow"/>.SearchBox query.
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
        /// Tracks whether the text in <see cref="MainWindow"/>.SearchBox is greater than its width.
        /// </summary>
        public static bool EntryMaxed = false;
        /// <summary>
        /// Tracks whether the search box query contains recognizable units from database.
        /// </summary>
        public static bool Recognized = false;
        /// <summary>
        /// The magnitude of the query entered in the <see cref="MainWindow"/>.SearchBox.
        /// </summary>
        public static BigDecimal Magnitude;
        /// <summary>
        /// The type of query entered in the <see cref="MainWindow"/>.SearchBox.
        /// </summary>
        public static string QueryType;
        /// <summary>
        /// Information for the best unit 1 match.
        /// </summary>
        public static Entry Unit1 = new Entry();
        /// <summary>
        /// Information for the best unit 2 match.
        /// </summary>
        public static Entry Unit2 = new Entry();
        /// <summary>
        /// List of unit 1 best matches.
        /// </summary>
        public static List<string> Unit1BestMatches = new List<string>();
        /// <summary>
        /// List of unit 2 best matches.
        /// </summary>
        public static List<string> Unit2BestMatches = new List<string>();
        /// <summary>
        /// Disables sorting for <see cref="MainWindow"/>.SearchView.
        /// </summary>
        public static bool DisableSort = false;
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
        /// Pen color according to <see cref="AppState"/>.Theme.
        /// </summary>
        public static Pen SystemColorPen;
        /// <summary>
        /// Tracks whether the user is currently exploring all units.
        /// </summary>
        public static bool Explore = false;
        //Currency API from https://fiscaldata.treasury.gov/datasets/treasury-reporting-rates-exchange/treasury-reporting-rates-of-exchange
        /// <summary>
        /// The year to get currency data from <see cref="AppState"/>.CurrencyAPI.
        /// </summary>
        public static string APIYear;
        /// <summary>
        /// The calendar quarter to get currency data from <see cref="AppState"/>.CurrencyAPI.
        /// </summary>
        public static int APIQuarter;
        /// <summary>
        /// Currency data API formatted with <see cref="AppState"/>.APIYear and <see cref="AppState"/>.APIQuarter.
        /// </summary>
        public static string CurrencyAPI;
        /// <summary>
        /// <see langword="Dictionary"/> of names from the currency API linked to its corresponding database names.
        /// </summary>
        public static readonly Dictionary<string, string> CurrencyAlias = new Dictionary<string, string>
        {
            { "Afghanistan", "Afghan Afghani" },
            { "Albania", "Albanian Lek" },
            { "Algeria", "Algerian Dinar" },
            { "Angola", "Angolan Kwanza" },
            { "Antigua & Barbuda", "Eastern Caribbean Dollar" },
            { "Argentina", "Argentina Peso" },
            { "Armenia", "Armenian Dram" },
            { "Australia", "Australian Dollar" },
            { "Azerbaijan", "Azerbaijani Manat" },
            { "Bahamas", "Bahamian Dollar" },
            { "Bahrain", "Bahraini Dinar" },
            { "Bangladesh", "Bangladeshi Taka" },
            { "Barbados", "Barbadian Dollar" },
            { "Belaruse", "Belarusian Ruble" },
            { "Belize", "Belize Dollar" },
            { "Benin", "West African CFA Franc" },
            { "Bermuda", "Bermudian Dollar" },
            { "Bolivia", "Bolivian Boliviano" },
            { "Bosnia", "Bosnia and Herzegovina Convertible Mark" },
            { "Botswana", "Botswana Pula" },
            { "Brazil", "Brazil Real" },
            { "Brunei", "Brunei Dollar" },
            { "Bulgaria", "Bulgarian Lev" },
            { "Burkina Faso", "West African CFA Franc" },
            { "Burma", "Myanmar Kyat" },
            { "Burundi", "Burundian Franc" },
            { "Cambodia", "Cambodian Riel" },
            { "Cameroon", "Central African CFA Franc" },
            { "Canada", "Canadian Dollar" },
            { "Cape Verde", "Cape Verdean Escudo" },
            { "Cayman Island", "Cayman Islands Dollar" },
            { "Central African Rep.", "Central African CFA Franc" },
            { "Chad", "Central African CFA Franc" },
            { "Chile", "Chilean Peso" },
            { "China", "Renminbi" },
            { "Colombia", "Colombian Peso" },
            { "Comoros", "Comorian Franc" },
            { "Congo", "Central African CFA Franc" },
            { "Costa Rica", "Costa Rican Colón" },
            { "Cote D'ivoire", "West African CFA Franc" },
            { "Croatia", "Croatian Kuna" },
            { "Cuba", "Cuban Peso" },
            { "Czech. Republic", "Czech Koruna" },
            { "Dem. Rep. of Congo", "Congolese Franc" },
            { "Denmark", "Danish Krone" },
            { "Djibouti", "Djiboutian Franc" },
            { "Dominican Republic", "Dominican Peso" },
            { "Ecuador", "United States Dollar" },
            { "Egypt", "Egyptian Pound" },
            { "El Salvador", "United States Dollar" },
            { "Equatorial Guinea", "Central African CFA Franc" },
            { "Eritrea", "Eritrean Nakfa" },
            { "Ethiopia", "Ethiopian Birr" },
            { "Euro Zone", "Euro" },
            { "Fiji", "Fijian Dollar" },
            { "Gabon", "Central African CFA Franc" },
            { "Gambia", "Gambian Dalasi" },
            { "Georgia", "Georgian Lari" },
            { "Ghana", "Ghanaian Cedi" },
            { "Grenada", "Eastern Caribbean Dollar" },
            { "Guatemala", "Guatemalan Quetzal" },
            { "Guinea", "Guinean Franc" },
            { "Guinea Bissau", "West African CFA Franc" },
            { "Guyana", "Guyanese Dollar" },
            { "Haiti", "Haitian Gourde" },
            { "Honduras", "Honduran Lempira" },
            { "Hong Kong", "Hong Kong Dollar" },
            { "Hungary", "Hungarian Forint" },
            { "Iceland", "Icelandic Króna" },
            { "India", "Indian Rupee" },
            { "Indonesia", "Indonesian Rupiah" },
            { "Iran", "Iranian Rial" },
            { "Iraq", "Iraqi Dinar" },
            { "Israel", "Israeli New Shekel" },
            { "Jamaica", "Jamaican Dollar" },
            { "Japan", "Japanese Yen" },
            { "Jordan", "Jordanian Dinar" },
            { "Kazakhstan", "Kazakhstani Tenge" },
            { "Kenya", "Kenyan Shilling" },
            { "Korea", "South Korean Won" },
            { "Kuwait", "Kuwaiti Dinar" },
            { "Kyrgyzstan", "Kyrgyzstani Som" },
            { "Laos", "Lao Kip" },
            { "Lebanon", "Lebanese Pound" },
            { "Lesotho", "Lesotho Loti" },
            { "Liberia", "Liberian Dollar" },
            { "Libya", "Libyan Dinar" },
            { "Madagascar", "Malagasy Ariary" },
            { "Malawi", "Malawian Kwacha" },
            { "Malaysia", "Malaysian Ringgit" },
            { "Maldives", "Maldivian Rufiyaa" },
            { "Mali", "West African CFA Franc" },
            { "Marshall Islands", "United States Dollar" },
            { "Mauritania", "Mauritanian Ouguiya" },
            { "Mauritius", "Mauritian Rupee" },
            { "Mexico", "Mexican Peso" },
            { "Micronesia", "United States Dollar" },
            { "Moldova", "Moldovan Leu" },
            { "Mongolia", "Mongolian Tögrög" },
            { "Morocco", "Moroccan Dirham" },
            { "Mozambique", "Mozambican Metical" },
            { "Nambia", "Namibian Dollar" },
            { "Nepal", "Nepalese Rupee" },
            { "Netherlands Antilles", "Netherlands Antillean Guilder" },
            { "New Zealand", "New Zealand Dollar" },
            { "Nicaragua", "Nicaraguan Córdoba" },
            { "Niger", "West African CFA Franc" },
            { "Nigeria", "Nigerian Naira" },
            { "Norway", "Norwegian Krone" },
            { "Oman", "Omani Rial" },
            { "Pakistan", "Pakistani Rupee" },
            { "Panama", "Panamanian Balboa" },
            { "Papua New Guinea", "Papua New Guinean Kina" },
            { "Paraguay", "Paraguayan Guaraní" },
            { "Peru", "Peruvian Sol" },
            { "Philippines", "Philippine Peso" },
            { "Poland", "Polish Złoty" },
            { "Qatar", "Qatari Riyal" },
            { "Rep. of N Macedonia", "North Macedonian Denar" },
            { "Republic of Palau", "United States Dollar" },
            { "Romania", "Romanian Leu" },
            { "Russia", "Russian Ruble" },
            { "Rwanda", "Rwandan Franc" },
            { "Sao Tome & Principe", "São Tomé and Príncipe Dobra" },
            { "Saudi Arabia", "Saudi Riyal" },
            { "Senegal", "West African CFA Franc" },
            { "Serbia", "Serbian Dinar" },
            { "Seychelles", "Seychellois Rupee" },
            { "Sierra Leone", "Sierra Leonean Leone" },
            { "Singapore", "Singapore Dollar" },
            { "Solomon Islands", "Solomon Islands Dollar" },
            { "Somali", "Somali Shilling" },
            { "South Sudan", "South Sudanese Pound" },
            { "South Africa", "South African Rand" },
            { "Sri Lanka", "Sri Lankan Rupee" },
            { "St. Lucia", "Eastern Caribbean Dollar" },
            { "Sudan", "Sudanese Pound" },
            { "Suriname", "Surinamese Dollar" },
            { "Swaziland", "Swazi Lilangeni" },
            { "Sweden", "Swedish Krona" },
            { "Switzerland", "Swiss Franc" },
            { "Syria", "Syrian Pound" },
            { "Taiwan", "New Taiwan Dollar" },
            { "Tajikistan", "Tajikistani Somoni" },
            { "Tanzania", "Tanzanian Shilling" },
            { "Thailand", "Thai Baht" },
            { "Timor", "United States Dollar" },
            { "Togo", "West African CFA Franc" },
            { "Tonga", "Tongan Paʻanga" },
            { "Trinidad & Tobago", "Trinidad and Tobago Dollar" },
            { "Tunisia", "Tunisian Dinar" },
            { "Turkey", "Turkish Lira" },
            { "Turkmenistan", "Turkmenistani Manat" },
            { "Uganda", "Ugandan Shilling" },
            { "Ukraine", "Ukrainian Hryvnia" },
            { "United Arab Emirates", "United Arab Emirates Dirham" },
            { "United Kingdom", "Sterling" },
            { "Uruguay", "Uruguayan Peso" },
            { "Uzbekistan", "Uzbekistani Soʻm" },
            { "Vanuatu", "Vanuatu Vatu" },
            { "Venezuela", "Venezuelan Bolívar" },
            { "Vietnam", "Vietnamese Đồng" },
            { "Western Samoa", "Samoan Tālā" },
            { "Yemen", "Yemeni Rial" },
            { "Zambia", "Zambian Kwacha" },
            { "Zimbabwe", "Real Time Gross Settlement Dollar" }
        };
    }
    /// <summary>
    /// Partially stores a previous state of the app.
    /// </summary>
    public static class PreviousState
    {
        //Search box
        public static string Query;
        public static BigDecimal Magnitude;
        public static string QueryType;
        public static List<string> Unit1BestMatches = new List<string>();
        public static List<string> Unit2BestMatches = new List<string>();
        //Sort menu
        public static string SortOrder;
        public static string SortBy;
        //Search view
        public static string SelectedUnit;
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
        public static int SignificantFigures = 10;
        public static string DecimalSeparator = ".";
        public static string IntegerGroupSeparator = ",";
        public static int IntegerGroupSize = 3;
        public static string DecimalGroupSeparator = " ";
        public static int DecimalGroupSize = 5;
        public static BigDecimal LargeMagnitude = new BigDecimal(1, 10);
        public static BigDecimal SmallMagnitude = new BigDecimal(1, -10);
        public static string Theme = "SYSTEM";
    }
    //
    //————————————————————Currency API————————————————————
    //
    public class Root
    {
        public Datum[] data { get; set; }
        public Links links { get; set; }
    }
    public class Links
    {
        public string self { get; set; }
        public string first { get; set; }
        public object prev { get; set; }
        public string next { get; set; }
        public string last { get; set; }
    }
    public class Datum
    {
        public string effective_date { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public string exchange_rate { get; set; }
    }
    //
    //————————————————————Settings Functions————————————————————
    //
    public partial class MainWindow : Form
    {
        /// <summary>
        /// Gets info about a unit as an <see cref="Entry"/> from cache. Faster than database file.
        /// </summary>
        /// <returns>
        /// The <see cref="Entry"/> of the unit if it exists in cache, otherwise an empty <see cref="Entry"/>.
        /// </returns>
        private static Entry GetUnitFromCache(string Unit)
        {
            foreach (KeyValuePair<string, Dictionary<string, Entry>> x in AppState.UnitCache)
            {
                if (x.Value.ContainsKey(Unit))
                {
                    return x.Value[Unit];
                }
            }
            return new Entry();
        }
        /// <summary>
        /// Calculates the inexact SI equivalences for certain units which will be stored in <see cref="AppState"/>.InexactValues
        /// </summary>
        private static void CalculateInexactValues()
        {
            BigDecimal Pi = BigDecimal.Parse("3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812", char.Parse(Settings.DecimalSeparator));
            BigDecimal Degree = Pi / 180;
            BigDecimal CandelaSquareInch = 1 / BigDecimal.Parse("0.00064516", char.Parse(Settings.DecimalSeparator));
            BigDecimal Neper = BigDecimal.Parse("0.868588963806503655302257837833210164588794011607333132228907566331729298417741549458449898676863496637412213489532607467283358574317927813138442129325", char.Parse(Settings.DecimalSeparator));
            BigDecimal PerMinute = (BigDecimal)1 / 60;
            BigDecimal PerHour = (BigDecimal)1 / 3600;
            BigDecimal Inch = BigDecimal.Parse("0.0254", char.Parse(Settings.DecimalSeparator));
            BigDecimal FootPerHour = BigDecimal.Parse("0.3048", char.Parse(Settings.DecimalSeparator)) * PerHour;
            BigDecimal Parsec = BigDecimal.Parse("30856775814913672.7891393795779647161073192116040917980140401922770232921869992968698321353388065559933270120238005882778324746263076049569688909836599", char.Parse(Settings.DecimalSeparator));
            BigDecimal kmPerHour = (BigDecimal)1000 * PerHour;
            BigDecimal kmPerGallon = 1000 / BigDecimal.Parse("0.003785411784", char.Parse(Settings.DecimalSeparator));
            BigDecimal PSI = BigDecimal.Parse("4.4482216152605", char.Parse(Settings.DecimalSeparator)) / BigDecimal.Parse("0.00064516", char.Parse(Settings.DecimalSeparator));
            BigDecimal Knot = (BigDecimal)1852 * PerHour;
            BigDecimal LitrePerMinute = BigDecimal.Parse("0.001", char.Parse(Settings.DecimalSeparator)) * PerMinute;
            BigDecimal LongAT = (BigDecimal)98 / 3000;
            BigDecimal MilePerDay = BigDecimal.Parse("1609.344", char.Parse(Settings.DecimalSeparator)) / 86400;
            BigDecimal Torr = (BigDecimal)101325 / 760;
            BigDecimal SecondPerFoot = (BigDecimal)1 / BigDecimal.Parse("0.3048", char.Parse(Settings.DecimalSeparator));
            BigDecimal SecondPerMile = SecondPerFoot / 5280;
            BigDecimal RightAngle = Pi / 2;
            BigDecimal SurveyFoot = (BigDecimal)1200 / 3937;
            BigDecimal SurveyMile = 5280 * SurveyFoot;
            BigDecimal SquareSurveyMile = SurveyMile * SurveyMile;
            BigDecimal ShortAT = (BigDecimal)175 / 6000;
            BigDecimal SquareSurveyFoot = SurveyFoot * SurveyFoot;
            BigDecimal Trit = BigDecimal.Parse("1.58496250072115618145373894394781650875981440769248106045575265454109822779435856252228047491808824209098066247505916734371755244106092482214208395062", char.Parse(Settings.DecimalSeparator));
            BigDecimal CubicSurveyFoot = SurveyFoot * SquareSurveyFoot;
            BigDecimal GallonPerHour = BigDecimal.Parse("0.003785411784", char.Parse(Settings.DecimalSeparator)) * PerHour;
            BigDecimal MilePerGallon = BigDecimal.Parse("1609.344", char.Parse(Settings.DecimalSeparator)) / BigDecimal.Parse("0.003785411784", char.Parse(Settings.DecimalSeparator));
            AppState.InexactValues = new Dictionary<string, string>()
            {
                { "Arcminute", BigDecimalNaturalFormat(Degree / 60) },
                { "Arcsecond", BigDecimalNaturalFormat(Degree / 3600) },
                { "Candela Per Square Foot", BigDecimalNaturalFormat(CandelaSquareInch / 144) },
                { "Candela Per Square Inch", BigDecimalNaturalFormat(CandelaSquareInch) },
                { "Centineper", BigDecimalNaturalFormat(0.01 * Neper) },
                { "Centesimal Minute", BigDecimalNaturalFormat(Pi / 20000) },
                { "Centesimal Second", BigDecimalNaturalFormat(Pi / 2000000) },
                { "Cubic Metre Per Minute", BigDecimalNaturalFormat(PerMinute) },
                { "Cubic Metre Per Hour", BigDecimalNaturalFormat(PerHour) },
                { "Day Per Foot", BigDecimalNaturalFormat(86400 * SecondPerFoot) },
                { "Day Per Mile", BigDecimalNaturalFormat(86400 * SecondPerMile) },
                { "Decineper", BigDecimalNaturalFormat(0.1 * Neper) },
                { "Degree", BigDecimalNaturalFormat(Degree) },
                { "Degree Per Second", BigDecimalNaturalFormat(Degree) },
                { "Desktop Publishing Pica", BigDecimalNaturalFormat(Inch / 6) },
                { "Desktop Publishing Point", BigDecimalNaturalFormat(Inch / 72) },
                { "Foot Per Day", BigDecimalNaturalFormat(FootPerHour / 24) },
                { "Foot Per Hour", BigDecimalNaturalFormat(FootPerHour) },
                { "Gigaparsec", BigDecimalNaturalFormat(1000000000 * Parsec) },
                { "Gradian", BigDecimalNaturalFormat(Pi / 200) },
                { "Hour Per Foot", BigDecimalNaturalFormat(3600 * SecondPerFoot) },
                { "Hour Per Mile", BigDecimalNaturalFormat(3600 * SecondPerMile) },
                { "Kilometre Per Day", BigDecimalNaturalFormat(kmPerHour / 24) },
                { "Kilometre Per Hour", BigDecimalNaturalFormat(kmPerHour) },
                { "Kilometre Per Minute", BigDecimalNaturalFormat(60 * kmPerHour) },
                { "Kilometre Per US Gallon", BigDecimalNaturalFormat(kmPerGallon) },
                { "Kiloparsec", BigDecimalNaturalFormat(1000 * Parsec) },
                { "Kilopound-Force Per Square Inch", BigDecimalNaturalFormat(1000 * PSI) },
                { "Knot", BigDecimalNaturalFormat(Knot) },
                { "Litre Per Hour", BigDecimalNaturalFormat(LitrePerMinute / 60) },
                { "Litre Per Minute", BigDecimalNaturalFormat(LitrePerMinute) },
                { "Long Assay Ton", BigDecimalNaturalFormat(LongAT) },
                { "Megaparsec", BigDecimalNaturalFormat(1000000 * Parsec) },
                { "Metre Per Day", BigDecimalNaturalFormat(PerHour / 24) },
                { "Metre Per Hour", BigDecimalNaturalFormat(PerHour) },
                { "Metre Per Minute", BigDecimalNaturalFormat(60 * PerHour) },
                { "Microarcsecond", BigDecimalNaturalFormat(Degree / 3600000000) },
                { "Mile Per Day", BigDecimalNaturalFormat(MilePerDay) },
                { "Milliarcsecond", BigDecimalNaturalFormat(Degree / 3600000) },
                { "Millitorr", BigDecimalNaturalFormat(Torr / 1000) },
                { "Minute Per Foot", BigDecimalNaturalFormat(60 * SecondPerFoot) },
                { "Minute Per Mile", BigDecimalNaturalFormat(60 * SecondPerMile) },
                { "NATO Mil", BigDecimalNaturalFormat(Pi / 3200) },
                { "Neper", BigDecimalNaturalFormat(Neper) },
                { "Parsec", BigDecimalNaturalFormat(Parsec) },
                { "Pound-Force Per Square Inch", BigDecimalNaturalFormat(PSI) },
                { "Revolution Per Hour", BigDecimalNaturalFormat(Pi / 1800) },
                { "Revolution Per Minute", BigDecimalNaturalFormat(Pi / 30) },
                { "Revolution Per Second", BigDecimalNaturalFormat(2 * Pi) },
                { "Right Angle", BigDecimalNaturalFormat(RightAngle) },
                { "Second Per Foot", BigDecimalNaturalFormat(SecondPerFoot) },
                { "Second Per Mile", BigDecimalNaturalFormat(SecondPerMile) },
                { "Section of Land", BigDecimalNaturalFormat(SquareSurveyMile) },
                { "Short Assay Ton", BigDecimalNaturalFormat(ShortAT) },
                { "Square Survey Foot", BigDecimalNaturalFormat(SquareSurveyFoot) },
                { "Square Survey Mile", BigDecimalNaturalFormat(SquareSurveyMile) },
                { "Straight Angle", BigDecimalNaturalFormat(Pi) },
                { "Swedish Streck", BigDecimalNaturalFormat(Pi / 3150) },
                { "Torr", BigDecimalNaturalFormat(Torr) },
                { "Township", BigDecimalNaturalFormat(36 * SquareSurveyMile) },
                { "Trit", BigDecimalNaturalFormat(Trit) },
                { "Tryte", BigDecimalNaturalFormat(8 * Trit) },
                { "Turn", BigDecimalNaturalFormat(2 * Pi) },
                { "US Acre", BigDecimalNaturalFormat(43560 * SquareSurveyFoot) },
                { "US Acre-Foot", BigDecimalNaturalFormat(43560 * CubicSurveyFoot) },
                { "US Cable Length", BigDecimalNaturalFormat(720 * SurveyFoot) },
                { "US Chain", BigDecimalNaturalFormat(66 * SurveyFoot) },
                { "US Fathom", BigDecimalNaturalFormat(6 * SurveyFoot) },
                { "US Furlong", BigDecimalNaturalFormat(660 * SurveyFoot) },
                { "US Gallon Per Hour", BigDecimalNaturalFormat(GallonPerHour) },
                { "US Gallon Per Mile", BigDecimalNaturalFormat(1 / MilePerGallon) },
                { "US League", BigDecimalNaturalFormat(3 * SurveyMile) },
                { "US Link", BigDecimalNaturalFormat(0.66 * SurveyFoot) },
                { "US Mile Per Gallon", BigDecimalNaturalFormat(MilePerGallon) },
                { "US Rod", BigDecimalNaturalFormat(16.5 * SurveyFoot) },
                { "US Square Rod", BigDecimalNaturalFormat(272.25 * SquareSurveyFoot) },
                { "US Survey Foot", BigDecimalNaturalFormat(SurveyFoot) },
                { "US Survey Mile", BigDecimalNaturalFormat(SurveyMile) },
                { "Warsaw Pact Mil", BigDecimalNaturalFormat(Pi / 3000) }
            };
        }
        /// <summary>
        /// Writes settings to settings file.
        /// </summary>
        private void WriteSettings()
        {
            //Erase previous settings to avoid an issue in Windows ini read api where it cannot read space.
            //Spaces accumulate as a result if the value to be written is a space.
            AppState.SettingsFile.DeleteSection("Settings");
            //Write default settings and create file if not exists
            AppState.SettingsFile.Write("Settings", "RememberPosition", $"{Settings.RememberPosition}");
            AppState.SettingsFile.Write("Settings", "RememberSize", $"{Settings.RememberSize}");
            AppState.SettingsFile.Write("Settings", "UpdateCurrencies", $"{Settings.UpdateCurrencies}");
            AppState.SettingsFile.Write("Settings", "WindowPosition", $"{Settings.WindowPosition.X},{Settings.WindowPosition.Y}");
            AppState.SettingsFile.Write("Settings", "WindowSize", $"{Settings.WindowSize.Width},{Settings.WindowSize.Height}");
            AppState.SettingsFile.Write("Settings", "Maximized", $"{Settings.Maximized}");
            AppState.SettingsFile.Write("Settings", "SortOrder", $"{Settings.SortOrder}");
            AppState.SettingsFile.Write("Settings", "SortBy", $"{Settings.SortBy}");
            AppState.SettingsFile.Write("Settings", "SignificantFigures", $"{Settings.SignificantFigures}");
            AppState.SettingsFile.Write("Settings", "DecimalSeparator", $"{Settings.DecimalSeparator}");
            AppState.SettingsFile.Write("Settings", "IntegerGroupSeparator", $"{Settings.IntegerGroupSeparator}");
            AppState.SettingsFile.Write("Settings", "IntegerGroupSize", $"{Settings.IntegerGroupSize}");
            AppState.SettingsFile.Write("Settings", "DecimalGroupSeparator", $"{Settings.DecimalGroupSeparator}");
            AppState.SettingsFile.Write("Settings", "DecimalGroupSize", $"{Settings.DecimalGroupSize}");
            AppState.SettingsFile.Write("Settings", "LargeMagnitude", $"{Settings.LargeMagnitude}");
            AppState.SettingsFile.Write("Settings", "SmallMagnitude", $"{Settings.SmallMagnitude}");
            AppState.SettingsFile.Write("Settings", "Theme", $"{Settings.Theme}");
        }
        /// <summary>
        /// Restores settings from settings file.
        /// </summary>
        private void RestoreSettings()
        {
            //Restore position
            if (AppState.SettingsFile.Read("Settings", "RememberPosition").Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                //Disable CenterScreen
                StartPosition = FormStartPosition.Manual;
                //Restore checkbox
                PositionCheckbox.Checked = true;
                //Restore window position
                var WindowPosition = AppState.SettingsFile.Read("Settings", "WindowPosition").Replace(" ", "").Split(",");
                if (int.TryParse(WindowPosition[0], out _) && int.TryParse(WindowPosition[1], out _))
                {
                    //Apply settings to program
                    Location = new Point(Convert.ToInt32(WindowPosition[0]), Convert.ToInt32(WindowPosition[1]));
                    //Restore to settings class
                    Settings.WindowPosition = Location;
                }
                //Restore to settings class
                Settings.RememberPosition = true;
            }
            //Restore size
            if (AppState.SettingsFile.Read("Settings", "RememberSize").Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                //Restore checkbox
                SizeCheckbox.Checked = true;
                //Check if window was maximized
                string Maximized = AppState.SettingsFile.Read("Settings", "Maximized");
                if (Maximized.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                {
                    WindowState = FormWindowState.Maximized;
                }
                //Get size otherwise
                else
                {
                    var WindowSize = AppState.SettingsFile.Read("Settings", "WindowSize").Replace(" ", "").Split(",");
                    if (int.TryParse(WindowSize[0], out _) && int.TryParse(WindowSize[1], out _))
                    {
                        //Restore previous size
                        Size = new Size(Convert.ToInt32(WindowSize[0]), Convert.ToInt32(WindowSize[1]));
                        //Restore to settings class
                        Settings.WindowSize = Size;
                    }
                }
                //Restore to settings class
                Settings.RememberSize = true;
                Settings.Maximized = Convert.ToBoolean(Maximized);
            }
            //Restore update currency
            if (AppState.SettingsFile.Read("Settings", "UpdateCurrencies").Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                //Restore checkbox
                CurrencyCheckbox.Checked = true;
                //Restore to settings class
                Settings.UpdateCurrencies = true;
            }
            //Restore sort order
            string Order = AppState.SettingsFile.Read("Settings", "SortOrder").ToUpper();
            if (Order == "ASCENDING")
            {
                SortAscending.PerformClick();
                Settings.SortOrder = Order;
            }
            else if (Order == "DESCENDING")
            {
                SortDescending.PerformClick();
                Settings.SortOrder = Order;
            }
            //Restore sort by
            string SortBy = AppState.SettingsFile.Read("Settings", "SortBy").ToUpper();
            if (SortBy == "UNIT")
            {
                SortUnit.PerformClick();
                Settings.SortBy = SortBy;
            }
            else if (SortBy == "MAGNITUDE")
            {
                SortMagnitude.PerformClick();
                Settings.SortBy = SortBy;
            }
            //Restore significant figures
            if (int.TryParse(AppState.SettingsFile.Read("Settings", "SignificantFigures"), out int SignificantFigures))
            {
                if (SignificantFigures > 0 && SignificantFigures <= SignificantFiguresEntry.Maximum)
                {
                    SignificantFiguresEntry.Value = SignificantFigures;
                    Settings.SignificantFigures = SignificantFigures;
                }
            }
            //Get separators
            string DecimalSeparator = AppState.SettingsFile.Read("Settings", "DecimalSeparator");
            string IntegerGroupSeparator = AppState.SettingsFile.Read("Settings", "IntegerGroupSeparator");
            string DecimalGroupSeparator = AppState.SettingsFile.Read("Settings", "DecimalGroupSeparator");
            if (DecimalSeparator.Length == 0)
            {
                DecimalSeparator = " "; //Issue with Windows ini read api where it cannot read space
            }
            if (IntegerGroupSeparator.Length == 0)
            {
                IntegerGroupSeparator = " ";
            }
            if (DecimalGroupSeparator.Length == 0)
            {
                DecimalGroupSeparator = " ";
            }
            //Decimal separator cannot equal integer group or decimal group separator
            if (DecimalSeparator == IntegerGroupSeparator)
            {
                if (DecimalSeparator == Settings.DecimalSeparator)
                {
                    IntegerGroupSeparator = Settings.IntegerGroupSeparator;
                }
                else
                {
                    DecimalSeparator = Settings.DecimalSeparator;
                }
            }
            if (DecimalSeparator == DecimalGroupSeparator)
            {
                if (DecimalSeparator == Settings.DecimalSeparator)
                {
                    DecimalGroupSeparator = Settings.DecimalGroupSeparator;
                }
                else
                {
                    DecimalSeparator = Settings.DecimalSeparator;
                }
            }
            //Restore decimal separator
            if (DecimalSeparator.Length == 1 && !int.TryParse(DecimalSeparator, out _))
            {
                DecimalSeparatorEntry.Text = DecimalSeparator;
                Settings.DecimalSeparator = DecimalSeparator;
            }
            //Restore integer group separator
            if (IntegerGroupSeparator.Length == 1 && !int.TryParse(IntegerGroupSeparator, out _))
            {
                IntegerGroupSeparatorEntry.Text = IntegerGroupSeparator;
                Settings.IntegerGroupSeparator = IntegerGroupSeparator;
            }
            //Restore integer group size
            if (int.TryParse(AppState.SettingsFile.Read("Settings", "IntegerGroupSize"), out int IntegerGroupSize))
            {
                if (IntegerGroupSize >= 0 && IntegerGroupSize <= 9)
                {
                    IntegerGroupSizeEntry.Value = IntegerGroupSize;
                    Settings.IntegerGroupSize = IntegerGroupSize;
                }
            }
            //Restore decimal group separator
            if (DecimalGroupSeparator.Length == 1 && !int.TryParse(DecimalGroupSeparator, out _))
            {
                DecimalGroupSeparatorEntry.Text = DecimalGroupSeparator;
                Settings.DecimalGroupSeparator = DecimalGroupSeparator;
            }
            //Restore decimal group size
            if (int.TryParse(AppState.SettingsFile.Read("Settings", "DecimalGroupSize"), out int DecimalGroupSize))
            {
                if (DecimalGroupSize >= 0 && DecimalGroupSize <= 9)
                {
                    DecimalGroupSizeEntry.Value = DecimalGroupSize;
                    Settings.DecimalGroupSize = DecimalGroupSize;
                }
            }
            //Restore scientific notation large magnitude
            var LargeMagnitude = AppState.SettingsFile.Read("Settings", "LargeMagnitude").ToUpper().Split("E");
            if (LargeMagnitude.Length == 2)
            {
                if (BigInteger.TryParse(LargeMagnitude[0], out BigInteger Mantissa) && int.TryParse(LargeMagnitude[1], out int Exponent))
                {
                    LargeMagnitudeEntry.Value = (decimal)Mantissa;
                    LargeExponentEntry.Value = Exponent;
                    Settings.LargeMagnitude = new BigDecimal(Mantissa, Exponent);
                }
            }
            //Restore scientific notation large magnitude
            var SmallMagnitude = AppState.SettingsFile.Read("Settings", "SmallMagnitude").ToUpper().Split("E");
            if (SmallMagnitude.Length == 2)
            {
                if (BigInteger.TryParse(SmallMagnitude[0], out BigInteger Mantissa) && int.TryParse(SmallMagnitude[1], out int Exponent))
                {
                    SmallMagnitudeEntry.Value = (decimal)Mantissa;
                    SmallExponentEntry.Value = Exponent * -1;
                    Settings.SmallMagnitude = new BigDecimal(Mantissa, Exponent);
                }
            }
            //Restore app theme
            string Theme = AppState.SettingsFile.Read("Settings", "Theme").ToUpper();
            if (Theme == "DARK")
            {
                DarkMode.Checked = true;
                Settings.Theme = Theme;
            }
            else if (Theme == "LIGHT")
            {
                LightMode.Checked = true;
                Settings.Theme = Theme;
            }
            else
            {
                SystemMode.Checked = true;
            }
            ChangeTheme(Settings.Theme);
        }
    }
}