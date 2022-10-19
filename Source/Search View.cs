using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Unitversal
{
    public partial class MainWindow : Form
    {
        /// <summary>
        /// Truncates a given string if its text overflows the size of the interpret label.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> of the truncated text if truncation is needed. Otherwise, returns full interpretation text.
        /// </returns>
        private string TruncateInterpretation(string Interpretation)
        {
            int StringWidth = TextRenderer.MeasureText(Interpretation, InterpretLabel.Font).Width;
            int Overflow = SortButton.Location.X - InterpretLabel.Location.X;
            //Truncate text if overflow
            if (StringWidth > Overflow)
            {
                Interpretation += "...";
                while (StringWidth > Overflow)
                {
                    Interpretation = Interpretation.Remove(Interpretation.Length - 4, 1);
                    StringWidth = TextRenderer.MeasureText(Interpretation, InterpretLabel.Font).Width;
                }
                return Interpretation;
            }
            //Replace truncated text if no longer overflowing
            else
            {
                return AppState.Interpretation;
            }
        }
        /// <summary>
        /// Updates the interpretation text using the unit best matches list.
        /// </summary>
        public void UpdateInterpretation()
        {
            if (AppState.Unit1BestMatches.Count > 0)
            {
                AppState.Interpretation = $"Interpretation: ";
                if (AppState.Unit1.Symbols.Contains(AppState.Unit1BestMatches[0]) || AppState.Unit1.Abbreviations.Contains(AppState.Unit1BestMatches[0]))
                {
                    AppState.Interpretation += $"{AppState.Unit1.Unit} ({AppState.Unit1BestMatches[0]})";
                }
                else
                {
                    AppState.Interpretation += AppState.Unit1BestMatches[0];
                }
                if (AppState.QueryType == "INFO")
                {
                    AppState.Interpretation += " Info";
                }
                else if (AppState.QueryType == "CONVERT")
                {
                    if (AppState.Unit2.Symbols.Contains(AppState.Unit2BestMatches[0]) || AppState.Unit2.Abbreviations.Contains(AppState.Unit2BestMatches[0]))
                    {
                        AppState.Interpretation += $" To {AppState.Unit2.Unit} ({AppState.Unit2BestMatches[0]})";
                    }
                    else
                    {
                        AppState.Interpretation += $" To {AppState.Unit2BestMatches[0]}";
                    }
                }
                else
                {
                    AppState.Interpretation += " To All";
                }
                InterpretLabel.Text = TruncateInterpretation(AppState.Interpretation);
            }
        }
        /// <summary>
        /// Get first non digit index of a given item text from the search view. This does not include scientific
        /// notation, separator symbols or "-" signs.
        /// </summary>
        /// <returns>
        /// An <see cref="int"/> of the first non digit index.
        /// </returns>
        public static int FirstNonDigitIndex(string Text)
        {
            char[] Digits = "0123456789".ToCharArray();
            char[] Separators = (Settings.DecimalSeparator + Settings.IntegerGroupSeparator + Settings.DecimalGroupSeparator).ToCharArray();
            for (int i = 0; i < Text.Length; i++)
            {
                if (!Digits.Contains(Text[i]) && !Separators.Contains(Text[i]))
                {
                    //Check if scientific notation "E" accounting for "-" signs
                    if (
                        i + 2 < Text.Length
                        &&
                        Text[i + 1] != '-'
                        &&
                        !Digits.Contains(Text[i + 1])
                        &&
                        !Digits.Contains(Text[i + 2])
                    )
                    {
                        return i;
                    }
                }
            }
            return 0;
        }
        //Custom sort comparison for search view
        public class CustomSort : System.Collections.IComparer
        {
            private int Column = 0; //Sorting column 0
            //Sorting comparison
            public int Compare(object x, object y)
            {
                //Disables the automatic sort
                if (!AppState.DisableSort)
                {
                    string Item1 = ((ListViewItem)x).SubItems[Column].Text;
                    string Item2 = ((ListViewItem)y).SubItems[Column].Text;
                    int FirstNonDigit1 = FirstNonDigitIndex(Item1);
                    int FirstNonDigit2 = FirstNonDigitIndex(Item2);
                    //Sort by unit
                    if (AppState.Explore == true || Settings.SortBy == "UNIT")
                    {
                        Item1 = Item1.Substring(FirstNonDigit1, Item1.Length - FirstNonDigit1).Trim();
                        Item2 = Item2.Substring(FirstNonDigit2, Item2.Length - FirstNonDigit2).Trim();
                        if (Settings.SortOrder == "ASCENDING")
                        {
                            return string.Compare(Item1, Item2);
                        }
                        else
                        {
                            return string.Compare(Item1, Item2) * -1;
                        }
                    }
                    //Sort by magnitude
                    else if (Settings.SortBy == "MAGNITUDE")
                    {
                        Item1 = Item1.Substring(0, FirstNonDigit1).Trim();
                        Item2 = Item2.Substring(0, FirstNonDigit2).Trim();
                        if (Settings.SortOrder == "ASCENDING")
                        {
                            return BigDecimalReverseFormat(Item1).CompareTo(BigDecimalReverseFormat(Item2));
                        }
                        else
                        {
                            return BigDecimalReverseFormat(Item1).CompareTo(BigDecimalReverseFormat(Item2)) * -1;
                        }
                    }
                }
                return 0;
            }
        }
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
        /// Converts between special temperature units that require specific equations with a given magnitude.
        /// </summary>
        /// <returns>
        /// A <see cref="BigDecimal"/> of the conversion.
        /// </returns>
        private BigDecimal Temperature(BigDecimal Magnitude, string Unit1, string Unit2)
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
        /// Gets and adds results to the search view after clearing. Also updates interpretation text.
        /// </summary>
        private void GetAddResults()
        {
            ClearSearchView();
            UpdateInterpretation();
            //Get unit 1 SI equivalent
            BigDecimal Unit1SI = 1;
            BigDecimal Unit2SI = 1;
            if (AppState.InexactValues.ContainsKey(AppState.Unit1.Unit))
            {
                Unit1SI = AppState.Magnitude * BigDecimal.Parse(AppState.InexactValues[AppState.Unit1.Unit]);
            }
            else
            {
                Unit1SI = AppState.Magnitude * BigDecimal.Parse(AppState.Unit1.SI);
            }
            BigDecimal Conversion;
            string ConversionString;
            //Info mode
            if (AppState.QueryType == "INFO")
            {
                ListViewItem[] Items = new ListViewItem[AppState.Unit1BestMatches.Count];
                for (int i = 0; i < Items.Length; i++)
                {
                    Items[i] = new ListViewItem(AppState.Unit1BestMatches[i]);
                }
                AppState.DisableSort = true; //Use best match sort order
                SearchView.BeginUpdate();
                SearchView.Items.AddRange(Items);
                SearchView.EndUpdate();
                AppState.DisableSort = false;
            }
            //Convert mode
            else if (AppState.QueryType == "CONVERT")
            {
                //Track units added
                HashSet<string> UnitsAdded = new HashSet<string>();
                List<ListViewItem> ItemList = new List<ListViewItem>();
                for (int i = 0; i < AppState.Unit2BestMatches.Count; i++)
                {
                    //Prevent adding duplicates due to different alternate names
                    if (UnitsAdded.Contains(AppState.UnitList[AppState.Unit2BestMatches[i]]))
                    {
                        continue;
                    }
                    Entry Item = AppState.UnitCache[AppState.Unit1.Type][AppState.UnitList[AppState.Unit2BestMatches[i]]];
                    //Only show conversion to same unit if same as unit 2
                    if (Item.Unit == AppState.Unit1.Unit && Item.Unit != AppState.Unit2.Unit)
                    {
                        continue;
                    }
                    //Get unit 2 SI equivalent
                    if (AppState.InexactValues.ContainsKey(Item.Unit))
                    {
                        Unit2SI = BigDecimal.Parse(AppState.InexactValues[Item.Unit]);
                    }
                    else
                    {
                        Unit2SI = BigDecimal.Parse(Item.SI);
                    }
                    //Conversions
                    if (
                        AppState.Unit1.Type == "Temperature"
                        &&
                        AppState.SpecialUnits.Contains(AppState.Unit1.Unit)
                        &&
                        AppState.SpecialUnits.Contains(Item.Unit)
                    )
                    {
                        Conversion = Temperature(AppState.Magnitude, AppState.Unit1.Unit, Item.Unit).Truncate(Settings.SignificantFigures);
                    }
                    else if (
                        (AppState.Unit1.Type == "Speed,Velocity" || AppState.Unit1.Type == "Fuel Efficiency")
                        &&
                        (
                            (AppState.SpecialUnits.Contains(AppState.Unit1.Unit) && !AppState.SpecialUnits.Contains(Item.Unit))
                            ||
                            (AppState.SpecialUnits.Contains(Item.Unit) && !AppState.SpecialUnits.Contains(AppState.Unit1.Unit))
                        )
                    )
                    {
                        Conversion = (1 / Unit1SI / Unit2SI).Truncate(Settings.SignificantFigures);
                    }
                    else
                    {
                        Conversion = (Unit1SI / Unit2SI).Truncate(Settings.SignificantFigures);
                    }
                    //Add converted result to list
                    if (Conversion != 1 && Item.Plurals.Count > 0) //Use plural forms if any
                    {
                        ConversionString = $"{BigDecimalFormat(Conversion)} {BestPlural(Item.Unit, Item.Plurals)}";
                    }
                    else
                    {
                        ConversionString = $"{BigDecimalFormat(Conversion)} {Item.Unit}";
                    }
                    ItemList.Add(new ListViewItem(ConversionString));
                    //Track unit added
                    UnitsAdded.Add(Item.Unit);
                }
                //Add converted results to search view
                AppState.DisableSort = true; //Use best match sort order
                SearchView.BeginUpdate();
                SearchView.Items.AddRange(ItemList.ToArray());
                SearchView.EndUpdate();
                AppState.DisableSort = false;
            }
            //Convert to all
            else
            {
                List<ListViewItem> ItemList = new List<ListViewItem>();
                foreach (KeyValuePair<string, Entry> x in AppState.UnitCache[AppState.Unit1.Type])
                {
                    if (AppState.Unit1.Unit != x.Key)
                    {
                        //Get unit 2 SI equivalent
                        if (AppState.InexactValues.ContainsKey(x.Key))
                        {
                            Unit2SI = BigDecimal.Parse(AppState.InexactValues[x.Key]);
                        }
                        else
                        {
                            Unit2SI = BigDecimal.Parse(x.Value.SI);
                        }
                        //Conversions
                        if (
                            AppState.Unit1.Type == "Temperature"
                            &&
                            AppState.SpecialUnits.Contains(AppState.Unit1.Unit)
                            &&
                            AppState.SpecialUnits.Contains(x.Key)
                        )
                        {
                            Conversion = Temperature(AppState.Magnitude, AppState.Unit1.Unit, x.Key).Truncate(Settings.SignificantFigures);
                        }
                        else if (
                            (AppState.Unit1.Type == "Speed,Velocity" || AppState.Unit1.Type == "Fuel Efficiency")
                            &&
                            (
                                (AppState.SpecialUnits.Contains(AppState.Unit1.Unit) && !AppState.SpecialUnits.Contains(x.Key))
                                ||
                                (AppState.SpecialUnits.Contains(x.Key) && !AppState.SpecialUnits.Contains(AppState.Unit1.Unit))
                            )
                        )
                        {
                            Conversion = (1 / Unit1SI / Unit2SI).Truncate(Settings.SignificantFigures);
                        }
                        else
                        {
                            Conversion = (Unit1SI / Unit2SI).Truncate(Settings.SignificantFigures);
                        }
                        //Add converted result to list
                        if (Conversion != 1 && x.Value.Plurals.Count > 0) //Use plural forms if any
                        {
                            ConversionString = $"{BigDecimalFormat(Conversion)} {BestPlural(x.Key, x.Value.Plurals)}";
                        }
                        else
                        {
                            ConversionString = $"{BigDecimalFormat(Conversion)} {x.Key}";
                        }
                        ItemList.Add(new ListViewItem(ConversionString));
                    }
                }
                //Add converted results to search view
                SearchView.BeginUpdate();
                SearchView.Items.AddRange(ItemList.ToArray());
                SearchView.EndUpdate();
            }
            //Resize column width to fit longest item
            SearchView.Columns[0].Width = -1;
        }
    }
}
