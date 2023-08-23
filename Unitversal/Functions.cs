namespace Unitversal;

public partial class MainWindow : Form
{
    //
    //————————————————————Settings File————————————————————
    //
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
                Calculate.SignificantFigures = SignificantFigures;
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
            BigDecimal.DecimalSeparator = DecimalSeparator;
            Settings.DecimalSeparator = DecimalSeparator;
        }
        //Restore integer group separator
        if (IntegerGroupSeparator.Length == 1 && !int.TryParse(IntegerGroupSeparator, out _))
        {
            IntegerGroupSeparatorEntry.Text = IntegerGroupSeparator;
            BigDecimal.IntegerGroupSeparator = IntegerGroupSeparator;
            Settings.IntegerGroupSeparator = IntegerGroupSeparator;
        }
        //Restore integer group size
        if (int.TryParse(AppState.SettingsFile.Read("Settings", "IntegerGroupSize"), out int IntegerGroupSize))
        {
            if (IntegerGroupSize >= 0 && IntegerGroupSize <= 9)
            {
                IntegerGroupSizeEntry.Value = IntegerGroupSize;
                BigDecimal.IntegerGroupSize = IntegerGroupSize;
                Settings.IntegerGroupSize = IntegerGroupSize;
            }
        }
        //Restore decimal group separator
        if (DecimalGroupSeparator.Length == 1 && !int.TryParse(DecimalGroupSeparator, out _))
        {
            DecimalGroupSeparatorEntry.Text = DecimalGroupSeparator;
            BigDecimal.DecimalGroupSeparator = DecimalGroupSeparator;
            Settings.DecimalGroupSeparator = DecimalGroupSeparator;
        }
        //Restore decimal group size
        if (int.TryParse(AppState.SettingsFile.Read("Settings", "DecimalGroupSize"), out int DecimalGroupSize))
        {
            if (DecimalGroupSize >= 0 && DecimalGroupSize <= 9)
            {
                DecimalGroupSizeEntry.Value = DecimalGroupSize;
                BigDecimal.DecimalGroupSize = DecimalGroupSize;
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
                Calculate.LargeMagnitude = new BigDecimal(Mantissa, Exponent);
                Settings.LargeMagnitude = Calculate.LargeMagnitude;
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
                Calculate.SmallMagnitude = new BigDecimal(Mantissa, Exponent);
                Settings.SmallMagnitude = Calculate.SmallMagnitude;
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
    //
    //————————————————————Search Box————————————————————
    //
    /// <summary>
    /// Scroll search box contents when search box is overflowed with text.
    /// </summary>
    private void ScrollTextContents(bool Resized)
    {
        //Get width of string in pixels
        int StringWidth = TextRenderer.MeasureText(SearchBox.Text, SearchBox.Font).Width;
        if (StringWidth > SearchBox.Width - 100)
        {
            AppState.EntryMaxed = true;
            //Scrolls content and preserve cursor position after search box becomes smaller from resize
            if (Resized)
            {
                int CurrentPosition = SearchBox.SelectionStart;
                SearchBox.SelectionStart = SearchBox.TextLength - 1; //Scroll
                SearchBox.SelectionStart = CurrentPosition; //Reset cursor position
            }
        }
        else
        {
            //Scrolls search box contents to beginning when box is no longer overflowed
            if (AppState.EntryMaxed)
            {
                int CurrentPosition = SearchBox.SelectionStart;
                SearchBox.SelectionStart = 0; //Scroll
                SearchBox.SelectionStart = CurrentPosition; //Reset cursor position
                AppState.EntryMaxed = false;
            }
        }
        SearchBox.Focus();
    }
    //
    //————————————————————Interpretation Text————————————————————
    //
    /// <summary>
    /// Assigns a given text to a given label and truncates the text if it overflows a given width.
    /// Truncates more frequently than AutoEllipsis.
    /// </summary>
    private void SetLabel(Label Label, int Width, string Text)
    {
        string TruncatedText = Text;
        int StringWidth = TextRenderer.MeasureText(TruncatedText, Label.Font).Width;
        //Truncate text if overflow
        if (StringWidth > Width)
        {
            TruncatedText += "...";
            while (StringWidth > Width)
            {
                TruncatedText = TruncatedText.Remove(TruncatedText.Length - 4, 1);
                StringWidth = TextRenderer.MeasureText(TruncatedText, Label.Font).Width;
            }
            Label.Text = TruncatedText;
        }
        //Restore truncated text if no longer overflowing
        else
        {
            Label.Text = Text;
        }
        Label.MaximumSize = new Size(Width, Label.Height);
    }
    /// <summary>
    /// Set the interpretation text to a given string.
    /// </summary>
    private void SetInterpretation(string Text)
    {
        AppState.Interpretation = Text;
        SetLabel(InterpretLabel, SortButton.Location.X - InterpretLabel.Location.X, AppState.Interpretation);
    }
    /// <summary>
    /// Updates the interpretation text using the unit best matches list.
    /// </summary>
    private void UpdateInterpretation()
    {
        AppState.Interpretation = $"Interpretation: ";
        if (
            Calculate.Unit1.Symbols.Contains(Calculate.Unit1BestMatches[0])
            ||
            Calculate.Unit1.Abbreviations.Contains(Calculate.Unit1BestMatches[0])
        )
        {
            AppState.Interpretation += $"{Calculate.Unit1.Unit} ({Calculate.Unit1BestMatches[0]})";
        }
        else
        {
            AppState.Interpretation += Calculate.Unit1BestMatches[0];
        }
        if (Calculate.QueryType == "INFO")
        {
            AppState.Interpretation += " Info";
        }
        else if (Calculate.QueryType == "CONVERT")
        {
            if (
                Calculate.Unit2.Symbols.Contains(Calculate.Unit2BestMatches[0])
                ||
                Calculate.Unit2.Abbreviations.Contains(Calculate.Unit2BestMatches[0])
            )
            {
                AppState.Interpretation += $" To {Calculate.Unit2.Unit} ({Calculate.Unit2BestMatches[0]})";
            }
            else
            {
                AppState.Interpretation += $" To {Calculate.Unit2BestMatches[0]}";
            }
        }
        else
        {
            AppState.Interpretation += " To All";
        }
        SetLabel(InterpretLabel, SortButton.Location.X - InterpretLabel.Location.X, AppState.Interpretation);
    }
    //
    //————————————————————Search View————————————————————
    //
    /// <summary>
    /// Select all items from a given <see cref="ListView"/>.
    /// </summary>
    private void ListViewSelectAll(ListView View)
    {
        foreach (ListViewItem x in View.Items)
        {
            x.Selected = true;
        }
    }
    /// <summary>
    /// Copy items from a given <see cref="ListView"/> to clipboard.
    /// </summary>
    private void ListViewCopy(ListView View)
    {
        string CopyText = "";
        foreach (ListViewItem x in View.SelectedItems)
        {
            CopyText += x.Text + "\n";
        }
        CopyText = CopyText.Remove(CopyText.Length - 1);
        Clipboard.SetText(CopyText);
    }
    /// <summary>
    /// Clear search view items, interpret label text and resets explore mode.
    /// </summary>
    private void ClearSearchView()
    {
        SearchView.Items.Clear();
        //Resize column width to fit longest item. Removes horizontal scroll bar after clearing.
        SearchView.Columns[0].Width = -1;
        InterpretLabel.Text = "";
    }
    /// <summary>
    /// Calculate conversion results and add to <see cref="MainWindow.SearchView"/>.
    /// </summary>
    private void UpdateResults()
    {
        //BeginUpdate and EndUpdate must be called an equal number of times.
        SearchView.BeginUpdate();
        ClearSearchView();
        UpdateInterpretation();
        if (Calculate.QueryType == "INFO")
        {
            foreach (string x in Calculate.Unit1BestMatches)
            {
                SearchView.Items.Add(x);
            }
        }
        else
        {
            Calculate.Conversions();
            //Sort if converting to all. For convert to and info mode, best match sort order is used.
            if (Calculate.QueryType == "ALL")
            {
                Calculate.SortResults(Settings.SortBy, Settings.SortOrder);
            }
            foreach (Tuple<string,string> x in Calculate.Results)
            {
                SearchView.Items.Add($"{x.Item1} {x.Item2}");
            }
        }
        //Resize column width to fit longest item
        SearchView.Columns[0].Width = -1;
        SearchView.EndUpdate();
    }
    /// <summary>
    /// Sort the results in <see cref="MainWindow.SearchView"/>.
    /// </summary>
    private void SortResults()
    {
        //Info mode cannot sort by magnitude, only ascending and descending
        if (Calculate.QueryType == "INFO")
        {
            if (Settings.SortOrder == PreviousState.SortOrder)
            {
                return;
            }
        }
        //BeginUpdate and EndUpdate must be called an equal number of times.
        SearchView.BeginUpdate();
        SearchView.Items.Clear();
        if (Calculate.QueryType == "INFO")
        {
            if (Settings.SortOrder == "ASCENDING")
            {
                Calculate.Unit1BestMatches.Sort();
            }
            else
            {
                Calculate.Unit1BestMatches.Sort((x, y) => y.CompareTo(x));
            }
            foreach (string x in Calculate.Unit1BestMatches)
            {
                SearchView.Items.Add(x);
            }
        }
        else
        {
            Calculate.SortResults(Settings.SortBy, Settings.SortOrder);
            foreach (Tuple<string, string> x in Calculate.Results)
            {
                SearchView.Items.Add($"{x.Item1} {x.Item2}");
            }
        }
        //Resize column width to fit longest item
        SearchView.Columns[0].Width = -1;
        SearchView.EndUpdate();
    }
    /// <summary>
    /// Load the explorer list views.
    /// </summary>
    private void LoadExplorer()
    {
        List<string> TempList;
        if (AllView.Items.Count == 0 || Settings.SortOrder != PreviousState.SortOrder)
        {
            //BeginUpdate and EndUpdate must be called an equal number of times.
            AllView.BeginUpdate();
            AllView.Items.Clear();
            Database.SortStringList(Database.UnitListNoPlural, Settings.SortOrder);
            foreach (string x in Database.UnitListNoPlural)
            {
                AllView.Items.Add(x);
            }
            //Resize column width to fit longest item
            AllView.Columns[0].Width = -1;
            AllView.EndUpdate();
        }
        if (CategoryView.Items.Count == 0 || Settings.SortOrder != PreviousState.SortOrder || AppState.SecondMenu != PreviousState.SecondMenu)
        {
            //BeginUpdate and EndUpdate must be called an equal number of times.
            CategoryView.BeginUpdate();
            CategoryView.Items.Clear();
            if (AppState.SecondMenu != "")
            {
                TempList = new List<string>(Database.UnitCache[AppState.SecondMenu].Keys);
                if (Settings.SortOrder == "ASCENDING")
                {
                    TempList.Sort();
                }
                else
                {
                    TempList.Sort((x, y) => y.CompareTo(x));
                }
                foreach (string x in TempList)
                {
                    CategoryView.Items.Add(x);
                }
            }
            else
            {
                Database.SortStringList(Database.UnitCategories, Settings.SortOrder);
                foreach (string x in Database.UnitCategories)
                {
                    CategoryView.Items.Add(x);
                }
            }
            //Resize column width to fit longest item
            CategoryView.Columns[0].Width = -1;
            CategoryView.EndUpdate();
        }
        if (BinaryView.Items.Count == 0 || Settings.SortOrder != PreviousState.SortOrder)
        {
            //BeginUpdate and EndUpdate must be called an equal number of times.
            BinaryView.BeginUpdate();
            BinaryView.Items.Clear();
            TempList = new List<string>(Database.BinaryPrefixes.Keys);
            if (Settings.SortOrder == "ASCENDING")
            {
                TempList.Sort();
            }
            else
            {
                TempList.Sort((x, y) => y.CompareTo(x));
            }
            foreach (string x in TempList)
            {
                BinaryView.Items.Add(x);
            }
            //Resize column width to fit longest item
            BinaryView.Columns[0].Width = -1;
            BinaryView.EndUpdate();
        }
        if (SIView.Items.Count == 0 || Settings.SortOrder != PreviousState.SortOrder)
        {
            //BeginUpdate and EndUpdate must be called an equal number of times.
            SIView.BeginUpdate();
            SIView.Items.Clear();
            TempList = new List<string>(Database.SIPrefixes.Keys);
            if (Settings.SortOrder == "ASCENDING")
            {
                TempList.Sort();
            }
            else
            {
                TempList.Sort((x, y) => y.CompareTo(x));
            }
            foreach (string x in TempList)
            {
                SIView.Items.Add(x);
            }
            //Resize column width to fit longest item
            SIView.Columns[0].Width = -1;
            SIView.EndUpdate();
        }
        if (EquivalentsView.Items.Count == 0 || Settings.SortOrder != PreviousState.SortOrder)
        {
            //BeginUpdate and EndUpdate must be called an equal number of times.
            EquivalentsView.BeginUpdate();
            EquivalentsView.Items.Clear();
            Database.SortStringList(Database.UnitCategories, Settings.SortOrder);
            foreach (string x in Database.UnitCategories)
            {
                EquivalentsView.Items.Add(x);
            }
            //Resize column width to fit longest item
            EquivalentsView.Columns[0].Width = -1;
            EquivalentsView.EndUpdate();
        }
        if (TemperatureView.Items.Count == 0 || Settings.SortOrder != PreviousState.SortOrder || AppState.FirstUnit != PreviousState.FirstUnit)
        {
            //BeginUpdate and EndUpdate must be called an equal number of times.
            TemperatureView.BeginUpdate();
            TemperatureView.Items.Clear();
            TempList = new List<string>(Database.TemperatureFormulas.Keys);
            if (AppState.FirstUnit != "")
            {
                TempList.Remove(AppState.FirstUnit);
            }
            if (Settings.SortOrder == "ASCENDING")
            {
                TempList.Sort();
            }
            else
            {
                TempList.Sort((x, y) => y.CompareTo(x));
            }
            foreach (string x in TempList)
            {
                TemperatureView.Items.Add(x);
            }
            //Resize column width to fit longest item
            TemperatureView.Columns[0].Width = -1;
            TemperatureView.EndUpdate();
        }
    }
    /// <summary>
    /// Show a given prefix's information in <see cref="MainWindow.InfoDisplay"/>.
    /// </summary>
    private void ShowPrefixInfo(string Item)
    {
        PrefixEntry Prefix = Database.SIPrefixes.ContainsKey(Item) ? Database.SIPrefixes[Item] : Database.BinaryPrefixes[Item];
        //Remove focus from display to keep contents from scrolling down
        Title.Focus();
        //Clear text
        DescriptionText.Clear();
        //Prefix text
        DescriptionText.AppendText("Prefix");
        DescriptionText.SelectionStart = 0;
        DescriptionText.SelectionLength = 6;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Prefix.Prefix);
        DescriptionText.AppendText("\n\n");
        int PreviousLength = DescriptionText.TextLength;
        //Symbol text
        DescriptionText.AppendText("Symbol");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 6;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Prefix.Symbol);
        DescriptionText.AppendText("\n\n");
        PreviousLength = DescriptionText.TextLength;
        //Factor text
        DescriptionText.AppendText("Factor");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 6;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Prefix.Factor);
        DescriptionText.AppendText("\n\n");
        PreviousLength = DescriptionText.TextLength;
        //Standard Form text
        DescriptionText.AppendText("Standard Form");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 13;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Prefix.StandardForm.Replace('.', char.Parse(Settings.DecimalSeparator)));
        DescriptionText.AppendText("\n\n");
        PreviousLength = DescriptionText.TextLength;
        //Description text
        DescriptionText.AppendText("Description");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 11;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Prefix.Description);
        //Make info display visible
        InfoDisplay.Visible = true;
        InfoDisplay.BringToFront();
    }
    /// <summary>
    /// Show a given unit's information in <see cref="MainWindow.InfoDisplay"/>.
    /// </summary>
    private void ShowUnitInfo(string Item)
    {
        //Get unit info from cache
        UnitEntry Unit = Database.GetUnitFromCache(Database.UnitList[Item]);
        //Remove focus from display to keep contents from scrolling down
        Title.Focus();
        //Clear text
        DescriptionText.Clear();
        //Unit text
        DescriptionText.AppendText("Unit");
        DescriptionText.SelectionStart = 0;
        DescriptionText.SelectionLength = 4;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Unit.Unit);
        DescriptionText.AppendText("\n\n");
        int PreviousLength = DescriptionText.TextLength;
        //Alternate names text
        DescriptionText.AppendText("Alternate Names");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 15;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        List<string> AlternateNames = new List<string>(Unit.AlternateNames);
        AlternateNames.AddRange(Unit.Variants);
        int IndexLength = AlternateNames.Count - 1;
        for (int i = 0; i < AlternateNames.Count; i++)
        {
            if (i == IndexLength)
            {
                DescriptionText.AppendText($"{AlternateNames[i]}");
            }
            else
            {
                DescriptionText.AppendText($"{AlternateNames[i]}, ");
            }
        }
        DescriptionText.AppendText("\n\n");
        PreviousLength = DescriptionText.TextLength;
        //Symbols text
        DescriptionText.AppendText("Symbols");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 7;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        for (int i = 0; i < Unit.Symbols.Count; i++)
        {
            if (i == Unit.Symbols.Count - 1)
            {
                DescriptionText.AppendText($"{Unit.Symbols[i]}");
            }
            else
            {
                DescriptionText.AppendText($"{Unit.Symbols[i]}, ");
            }
        }
        DescriptionText.AppendText("\n\n");
        PreviousLength = DescriptionText.TextLength;
        //Abbreviations text
        DescriptionText.AppendText("Abbreviations");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 13;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        for (int i = 0; i < Unit.Abbreviations.Count; i++)
        {
            if (i == Unit.Abbreviations.Count - 1)
            {
                DescriptionText.AppendText($"{Unit.Abbreviations[i]}");
            }
            else
            {
                DescriptionText.AppendText($"{Unit.Abbreviations[i]}, ");
            }
        }
        DescriptionText.AppendText("\n\n");
        PreviousLength = DescriptionText.TextLength;
        //Type text
        DescriptionText.AppendText("Type");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 4;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Unit.Type);
        DescriptionText.AppendText("\n\n");
        PreviousLength = DescriptionText.TextLength;
        //SI text
        if (Unit.Type.Equals("Currency", StringComparison.OrdinalIgnoreCase))
        {
            DescriptionText.AppendText("USD Equivalent");
            DescriptionText.SelectionStart = PreviousLength;
            DescriptionText.SelectionLength = 14;
        }
        else
        {
            DescriptionText.AppendText("SI Equivalent");
            DescriptionText.SelectionStart = PreviousLength;
            DescriptionText.SelectionLength = 13;
        }
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Unit.SI.PlainString().Replace('.', char.Parse(Settings.DecimalSeparator)));
        if (Database.InexactValues.ContainsKey(Unit.Unit))
        {
            DescriptionText.AppendText("...");
        }
        DescriptionText.AppendText("\n\n");
        PreviousLength = DescriptionText.TextLength;
        //Description text
        DescriptionText.AppendText("Description");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 11;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Unit.Description);
        //Make about display visible
        InfoDisplay.Visible = true;
        InfoDisplay.BringToFront();
    }
    /// <summary>
    /// Show the formula for converting between two given units in <see cref="MainWindow.InfoDisplay"/>.
    /// </summary>
    private void ShowTemperatureFormula(string Unit1, string Unit2)
    {
        Tuple<string, string> Info = Database.TemperatureFormulas[Unit1][Unit2];
        //Remove focus from display to keep contents from scrolling down
        Title.Focus();
        //Clear text
        DescriptionText.Clear();
        //From text
        DescriptionText.AppendText("From");
        DescriptionText.SelectionStart = 0;
        DescriptionText.SelectionLength = 4;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Unit1);
        DescriptionText.AppendText("\n\n");
        int PreviousLength = DescriptionText.TextLength;
        //To text
        DescriptionText.AppendText("To");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 2;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Unit2);
        DescriptionText.AppendText("\n\n");
        PreviousLength = DescriptionText.TextLength;
        //Formula text
        DescriptionText.AppendText("Formula");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 7;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Info.Item1);
        DescriptionText.AppendText("\n\n");
        PreviousLength = DescriptionText.TextLength;
        //Description text
        DescriptionText.AppendText("Description");
        DescriptionText.SelectionStart = PreviousLength;
        DescriptionText.SelectionLength = 11;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Bold);
        DescriptionText.SelectionStart = DescriptionText.TextLength;
        DescriptionText.SelectionFont = new Font(DescriptionText.Font, FontStyle.Regular);
        DescriptionText.AppendText(" ");
        DescriptionText.AppendText(Info.Item2);
        //Make info display visible
        InfoDisplay.Visible = true;
        InfoDisplay.BringToFront();
    }
    //
    //————————————————————Settings Panel————————————————————
    //
    /// <summary>
    /// Check if the decimal, integer grouping and decimal grouping separators are valid in their
    /// respective entry boxes.
    /// </summary>
    /// /// <returns>
    /// <see langword="true"/> if valid otherwise <see langword="false"/>.
    /// </returns>
    private bool SeparatorCheck()
    {
        //Cannot be empty
        if (DecimalSeparatorEntry.Text == "")
        {
            MessageBox.Show("Decimal separator cannot be empty.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalSeparatorEntry.Text = Settings.DecimalSeparator;
            return false;
        }
        if (IntegerGroupSeparatorEntry.Text == "")
        {
            MessageBox.Show("Integer grouping separator cannot be empty.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            IntegerGroupSeparatorEntry.Text = Settings.IntegerGroupSeparator;
            return false;
        }
        if (DecimalGroupSeparatorEntry.Text == "")
        {
            MessageBox.Show("Decimal grouping separator cannot be empty.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalGroupSeparatorEntry.Text = Settings.DecimalGroupSeparator;
            return false;
        }
        //Cannot be reserved character
        if (Search.ReservedCharacters.Contains(DecimalSeparatorEntry.Text))
        {
            MessageBox.Show("Decimal separator cannot be - E.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalSeparatorEntry.Text = Settings.DecimalSeparator;
            return false;
        }
        if (Search.ReservedCharacters.Contains(IntegerGroupSeparatorEntry.Text))
        {
            MessageBox.Show("Integer grouping separator cannot be - E.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            IntegerGroupSeparatorEntry.Text = Settings.IntegerGroupSeparator;
            return false;
        }
        if (Search.ReservedCharacters.Contains(DecimalGroupSeparatorEntry.Text))
        {
            MessageBox.Show("Decimal grouping separator cannot be - E.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalGroupSeparatorEntry.Text = Settings.DecimalGroupSeparator;
            return false;
        }
        //Cannot be numeral
        if (int.TryParse(DecimalSeparatorEntry.Text, out _))
        {
            MessageBox.Show("Decimal separator cannot be a numeral.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalSeparatorEntry.Text = Settings.DecimalSeparator;
            return false;
        }
        if (int.TryParse(IntegerGroupSeparatorEntry.Text, out _))
        {
            MessageBox.Show("Integer grouping separator cannot be a numeral.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            IntegerGroupSeparatorEntry.Text = Settings.IntegerGroupSeparator;
            return false;
        }
        if (int.TryParse(DecimalGroupSeparatorEntry.Text, out _))
        {
            MessageBox.Show("Decimal grouping separator cannot be a numeral.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalGroupSeparatorEntry.Text = Settings.DecimalGroupSeparator;
            return false;
        }
        //Decimal and grouping separators cannot be same
        if (DecimalSeparatorEntry.Text == IntegerGroupSeparatorEntry.Text)
        {
            MessageBox.Show("Decimal separator cannot be the same as the integer grouping separator.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalSeparatorEntry.Text = Settings.DecimalSeparator;
            IntegerGroupSeparatorEntry.Text = Settings.IntegerGroupSeparator;
            return false;
        }
        if (DecimalSeparatorEntry.Text == DecimalGroupSeparatorEntry.Text)
        {
            MessageBox.Show("Decimal separator cannot be the same as the decimal grouping separator.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalSeparatorEntry.Text = Settings.DecimalSeparator;
            DecimalGroupSeparatorEntry.Text = Settings.DecimalGroupSeparator;
            return false;
        }
        return true;
    }
    /// <summary>
    /// Updates the database file, <see cref="Database.UnitCache"/> and <see cref="MainWindow.SearchView"/>
    /// with new exchange rates.
    /// </summary>
    private void UpdateCurrencies()
    {
        UpdateCurrencyButton.Invoke((MethodInvoker)(() => UpdateCurrencyButton.Enabled = false));
        CurrencyUpdateText.Invoke((MethodInvoker)(() => CurrencyUpdateText.Text = "Updating currencies..."));
        int Status = Database.UpdateCurrencies(AppState.DatabasePath);
        if (Status == 1)
        {
            //Update search view
            if (SearchView.Items.Count > 0 && Calculate.Unit1.Type == "Currency")
            {
                SearchView.Invoke(ClearSearchView);
                SearchView.Invoke(UpdateInterpretation);
                SearchView.Invoke(UpdateResults);
            }
            if (SettingsPanel.Visible)
            {
                CurrencyUpdateText.Invoke((MethodInvoker)(() => CurrencyUpdateText.Text = "Currencies updated successfully."));
            }
            else
            {
                CurrencyUpdateText.Invoke((MethodInvoker)(() => CurrencyUpdateText.Text = ""));
            }
        }
        else if (Status == 0)
        {
            MessageBox.Show("No internet connection! Unable to update currencies.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            CurrencyUpdateText.Invoke((MethodInvoker)(() => CurrencyUpdateText.Text = ""));
        }
        else if (Status == -1)
        {
            MessageBox.Show("An error has occurred. Unable to update currencies.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            CurrencyUpdateText.Invoke((MethodInvoker)(() => CurrencyUpdateText.Text = ""));
        }
        UpdateCurrencyButton.Invoke((MethodInvoker)(() => UpdateCurrencyButton.Enabled = true));
    }
}
