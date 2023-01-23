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
        if (StringWidth > SearchBox.Width)
        {
            AppState.EntryMaxed = true;
            //Scrolls content and preserve cursor position after search box becomes smaller from resize
            if (Resized)
            {
                int CurrentPosition = SearchBox.SelectionStart;
                SearchBox.SelectionStart = SearchBox.TextLength - 1; //Scroll
                SearchBox.SelectionStart = CurrentPosition;
            }
        }
        else
        {
            //Scrolls search box contents to beginning when box is no longer overflowed
            if (AppState.EntryMaxed)
            {
                int CurrentPosition = SearchBox.SelectionStart;
                SearchBox.SelectionStart = 0; //Scroll
                //SearchBox.ScrollToCaret();
                SearchBox.SelectionStart = CurrentPosition;
                AppState.EntryMaxed = false;
            }
        }
        SearchBox.Focus();
    }
    //
    //————————————————————Interpretation Text————————————————————
    //
    /// <summary>
    /// Truncates a given string if its text overflows the size of the interpret label.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> of the truncated text if truncation is needed. Otherwise, returns full interpretation text.
    /// </returns>
    private string TruncateInterpretation(string Interpretation)
    {
        string TruncatedText = Interpretation;
        int StringWidth = TextRenderer.MeasureText(Interpretation, InterpretLabel.Font).Width;
        int Overflow = SortButton.Location.X - InterpretLabel.Location.X;
        //Truncate text if overflow
        if (StringWidth > Overflow)
        {
            TruncatedText += "...";
            while (StringWidth > Overflow)
            {
                TruncatedText = TruncatedText.Remove(TruncatedText.Length - 4, 1);
                StringWidth = TextRenderer.MeasureText(TruncatedText, InterpretLabel.Font).Width;
            }
            return TruncatedText;
        }
        //Restore truncated text if no longer overflowing
        else
        {
            return Interpretation;
        }
    }
    /// <summary>
    /// Updates the interpretation text using the unit best matches list.
    /// </summary>
    private void UpdateInterpretation()
    {
        if (Calculate.Unit1BestMatches.Count > 0)
        {
            AppState.Interpretation = $"Interpretation: ";
            if (Calculate.Unit1.Symbols.Contains(Calculate.Unit1BestMatches[0]) || Calculate.Unit1.Abbreviations.Contains(Calculate.Unit1BestMatches[0]))
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
                if (Calculate.Unit2.Symbols.Contains(Calculate.Unit2BestMatches[0]) || Calculate.Unit2.Abbreviations.Contains(Calculate.Unit2BestMatches[0]))
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
            InterpretLabel.Text = TruncateInterpretation(AppState.Interpretation);
        }
    }
    //
    //————————————————————Search View————————————————————
    //
    /// <summary>
    /// Clear search view items, interpret label text and resets explore mode.
    /// </summary>
    private void ClearSearchView()
    {
        SearchView.Items.Clear();
        InterpretLabel.Text = "";
        //Resize column width to fit longest item. Removes horizontal scroll bar after clearing.
        SearchView.Columns[0].Width = -1;
        //Reset explore mode
        if (AppState.Explore)
        {
            AppState.Explore = false;
        }
    }
    /// <summary>
    /// Calculate conversion results and add to <see cref="MainWindow.SearchView"/>.
    /// </summary>
    private void GetAddResults()
    {
        ClearSearchView();
        UpdateInterpretation();
        Calculate.Conversions();
        //Sort if converting to all. For convert to and info mode, best match sort order is used.
        if (Calculate.QueryType == "ALL")
        {
            Calculate.SortResults(Settings.SortBy, Settings.SortOrder);
        }
        //BeginUpdate and EndUpdate must be called an equal number of times.
        SearchView.BeginUpdate();
        foreach (string x in Calculate.Results)
        {
            SearchView.Items.Add(x);
        }
        SearchView.EndUpdate();
        //Resize column width to fit longest item
        SearchView.Columns[0].Width = -1;
    }
    /// <summary>
    /// Sort the results in <see cref="MainWindow.SearchView"/>.
    /// </summary>
    private void SortResults()
    {
        //BeginUpdate and EndUpdate must be called an equal number of times.
        SearchView.BeginUpdate();
        if (AppState.Explore == true)
        {
            if (Settings.SortOrder == PreviousState.SortOrder)
            {
                SearchView.EndUpdate();
                return;
            }
            Database.SortUnitListNoPlural(Settings.SortOrder);
            SearchView.Items.Clear();
            foreach (string x in Database.UnitListNoPlural)
            {
                SearchView.Items.Add(x);
            }
        }
        else
        {
            Calculate.SortResults(Settings.SortBy, Settings.SortOrder);
            SearchView.Items.Clear();
            foreach (string x in Calculate.Results)
            {
                SearchView.Items.Add(x);
            }
        }
        SearchView.EndUpdate();
        //Resize column width to fit longest item
        SearchView.Columns[0].Width = -1;
    }
    //
    //————————————————————Database————————————————————
    //
    /// <summary>
    /// Updates the database file, <see cref="Database"/>.UnitCache and <see cref="MainWindow"/>.SearchView
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
                SearchView.Invoke(GetAddResults);
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
