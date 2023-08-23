namespace UnitversalLibrary;

/// <summary>
/// Stores information about a unit's database entry.
/// </summary>
public class UnitEntry
{
    public string Unit;
    public string Type;
    public BigDecimal SI;
    public string Description;
    public List<string> AlternateNames = new List<string>();
    public List<string> Symbols = new List<string>();
    public List<string> Abbreviations = new List<string>();
    public List<string> Plurals = new List<string>();
    public List<string> Variants = new List<string>();
}
/// <summary>
/// Stores information about a prefix's database entry.
/// </summary>
public class PrefixEntry
{
    public PrefixEntry(string Prefix, string Symbol, string Factor, string StandardForm, string Description)
    {
        this.Prefix = Prefix;
        this.Symbol = Symbol;
        this.Factor = Factor;
        this.StandardForm = StandardForm;
        this.Description = Description;
    }
    public string Prefix;
    public string Symbol;
    public string Factor;
    public string StandardForm;
    public string Description;
}
/// <summary>
/// Currency API data root.
/// </summary>
public class Root
{
    public Datum[] data { get; set; }
    public Links links { get; set; }
}
/// <summary>
/// Currency API data links.
/// </summary>
public class Links
{
    public string self { get; set; }
    public string first { get; set; }
    public object prev { get; set; }
    public string next { get; set; }
    public string last { get; set; }
}
/// <summary>
/// Currency API datum.
/// </summary>
public class Datum
{
    public string effective_date { get; set; }
    public string country { get; set; }
    public string currency { get; set; }
    public string exchange_rate { get; set; }
}
/// <summary>
/// Unitversal database functions.
/// </summary>
public static class Database
{
    /// <summary>
    /// If the "Alternate Names" table exists in database.
    /// </summary>
    private static bool AlternateExist = false;
    /// <summary>
    /// If the "Binary Prefixes" table exists in database.
    /// </summary>
    private static bool BinaryPrefixExist = false;
    /// <summary>
    /// If the "Case Sensitive" table exists in database.
    /// </summary>
    private static bool CaseExist = false;
    /// <summary>
    /// If the "Currency Alias" table exists in database.
    /// </summary>
    private static bool CurrencyExist = false;
    /// <summary>
    /// If the "Info" table exists in database.
    /// </summary>
    private static bool InfoExist = false;
    /// <summary>
    /// If the "SI Equivalents" table exists in database.
    /// </summary>
    private static bool SIExists = false;
    /// <summary>
    /// If the "SI Prefixes" table exists in database.
    /// </summary>
    private static bool SIPrefixExists = false;
    /// <summary>
    /// If the "Special Units" table exists in database.
    /// </summary>
    private static bool SpecialExist = false;
    /// <summary>
    /// If the "Temperature Formulas" table exists in database.
    /// </summary>
    private static bool TemperatureExists = false;
    /// <summary>
    /// <see langword="Dictionary"/> of all unit names and alternate names as keys linked to their primary unit names as values.
    /// </summary>
    public static Dictionary<string, string> UnitList = new Dictionary<string, string>();
    /// <summary>
    /// <see langword="HashSet"/> of all plural unit names.
    /// </summary>
    public static HashSet<string> UnitListPlural = new HashSet<string>();
    /// <summary>
    /// <see langword="List"/> of all unit names excluding plurals. Presorted in ascending order.
    /// </summary>
    public static List<string> UnitListNoPlural = new List<string>();
    /// <summary>
    /// Unit information stored in a nested <see langword="Dictionary"/> in the format (Type, (Unit, <see cref="UnitEntry"/>)).
    /// </summary>
    public static Dictionary<string, Dictionary<string, UnitEntry>> UnitCache = new Dictionary<string, Dictionary<string, UnitEntry>>();
    /// <summary>
    /// <see langword="List"/> of categories of units in <see cref="Database.UnitCache"/>. Presorted in ascending order.
    /// </summary>
    public static List<string> UnitCategories = new List<string>();
    /// <summary>
    /// <see langword="HashSet"/> of case sensitive unit names. Stored in uppercase.
    /// </summary>
    public static HashSet<string> CasingList = new HashSet<string>();
    /// <summary>
    /// Temporary <see langword="List"/> of items to display in explore mode.
    /// </summary>
    public static List<string> ExploringItems;
    /// <summary>
    /// <see langword="Dictionary"/> of all inexact SI equivalent values of units as a string.
    /// </summary>
    public static Dictionary<string, BigDecimal> InexactValues;
    /// <summary>
    /// <see langword="HashSet"/> of special units that require specific equations for conversion.
    /// </summary>
    public static HashSet<string> SpecialUnits = new HashSet<string>();
    /// <summary>
    /// <see langword="Dictionary"/> where the key is a unit type and the value is the SI equivalent unit name used for that type.
    /// </summary>
    public static Dictionary<string, string> SIEquivalents = new Dictionary<string, string>();
    /// <summary>
    /// <see langword="Dictionary"/> of SI prefixes where the key is prefix name and value is prefix information stored as a <see cref="PrefixEntry"/>.
    /// </summary>
    public static Dictionary<string, PrefixEntry> SIPrefixes = new Dictionary<string, PrefixEntry>();
    /// <summary>
    /// <see langword="Dictionary"/> of binary prefixes where the key is prefix name and value is prefix information stored as a <see cref="PrefixEntry"/>.
    /// </summary>
    public static Dictionary<string, PrefixEntry> BinaryPrefixes = new Dictionary<string, PrefixEntry>();
    /// <summary>
    /// <see langword="Dictionary"/> that stores the formulas for converting between different
    /// temperature scales in the format (Unit1, (Unit2, (Formula, Description))).
    /// </summary>
    public static Dictionary<string, Dictionary<string, Tuple<string, string>>> TemperatureFormulas = new Dictionary<string, Dictionary<string, Tuple<string, string>>>();
    /// <summary>
    /// Currency API from https://fiscaldata.treasury.gov/datasets/treasury-reporting-rates-exchange/treasury-reporting-rates-of-exchange
    /// Formatted with <see cref="Database.APIYear"/> and <see cref="Database.APIQuarter"/>.
    /// </summary>
    public static string CurrencyAPI {
        get => $"https://api.fiscaldata.treasury.gov/services/api/fiscal_service/v1/accounting/od/rates_of_exchange?fields=effective_date,country,currency,exchange_rate&filter=record_calendar_year:eq:{Database.APIYear},record_calendar_quarter:eq:{Database.APIQuarter},page[size]=200&sort=country";
    }
    /// <summary>
    /// The year to get currency data from <see cref="Database.CurrencyAPI"/>.
    /// </summary>
    public static int APIYear;
    /// <summary>
    /// The calendar quarter to get currency data from <see cref="Database.CurrencyAPI"/>.
    /// </summary>
    public static int APIQuarter;
    /// <summary>
    /// <see langword="Dictionary"/> of countries from the currency API linked to its corresponding
    /// currency from database.
    /// </summary>
    public static Dictionary<string, string> CurrencyAlias = new Dictionary<string, string>();
    /// <summary>
    /// <see langword="string"/> of the sort orders for string lists.
    /// </summary>
    private static Dictionary<Int32, string> SortOrders = new Dictionary<Int32, string>
    {
        { UnitCategories.GetHashCode(), "ASCENDING" },
        { UnitListNoPlural.GetHashCode(), "ASCENDING" }
    };
    /// <summary>
    /// Sort a presorted string list ascending or descending in O(n).
    /// </summary>
    public static void SortStringList(List<string> List, string SortOrder)
    {
        if (SortOrder != SortOrders[List.GetHashCode()])
        {
            //Reverse() is O(n) for presorted list.
            List.Reverse();
            SortOrders[List.GetHashCode()] = SortOrder;
        }
    }
    /// <summary>
    /// Gets info about a unit as a <see cref="UnitEntry"/> from cache. Faster than database file.
    /// </summary>
    /// <returns>
    /// The <see cref="UnitEntry"/> of the unit if it exists in cache, otherwise <see langword="null"/>.
    /// </returns>
    public static UnitEntry GetUnitFromCache(string Unit)
    {
        foreach ((string Type, Dictionary<string, UnitEntry> Units) in UnitCache)
        {
            if (Units.ContainsKey(Unit))
            {
                return Units[Unit];
            }
        }
        return null;
    }
    /// <summary>
    /// Calculates the inexact SI equivalences for the units in the "Inexact Values" table in database.
    /// Results are stored in <see cref="Database.InexactValues"/>
    /// </summary>
    public static void CalculateInexactValues()
    {
        BigDecimal Pi = BigDecimal.Parse("3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812");
        BigDecimal Degree = Pi / 180;
        BigDecimal CandelaSquareInch = 1 / BigDecimal.Parse("0.00064516");
        BigDecimal Neper = BigDecimal.Parse("0.868588963806503655302257837833210164588794011607333132228907566331729298417741549458449898676863496637412213489532607467283358574317927813138442129325");
        BigDecimal PerMinute = (BigDecimal)1 / 60;
        BigDecimal PerHour = (BigDecimal)1 / 3600;
        BigDecimal Inch = BigDecimal.Parse("0.0254");
        BigDecimal FootPerHour = BigDecimal.Parse("0.3048") * PerHour;
        BigDecimal Parsec = BigDecimal.Parse("30856775814913672.7891393795779647161073192116040917980140401922770232921869992968698321353388065559933270120238005882778324746263076049569688909836599");
        BigDecimal kmPerHour = (BigDecimal)1000 * PerHour;
        BigDecimal kmPerGallon = 1000 / BigDecimal.Parse("0.003785411784");
        BigDecimal PSI = BigDecimal.Parse("4.4482216152605") / BigDecimal.Parse("0.00064516");
        BigDecimal Knot = (BigDecimal)1852 * PerHour;
        BigDecimal LitrePerMinute = BigDecimal.Parse("0.001") * PerMinute;
        BigDecimal LongAT = (BigDecimal)98 / 3000;
        BigDecimal MilePerDay = BigDecimal.Parse("1609.344") / 86400;
        BigDecimal Torr = (BigDecimal)101325 / 760;
        BigDecimal SecondPerFoot = (BigDecimal)1 / BigDecimal.Parse("0.3048");
        BigDecimal SecondPerMile = SecondPerFoot / 5280;
        BigDecimal RightAngle = Pi / 2;
        BigDecimal SurveyFoot = (BigDecimal)1200 / 3937;
        BigDecimal SurveyMile = 5280 * SurveyFoot;
        BigDecimal SquareSurveyMile = SurveyMile * SurveyMile;
        BigDecimal ShortAT = (BigDecimal)175 / 6000;
        BigDecimal SquareSurveyFoot = SurveyFoot * SurveyFoot;
        BigDecimal Trit = BigDecimal.Parse("1.58496250072115618145373894394781650875981440769248106045575265454109822779435856252228047491808824209098066247505916734371755244106092482214208395062");
        BigDecimal CubicSurveyFoot = SurveyFoot * SquareSurveyFoot;
        BigDecimal GallonPerHour = BigDecimal.Parse("0.003785411784") * PerHour;
        BigDecimal MilePerGallon = BigDecimal.Parse("1609.344") / BigDecimal.Parse("0.003785411784");
        InexactValues = new Dictionary<string, BigDecimal>()
        {
            { "Arcminute", Degree / 60 },
            { "Arcsecond", Degree / 3600 },
            { "Candela Per Square Foot", CandelaSquareInch / 144 },
            { "Candela Per Square Inch", CandelaSquareInch },
            { "Centesimal Minute", Pi / 20000 },
            { "Centesimal Second", Pi / 2000000 },
            { "Centineper", 0.01 * Neper },
            { "Circular Mil", Pi * new BigDecimal(16129, -14) },
            { "Cubic Metre Per Hour", PerHour },
            { "Cubic Metre Per Minute", PerMinute },
            { "Day Per Foot", 86400 * SecondPerFoot },
            { "Day Per Mile", 86400 * SecondPerMile },
            { "Decineper", 0.1 * Neper },
            { "Degree", Degree },
            { "Degree Per Second", Degree },
            { "Desktop Publishing Pica", Inch / 6 },
            { "Desktop Publishing Point", Inch / 72 },
            { "Enzyme Unit", new BigDecimal(1, -6) * PerMinute },
            { "Foot Per Day", FootPerHour / 24 },
            { "Foot Per Hour", FootPerHour },
            { "Gigaparsec", 1000000000 * Parsec },
            { "Gradian", Pi / 200 },
            { "Hour Per Foot", 3600 * SecondPerFoot },
            { "Hour Per Mile", 3600 * SecondPerMile },
            { "Kilometre Per Day", kmPerHour / 24 },
            { "Kilometre Per Hour", kmPerHour },
            { "Kilometre Per Minute", 60 * kmPerHour },
            { "Kilometre Per US Gallon", kmPerGallon },
            { "Kiloparsec", 1000 * Parsec },
            { "Kilopound-Force Per Square Inch", 1000 * PSI },
            { "Knot", Knot },
            { "Litre Per Hour", LitrePerMinute / 60 },
            { "Litre Per Minute", LitrePerMinute },
            { "Long Assay Ton", LongAT },
            { "Megaparsec", 1000000 * Parsec },
            { "Metre Diameter Circle", Pi / 4 },
            { "Metre Per Day", PerHour / 24 },
            { "Metre Per Hour", PerHour },
            { "Metre Per Minute", 60 * PerHour },
            { "Metre Radius Circle", Pi },
            { "Microarcsecond", Degree / 3600000000 },
            { "Mile Per Day", MilePerDay },
            { "Milliarcsecond", Degree / 3600000 },
            { "Millitorr", Torr / 1000 },
            { "Minute Per Foot", 60 * SecondPerFoot },
            { "Minute Per Mile", 60 * SecondPerMile },
            { "NATO Mil", Pi / 3200 },
            { "Neper", Neper },
            { "Parsec", Parsec },
            { "Pound-Force Per Square Inch", PSI },
            { "Revolution Per Hour", Pi / 1800 },
            { "Revolution Per Minute", Pi / 30 },
            { "Revolution Per Second", 2 * Pi },
            { "Right Angle", RightAngle },
            { "Second Per Foot", SecondPerFoot },
            { "Second Per Mile", SecondPerMile },
            { "Section of Land", SquareSurveyMile },
            { "Short Assay Ton", ShortAT },
            { "Square Survey Foot", SquareSurveyFoot },
            { "Square Survey Mile", SquareSurveyMile },
            { "Straight Angle", Pi },
            { "Swedish Streck", Pi / 3150 },
            { "Torr", Torr },
            { "Township", 36 * SquareSurveyMile },
            { "Trit", Trit },
            { "Tryte", 8 * Trit },
            { "Turn", 2 * Pi },
            { "US Acre", 43560 * SquareSurveyFoot },
            { "US Acre-Foot", 43560 * CubicSurveyFoot },
            { "US Cable Length", 720 * SurveyFoot },
            { "US Chain", 66 * SurveyFoot },
            { "US Fathom", 6 * SurveyFoot },
            { "US Furlong", 660 * SurveyFoot },
            { "US Gallon Per Hour", GallonPerHour },
            { "US Gallon Per Mile", 1 / MilePerGallon },
            { "US League", 3 * SurveyMile },
            { "US Link", 0.66 * SurveyFoot },
            { "US Mile Per Gallon", MilePerGallon },
            { "US Rod", 16.5 * SurveyFoot },
            { "US Square Rod", 272.25 * SquareSurveyFoot },
            { "US Survey Foot", SurveyFoot },
            { "US Survey Mile", SurveyMile },
            { "Warsaw Pact Mil", Pi / 3000 }
        };
    }
    /// <summary>
    /// Check for database file existence and corruption.
    /// </summary>
    /// <returns>
    /// 1 if there are no issues, 0 if database does not exist and -1 if database is corrupted.
    /// </returns>
    public static int Check(string DatabasePath)
    {
        void CheckTable(ref SqliteCommand Command, string Table, ref bool Flag)
        {
            Command.CommandText = $"SELECT tbl_name FROM sqlite_schema WHERE tbl_name='{Table}'";
            using (SqliteDataReader Reader = Command.ExecuteReader())
            {
                Flag = Reader.HasRows;
            }
        }
        if (File.Exists(DatabasePath))
        {
            using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadOnly"))
            {
                Connection.Open();
                SqliteCommand Command = Connection.CreateCommand();
                CheckTable(ref Command, "Alternate Names", ref Database.AlternateExist);
                CheckTable(ref Command, "Binary Prefixes", ref Database.BinaryPrefixExist);
                CheckTable(ref Command, "Case Sensitive", ref Database.CaseExist);
                CheckTable(ref Command, "Currency Alias", ref Database.CurrencyExist);
                CheckTable(ref Command, "Info", ref Database.InfoExist);
                CheckTable(ref Command, "SI Equivalents", ref Database.SIExists);
                CheckTable(ref Command, "SI Prefixes", ref Database.SIPrefixExists);
                CheckTable(ref Command, "Special Units", ref Database.SpecialExist);
                CheckTable(ref Command, "Temperature Formulas", ref Database.TemperatureExists);
                if (
                    !Database.AlternateExist ||
                    !Database.BinaryPrefixExist ||
                    !Database.CaseExist ||
                    !Database.CurrencyExist ||
                    !Database.InfoExist ||
                    !Database.SIExists ||
                    !Database.SIPrefixExists ||
                    !Database.SpecialExist ||
                    !Database.TemperatureExists
                )
                {
                    return -1;
                }
            }
            return 1;
        }
        return 0;
    }
    /// <summary>
    /// Creates the <see cref="Database.UnitList"/>, <see cref="Database.UnitListPlural"/>,
    /// <see cref="Database.UnitListNoPlural"/> and <see cref="Database.UnitCache"/>.
    /// </summary>
    public static void GetAllUnits(string DatabasePath)
    {
        //Fault tolerance; app still works if "Alternate Names" table does not exist
        if (!Database.InfoExist)
        {
            return;
        }
        using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadOnly"))
        {
            Connection.Open();
            SqliteCommand Command = Connection.CreateCommand();
            //Joined query not used due to low performance
            //Query Info table
            Command.CommandText = "SELECT * FROM Info";
            using (SqliteDataReader Reader = Command.ExecuteReader())
            {
                UnitEntry NewEntry;
                BigDecimal SI;
                while (Reader.Read())
                {
                    //Required values: Unit, Type, SI Equivalent
                    if (
                        Reader[0].GetType() == typeof(DBNull)
                        ||
                        Reader[1].GetType() == typeof(DBNull)
                        ||
                        Reader[2].GetType() == typeof(DBNull)
                        ||
                        !BigDecimal.TryParse(Reader.GetString(2), out SI)
                    )
                    {
                        continue;
                    }
                    //Get unit info
                    NewEntry = new UnitEntry();
                    NewEntry.Unit = Reader.GetString(0);
                    NewEntry.Type = Reader.GetString(1);
                    NewEntry.SI = SI;
                    if (Reader[3].GetType() != typeof(DBNull))
                    {
                        NewEntry.Description = Reader.GetString(3);
                    }
                    UnitList.Add(NewEntry.Unit, NewEntry.Unit);
                    UnitListNoPlural.Add(NewEntry.Unit);
                    //Cache unit
                    if (!UnitCache.ContainsKey(NewEntry.Type))
                    {
                        UnitCache.Add(NewEntry.Type, new Dictionary<string, UnitEntry>());
                        UnitCategories.Add(NewEntry.Type);
                    }
                    UnitCache[NewEntry.Type].Add(NewEntry.Unit, NewEntry);
                }
            }
            if (Database.AlternateExist)
            {
                //Query Alternate Names table
                Command.CommandText = "SELECT * FROM [Alternate Names]";
                using (SqliteDataReader Reader = Command.ExecuteReader())
                {
                    UnitEntry Entry;
                    string AlternateName;
                    while (Reader.Read())
                    {
                        //Required values: Unit, Alternate Name
                        if (
                            Reader[0].GetType() == typeof(DBNull)
                            ||
                            Reader[1].GetType() == typeof(DBNull)
                        )
                        {
                            continue;
                        }
                        Entry = GetUnitFromCache(Reader.GetString(0));
                        if (Entry == null)
                        {
                            continue;
                        }
                        AlternateName = Reader.GetString(1);
                        if (Reader[4].GetType() != typeof(DBNull) && Reader.GetString(4) == "T")
                        {
                            Entry.Plurals.Add(AlternateName);
                            UnitListPlural.Add(AlternateName);
                        }
                        //Exclusively not plural
                        else if (Reader[2].GetType() != typeof(DBNull) && Reader.GetString(2) == "T")
                        {
                            Entry.Symbols.Add(AlternateName);
                            UnitListNoPlural.Add(AlternateName);
                        }
                        else if (Reader[3].GetType() != typeof(DBNull) && Reader.GetString(3) == "T")
                        {
                            Entry.Abbreviations.Add(AlternateName);
                            UnitListNoPlural.Add(AlternateName);
                        }
                        else if (Reader[5].GetType() != typeof(DBNull) && Reader.GetString(5) == "T")
                        {
                            Entry.Variants.Add(AlternateName);
                            UnitListNoPlural.Add(AlternateName);
                        }
                        else
                        {
                            Entry.AlternateNames.Add(AlternateName);
                            UnitListNoPlural.Add(AlternateName);
                        }
                        if (!UnitList.ContainsKey(AlternateName))
                        {
                            UnitList.Add(AlternateName, Entry.Unit);
                        }
                    }
                }
            }
            UnitCategories.Sort();
            UnitListNoPlural.Sort();
        }
    }
    /// <summary>
    /// Get all binary prefixes from the "Binary Prefixes" table in the database file and store it in
    /// <see cref="Database.BinaryPrefixes"/>.
    /// </summary>
    public static void GetBinaryPrefixes(string DatabasePath)
    {
        //Redundancy
        if (!Database.BinaryPrefixExist)
        {
            Database.BinaryPrefixes = RedundantData.BinaryPrefixes;
            return;
        }
        using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadOnly"))
        {
            Connection.Open();
            SqliteCommand Command = Connection.CreateCommand();
            Command.CommandText = "SELECT * FROM [Binary Prefixes]";
            using (SqliteDataReader Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    //Required values: Prefix, Factor, Standard Form
                    if (
                        Reader[0].GetType() == typeof(DBNull)
                        ||
                        Reader[2].GetType() == typeof(DBNull)
                        ||
                        Reader[3].GetType() == typeof(DBNull)
                    )
                    {
                        continue;
                    }
                    Database.BinaryPrefixes.Add(Reader.GetString(0), new PrefixEntry(Reader.GetString(0), Reader.GetString(1), Reader.GetString(2), Reader.GetString(3), Reader.GetString(4)));
                }
            }
        }
    }
    /// <summary>
    /// Get all case sensitive units from the "Case Sensitive" table in the database file and store it in
    /// <see cref="Database.CasingList"/>.
    /// </summary>
    public static void GetCaseSensitive(string DatabasePath)
    {
        //Redundancy
        if (!Database.CaseExist)
        {
            Database.CasingList = RedundantData.CasingList;
            return;
        }
        using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadOnly"))
        {
            Connection.Open();
            SqliteCommand Command = Connection.CreateCommand();
            Command.CommandText = "SELECT * FROM [Case Sensitive]";
            using (SqliteDataReader Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    //Required values: Unit
                    if (
                        Reader[0].GetType() == typeof(DBNull)
                    )
                    {
                        continue;
                    }
                    Database.CasingList.Add(Reader.GetString(0));
                }
            }
        }
    }
    /// <summary>
    /// Get all currency aliases from the "Currency Alias" table in the database file and store it in
    /// <see cref="Database.CurrencyAlias"/>.
    /// </summary>
    public static void GetCurrencyAliases(string DatabasePath)
    {
        //Redundancy
        if (!Database.CurrencyExist)
        {
            Database.CurrencyAlias = RedundantData.CurrencyAlias;
            return;
        }
        using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadOnly"))
        {
            Connection.Open();
            SqliteCommand Command = Connection.CreateCommand();
            Command.CommandText = "SELECT * FROM [Currency Alias]";
            using (SqliteDataReader Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    //Required values: API Country, Unit
                    if (
                        Reader[0].GetType() == typeof(DBNull)
                        ||
                        Reader[1].GetType() == typeof(DBNull)
                    )
                    {
                        continue;
                    }
                    Database.CurrencyAlias.Add(Reader.GetString(0), Reader.GetString(1));
                }
            }
        }
    }
    /// <summary>
    /// Get all units from the "Special Units" table in the database file and store it in
    /// <see cref="Database.SpecialUnits"/>.
    /// </summary>
    public static void GetSpecialUnits(string DatabasePath)
    {
        //Redundancy
        if (!Database.SpecialExist)
        {
            Database.SpecialUnits = RedundantData.SpecialUnits;
            return;
        }
        using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadOnly"))
        {
            Connection.Open();
            SqliteCommand Command = Connection.CreateCommand();
            Command.CommandText = "SELECT * FROM [Special Units]";
            using (SqliteDataReader Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    //Required values: Unit
                    if (
                        Reader[0].GetType() == typeof(DBNull)
                    )
                    {
                        continue;
                    }
                    Database.SpecialUnits.Add(Reader.GetString(0));
                }
            }
        }
    }
    /// <summary>
    /// Get all SI equivalents from the "SI Equivalents" table in the database file and store it in
    /// <see cref="Database.SIEquivalents"/>.
    /// </summary>
    public static void GetSIEquivalents(string DatabasePath)
    {
        //Redundancy
        if (!Database.SIExists)
        {
            Database.SIEquivalents = RedundantData.SIEquivalents;
            return;
        }
        using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadOnly"))
        {
            Connection.Open();
            SqliteCommand Command = Connection.CreateCommand();
            Command.CommandText = "SELECT * FROM [SI Equivalents]";
            using (SqliteDataReader Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    //Required values: Type, Unit
                    if (
                        Reader[0].GetType() == typeof(DBNull)
                        ||
                        Reader[1].GetType() == typeof(DBNull)
                    )
                    {
                        continue;
                    }
                    Database.SIEquivalents.Add(Reader.GetString(0), Reader.GetString(1));
                }
            }
        }
    }
    /// <summary>
    /// Get all SI prefixes from the "SI Prefixes" table in the database file and store it in
    /// <see cref="Database.SIPrefixes"/>.
    /// </summary>
    public static void GetSIPrefixes(string DatabasePath)
    {
        //Redundancy
        if (!Database.SIPrefixExists)
        {
            Database.SIPrefixes = RedundantData.SIPrefixes;
            return;
        }
        using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadOnly"))
        {
            Connection.Open();
            SqliteCommand Command = Connection.CreateCommand();
            Command.CommandText = "SELECT * FROM [SI Prefixes]";
            using (SqliteDataReader Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    //Required values: Prefix, Factor, Standard Form
                    if (
                        Reader[0].GetType() == typeof(DBNull)
                        ||
                        Reader[2].GetType() == typeof(DBNull)
                        ||
                        Reader[3].GetType() == typeof(DBNull)
                    )
                    {
                        continue;
                    }
                    Database.SIPrefixes.Add(Reader.GetString(0), new PrefixEntry(Reader.GetString(0), Reader.GetString(1), Reader.GetString(2), Reader.GetString(3), Reader.GetString(4)));
                }
            }
        }
    }
    /// <summary>
    /// Get all temperature formulas from the "Temperature Formulas" table in the database file and store it in
    /// <see cref="Database.TemperatureFormulas"/>.
    /// </summary>
    public static void GetTemperatureFormulas(string DatabasePath)
    {
        if (!Database.TemperatureExists)
        {
            Database.TemperatureFormulas = RedundantData.TemperatureFormulas;
            return;
        }
        using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadOnly"))
        {
            Connection.Open();
            SqliteCommand Command = Connection.CreateCommand();
            Command.CommandText = "SELECT * FROM [Temperature Formulas]";
            using (SqliteDataReader Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    //Required values: From, To, Equation
                    if (
                        Reader[0].GetType() == typeof(DBNull)
                        ||
                        Reader[1].GetType() == typeof(DBNull)
                        ||
                        Reader[2].GetType() == typeof(DBNull)
                    )
                    {
                        continue;
                    }
                    if (!Database.TemperatureFormulas.ContainsKey(Reader.GetString(0)))
                    {
                        Database.TemperatureFormulas[Reader.GetString(0)] = new Dictionary<string, Tuple<string, string>>();
                    }
                    Database.TemperatureFormulas[Reader.GetString(0)][Reader.GetString(1)] = new Tuple<string, string>(Reader.GetString(2), Reader.GetString(3));
                }
            }
        }
    }
    /// <summary>
    /// Check for internet connection.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if there is connection, <see langword="false"/> otherwise.
    /// </returns>
    private static bool CheckInternet()
    {
        try
        {
            HttpClient Client = new HttpClient();
            HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Head, "https://www.nist.gov");
            using (HttpResponseMessage WebResponse = Client.Send(Request))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// Get the current time from an NTP server in UTC.
    /// </summary>
    /// <returns>
    /// <see cref="DateTime"/> of the current time in UTC.
    /// </returns>
    public static DateTime GetDateTime(string NTPServer)
    {
        byte[] NTPData = new byte[48];
        NTPData[0] = 0x1B;
        IPAddress[] Addresses = Dns.GetHostEntry(NTPServer).AddressList;
        IPEndPoint IPEndpoint = new IPEndPoint(Addresses[0], 123);
        using (Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            Socket.Connect(IPEndpoint);
            Socket.ReceiveTimeout = 2000;
            Socket.Send(NTPData);
            Socket.Receive(NTPData);
        }
        const byte TransmitTimeOffset = 40;
        ulong Seconds = 0;
        ulong SecondFraction = 0;
        for (int i = 0; i <= 3; i++)
        {
            Seconds = 256 * Seconds + NTPData[TransmitTimeOffset + i];
            SecondFraction = 256 * SecondFraction + NTPData[TransmitTimeOffset + i + 4];
        }
        ulong Milliseconds = (Seconds * 1000) + ((SecondFraction * 1000) / 0x100000000L);
        return new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)Milliseconds);
    }
    /// <summary>
    /// Updates the year and quarter for <see cref="Database.CurrencyAPI"/>.
    /// </summary>
    public static void UpdateCurrencyAPI()
    {
        DateTime Today = GetDateTime("time.nist.gov");
        Database.APIYear = Today.Year;
        int CurrentMonth = Today.Month;
        if (CurrentMonth <= 3) //Before Q1
        {
            Database.APIQuarter = 4;
            Database.APIYear -= 1;
        }
        else if (CurrentMonth >= 4 && CurrentMonth <= 6) //Before Q2
        {
            Database.APIQuarter = 1;
        }
        else if (CurrentMonth >= 7 && CurrentMonth <= 9) //Before Q3
        {
            Database.APIQuarter = 2;
        }
        else if (CurrentMonth >= 10) //Before Q4
        {
            Database.APIQuarter = 3;
        }
    }
    /// <summary>
    /// Gets currency data from the <see cref="Database.CurrencyAPI"/>.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if data was successfully received, <see langword="false"/> otherwise.
    /// </returns>
    private static bool GetCurrencyData(string URL, out Root Results)
    {
        try
        {
            HttpClient Client = new HttpClient();
            HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Get, URL);
            using (HttpResponseMessage WebResponse = Client.Send(Request))
            {
                using (StreamReader Reader = new StreamReader(WebResponse.Content.ReadAsStream()))
                {
                    Results = JsonSerializer.Deserialize<Root>(Reader.ReadToEnd());
                    if (Results.data.Length == 0)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
        catch
        {
            Results = null;
            return false;
        }
    }
    /// <summary>
    /// Updates the database file and <see cref="Database.UnitCache"/> with new exchange rates.
    /// </summary>
    /// <returns>
    /// 1 if the database was successfully updated, 0 if no internet connection, -1 for all other errors.
    /// </returns>
    public static int UpdateCurrencies(string DatabasePath)
    {
        if (!CheckInternet())
        {
            return 0;
        }
        try
        {
            UpdateCurrencyAPI();
        }
        catch
        {
            return -1;
        }
        Datum[] Page1;
        Datum[] Page2;
        Datum[] Data;
        List<Datum> Currencies = new List<Datum>();
        //Get first page
        Root Response;
        bool Status = GetCurrencyData(Database.CurrencyAPI, out Response);
        if (!Status)
        {
            int Low = 8005; //2001 Q1 in number of quarters
            int Middle;
            int High = Database.APIYear * 4 + Database.APIQuarter;
            while (Low < High)
            {
                Middle = (Low + High) / 2;
                (Database.APIYear, Database.APIQuarter) = Math.DivRem(Middle, 4);
                if (Database.APIQuarter == 0)
                {
                    Database.APIQuarter = 4;
                    Database.APIYear -= 1;
                }
                if (Status = GetCurrencyData(Database.CurrencyAPI, out Response))
                {
                    if (Low == Middle)
                    {
                        break;
                    }
                    Low = Middle;
                }
                else
                {
                    High = Middle;
                }
            }
            if (!Status)
            {
                return -1;
            }
        }
        Page1 = Response.data;
        //Get next page
        Status = Response.links.next != null ? GetCurrencyData(Database.CurrencyAPI + Response.links.next, out Response) : false;
        //Aggregate data
        if (Status)
        {
            Page2 = Response.data;
            Data = new Datum[Page1.Length + Page2.Length];
            Page1.CopyTo(Data, 0);
            Page2.CopyTo(Data, Page1.Length);
        }
        else
        {
            Data = Page1;
        }
        //Filter results
        int LastItem;
        for (int i = 0; i < Data.Length; i++)
        {
            //Currencies no longer used
            if (
                Data[i].currency.Equals("Chavito", StringComparison.OrdinalIgnoreCase)
                ||
                Data[i].currency.Equals("Fuerte (OLD)", StringComparison.OrdinalIgnoreCase)
                ||
                Data[i].currency.Equals("Old Leone", StringComparison.OrdinalIgnoreCase)
            )
            {
                continue;
            }
            else
            {
                //Remove duplicate with earlier date
                if (Currencies.Count > 0)
                {
                    LastItem = Currencies.Count - 1;
                    if (
                        Currencies[LastItem].country.Equals(Data[i].country, StringComparison.OrdinalIgnoreCase)
                        &&
                        Currencies[LastItem].currency.Equals(Data[i].currency, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        if (Currencies[LastItem].effective_date.CompareTo(Data[i].effective_date) == -1)
                        {
                            Currencies.RemoveAt(LastItem);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                Currencies.Add(Data[i]);
            }
        }
        using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadWrite"))
        {
            string Currency;
            string USDEquivalent;
            HashSet<string> UpdatedCurrencies = new HashSet<string>();
            Connection.Open();
            SqliteCommand Command = Connection.CreateCommand();
            using (SqliteTransaction Transaction = Connection.BeginTransaction())
            {
                Command.Transaction = Transaction;
                try
                {
                    foreach (Datum x in Currencies)
                    {
                        //Get name from database
                        if (!Database.CurrencyAlias.ContainsKey(x.country))
                        {
                            continue;
                        }
                        Currency = Database.CurrencyAlias[x.country];
                        //Avoid duplicate updates for countries that use same currency
                        if (UpdatedCurrencies.Contains(Currency))
                        {
                            continue;
                        }
                        UpdatedCurrencies.Add(Currency);
                        //Write to database
                        USDEquivalent = decimal.Round(1 / decimal.Parse(x.exchange_rate), 20).ToString();
                        Command.CommandText = $"UPDATE Info SET [SI Equivalent] = '{USDEquivalent}' WHERE Unit = '{Currency}'";
                        Command.ExecuteNonQuery();
                        //Update cache
                        Database.UnitCache["Currency"][Currency].SI = BigDecimal.Parse(USDEquivalent);
                    }
                    Transaction.Commit();
                }
                catch (Exception)
                {
                    Transaction.Rollback();
                    return -1;
                }
            }
        }
        return 1;
    }
}
/// <summary>
/// Unitversal search functions.
/// </summary>
public static class Search
{
    /// <summary>
    /// <see cref="char"/>[] of the digits 0-9.
    /// </summary>
    public readonly static char[] Digits = "0123456789".ToCharArray();
    /// <summary>
    /// Characters reserved for internal functions.
    /// </summary>
    public static HashSet<string> ReservedCharacters = new HashSet<string>
    {
        { "-" },
        { "−" },
        { "E" },
        { "e" },
    };
    /// <summary>
    /// Get the index of the unit name in a query string.
    /// </summary>
    /// <returns>
    /// An <see cref="int"/> of the unit name index or -1 if there is none.
    /// </returns>
    public static int UnitNameIndex(string Text, char[] SeparatorSymbols)
    {
        char Character;
        bool Digit;
        bool Separator;
        bool Minus;
        int Conditional = -1;
        for (int i = 0; i < Text.Length; i++)
        {
            Character = Text[i];
            Digit = Digits.Contains(Character);
            Separator = SeparatorSymbols.Contains(Character);
            Minus = Character == '-' || Character == '−' ? true : false;
            if (!Digit && !Separator && !Minus)
            {
                if (Conditional == -1)
                {
                    if (char.ToUpper(Character) == 'E')
                    {
                        Conditional = i;
                        continue;
                    }
                    return i;
                }
                return Conditional;
            }
            else if (Digit)
            {
                Conditional = -1;
            }
        }
        return -1;
    }
    /// <summary>
    /// Calculate longest substring between two strings.
    /// </summary>
    /// <returns>
    /// The length of the longest substring as an <see cref="int"/>.
    /// </returns>
    public static int LongestSubstring(string a, string b)
    {
        int Longest = 0;
        int[,] Matrix = new int[a.Length, b.Length];
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = 0; j < b.Length; j++)
            {
                if (a[i] == b[j])
                {
                    if (i == 0 || j == 0)
                    {
                        Matrix[i, j] = 1;
                    }
                    else
                    {
                        Matrix[i, j] = Matrix[i - 1, j - 1] + 1;
                    }
                    if (Matrix[i, j] > Longest)
                    {
                        Longest = Matrix[i, j];
                    }
                }
                else
                {
                    Matrix[i, j] = 0;
                }
            }
        }
        return Longest;
    }
    /// <summary>
    /// Calculate longest subsequence between two strings.
    /// </summary>
    /// <returns>
    /// The length of the longest subsequences as an <see cref="int"/>.
    /// </returns>
    public static int LongestSubsequence(string a, string b)
    {
        int[,] Matrix = new int[a.Length + 1, b.Length + 1];
        for (int i = 0; i <= a.Length; i++)
        {
            Matrix[i, 0] = 0;
        }
        for (int i = 0; i <= b.Length; i++)
        {
            Matrix[0, i] = 0;
        }
        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                if (a[i - 1] == b[j - 1])
                {
                    Matrix[i, j] = Matrix[i - 1, j - 1] + 1;
                }
                else
                {
                    Matrix[i, j] = Math.Max(Matrix[i - 1, j], Matrix[i, j - 1]);
                }
            }
        }
        return Matrix[a.Length, b.Length];
    }
    /// <summary>
    /// Calculate the similarity between two unit names <paramref name="Query"/> which is a user input
    /// and <paramref name="Reference"/> which is a name from database.
    /// </summary>
    /// <returns>
    /// <see cref="Tuple{float, float, float}"/> of the similarity in the format
    /// (Longest Subsequence, Longest Subsequence Case Percentage, Longest Substring).
    /// </returns>
    private static Tuple<float, float, float> Similarity(string Query, string Reference)
    {
        string QueryUpper = Query.ToUpper();
        string ReferenceUpper = Reference.ToUpper();
        int MaxLength = Math.Max(Query.Length, Reference.Length);
        float SubstringNoCase = LongestSubstring(QueryUpper, ReferenceUpper);
        float SubsequenceNoCase = LongestSubsequence(QueryUpper, ReferenceUpper);
        if (Database.CasingList.Contains(ReferenceUpper))
        {
            float SubsequenceCase = LongestSubsequence(Query, Reference);
            return new Tuple<float, float, float>(SubstringNoCase, SubsequenceNoCase, SubsequenceCase / MaxLength);
        }
        return new Tuple<float, float, float>(SubstringNoCase, SubsequenceNoCase, SubsequenceNoCase / MaxLength);
    }
    /// <summary>
    /// Compares two best matches.
    /// </summary>
    /// <returns>
    /// -1 if <paramref name="x"/> precedes <paramref name="y"/>, 0 if <paramref name="x"/> and <paramref name="y"/>
    /// are in the same position and 1 if <paramref name="x"/> comes after <paramref name="y"/>.
    /// </returns>
    private static int CompareBest(Tuple<string, float, float, float> x, Tuple<string, float, float, float> y)
    {
        if (x.Item2 > y.Item2)
        {
            return -1;
        }
        if (x.Item2 == y.Item2)
        {
            if (x.Item3 > y.Item3)
            {
                return -1;
            }
            if (x.Item3 == y.Item3)
            {
                if (x.Item4 > y.Item4)
                {
                    return -1;
                }
                if (x.Item4 == y.Item4)
                {
                    int Length = x.Item1.Length.CompareTo(y.Item1.Length);
                    if (Length == 0)
                    {
                        return x.Item1.CompareTo(y.Item1); //Alphabetize if length is same
                    }
                    return Length;
                }
            }
        }
        return 1;
    }
    /// <summary>
    /// Get best matches of a unit name from database. If <paramref name="Type"/> is specified, matches will
    /// only be performed for units of that type as stored in <see cref="Database.UnitCache"/>.
    /// </summary>
    /// <returns>
    /// A <see langword="List"/> of the best matches of the given unit name sorted in order from best to
    /// worst.
    /// </returns>
    public static List<string> BestMatches(string Unit, string Type = "")
    {
        List<Tuple<string, float, float, float>> Matches = new List<Tuple<string, float, float, float>>();
        Tuple<float, float, float> Match;
        if (Type != "")
        {
            List<string> NameList;
            foreach ((string Name, UnitEntry x) in Database.UnitCache[Type])
            {
                NameList = new List<string>(
                    x.Symbols
                    .Concat(x.Abbreviations)
                    .Concat(x.AlternateNames)
                    .Concat(x.Plurals)
                    .Concat(x.Variants)
                ) { x.Unit };
                foreach (string Unit2 in NameList)
                {
                    Match = Similarity(Unit, Unit2);
                    if (Match.Item1 > 0)
                    {
                        Matches.Add(new Tuple<string, float, float, float>(Unit2, Match.Item1, Match.Item2, Match.Item3));
                    }
                }
            }
        }
        else
        {
            foreach ((string Secondary, string Primary) in Database.UnitList)
            {
                Match = Similarity(Unit, Secondary);
                if (Match.Item1 > 0)
                {
                    Matches.Add(new Tuple<string, float, float, float>(Secondary, Match.Item1, Match.Item2, Match.Item3));
                }
            }
        }
        Matches.Sort(CompareBest);
        if (Matches.Count > 20)
        {
            Matches.RemoveRange(20, Matches.Count - 20);
        }
        return Matches.ConvertAll(x => x.Item1);
    }
    /// <summary>
    /// Get best match for a given <paramref name="Unit"/> from a given list of strings.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> that represents the best match.
    /// </returns>
    public static string BestMatchFromList(string Unit, List<string> List)
    {
        List<Tuple<string, float, float, float>> Matches = new List<Tuple<string, float, float, float>>();
        Tuple<float, float, float> Match;
        foreach (string Unit2 in List)
        {
            Match = Similarity(Unit, Unit2);
            Matches.Add(new Tuple<string, float, float, float>(Unit2, Match.Item1, Match.Item2, Match.Item3));
        }
        Matches.Sort(CompareBest);
        return Matches[0].Item1;
    }
}
/// <summary>
/// Unitversal calculation functions.
/// </summary>
public static class Calculate
{
    /// <summary>
    /// The magnitude of the conversion.
    /// </summary>
    public static BigDecimal Magnitude;
    /// <summary>
    /// <see cref="UnitEntry"/> for unit 1, the default unit.
    /// </summary>
    public static UnitEntry Unit1;
    /// <summary>
    /// <see cref="UnitEntry"/> for unit 2, the unit to convert to.
    /// </summary>
    public static UnitEntry Unit2;
    /// <summary>
    /// List of <see cref="string"/> units that best matches <see cref="Calculate.Unit1"/>.
    /// </summary>
    public static List<string> Unit1BestMatches = new List<string>();
    /// <summary>
    /// List of <see cref="string"/> units that best matches <see cref="Calculate.Unit2"/>.
    /// </summary>
    public static List<string> Unit2BestMatches = new List<string>();
    /// <summary>
    /// The type of conversion to perform.
    /// </summary>
    public static string QueryType;
    /// <summary>
    /// The number of significant figures to display for conversion results.
    /// </summary>
    public static int SignificantFigures = 10;
    /// <summary>
    /// The upper limit for using scientific notation; exclusive.
    /// </summary>
    public static BigDecimal LargeMagnitude = new BigDecimal(1, 10);
    /// <summary>
    /// The lower limit for using scientific notation; exclusive.
    /// </summary>
    public static BigDecimal SmallMagnitude = new BigDecimal(1, -10);
    /// <summary>
    /// Stores the results of the conversion as a tuple of strings in the format (magnitude, unit).
    /// </summary>
    public static List<Tuple<string,string>> Results = new List<Tuple<string, string>>();
    /// <summary>
    /// Converts between special temperature units that require specific equations with a given magnitude.
    /// </summary>
    /// <returns>
    /// A <see cref="BigDecimal"/> of the conversion.
    /// </returns>
    private static BigDecimal Temperature(BigDecimal Magnitude, string Unit1, string Unit2)
    {
        //Equations from https://web.archive.org/web/20220816165639/https://en.wikipedia.org/wiki/Conversion_of_scales_of_temperature
        if (Unit1 == "Kelvin")
        {
            if (Unit2 == "Celsius")
            {
                return Magnitude - 273.15;
            }
            else if (Unit2 == "Fahrenheit")
            {
                return Magnitude * 9 / 5 - 459.67;
            }
            else if (Unit2 == "Rankine")
            {
                return Magnitude * 9 / 5;
            }
            else if (Unit2 == "Delisle")
            {
                return (373.15 - Magnitude) * 3 / 2;
            }
            else if (Unit2 == "Newton (Temperature)")
            {
                return (Magnitude - 273.15) * 33 / 100;
            }
            else if (Unit2 == "Réaumur")
            {
                return (Magnitude - 273.15) * 4 / 5;
            }
            else if (Unit2 == "Rømer")
            {
                return (Magnitude - 273.15) * 21 / 40 + 7.5;
            }
        }
        else if (Unit1 == "Celsius")
        {
            if (Unit2 == "Fahrenheit")
            {
                return Magnitude * 9 / 5 + 32;
            }
            else if (Unit2 == "Kelvin")
            {
                return Magnitude + 273.15;
            }
            else if (Unit2 == "Rankine")
            {
                return (Magnitude + 273.15) * 9 / 5;
            }
            else if (Unit2 == "Delisle")
            {
                return (100 - Magnitude) * 3 / 2;
            }
            else if (Unit2 == "Newton (Temperature)")
            {
                return Magnitude * 33 / 100;
            }
            else if (Unit2 == "Réaumur")
            {
                return Magnitude * 4 / 5;
            }
            else if (Unit2 == "Rømer")
            {
                return Magnitude * 21 / 40 + 7.5;
            }
        }
        else if (Unit1 == "Fahrenheit")
        {
            if (Unit2 == "Celsius")
            {
                return (Magnitude - 32) * 5 / 9;
            }
            else if (Unit2 == "Kelvin")
            {
                return (Magnitude + 459.67) * 5 / 9;
            }
            else if (Unit2 == "Rankine")
            {
                return Magnitude + 459.67;
            }
            else if (Unit2 == "Delisle")
            {
                return (212 - Magnitude) * 5 / 6;
            }
            else if (Unit2 == "Newton (Temperature)")
            {
                return (Magnitude - 32) * 11 / 60;
            }
            else if (Unit2 == "Réaumur")
            {
                return (Magnitude - 32) * 4 / 9;
            }
            else if (Unit2 == "Rømer")
            {
                return (Magnitude - 32) * 7 / 24 + 7.5;
            }
        }
        else if (Unit1 == "Rankine")
        {
            if (Unit2 == "Celsius")
            {
                return (Magnitude - 491.67) * 5 / 9;
            }
            else if (Unit2 == "Fahrenheit")
            {
                return Magnitude - 459.67;
            }
            else if (Unit2 == "Kelvin")
            {
                return Magnitude * 5 / 9;
            }
            else if (Unit2 == "Delisle")
            {
                return (671.67 - Magnitude) * 5 / 6;
            }
            else if (Unit2 == "Newton (Temperature)")
            {
                return (Magnitude - 491.67) * 11 / 60;
            }
            else if (Unit2 == "Réaumur")
            {
                return (Magnitude - 491.67) * 4 / 9;
            }
            else if (Unit2 == "Rømer")
            {
                return (Magnitude - 491.67) * 7 / 24 + 7.5;
            }
        }
        else if (Unit1 == "Rømer")
        {
            if (Unit2 == "Celsius")
            {
                return (Magnitude - 7.5) * 40 / 21;
            }
            else if (Unit2 == "Fahrenheit")
            {
                return (Magnitude - 7.5) * 24 / 7 + 32;
            }
            else if (Unit2 == "Kelvin")
            {
                return (Magnitude - 7.5) * 40 / 21 + 273.15;
            }
            else if (Unit2 == "Rankine")
            {
                return (Magnitude - 7.5) * 24 / 7 + 491.67;
            }
            else if (Unit2 == "Delisle")
            {
                return (60 - Magnitude) * 20 / 7;
            }
            else if (Unit2 == "Newton (Temperature)")
            {
                return (Magnitude - 7.5) * 22 / 35;
            }
            else if (Unit2 == "Réaumur")
            {
                return (Magnitude - 7.5) * 32 / 21;
            }
        }
        else if (Unit1 == "Newton (Temperature)")
        {
            if (Unit2 == "Celsius")
            {
                return Magnitude * 100 / 33;
            }
            else if (Unit2 == "Fahrenheit")
            {
                return Magnitude * 60 / 11 + 32;
            }
            else if (Unit2 == "Kelvin")
            {
                return Magnitude * 100 / 33 + 273.15;
            }
            else if (Unit2 == "Rankine")
            {
                return Magnitude * 60 / 11 + 491.67;
            }
            else if (Unit2 == "Delisle")
            {
                return (33 - Magnitude) * 50 / 11;
            }
            else if (Unit2 == "Réaumur")
            {
                return Magnitude * 80 / 33;
            }
            else if (Unit2 == "Rømer")
            {
                return Magnitude * 35 / 22 + 7.5;
            }
        }
        else if (Unit1 == "Delisle")
        {
            if (Unit2 == "Celsius")
            {
                return 100 - Magnitude * 2 / 3;
            }
            else if (Unit2 == "Fahrenheit")
            {
                return 212 - Magnitude * 6 / 5;
            }
            else if (Unit2 == "Kelvin")
            {
                return 373.15 - Magnitude * 2 / 3;
            }
            else if (Unit2 == "Rankine")
            {
                return 671.67 - Magnitude * 6 / 5;
            }
            else if (Unit2 == "Newton (Temperature)")
            {
                return 33 - Magnitude * 11 / 50;
            }
            else if (Unit2 == "Réaumur")
            {
                return 80 - Magnitude * 8 / 15;
            }
            else if (Unit2 == "Rømer")
            {
                return 60 - Magnitude * 7 / 20;
            }
        }
        else if (Unit1 == "Réaumur")
        {
            if (Unit2 == "Celsius")
            {
                return Magnitude * 5 / 4;
            }
            else if (Unit2 == "Fahrenheit")
            {
                return Magnitude * 9 / 4 + 32;
            }
            else if (Unit2 == "Kelvin")
            {
                return Magnitude * 5 / 4 + 273.15;
            }
            else if (Unit2 == "Rankine")
            {
                return Magnitude * 9 / 4 + 491.67;
            }
            else if (Unit2 == "Delisle")
            {
                return (80 - Magnitude) * 15 / 8;
            }
            else if (Unit2 == "Newton (Temperature)")
            {
                return Magnitude * 33 / 80;
            }
            else if (Unit2 == "Rømer")
            {
                return Magnitude * 21 / 32 + 7.5;
            }
        }
        return Magnitude;
    }
    /// <summary>
    /// Converts <paramref name="Unit1"/> to <paramref name="Unit2"/> using given a <paramref name="Magnitude"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="Tuple{string, string}"/> of the conversion.
    /// </returns>
    private static Tuple<string, string> Convert(BigDecimal Magnitude, UnitEntry Unit1, UnitEntry Unit2)
    {
        Tuple<string, string> Result(string Value, bool Plural = false)
        {
            List<string> NameList;
            if (Plural)
            {
                NameList = Unit2.Plurals;
            }
            else
            {
                NameList = new List<string>(Unit2.AlternateNames.Concat(Unit2.Variants))
                {
                    Unit2.Unit
                };
            }
            //BestMatchFromList() is used to show results similar to user query e.g. meter vs metre
            if (Calculate.QueryType == "CONVERT")
            {
                if (NameList.Count == 0)
                {
                    return new Tuple<string, string>(Value, Unit2BestMatches[0]);
                }
                return new Tuple<string, string>(Value, Search.BestMatchFromList(Unit2BestMatches[0], NameList));
            }
            if (NameList.Count == 0)
            {
                return new Tuple<string, string>(Value, Unit1BestMatches[0]);
            }
            return new Tuple<string, string>(Value, Search.BestMatchFromList(Unit1BestMatches[0], NameList));
        }
        BigDecimal Conversion;
        BigDecimal Unit1SI;
        BigDecimal Unit2SI;
        if (Database.InexactValues.ContainsKey(Unit1.Unit))
        {
            Unit1SI = Magnitude * Database.InexactValues[Unit1.Unit];
        }
        else
        {
            Unit1SI = Magnitude * Unit1.SI;
        }
        if (Database.InexactValues.ContainsKey(Unit2.Unit))
        {
            Unit2SI = Database.InexactValues[Unit2.Unit];
        }
        else
        {
            Unit2SI = Unit2.SI;
        }
        if (Unit2SI == 0) //Division by 0
        {
            return Result("∞", true);
        }
        if (
            Unit1.Type == "Temperature"
            &&
            Database.SpecialUnits.Contains(Unit1.Unit)
            &&
            Database.SpecialUnits.Contains(Unit2.Unit)
        )
        {
            Conversion = Calculate.Temperature(Magnitude, Unit1.Unit, Unit2.Unit).Round(SignificantFigures);
        }
        else if (Unit1.Type == "Temperature" && Database.SpecialUnits.Contains(Unit1.Unit))
        {
            Conversion = (Calculate.Temperature(Magnitude, Unit1.Unit, "Kelvin") / Unit2SI).Round(SignificantFigures);
        }
        else if (Unit1.Type == "Temperature" && Database.SpecialUnits.Contains(Unit2.Unit))
        {
            Conversion = Calculate.Temperature(Unit1SI, "Kelvin", Unit2.Unit).Round(SignificantFigures);
        }
        else if (
            (Unit1.Type == "Speed,Velocity" || Unit1.Type == "Fuel Efficiency")
            &&
            (
                (Database.SpecialUnits.Contains(Unit1.Unit) && !Database.SpecialUnits.Contains(Unit2.Unit))
                ||
                (Database.SpecialUnits.Contains(Unit2.Unit) && !Database.SpecialUnits.Contains(Unit1.Unit))
            )
        )
        {
            if (Unit1SI == 0) //Division by 0
            {
                return Result("∞", true);
            }
            Conversion = (1 / (Unit1SI * Unit2SI)).Round(SignificantFigures);
        }
        else
        {
            Conversion = (Unit1SI / Unit2SI).Round(SignificantFigures);
        }
        if (Conversion != 1 && Unit2.Plurals.Count > 0) //Use plural forms if any
        {
            return Result(Conversion.ToFormattedString(SmallMagnitude, LargeMagnitude), true);
        }
        else
        {
            return Result(Conversion.ToFormattedString(SmallMagnitude, LargeMagnitude));
        }
    }
    /// <summary>
    /// Calculate conversions.
    /// </summary>
    public static void Conversions()
    {
        Calculate.Results.Clear();
        //Convert mode
        if (Calculate.QueryType == "CONVERT")
        {
            //Track units added
            HashSet<string> UnitsAdded = new HashSet<string>();
            for (int i = 0; i < Calculate.Unit2BestMatches.Count; i++)
            {
                //Prevent adding duplicates due to different alternate names
                if (UnitsAdded.Contains(Database.UnitList[Calculate.Unit2BestMatches[i]]))
                {
                    continue;
                }
                UnitEntry Item = Database.UnitCache[Unit1.Type][Database.UnitList[Calculate.Unit2BestMatches[i]]];
                //Only show conversion to same unit on user query
                if (Item.Unit == Calculate.Unit1.Unit && Item.Unit != Calculate.Unit2.Unit)
                {
                    continue;
                }
                Calculate.Results.Add(Convert(Magnitude, Calculate.Unit1, Item));
                //Track unit added
                UnitsAdded.Add(Item.Unit);
            }
        }
        //Convert to all
        else
        {
            foreach ((string Name, UnitEntry Unit) in Database.UnitCache[Unit1.Type])
            {
                if (Calculate.Unit1.Unit == Name)
                {
                    continue;
                }
                Calculate.Results.Add(Convert(Magnitude, Calculate.Unit1, Unit));
            }
        }
    }
    /// <summary>
    /// Compares two results from <see cref="Calculate.Results"/>.
    /// </summary>
    /// <returns>
    /// -1 if <paramref name="x"/> precedes <paramref name="y"/>, 0 if <paramref name="x"/> and <paramref name="y"/>
    /// are in the same position and 1 if <paramref name="x"/> comes after <paramref name="y"/>.
    /// </returns>
    private static int ResultsCompare(Tuple<string,string> x, Tuple<string, string> y, string SortBy, string SortOrder)
    {
        //Sort by unit
        if (SortBy == "UNIT")
        {
            int Order = string.Compare(x.Item2, y.Item2);
            if (SortOrder == "ASCENDING")
            {
                return Order;
            }
            else
            {
                return Order * -1;
            }
        }
        //Sort by magnitude
        else if (SortBy == "MAGNITUDE")
        {
            int Order = BigDecimal.ReverseFormat(
                            x.Item1,
                            BigDecimal.DecimalSeparator,
                            BigDecimal.IntegerGroupSeparator,
                            BigDecimal.DecimalGroupSeparator
                        ).CompareTo(BigDecimal.ReverseFormat(
                            y.Item1,
                            BigDecimal.DecimalSeparator,
                            BigDecimal.IntegerGroupSeparator,
                            BigDecimal.DecimalGroupSeparator
                        ));
            if (SortOrder == "ASCENDING")
            {
                return Order;
            }
            else
            {
                return Order * -1;
            }
        }
        return 0;
    }
    /// <summary>
    /// Sort <see cref="Calculate.Results"/> according to specified sort order.
    /// </summary>
    public static void SortResults(string SortBy, string SortOrder)
    {
        Calculate.Results.Sort((x, y) => ResultsCompare(x, y, SortBy, SortOrder));
    }
}