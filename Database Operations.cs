using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Net;

namespace Unitversal
{
    public partial class MainWindow : Form
    {
        /// <summary>
        /// Check for database file existence and corruption. Displays message box if there is an issue.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if there are no issues, <see langword="false"/> otherwise.
        /// </returns>
        private static bool DatabaseExists()
        {
            //Check existence of database file
            bool DatabaseExist = File.Exists(AppState.DatabasePath);
            if (DatabaseExist)
            {
                using (SqliteConnection Connection = new SqliteConnection($"Data Source={AppState.DatabasePath};Mode=ReadOnly"))
                {
                    //Open connection
                    Connection.Open();
                    //Create command
                    SqliteCommand Command = Connection.CreateCommand();
                    Command.CommandText = "SELECT COUNT(*) FROM Info";
                    //Get results for info
                    try
                    {
                        using (SqliteDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                AppState.InfoExist = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        AppState.InfoExist = false;
                    }
                    //Get results for alternate name
                    Command.CommandText = "SELECT COUNT(*) FROM [Alternate Names]";
                    try
                    {
                        using (SqliteDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                AppState.AlternateExist = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        AppState.AlternateExist = false;
                    }
                    //Show error for corrupt database
                    if (!AppState.InfoExist || !AppState.AlternateExist)
                    {
                        MessageBox.Show("The units database is corrupted! Please redownload the database, make sure it is placed in the same directory as the app, then restart.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Unable to find the units database! Please redownload the database, make sure it is placed in the same directory as the app, then restart.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        /// <summary>
        /// Gets all information about a unit and its alternate names from database file.
        /// </summary>
        /// <returns>
        /// A <see cref="Entry"/> object of the unit.
        /// </returns>
        private static Entry GetUnit(string Unit)
        {
            Entry Entry = new Entry();
            try
            {
                //Create connection
                using (SqliteConnection Connection = new SqliteConnection($"Data Source={AppState.DatabasePath};Mode=ReadOnly"))
                {
                    //Open connection
                    Connection.Open();
                    //Create command
                    SqliteCommand Command = Connection.CreateCommand();
                    Command.CommandText = @"
                        SELECT * FROM Info WHERE Unit = $Unit
                        UNION
                        SELECT Info.* FROM Info INNER JOIN [Alternate Names] ON Info.Unit = [Alternate Names].Unit WHERE [Alternate Names].[Alternate Name] = $Unit
                    ";
                    //Parameters
                    Command.Parameters.AddWithValue("$Unit", Unit);
                    //Get unit info
                    using (SqliteDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            //Required values: Unit, Type, SI Equivalent
                            if (Reader[0].GetType() == typeof(DBNull) || Reader[1].GetType() == typeof(DBNull) || Reader[2].GetType() == typeof(DBNull))
                            {
                                continue;
                            }
                            Entry.Unit = Reader.GetString(0);
                            Entry.Type = Reader.GetString(1);
                            Entry.SI = Reader.GetString(2);
                            if (Reader[3].GetType() != typeof(DBNull))
                            {
                                Entry.Description = Reader.GetString(3);
                            }
                        }
                    }
                    //Clear previous alternate names
                    Entry.AlternateNames = new List<string>();
                    Entry.Symbols = new List<string>();
                    Entry.Abbreviations = new List<string>();
                    Entry.AmericanSpelling = new List<string>();
                    Entry.Plurals = new List<string>();
                    //New command
                    Command.CommandText = @"SELECT * FROM [Alternate Names] WHERE Unit = :Unit";
                    //Parameters
                    Command.Parameters.AddWithValue(":Unit", Entry.Unit);
                    //Get alternate names
                    using (SqliteDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            if (Reader[2].GetType() != typeof(DBNull) && Reader.GetString(2) == "T")
                            {
                                Entry.Symbols.Add(Reader.GetString(1));
                            }
                            else if (Reader[3].GetType() != typeof(DBNull) && Reader.GetString(3) == "T")
                            {
                                Entry.Abbreviations.Add(Reader.GetString(1));
                            }
                            else if (Reader[5].GetType() != typeof(DBNull) && Reader.GetString(5) == "T")
                            {
                                Entry.Plurals.Add(Reader.GetString(1));
                            }
                            //No plurals
                            else if (Reader[4].GetType() != typeof(DBNull) && Reader.GetString(4) == "T")
                            {
                                Entry.AmericanSpelling.Add(Reader.GetString(1));
                            }
                            else
                            {
                                Entry.AlternateNames.Add(Reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch
            {
                DatabaseExists();
            }
            return Entry;
        }
        /// <summary>
        /// Creates the <see cref="AppState"/>.UnitList, <see cref="AppState"/>.UnitListPlural, <see cref="AppState"/>.UnitListNoPlural and <see cref="AppState"/>.UnitCache.
        /// </summary>
        private static void GetUnits()
        {
            using (SqliteConnection Connection = new SqliteConnection($"Data Source={AppState.DatabasePath};Mode=ReadOnly"))
            {
                //Open connection
                Connection.Open();
                //Create command
                SqliteCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT Unit FROM Info WHERE Unit IS NOT NULL";
                //Get unit info
                using (SqliteDataReader Reader = Command.ExecuteReader())
                {
                    Entry NewEntry;
                    List<ListViewItem> UnitListNoPlural = new List<ListViewItem>();
                    while (Reader.Read())
                    {
                        NewEntry = GetUnit(Reader.GetString(0));
                        AppState.UnitList.Add(NewEntry.Unit, NewEntry.Unit);
                        UnitListNoPlural.Add(new ListViewItem(NewEntry.Unit));
                        //Add alternate names
                        foreach (string x in NewEntry.AlternateNames)
                        {
                            if (AppState.UnitList.ContainsKey(x))
                            {
                                continue;
                            }
                            AppState.UnitList.Add(x, NewEntry.Unit);
                            UnitListNoPlural.Add(new ListViewItem(x));
                        }
                        foreach (string x in NewEntry.Symbols)
                        {
                            if (AppState.UnitList.ContainsKey(x))
                            {
                                continue;
                            }
                            AppState.UnitList.Add(x, NewEntry.Unit);
                            UnitListNoPlural.Add(new ListViewItem(x));
                        }
                        foreach (string x in NewEntry.Abbreviations)
                        {
                            if (AppState.UnitList.ContainsKey(x))
                            {
                                continue;
                            }
                            AppState.UnitList.Add(x, NewEntry.Unit);
                            UnitListNoPlural.Add(new ListViewItem(x));
                        }
                        foreach (string x in NewEntry.AmericanSpelling)
                        {
                            if (AppState.UnitList.ContainsKey(x))
                            {
                                continue;
                            }
                            AppState.UnitList.Add(x, NewEntry.Unit);
                            UnitListNoPlural.Add(new ListViewItem(x));
                        }
                        foreach (string x in NewEntry.Plurals)
                        {
                            if (AppState.UnitList.ContainsKey(x))
                            {
                                continue;
                            }
                            AppState.UnitList.Add(x, NewEntry.Unit);
                            AppState.UnitListPlural.Add(x);
                        }
                        //Unit list without plurals
                        AppState.UnitListNoPlural = UnitListNoPlural.ToArray();
                        //Cache unit
                        if (!AppState.UnitCache.ContainsKey(NewEntry.Type))
                        {
                            AppState.UnitCache.Add(NewEntry.Type, new Dictionary<string, Entry>());
                        }
                        AppState.UnitCache[NewEntry.Type].Add(NewEntry.Unit, NewEntry);
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
        private static bool InternetCheck()
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
        /// Gets currency data from the <see cref="AppState"/>.CurrencyAPI.
        /// </summary>
        /// <returns>
        /// A <see cref="Root"/> of the deserialized JSON.
        /// </returns>
        private static Root GetCurrencyData(string URL)
        {
            try
            {
                HttpClient Client = new HttpClient();
                HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Get, URL);
                using (HttpResponseMessage WebResponse = Client.Send(Request))
                {
                    using (StreamReader Reader = new StreamReader(WebResponse.Content.ReadAsStream()))
                    {
                        return JsonSerializer.Deserialize<Root>(Reader.ReadToEnd());
                    }
                }
            }
            catch
            {
                if (InternetCheck())
                {
                    MessageBox.Show("An error has occurred. Unable to update currencies.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("No internet connection! Unable to update currencies.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return null;
        }
        /// <summary>
        /// Updates the database file, <see cref="AppState"/>.UnitCache and 
        /// <see cref="MainWindow"/>.SearchView with new exchange rates.
        /// </summary>
        private void UpdateCurrencies()
        {
            UpdateCurrencyButton.Invoke((MethodInvoker)(() => UpdateCurrencyButton.Enabled = false));
            CurrencyUpdateText.Invoke((MethodInvoker)(() => CurrencyUpdateText.Text = "Updating currencies..."));
            //Data
            Datum[] Data1 = null;
            Datum[] Data2 = null;
            Datum[] Data = null;
            List<Datum> Currencies = new List<Datum>();
            //Get first page
            Root Response = GetCurrencyData(AppState.CurrencyAPI);
            if (Response == null) //Catch exception
            {
                CurrencyUpdateText.Invoke((MethodInvoker)(() => CurrencyUpdateText.Text = ""));
                UpdateCurrencyButton.Invoke((MethodInvoker)(() => UpdateCurrencyButton.Enabled = true));
                return;
            }
            if (Response.data.Length == 0)
            {
                AppState.APIQuarter -= 1;
                AppState.CurrencyAPI = $"https://api.fiscaldata.treasury.gov/services/api/fiscal_service/v1/accounting/od/rates_of_exchange?fields=effective_date,country,currency,exchange_rate&filter=record_calendar_year:gte:{AppState.APIYear},record_calendar_quarter:gte:{AppState.APIQuarter},page[size]=200&sort=country";
                UpdateCurrencies();
                return;
            }
            Data1 = Response.data;
            //Get next page
            if (Response.links.next != null)
            {
                Response = GetCurrencyData(AppState.CurrencyAPI + Response.links.next);
                Data2 = Response.data;
            }
            //Aggregate data
            if (Data2 != null)
            {
                Data = new Datum[Data1.Length + Data2.Length];
                Data1.CopyTo(Data, 0);
                Data2.CopyTo(Data, Data1.Length);
            }
            else
            {
                Data = Data1;
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
                    if (Currencies.Contains(Item))
                    {
                        Currencies.Remove(Item);
                    }
                    Currencies.Add(Data[i]);
                }
            }
            using (SqliteConnection Connection = new SqliteConnection($"Data Source={AppState.DatabasePath};Mode=ReadWrite"))
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
                        if (!AppState.CurrencyAlias.ContainsKey(x.country))
                        {
                            continue;
                        }
                        Currency = AppState.CurrencyAlias[x.country];
                        //Avoid duplicate updates for countries with same currency
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
                        AppState.UnitCache["Currency"][Reader.GetString(0)].SI = Reader.GetString(1);
                    }
                }
                //Update search view
                if (AppState.Unit1.Type == "Currency" || AppState.Unit2.Type == "Currency")
                {
                    if (SearchView.Items.Count > 0)
                    {
                        SearchView.Invoke(GetAddResults);
                    }
                }
            }
            if (SettingsPanel.Visible)
            {
                CurrencyUpdateText.Invoke((MethodInvoker)(() => CurrencyUpdateText.Text = "Currencies updated successfully."));
            }
            else
            {
                CurrencyUpdateText.Invoke((MethodInvoker)(() => CurrencyUpdateText.Text = ""));
            }
            UpdateCurrencyButton.Invoke((MethodInvoker)(() => UpdateCurrencyButton.Enabled = true));
        }
    }
}