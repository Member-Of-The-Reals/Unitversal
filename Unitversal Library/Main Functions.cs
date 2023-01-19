namespace UnitversalLibrary;

/// <summary>
/// Stores information from the unit's database entry.
/// </summary>
public class Entry
{
    public string Unit;
    public string Type;
    public BigDecimal SI;
    public string Description;
    public List<string> AlternateNames = new List<string>();
    public List<string> Symbols = new List<string>();
    public List<string> Abbreviations = new List<string>();
    public List<string> AmericanSpelling = new List<string>();
    public List<string> Plurals = new List<string>();
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
    /// <see langword="Dictionary"/> of all unit names and alternate names as keys linked to their primary unit names as values.
    /// </summary>
    public static Dictionary<string, string> UnitList = new Dictionary<string, string>();
    /// <summary>
    /// <see langword="HashSet"/> of all plural unit names.
    /// </summary>
    public static HashSet<string> UnitListPlural = new HashSet<string>();
    /// <summary>
    /// <see langword="List"/> of all unit names excluding plurals.
    /// </summary>
    public static List<string> UnitListNoPlural = new List<string>();
    /// <summary>
    /// Unit information stored as an <see cref="Entry"/> inside a <see langword="Dictionary"/> and organized by unit type inside the main <see langword="Dictionary"/>.
    /// </summary>
    public static Dictionary<string, Dictionary<string, Entry>> UnitCache = new Dictionary<string, Dictionary<string, Entry>>();
    /// <summary>
    /// <see langword="Dictionary"/> of all inexact SI equivalent values of units as a string.
    /// </summary>
    public static Dictionary<string, BigDecimal> InexactValues;
    /// <summary>
    /// <see langword="HashSet"/> of special units that require specific equations for conversion.
    /// </summary>
    public static readonly HashSet<string> SpecialUnits = new HashSet<string>
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
    /// <summary>
    /// Gets info about a unit as an <see cref="Entry"/> from cache. Faster than database file.
    /// </summary>
    /// <returns>
    /// The <see cref="Entry"/> of the unit if it exists in cache, otherwise an empty <see cref="Entry"/>.
    /// </returns>
    public static Entry GetUnitFromCache(string Unit)
    {
        foreach (KeyValuePair<string, Dictionary<string, Entry>> x in UnitCache)
        {
            if (x.Value.ContainsKey(Unit))
            {
                return x.Value[Unit];
            }
        }
        return new Entry();
    }
    /// <summary>
    /// Calculates the inexact SI equivalences for certain units which will be stored in <see cref="Database"/>.InexactValues
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
            { "Centineper", 0.01 * Neper },
            { "Centesimal Minute", Pi / 20000 },
            { "Centesimal Second", Pi / 2000000 },
            { "Cubic Metre Per Minute", PerMinute },
            { "Cubic Metre Per Hour", PerHour },
            { "Day Per Foot", 86400 * SecondPerFoot },
            { "Day Per Mile", 86400 * SecondPerMile },
            { "Decineper", 0.1 * Neper },
            { "Degree", Degree },
            { "Degree Per Second", Degree },
            { "Desktop Publishing Pica", Inch / 6 },
            { "Desktop Publishing Point", Inch / 72 },
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
            { "Metre Per Day", PerHour / 24 },
            { "Metre Per Hour", PerHour },
            { "Metre Per Minute", 60 * PerHour },
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
    /// Check for database file existence and corruption. Displays message box if there is an issue.
    /// </summary>
    /// <returns>
    /// 1 if there are no issues, 0 if database does not exist and -1 if database is corrupted.
    /// </returns>
    public static int Check(string DatabasePath)
    {
        //Check existence of database file
        if (File.Exists(DatabasePath))
        {
            using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadOnly"))
            {
                Connection.Open();
                //Check info table
                SqliteCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Info'";
                try
                {
                    using (SqliteDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    return -1;
                }
                //Check alternate names table
                Command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Alternate Names'";
                try
                {
                    using (SqliteDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    return -1;
                }
                return 1;
            }
        }
        return 0;
    }
    /// <summary>
    /// Creates the <see cref="Database"/>.UnitList, <see cref="Database"/>.UnitListPlural, <see cref="Database"/>.UnitListNoPlural and <see cref="Database"/>.UnitCache.
    /// </summary>
    public static void GetUnits(string DatabasePath)
    {
        using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadOnly"))
        {
            Connection.Open();
            //Query database
            SqliteCommand Command = Connection.CreateCommand();
            Command.CommandText = "SELECT * FROM Info LEFT JOIN [Alternate Names] ON Info.Unit = [Alternate Names].Unit ORDER BY Unit";
            using (SqliteDataReader Reader = Command.ExecuteReader())
            {
                Entry NewEntry;
                string PreviousUnit = "";
                BigDecimal SI;
                string AlternateName;
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
                    if (Reader.GetString(0) != PreviousUnit)
                    {
                        NewEntry = new Entry();
                        NewEntry.Unit = Reader.GetString(0);
                        NewEntry.Type = Reader.GetString(1);
                        NewEntry.SI = SI;
                        NewEntry.Description = Reader.GetString(3);
                        UnitList.Add(NewEntry.Unit, NewEntry.Unit);
                        UnitListNoPlural.Add(NewEntry.Unit);
                    }
                    else
                    {
                        NewEntry = GetUnitFromCache(PreviousUnit);
                    }
                    //Get alternate names
                    if (Reader[5].GetType() != typeof(DBNull))
                    {
                        AlternateName = Reader.GetString(5);
                        if (Reader[6].GetType() != typeof(DBNull) && Reader.GetString(6) == "T")
                        {
                            NewEntry.Symbols.Add(AlternateName);
                            UnitListNoPlural.Add(AlternateName);
                        }
                        else if (Reader[7].GetType() != typeof(DBNull) && Reader.GetString(7) == "T")
                        {
                            NewEntry.Abbreviations.Add(AlternateName);
                            UnitListNoPlural.Add(AlternateName);
                        }
                        else if (Reader[9].GetType() != typeof(DBNull) && Reader.GetString(9) == "T")
                        {
                            NewEntry.Plurals.Add(AlternateName);
                            UnitListPlural.Add(AlternateName);
                        }
                        //Exclusively not plural
                        else if (Reader[8].GetType() != typeof(DBNull) && Reader.GetString(8) == "T")
                        {
                            NewEntry.AmericanSpelling.Add(AlternateName);
                            UnitListNoPlural.Add(AlternateName);
                        }
                        else
                        {
                            NewEntry.AlternateNames.Add(AlternateName);
                            UnitListNoPlural.Add(AlternateName);
                        }
                        if (!UnitList.ContainsKey(AlternateName))
                        {
                            UnitList.Add(AlternateName, NewEntry.Unit);
                        }
                    }
                    //Cache unit
                    if (!UnitCache.ContainsKey(NewEntry.Type))
                    {
                        UnitCache.Add(NewEntry.Type, new Dictionary<string, Entry>());
                    }
                    if (!UnitCache[NewEntry.Type].ContainsKey(NewEntry.Unit))
                    {
                        UnitCache[NewEntry.Type].Add(NewEntry.Unit, NewEntry);
                    }
                    PreviousUnit = NewEntry.Unit;
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
            IPHostEntry Access = Dns.GetHostEntry("1.1.1.1");
            return true;
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// Get the current time from an NTP server in local time zone.
    /// </summary>
    /// <returns>
    /// <see cref="DateTime"/> of the current time in local time zone.
    /// </returns>
    public static DateTime GetDateTime(string NTPServer)
    {
        byte[] NTPData = new byte[48];
        NTPData[0] = 0x1B;
        IPAddress[] Addresses = Dns.GetHostEntry(NTPServer).AddressList;
        IPEndPoint IPEndpoint = new IPEndPoint(Addresses[0], 123);
        using (var Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
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
        DateTime Time = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)Milliseconds);
        return Time;
    }
    /// <summary>
    /// Updates the year and quarter for <see cref="Database.CurrencyAPI"/>.
    /// </summary>
    public static void UpdateCurrencyAPI(bool UseOldData = false)
    {
        DateTime Today = GetDateTime("time.nist.gov");
        Database.APIYear = int.Parse(Today.ToString("yyyy"));
        int CurrentMonth = int.Parse(Today.ToString("MM"));
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
    /// /// <returns>
    /// 1 if the database was successfully updated, 0 if no internet connection, -1 for all other errors.
    /// </returns>
    public static int UpdateCurrencies(string DatabasePath)
    {
        if (CheckInternet() == false)
        {
            return 0;
        }
        UpdateCurrencyAPI();
        //Data
        Datum[] Page1 = null;
        Datum[] Page2 = null;
        Datum[] Data = null;
        List<Datum> Currencies = new List<Datum>();
        //Get first page
        Root Response;
        bool Status = GetCurrencyData(Database.CurrencyAPI, out Response);
        if (Status == false)
        {
            return -1;
        }
        if (Response == null)
        {
            //Go back one quarter if no data
            if (Database.APIQuarter == 1)
            {
                Database.APIQuarter = 4;
                Database.APIYear -= 1;
            }
            else
            {
                Database.APIQuarter -= 1;
            }
            return UpdateCurrencies(DatabasePath);
        }
        Page1 = Response.data;
        //Get next page
        if (Response.links.next != null)
        {
            GetCurrencyData(Database.CurrencyAPI + Response.links.next, out Response);
            Page2 = Response.data;
        }
        //Aggregate data
        if (Page2 != null)
        {
            Data = new Datum[Page1.Length + Page2.Length];
            Page1.CopyTo(Data, 0);
            Page2.CopyTo(Data, Page1.Length);
        }
        else
        {
            Data = Page1;
        }
        for (int i = 0; i < Data.Length; i++)
        {
            //Currencies no longer used
            if (
                Data[i].currency.Equals("Chavito", StringComparison.OrdinalIgnoreCase)
                ||
                Data[i].currency.Equals("Fuerte (OLD)", StringComparison.OrdinalIgnoreCase)
            )
            {
                continue;
            }
            else
            {
                //Remove duplicates with earlier date
                Datum Item = Currencies.Find(item => item.country.Equals(Data[i].country, StringComparison.OrdinalIgnoreCase));
                if (Item != null && Currencies.Contains(Item))
                {
                    Currencies.Remove(Item);
                }
                Currencies.Add(Data[i]);
            }
        }
        using (SqliteConnection Connection = new SqliteConnection($"Data Source={DatabasePath};Mode=ReadWrite"))
        {
            string Currency;
            string USDEquivalent;
            string Query;
            HashSet<string> UpdatedCurrencies = new HashSet<string>();
            //Open connection
            Connection.Open();
            //Create command
            SqliteCommand Command = Connection.CreateCommand();
            using (SqliteTransaction Transaction = Connection.BeginTransaction())
            {
                Command.Transaction = Transaction;
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
                    Query = $"UPDATE Info SET [SI Equivalent] = '{USDEquivalent}' WHERE Unit = '{Currency}'";
                    Command.CommandText = Query;
                    Command.ExecuteNonQuery();
                }
                Transaction.Commit();
                Command.Transaction = null;
            }
            //Update cache
            Command.CommandText = "SELECT Unit, [SI Equivalent] FROM Info WHERE Type = 'Currency'";
            using (SqliteDataReader Reader = Command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    Database.UnitCache["Currency"][Reader.GetString(0)].SI = BigDecimal.Parse(Reader.GetString(1));
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
    readonly static char[] Digits = "0123456789".ToCharArray();
    /// <summary>
    /// Get first non digit index of a given string. This does not include scientific notation,
    /// separator symbols or "-" signs.
    /// </summary>
    /// <returns>
    /// An <see cref="int"/> of the first non digit index or -1 if there is none.
    /// </returns>
    public static int FirstNonDigitIndex(string Text, char[] SeparatorSymbols)
    {
        int i = 0;
        while (i < Text.Length)
        {
            char Character = Text[i];
            if (!Digits.Contains(Character) && !SeparatorSymbols.Contains(Character))
            {
                //Check if scientific notation "E" accounting for "-" signs
                if (Character == 'E' && i + 1 < Text.Length)
                {
                    if (Digits.Contains(Text[i + 1]))
                    {
                        i += 2;
                        continue;
                    }
                    else if (
                        Text[i + 1] == '-'
                        &&
                        i + 2 < Text.Length
                    )
                    {
                        if (Digits.Contains(Text[i + 2]))
                        {
                            i += 3;
                            continue;
                        }
                    }
                }
                return i;
            }
            i++;
        }
        return -1;
    }
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
    /// Compares two best matches.
    /// </summary>
    /// <returns>
    /// -1 if <paramref name="x"/> precedes <paramref name="y"/>, 0 if <paramref name="x"/> and <paramref name="y"/>
    /// are in the same position and 1 if <paramref name="x"/> comes after <paramref name="y"/>.
    /// </returns>
    private static int CompareBest(Tuple<string, int, int> x, Tuple<string, int, int> y)
    {
        if (x.Item1.Length < y.Item1.Length) //Shortest strings rank first
        {
            return -1;
        }
        else if (x.Item1.Length == y.Item1.Length && x.Item3 > y.Item3) //Case sensitive preferred
        {
            return -1;
        }
        else if (x.Item1.Length == y.Item1.Length && x.Item3 == y.Item3) //Alphabetize if same
        {
            return x.Item1.CompareTo(y.Item1);
        }
        return 1;
    }
    /// <summary>
    /// Get best matches of a unit name from database.
    /// </summary>
    /// <returns>
    /// A <see langword="List"/> of the best matches of a unit name from the
    /// database and the number of matching characters as a <see cref="KeyValuePair"/>.
    /// </returns>
    public static List<string> BestMatches(string Unit)
    {
        List<Tuple<string, int, int>> SubsequenceMatches = new List<Tuple<string, int, int>>();
        List<Tuple<string, int, int>> SubstringMatches = new List<Tuple<string, int, int>>();
        int SubsequenceBest = 0;
        int SubstringBest = 0;
        int Casing = 0; //0 means case insensitive, 1 means case sensitive
        int SubsequenceScore;
        int SubstringScore;
        int Case1;
        int Case2;
        int NoCase1;
        int NoCase2;
        foreach (KeyValuePair<string, string> x in Database.UnitList)
        {
            Case1 = LongestSubsequence(Unit, x.Key); //Case sensitive
            NoCase1 = LongestSubsequence(Unit.ToUpper(), x.Key.ToUpper()); //Case insensitive
            Case2 = LongestSubstring(Unit, x.Key);
            NoCase2 = LongestSubstring(Unit.ToUpper(), x.Key.ToUpper());
            //Determine if case insensitive match is better based on percentage
            if ((float)NoCase1 / x.Key.Length > (float)Case1 / x.Key.Length)
            {
                SubsequenceScore = NoCase1;
                Casing = 0;
            }
            else //Case sensitivity is preferred as it best captures meaning of inexact query
            {
                SubsequenceScore = Case1;
                Casing = 1;
            }
            if ((float)NoCase2 / x.Key.Length > (float)Case2 / x.Key.Length)
            {
                SubstringScore = NoCase2;
                Casing = 0;
            }
            else
            {
                SubstringScore = Case2;
                Casing = 1;
            }
            //Add to list only if equal or better than best
            if (SubsequenceScore > 0)
            {
                if (SubsequenceScore >= SubsequenceBest)
                {
                    SubsequenceMatches.Add(new Tuple<string, int, int>(x.Key, SubsequenceScore, Casing));
                    SubsequenceBest = SubsequenceScore;
                }
            }
            if (SubstringScore > 0)
            {
                if (SubstringScore >= SubstringBest)
                {
                    SubstringMatches.Add(new Tuple<string, int, int>(x.Key, SubstringScore, Casing));
                    SubstringBest = SubstringScore;
                }
            }
        }
        if (SubsequenceBest > SubstringBest + 2) //+2 adjustment because longest common substring is preferred
        {
            SubsequenceMatches.RemoveAll(x => x.Item2 != SubsequenceBest);
            SubsequenceMatches.Sort(CompareBest);
            return SubsequenceMatches.ConvertAll(x => x.Item1);
        }
        else //Longest common substring is preferred as it best captures meaning of an inexact query
        {
            SubstringMatches.RemoveAll(x => x.Item2 != SubstringBest);
            SubstringMatches.Sort(CompareBest);
            return SubstringMatches.ConvertAll(x => x.Item1);
        }
    }
    /// <summary>
    /// Get best plural form for a given unit from a given list of plurals.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> that represents the best plural form.
    /// </returns>
    public static string BestPlural(string Unit, List<string> Plurals)
    {
        List<Tuple<string, int>> SubsequenceMatches = new List<Tuple<string, int>>();
        int SubsequenceBest = 0;
        int SubsequenceScore;
        foreach (string x in Plurals)
        {
            SubsequenceScore = LongestSubsequence(Unit, x);
            if (SubsequenceScore >= SubsequenceBest)
            {
                SubsequenceMatches.Add(new Tuple<string, int>(x, SubsequenceScore));
                SubsequenceBest = SubsequenceScore;
            }
        }
        SubsequenceMatches.RemoveAll(x => x.Item2 != SubsequenceBest);
        SubsequenceMatches.Sort((x, y) => x.Item1.Length.CompareTo(y.Item1.Length));
        return SubsequenceMatches[0].Item1;
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
    /// <see cref="Entry"/> for unit 1, the default unit.
    /// </summary>
    public static Entry Unit1;
    /// <summary>
    /// <see cref="Entry"/> for unit 2, the unit to convert to.
    /// </summary>
    public static Entry Unit2;
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
    /// Stores the results of the conversion.
    /// </summary>
    public static List<string> Results = new List<string>();
    /// <summary>
    /// Converts between special temperature units that require specific equations with a given magnitude.
    /// </summary>
    /// <returns>
    /// A <see cref="BigDecimal"/> of the conversion.
    /// </returns>
    public static BigDecimal Temperature(BigDecimal Magnitude, string Unit1, string Unit2)
    {
        //Equations from https://en.wikipedia.org/wiki/Conversion_of_scales_of_temperature
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
    /// Calculate conversions.
    /// </summary>
    public static void Conversions()
    {
        //Get unit 1 SI equivalent
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
        BigDecimal Conversion;
        string ConversionString;
        Results.Clear();
        //Info mode
        if (QueryType == "INFO")
        {
            for (int i = 0; i < Results.Count; i++)
            {
                Results.Add(Unit1BestMatches[i]);
            }
        }
        //Convert mode
        else if (QueryType == "CONVERT")
        {
            //Track units added
            HashSet<string> UnitsAdded = new HashSet<string>();
            for (int i = 0; i < Unit2BestMatches.Count; i++)
            {
                //Prevent adding duplicates due to different alternate names
                if (UnitsAdded.Contains(Database.UnitList[Unit2BestMatches[i]]))
                {
                    continue;
                }
                Entry Item = Database.UnitCache[Unit1.Type][Database.UnitList[Unit2BestMatches[i]]];
                //Only show conversion to same unit if same as unit 2
                if (Item.Unit == Unit1.Unit && Item.Unit != Unit2.Unit)
                {
                    continue;
                }
                //Get unit 2 SI equivalent
                if (Database.InexactValues.ContainsKey(Item.Unit))
                {
                    Unit2SI = Database.InexactValues[Item.Unit];
                }
                else
                {
                    Unit2SI = Item.SI;
                }
                //Conversions
                if (
                    Unit1.Type == "Temperature"
                    &&
                    Database.SpecialUnits.Contains(Unit1.Unit)
                    &&
                    Database.SpecialUnits.Contains(Item.Unit)
                )
                {
                    Conversion = Calculate.Temperature(Magnitude, Unit1.Unit, Item.Unit).Round(SignificantFigures);
                }
                else if (
                (Unit1.Type == "Speed,Velocity" || Unit1.Type == "Fuel Efficiency")
                &&
                (
                (Database.SpecialUnits.Contains(Unit1.Unit) && !Database.SpecialUnits.Contains(Item.Unit))
                ||
                (Database.SpecialUnits.Contains(Item.Unit) && !Database.SpecialUnits.Contains(Unit1.Unit))
                    )
                )
                {
                    Conversion = (1 / Unit1SI / Unit2SI).Round(SignificantFigures);
                }
                else
                {
                    Conversion = (Unit1SI / Unit2SI).Round(SignificantFigures);
                }
                //Add converted result to list
                if (Conversion != 1 && Item.Plurals.Count > 0) //Use plural forms if any
                {
                    ConversionString = $"{Conversion.ToFormattedString(SmallMagnitude, LargeMagnitude)} {Search.BestPlural(Item.Unit, Item.Plurals)}";
                }
                else
                {
                    ConversionString = $"{Conversion.ToFormattedString(SmallMagnitude, LargeMagnitude)} {Item.Unit}";
                }
                Results.Add(ConversionString);
                //Track unit added
                UnitsAdded.Add(Item.Unit);
            }
        }
        //Convert to all
        else
        {
            foreach (KeyValuePair<string, Entry> x in Database.UnitCache[Unit1.Type])
            {
                if (Unit1.Unit != x.Key)
                {
                    //Get unit 2 SI equivalent
                    if (Database.InexactValues.ContainsKey(x.Key))
                    {
                        Unit2SI = Database.InexactValues[x.Key];
                    }
                    else
                    {
                        Unit2SI = x.Value.SI;
                    }
                    //Conversions
                    if (
                        Unit1.Type == "Temperature"
                        &&
                        Database.SpecialUnits.Contains(Unit1.Unit)
                        &&
                        Database.SpecialUnits.Contains(x.Key)
                    )
                    {
                        Conversion = Calculate.Temperature(Magnitude, Unit1.Unit, x.Key).Round(SignificantFigures);
                    }
                    else if (
                        (Unit1.Type == "Speed,Velocity" || Unit1.Type == "Fuel Efficiency")
                        &&
                        (
                            (Database.SpecialUnits.Contains(Unit1.Unit) && !Database.SpecialUnits.Contains(x.Key))
                            ||
                            (Database.SpecialUnits.Contains(x.Key) && !Database.SpecialUnits.Contains(Unit1.Unit))
                        )
                    )
                    {
                        Conversion = (1 / Unit1SI / Unit2SI).Round(SignificantFigures);
                    }
                    else
                    {
                        Conversion = (Unit1SI / Unit2SI).Round(SignificantFigures);
                    }
                    //Add converted result to list
                    if (Conversion != 1 && x.Value.Plurals.Count > 0) //Use plural forms if any
                    {
                        ConversionString = $"{Conversion.ToFormattedString(SmallMagnitude, LargeMagnitude)} {Search.BestPlural(x.Key, x.Value.Plurals)}";
                    }
                    else
                    {
                        ConversionString = $"{Conversion.ToFormattedString(SmallMagnitude, LargeMagnitude)} {x.Key}";
                    }
                    Results.Add(ConversionString);
                }
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
    private static int ResultsCompare(string x, string y, string SortBy, string SortOrder)
    {
        int FirstNonDigit1 = Search.FirstNonDigitIndex(x, BigDecimal.Separators);
        int FirstNonDigit2 = Search.FirstNonDigitIndex(y, BigDecimal.Separators);
        //Sort by unit
        if (SortBy == "UNIT")
        {
            x = x.Substring(FirstNonDigit1, x.Length - FirstNonDigit1).Trim();
            y = y.Substring(FirstNonDigit2, y.Length - FirstNonDigit2).Trim();
            int Order = string.Compare(x, y);
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
            x = x.Substring(0, FirstNonDigit1).Trim();
            y = y.Substring(0, FirstNonDigit2).Trim();
            int Order = BigDecimal.ReverseFormat(
                            x,
                            BigDecimal.DecimalSeparator,
                            BigDecimal.IntegerGroupSeparator,
                            BigDecimal.DecimalGroupSeparator
                        ).CompareTo(BigDecimal.ReverseFormat(
                            y,
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