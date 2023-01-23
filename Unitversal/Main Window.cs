namespace Unitversal;

public partial class MainWindow : Form
{
    //
    //————————————————————Main Window————————————————————
    //
    public MainWindow()
    {
        //Initialize main window components
        InitializeComponent();
        //Redraw when resized to avoid visual artifacts
        SetStyle(ControlStyles.ResizeRedraw, true);
        //Add autosizing column to search view width
        SearchView.Columns.Add("Result", -1, HorizontalAlignment.Center);
        //Change renderer for right click and sort menu
        RightClickMenu.Renderer = new ContextMenuRenderer();
        SortMenu.Renderer = new ContextMenuRenderer();
        AppAuthorText.Text = $"{Application.ProductName} {Application.ProductVersion}\r\n© 2023 Member Of The Reals";
    }
    //Before main windows shows
    private void MainWindow_Load(object sender, EventArgs e)
    {
        //Check and create settings file
        if (File.Exists(AppState.SettingsFile.Path))
        {
            RestoreSettings();
        }
        else
        {
            WriteSettings();
            //Default theme
            ChangeTheme("SYSTEM");
        }
        int DatabaseStatus = Database.Check(AppState.DatabasePath);
        if (DatabaseStatus == 1)
        {
            Database.GetUnits(AppState.DatabasePath);
            Database.CalculateInexactValues();
        }
        else if (DatabaseStatus == 0)
        {
            MessageBox.Show("Unable to find the units database! Please redownload the database, make sure it is placed in the same directory as the app, then restart.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
            MessageBox.Show("The units database is corrupted! Please redownload the database, make sure it is placed in the same directory as the app, then restart.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        if (Settings.UpdateCurrencies)
        {
            UpdateCurrencies();
        }
    }
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            //WS_MINIMIZEBOX style (0x00020000L) allows minimization when taskbar icon is clicked
            cp.Style |= 0x00020000;
            return cp;
        }
    }
    //Resizable main window
    protected override void WndProc(ref Message m)
    {
        const int HandleSize = 3;
        //WM_NCHITTEST message: determine what part of the window corresponds to a particular screen coordinate
        if (m.Msg == 0x84 && AppState.Resize)
        {
            base.WndProc(ref m);
            //HTCLIENT: indicates cursor is in a client area
            if ((int)m.Result == 0x1)
            {
                //Get point on screen
                Point ScreenPoint = new Point(m.LParam.ToInt32());
                //Convert screen coordinates to client coordinates
                Point ClientPoint = PointToClient(ScreenPoint);
                if (ClientPoint.Y <= HandleSize)
                {
                    if (ClientPoint.X <= HandleSize)
                    {
                        //HTTOPLEFT
                        m.Result = (IntPtr)13;
                    }
                    else if (ClientPoint.X < (Size.Width - HandleSize - 1))
                    {
                        //HTTOP
                        m.Result = (IntPtr)12;
                    }
                    else
                    {
                        //HTTOPRIGHT
                        m.Result = (IntPtr)14;
                    }
                }
                else if (ClientPoint.Y < (Size.Height - HandleSize - 1))
                {
                    if (ClientPoint.X <= HandleSize)
                    {
                        //HTLEFT
                        m.Result = (IntPtr)10;
                    }
                    else if (ClientPoint.X >= (Size.Width - HandleSize - 1))
                    {
                        //HTRIGHT
                        m.Result = (IntPtr)11;
                    }
                }
                else
                {
                    if (ClientPoint.X <= HandleSize)
                    {
                        //HTBOTTOMLEFT
                        m.Result = (IntPtr)16;
                    }
                    else if (ClientPoint.X < (Size.Width - HandleSize - 1))
                    {
                        //HTBOTTOM
                        m.Result = (IntPtr)15;
                    }
                    else
                    {
                        //HTBOTTOMRIGHT
                        m.Result = (IntPtr)17;
                    }
                }
            }
            return;
        }
        base.WndProc(ref m);
    }
    //Draggable title bar
    [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
    private extern static void ReleaseCapture();
    [DllImport("user32.DLL", EntryPoint = "SendMessage")]
    private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
    private void TitleBar_MouseDown(object sender, MouseEventArgs e)
    {
        ReleaseCapture();
        SendMessage(Handle, 0x112, 0xf012, 0);
        //Deselect widgets when clicking on empty space in window
        Title.Focus();
        SearchView.SelectedItems.Clear();
    }
    //Draggable title
    private void Title_MouseDown(object sender, MouseEventArgs e)
    {
        ReleaseCapture();
        SendMessage(Handle, 0x112, 0xf012, 0);
        //Deselect widgets when clicking on empty space in window
        Title.Focus();
        SearchView.SelectedItems.Clear();
    }
    private void CloseButton_Click(object sender, EventArgs e)
    {
        Close();
    }
    private void MaximizeButton_Click(object sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Maximized)
        {
            WindowState = FormWindowState.Normal;
        }
        else
        {
            WindowState = FormWindowState.Maximized;
        }
    }
    private void MinimizeButton_Click(object sender, EventArgs e)
    {
        WindowState = FormWindowState.Minimized;
    }
    private void MainWindow_Resize(object sender, EventArgs e)
    {
        //Prevent resizing outside taskbar
        MaximizedBounds = Screen.GetWorkingArea(this);
        //If window state changed
        if (WindowState == FormWindowState.Maximized && !Settings.Maximized)
        {
            //Change button symbol
            MaximizeButton.Text = "";
            //Resize title bar
            TitleBar.Width = Width + 2;
            TitleBar.Location = new Point(0, 0);
            //Disable resize from edge or corner of window
            AppState.Resize = false;
            Settings.Maximized = true;
        }
        else if (WindowState == FormWindowState.Normal && Settings.Maximized)
        {
            //Change button symbol
            MaximizeButton.Text = "";
            //Resize title bar
            TitleBar.Width = Width - 2;
            TitleBar.Location = new Point(1, 1);
            //Allow resize from edge or corner of window
            AppState.Resize = true;
            Settings.Maximized = false;
        }
        //Resize intepret label
        InterpretLabel.MaximumSize = new Size(SortButton.Location.X - InterpretLabel.Location.X, 15);
        if (SearchView.Items.Count > 0 && WindowState != FormWindowState.Minimized)
        {
            InterpretLabel.Text = TruncateInterpretation(AppState.Interpretation);
        }
        //Resize column width to fit longest item
        if (SearchView.Columns.Count > 0)
        {
            SearchView.Columns[0].Width = -1;
        }
        ScrollTextContents(true);
        Settings.WindowSize = Size;
    }
    private void MainWindow_LocationChanged(object sender, EventArgs e)
    {
        //Update window position settings
        Settings.WindowPosition = Location;
    }
    private void MainWindow_MouseClick(object sender, MouseEventArgs e)
    {
        //Deselect widgets when clicking on empty space in window
        Title.Focus();
        SearchView.SelectedItems.Clear();
    }
    //Save settings on close
    private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
        WriteSettings();
    }
    //
    //————————————————————Right Click Menu————————————————————
    //
    private void RightClickOpen_Click(object sender, EventArgs e)
    {
        SearchView_ItemActivate(this, EventArgs.Empty);
    }
    private void RightClickCut_Click(object sender, EventArgs e)
    {
        SearchBox.Cut();
    }
    private void RightClickCopy_Click(object sender, EventArgs e)
    {
        if (AppState.RightClickLocation == "Box")
        {
            SearchBox.Copy();
        }
        else if (AppState.RightClickLocation == "View")
        {
            var CopyText = "";
            foreach (ListViewItem x in SearchView.SelectedItems)
            {
                CopyText = CopyText + x.Text + "\n";
            }
            CopyText = CopyText.Remove(CopyText.Length - 1);
            Clipboard.SetText(CopyText);
        }
        else if (AppState.RightClickLocation == "Description")
        {
            DescriptionText.Copy();
        }
    }
    private void RightClickPaste_Click(object sender, EventArgs e)
    {
        SearchBox.Focus();
        SearchBox.Paste();
    }
    private void RightClickSelectAll_Click(object sender, EventArgs e)
    {
        if (AppState.RightClickLocation == "Box")
        {
            SearchBox.SelectAll();
            SearchBox.Focus();
        }
        else if (AppState.RightClickLocation == "View")
        {
            foreach (ListViewItem item in SearchView.Items)
            {
                item.Selected = true;
            }
        }
        else if (AppState.RightClickLocation == "Description")
        {
            DescriptionText.SelectAll();
            DescriptionText.Focus();
        }
    }
    //
    //————————————————————Search Box————————————————————
    //
    //Search box right click menu behavior
    private void SearchBox_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            //Set click location
            AppState.RightClickLocation = "Box";
            //Open is removed for search box
            RightClickOpen.Visible = false;
            //Cut is added for search box
            RightClickCut.Visible = true;
            //Paste is added for search box
            RightClickPaste.Visible = true;
            //Enable or disable cut and copy depending on selected text
            if (SearchBox.SelectedText != "")
            {
                RightClickCut.Enabled = true;
                RightClickCopy.Enabled = true;
            }
            else
            {
                RightClickCut.Enabled = false;
                RightClickCopy.Enabled = false;
            }
            //Enable or disable paste depending on clipboard content
            if (Clipboard.GetText() != "")
            {
                RightClickPaste.Enabled = true;
            }
            else
            {
                RightClickPaste.Enabled = false;
            }
        }
    }
    private void ClearSearchButton_Click(object sender, EventArgs e)
    {
        SearchBox.Clear();
    }
    //Update results when typing in search box
    private void SearchBox_TextChanged(object sender, EventArgs e)
    {
        if (SearchBox.Text.Length > 0)
        {
            ClearSearchButton.Visible = true;
        }
        else
        {
            ClearSearchButton.Visible = false;
        }
        AppState.Recognized = false; //Reset tracker
        ScrollTextContents(false);
        //Current query without leading and trailing space
        string CurrentQuery = SearchBox.Text.Trim();
        //Return if query is same as previous to reduce redundant processing
        //Checking if explore false prevents inescapable explore mode when user inputs leading or trailing spaces
        if (AppState.Explore == false && CurrentQuery.Equals(PreviousState.Query))
        {
            return;
        }
        else if (CurrentQuery.Length == 0)
        {
            ClearSearchView();
            PreviousState.Query = "";
            return;
        }
        //Explore mode
        else if (
            CurrentQuery.Equals("EXPLORE", StringComparison.OrdinalIgnoreCase)
            ||
            (!"0123456789".Contains(CurrentQuery[0]) && Search.LongestSubstring(CurrentQuery.ToUpper(), "EXPLORE") > 3)
        )
        {
            ExploreButton_Click(this, EventArgs.Empty);
            PreviousState.Query = CurrentQuery;
            return;
        }
        else
        {
            PreviousState.Query = CurrentQuery;
            string Unit1 = "";
            string Unit2 = "";
            string[] TokenizedQuery = CurrentQuery.Split(" ");
            //Remove integer and decimal group separators
            TokenizedQuery[0] = TokenizedQuery[0].Replace(Settings.IntegerGroupSeparator, "");
            TokenizedQuery[0] = TokenizedQuery[0].Replace(Settings.DecimalGroupSeparator, "");
            //Check for and get valid magnitude
            if (BigDecimal.TryParse(TokenizedQuery[0], out Calculate.Magnitude, char.Parse(Settings.DecimalSeparator)))
            {
                //Checking for "to" string
                int ToIndex = Array.FindIndex(TokenizedQuery, x => x.Equals("to", StringComparison.OrdinalIgnoreCase));
                //Get units from query
                for (int i = 1; i < TokenizedQuery.Length; i++)
                {
                    //If "to" present, get both units
                    if (ToIndex != -1)
                    {
                        if (i < ToIndex)
                        {
                            Unit1 += $" {TokenizedQuery[i]}";
                        }
                        else if (i > ToIndex)
                        {
                            Unit2 += $" {TokenizedQuery[i]}";
                        }
                    }
                    //If "to" not present, get first unit
                    else
                    {
                        Unit1 += $" {TokenizedQuery[i]}";
                    }
                }
                //Remove leading and trailing spaces
                Unit1 = Unit1.Trim();
                Unit2 = Unit2.Trim();
                if (Unit1 != "")
                {
                    //Convert multiple spaces to one
                    Unit1 = Regex.Replace(Unit1, @"\s+", " ");
                    Unit2 = Regex.Replace(Unit2, @"\s+", " ");
                    //Determine convert mode
                    Calculate.QueryType = Unit2 != "" ? "CONVERT" : "ALL";
                    //Check unit 1 recognition
                    if (Database.UnitList.ContainsKey(Unit1)) //Check if unit 1 in unit list; case sensitive exact match
                    {
                        Calculate.Unit1BestMatches.Clear();
                        Calculate.Unit1BestMatches.Add(Unit1);
                    }
                    else
                    {
                        //Get best matches of unit 1
                        Calculate.Unit1BestMatches = Search.BestMatches(Unit1);
                    }
                    if (Calculate.Unit1BestMatches.Count > 0)
                    {
                        Calculate.Unit1 = Database.GetUnitFromCache(Database.UnitList[Calculate.Unit1BestMatches[0]]);
                        Calculate.Unit1BestMatches.RemoveRange(1, Calculate.Unit1BestMatches.Count - 1); //Only the best match matters for unit 1
                        AppState.Recognized = true;
                    }
                    //Check unit 2 recognition
                    if (Calculate.QueryType == "CONVERT")
                    {
                        //Get best matches of unit 2
                        Calculate.Unit2BestMatches = Search.BestMatches(Unit2);
                        if (Calculate.Unit2BestMatches.Count > 0)
                        {
                            //For error message in case unit 1 and 2 are not compatible
                            string IncompatibleMatch = Calculate.Unit2BestMatches[0];
                            Entry IncompatibleUnit = Database.GetUnitFromCache(Database.UnitList[IncompatibleMatch]);
                            //Remove units of different type than best match
                            Calculate.Unit2BestMatches.RemoveAll(x => !Database.UnitCache[Calculate.Unit1.Type].ContainsKey(Database.UnitList[x]));
                            if (Calculate.Unit2BestMatches.Count > 0)
                            {
                                Calculate.Unit2 = Database.GetUnitFromCache(Database.UnitList[Calculate.Unit2BestMatches[0]]);
                                //Determine recognition for unit 1 and 2
                                AppState.Recognized = AppState.Recognized == false ? false : true;
                            }
                            else
                            {
                                //Incompatible types error interpretation text
                                ClearSearchView(); //Also clears interpret text
                                AppState.Interpretation = $"Cannot Convert {Calculate.Unit1.Type} to {IncompatibleUnit.Type} (";
                                //Unit 1
                                if (Calculate.Unit1.Symbols.Contains(Calculate.Unit1BestMatches[0]) || Calculate.Unit1.Abbreviations.Contains(Calculate.Unit1BestMatches[0]))
                                {
                                    AppState.Interpretation += $"{Calculate.Unit1.Unit} ({Calculate.Unit1BestMatches[0]}) to ";
                                }
                                else
                                {
                                    AppState.Interpretation += $"{Calculate.Unit1BestMatches[0]} to ";
                                }
                                //Incompatible unit
                                if (IncompatibleUnit.Symbols.Contains(IncompatibleMatch) || IncompatibleUnit.Abbreviations.Contains(IncompatibleMatch))
                                {
                                    AppState.Interpretation += $"{IncompatibleUnit.Unit} ({IncompatibleMatch}))";
                                }
                                else
                                {
                                    AppState.Interpretation += $"{IncompatibleMatch})";
                                }
                                InterpretLabel.Text = TruncateInterpretation(AppState.Interpretation);
                                return;
                            }
                        }
                        else
                        {
                            AppState.Recognized = false;
                        }
                    }
                }
            }
            //Info mode shows clickable items when no magnitude is entered
            else
            {
                //Get unit from query
                for (int i = 0; i < TokenizedQuery.Length; i++)
                {
                    Unit1 += $" {TokenizedQuery[i]}";
                }
                Unit1 = Unit1.Trim();
                //Set info mode
                Calculate.QueryType = "INFO";
                //Get best matches of unit
                Calculate.Unit1BestMatches = Search.BestMatches(Unit1);
                if (Calculate.Unit1BestMatches.Count > 0)
                {
                    //If best match is not plural, remove all plural forms. This ensures plurals only show by user query.
                    if (!Database.UnitListPlural.Contains(Calculate.Unit1BestMatches[0]))
                    {
                        Calculate.Unit1BestMatches.RemoveAll(x => Database.UnitListPlural.Contains(x));
                    }
                    if (Calculate.Unit1BestMatches.Count > 0)
                    {
                        Calculate.Unit1 = Database.GetUnitFromCache(Database.UnitList[Calculate.Unit1BestMatches[0]]);
                        AppState.Recognized = true;
                    }
                }
            }
        }
        //Process query only if recognized
        if (AppState.Recognized)
        {
            //Process only if changed to reduce slow GUI rendering
            if (
                SearchView.Items.Count == 0
                ||
                Calculate.Magnitude != PreviousState.Magnitude
                ||
                !Calculate.Unit1BestMatches.SequenceEqual(PreviousState.Unit1BestMatches)
                ||
                !Calculate.Unit2BestMatches.SequenceEqual(PreviousState.Unit2BestMatches)
                ||
                Calculate.QueryType != PreviousState.QueryType
                ||
                AppState.Explore == true
            )
            {
                PreviousState.Magnitude = Calculate.Magnitude;
                PreviousState.Unit1BestMatches = new List<string>(Calculate.Unit1BestMatches);
                PreviousState.Unit2BestMatches = new List<string>(Calculate.Unit2BestMatches);
                PreviousState.QueryType = Calculate.QueryType;
                GetAddResults();
            }
            else
            {
                UpdateInterpretation(); //Display new alternate name matches if any
            }
        }
        else
        {
            ClearSearchView(); //Also clears interpret text
            InterpretLabel.Text = "Query Not Recognized";
        }
    }
    //
    //————————————————————Interpretation Text————————————————————
    //
    private void InterpretLabel_DoubleClick(object sender, EventArgs e)
    {
        if (!AppState.Explore)
        {
            if (InterpretLabel.Text.Contains("Interpretation"))
            {
                AppState.InterpretInfo = true;
                SearchView_ItemActivate(this, EventArgs.Empty);
            }
        }
    }
    //Flat tooltip style
    private void InterpretToolTip_Draw(object sender, DrawToolTipEventArgs e)
    {
        //Draw the standard background.
        e.DrawBackground();
        //Draw the custom border.
        e.Graphics.DrawLines(AppState.SystemColorPen, new Point[] {
            new Point (0, e.Bounds.Height - 1),
            new Point (0, 0),
            new Point (e.Bounds.Width - 1, 0)
        });
        e.Graphics.DrawLines(AppState.SystemColorPen, new Point[] {
            new Point (0, e.Bounds.Height - 1),
            new Point (e.Bounds.Width - 1, e.Bounds.Height - 1),
            new Point (e.Bounds.Width - 1, 0)
        });
        //Specify custom text formatting flags.
        TextFormatFlags sf = TextFormatFlags.VerticalCenter
                           | TextFormatFlags.HorizontalCenter
                           | TextFormatFlags.NoFullWidthCharacterBreak;
        //Draw the standard text with customized formatting options.
        e.DrawText(sf);
    }
    private void InterpretLabel_MouseHover(object sender, EventArgs e)
    {
        int StringWidth = TextRenderer.MeasureText(AppState.Interpretation, InterpretLabel.Font).Width;
        int Overflow = SortButton.Location.X - InterpretLabel.Location.X;
        if (StringWidth >= Overflow)
        {
            //Using show method prevents tooltip from disappearing when cursor is stationary compared to default popup
            //+1 offset prevents mouse leave event from triggering
            InterpretToolTip.Show(AppState.Interpretation, InterpretLabel, InterpretLabel.PointToClient(new Point(Cursor.Position.X + 1, Cursor.Position.Y + 1)));
            AppState.InterpretToolTipShown = true;
        }
        else if (AppState.InterpretToolTipShown)
        {
            InterpretLabel_MouseLeave(this, EventArgs.Empty);
        }
    }
    private void InterpretLabel_MouseLeave(object sender, EventArgs e)
    {
        InterpretToolTip.Hide(InterpretLabel);
        AppState.InterpretToolTipShown = false;
    }
    private void InterpretLabel_TextChanged(object sender, EventArgs e)
    {
        if (AppState.InterpretToolTipShown)
        {
            InterpretLabel_MouseHover(this, EventArgs.Empty);
        }
    }
    //
    //————————————————————Sort Button————————————————————
    //
    private void SortButton_Click(object sender, EventArgs e)
    {
        //Show sort menu if not shown
        if (!AppState.SortShown)
        {
            AppState.SortShown = true;
            SortMenu.Show(SortButton.PointToScreen(new Point(0, SortButton.Height)));
        }
        //Hide sort menu if shown
        else
        {
            AppState.SortShown = false;
            SortMenu.Hide();
        }
    }
    private void SortMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
    {
        //Check if button was pressed when menu closed
        bool ButtonClicked = SortButton.ClientRectangle.Contains(SortButton.PointToClient(Cursor.Position));
        //Set sort shown to false if sort menu closed without button
        if (!ButtonClicked)
        {
            AppState.SortShown = false;
        }
    }
    private void SortAscending_Click(object sender, EventArgs e)
    {
        //Ensure only one item is checked
        if (SortAscending.Checked)
        {
            if (SortDescending.Checked)
            {
                SortDescending.Checked = false;
            }
            Settings.SortOrder = "ASCENDING";
        }
        else if (!SortDescending.Checked)
        {
            SortAscending.Checked = true;
        }
        //Sort results
        if (SearchView.Items.Count > 0 && Settings.SortOrder != PreviousState.SortOrder)
        {
            SortResults();
        }
        //Renew previous state
        PreviousState.SortOrder = Settings.SortOrder;
    }
    private void SortDescending_Click(object sender, EventArgs e)
    {
        //Ensure only one item is checked
        if (SortDescending.Checked)
        {
            if (SortAscending.Checked)
            {
                SortAscending.Checked = false;
            }
            Settings.SortOrder = "DESCENDING";
        }
        else if (!SortAscending.Checked)
        {
            SortDescending.Checked = true;
        }
        //Sort results
        if (SearchView.Items.Count > 0 && Settings.SortOrder != PreviousState.SortOrder)
        {
            SortResults();
        }
        //Renew previous state
        PreviousState.SortOrder = Settings.SortOrder;
    }
    private void SortUnit_Click(object sender, EventArgs e)
    {
        //Ensure only one item is checked
        if (SortUnit.Checked)
        {
            if (SortMagnitude.Checked)
            {
                SortMagnitude.Checked = false;
            }
            Settings.SortBy = "UNIT";
        }
        else if (!SortMagnitude.Checked)
        {
            SortUnit.Checked = true;
        }
        //Sort results
        if (SearchView.Items.Count > 0 && Settings.SortBy != PreviousState.SortBy)
        {
            SortResults();
        }
        //Renew previous state
        PreviousState.SortBy = Settings.SortBy;
    }
    private void SortMagnitude_Click(object sender, EventArgs e)
    {
        //Ensure only one item is checked
        if (SortMagnitude.Checked)
        {
            if (SortUnit.Checked)
            {
                SortUnit.Checked = false;
            }
            Settings.SortBy = "MAGNITUDE";
        }
        else if (!SortUnit.Checked)
        {
            SortMagnitude.Checked = true;
        }
        //Sort results
        if (SearchView.Items.Count > 0 && Settings.SortBy != PreviousState.SortBy)
        {
            SortResults();
        }
        //Renew previous state
        PreviousState.SortBy = Settings.SortBy;
    }
    //
    //————————————————————Search View————————————————————
    //
    //Search view minimum size
    private void SearchView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
        //-19 adjustment prevents horizontal scrollbar from appearing
        if (SearchView.Columns[0].Width < SearchView.Width - 19)
        {
            SearchView.Columns[0].Width = SearchView.Width - 19;
        }
    }
    //Search view right click menu behavior
    private void SearchView_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            //Track click location
            AppState.RightClickLocation = "View";
            //Open is added for search view
            RightClickOpen.Visible = true;
            //Cut is removed for search view
            RightClickCut.Visible = false;
            //Paste is removed for search view
            RightClickPaste.Visible = false;
            //Enable or disable cut or copy depending on selection
            if (SearchView.SelectedItems.Count > 0)
            {
                RightClickOpen.Enabled = true;
                RightClickCut.Enabled = true;
                RightClickCopy.Enabled = true;
            }
            else
            {
                RightClickOpen.Enabled = false;
                RightClickCut.Enabled = false;
                RightClickCopy.Enabled = false;
            }
        }
    }
    //Description text right click menu behavior
    private void DescriptionText_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            //Track click location
            AppState.RightClickLocation = "Description";
            //Open is removed for description text
            RightClickOpen.Visible = false;
            //Cut is removed for description text
            RightClickCut.Visible = false;
            //Paste is removed for description text
            RightClickPaste.Visible = false;
            //Enable or disable cut or copy depending on selection
            if (DescriptionText.SelectedText != "")
            {
                RightClickCopy.Enabled = true;
            }
            else
            {
                RightClickCopy.Enabled = false;
            }
        }
    }
    //Search view key binds
    private void SearchView_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyData.ToString() == "C, Control")
        {
            var CopyText = "";
            foreach (ListViewItem x in SearchView.SelectedItems)
            {
                CopyText = CopyText + x.Text + "\n";
            }
            CopyText = CopyText.Remove(CopyText.Length - 1);
            Clipboard.SetText(CopyText);
        }
        if (e.KeyData.ToString() == "A, Control")
        {
            foreach (ListViewItem item in SearchView.Items)
            {
                item.Selected = true;
            }
        }
    }
    //
    //————————————————————Info Display————————————————————
    //
    //Double click item for more information
    private void SearchView_ItemActivate(object sender, EventArgs e)
    {
        string Item;
        //If interpretation text was clicked
        if (AppState.InterpretInfo == true)
        {
            Item = Calculate.Unit1.Unit;
            AppState.InterpretInfo = false;
        }
        else
        {
            //Check for selected items
            if (SearchView.SelectedItems.Count > 0)
            {
                Item = SearchView.SelectedItems[0].SubItems[0].Text;
                //Remove magnitude to get unit name
                if (!AppState.Explore && Calculate.QueryType != "INFO")
                {
                    int FirstNonDigit = Search.FirstNonDigitIndex(Item, BigDecimal.Separators);
                    Item = Item.Substring(FirstNonDigit, Item.Length - FirstNonDigit).Trim();
                }
            }
            else
            {
                //If called using save button
                if (PreviousState.SelectedUnit.Length > 0)
                {
                    Item = PreviousState.SelectedUnit;
                }
                else
                {
                    return;
                }
            }
        }
        if (Database.UnitList.ContainsKey(Item))
        {
            //Check cached information to reduce slow GUI rendering
            if (
                Database.UnitList[Item] == PreviousState.SelectedUnit
                &&
                //Decimal separator needs to be updated if different
                Settings.DecimalSeparator == PreviousState.DecimalSeparator
            )
            {
                //Remove focus to keep contents at top
                Title.Focus();
                //Scroll contents to top
                DescriptionText.SelectionStart = 0;
                DescriptionText.ScrollToCaret();
                //Show cached text
                InfoDisplay.Visible = true;
                InfoDisplay.BringToFront();
            }
            else
            {
                //Get unit info from cache
                Entry Unit = Database.GetUnitFromCache(Database.UnitList[Item]);
                //Update previously selected unit
                PreviousState.SelectedUnit = Database.UnitList[Item];
                //Remove focus from display to keep contents from scrolling down
                Title.Focus();
                //Clear textboxes
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
                AlternateNames.AddRange(Unit.AmericanSpelling);
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
                PreviousState.DecimalSeparator = Settings.DecimalSeparator;
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
        }
    }
    //Description text open clicked links
    private void DescriptionText_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        System.Diagnostics.Process.Start("explorer.exe", e.LinkText);
    }
    private void ReturnButton_Click(object sender, EventArgs e)
    {
        InfoDisplay.Visible = false;
    }
    private void SettingsButton_Click(object sender, EventArgs e)
    {
        SettingsPanel.Visible = true;
        SettingsPanel.BringToFront();
    }
    //
    //————————————————————Settings Window————————————————————
    //
    private void ExploreButton_Click(object sender, EventArgs e)
    {
        CancButton_Click(this, EventArgs.Empty); //Resets unsaved settings in settings menu
        if (Database.UnitListNoPlural.Count > 0) //In case of database issues
        {
            //Check if already exploring to reduce redundant processing
            if (!AppState.Explore)
            {
                AppState.Explore = true;
                SearchView.Items.Clear(); //Clear search view
                AppState.Interpretation = "Interpretation: Explore All Units";
                InterpretLabel.Text = TruncateInterpretation(AppState.Interpretation);
                Database.SortUnitListNoPlural(Settings.SortOrder);
                //BeginUpdate and EndUpdate must be called an equal number of times.
                SearchView.BeginUpdate();
                foreach (string x in Database.UnitListNoPlural)
                {
                    SearchView.Items.Add(x);
                }
                SearchView.EndUpdate();
            }
        }
        //Close settings panel
        SettingsPanel.Visible = false;
    }
    private void UpdateCurrencyButton_Click(object sender, EventArgs e)
    {
        Thread Task = new Thread(UpdateCurrencies);
        Task.Start();
    }
    //Hides blinking caret
    [DllImport("user32.dll")]
    static extern bool HideCaret(IntPtr hWnd);
    private void AppAuthorText_GotFocus(object sender, EventArgs e)
    {
        HideCaret(AppAuthorText.Handle);
    }
    private void AboutButton_Click(object sender, EventArgs e)
    {
        AboutDisplay.Visible = true;
        AboutDisplay.BringToFront();
    }
    private void SaveButton_Click(object sender, EventArgs e)
    {
        //Decimal separator error check
        if (DecimalSeparatorEntry.Text == "")
        {
            MessageBox.Show("Decimal separator cannot be empty.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalSeparatorEntry.Text = Settings.DecimalSeparator;
            return;
        }
        else if (int.TryParse(DecimalSeparatorEntry.Text, out _))
        {
            MessageBox.Show("Decimal separator cannot be a numeral.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalSeparatorEntry.Text = Settings.DecimalSeparator;
            return;
        }
        else if (DecimalSeparatorEntry.Text == IntegerGroupSeparatorEntry.Text)
        {
            MessageBox.Show("Decimal separator cannot be the same as the integer grouping separator.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalSeparatorEntry.Text = Settings.DecimalSeparator;
            return;
        }
        else if (DecimalSeparatorEntry.Text == DecimalGroupSeparatorEntry.Text)
        {
            MessageBox.Show("Decimal separator cannot be the same as the decimal grouping separator.", "Invalid Separator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DecimalSeparatorEntry.Text = Settings.DecimalSeparator;
            return;
        }
        //Hide settings panel
        SettingsPanel.Visible = false;
        SettingsPanel.VerticalScroll.Value = 0; //Scroll to top
        //General
        Settings.RememberPosition = PositionCheckbox.Checked;
        Settings.RememberSize = SizeCheckbox.Checked;
        Settings.UpdateCurrencies = CurrencyCheckbox.Checked;
        //Conversions
        Calculate.SignificantFigures = Convert.ToInt32(SignificantFiguresEntry.Value);
        BigDecimal.DecimalSeparator = DecimalSeparatorEntry.Text;
        BigDecimal.IntegerGroupSeparator = IntegerGroupSeparatorEntry.Text;
        BigDecimal.IntegerGroupSize = Convert.ToInt32(IntegerGroupSizeEntry.Value);
        BigDecimal.DecimalGroupSeparator = DecimalGroupSeparatorEntry.Text;
        BigDecimal.DecimalGroupSize = Convert.ToInt32(DecimalGroupSizeEntry.Value);
        Settings.SignificantFigures = Calculate.SignificantFigures;
        Settings.DecimalSeparator = BigDecimal.DecimalSeparator;
        Settings.IntegerGroupSeparator = BigDecimal.IntegerGroupSeparator;
        Settings.IntegerGroupSize = BigDecimal.IntegerGroupSize;
        Settings.DecimalGroupSeparator = BigDecimal.DecimalGroupSeparator;
        Settings.DecimalGroupSize = BigDecimal.DecimalGroupSize;
        //Scientific notation
        Calculate.LargeMagnitude = new BigDecimal((BigInteger)LargeMagnitudeEntry.Value, (int)LargeExponentEntry.Value);
        Calculate.SmallMagnitude = new BigDecimal((BigInteger)SmallMagnitudeEntry.Value, (int)SmallExponentEntry.Value * -1);
        Settings.LargeMagnitude = Calculate.LargeMagnitude;
        Settings.SmallMagnitude = Calculate.SmallMagnitude;
        //Appearance
        Settings.Theme = SystemMode.Checked == true ? "SYSTEM" : LightMode.Checked == true ? "LIGHT" : "DARK";
        ChangeTheme(Settings.Theme);
        //Currency update text
        if (CurrencyUpdateText.Text == "Currencies updated successfully.") //Prevents clearing "Updating currencies..." text if not finished
        {
            CurrencyUpdateText.Text = "";
        }
        //Write to settings file
        WriteSettings();
        //Update search view
        if (SearchView.Items.Count > 0)
        {
            GetAddResults();
        }
        //Update info display
        if (InfoDisplay.Visible)
        {
            SearchView_ItemActivate(this, EventArgs.Empty);
        }
    }
    private void CancButton_Click(object sender, EventArgs e)
    {
        //Show settings panel
        SettingsPanel.Visible = false;
        SettingsPanel.VerticalScroll.Value = 0; //Scroll to top
        //General
        PositionCheckbox.Checked = Convert.ToBoolean(Settings.RememberPosition);
        SizeCheckbox.Checked = Convert.ToBoolean(Settings.RememberSize);
        CurrencyCheckbox.Checked = Convert.ToBoolean(Settings.UpdateCurrencies);
        //Converions
        SignificantFiguresEntry.Value = Settings.SignificantFigures;
        DecimalSeparatorEntry.Text = Settings.DecimalSeparator;
        IntegerGroupSeparatorEntry.Text = Settings.IntegerGroupSeparator;
        IntegerGroupSizeEntry.Value = Settings.IntegerGroupSize;
        DecimalGroupSeparatorEntry.Text = Settings.DecimalGroupSeparator;
        DecimalGroupSizeEntry.Value = Settings.DecimalGroupSize;
        //Scientific notation
        LargeMagnitudeEntry.Value = (decimal)Settings.LargeMagnitude.Mantissa;
        LargeExponentEntry.Value = Settings.LargeMagnitude.Exponent;
        SmallMagnitudeEntry.Value = (decimal)Settings.SmallMagnitude.Mantissa;
        SmallExponentEntry.Value = Settings.SmallMagnitude.Exponent * -1;
        //Appearance
        if (Settings.Theme == "LIGHT")
        {
            LightMode.Checked = true;
        }
        else if (Settings.Theme == "DARK")
        {
            DarkMode.Checked = true;
        }
        else
        {
            SystemMode.Checked = true;
        }
        //Currency update text
        if (CurrencyUpdateText.Text == "Currencies updated successfully.") //Prevents clearing "Updating currencies..." text if not finished
        {
            CurrencyUpdateText.Text = "";
        }
    }
    //
    //————————————————————About Display————————————————————
    //
    private void NoticeButton_Click(object sender, EventArgs e)
    {
        NoticeTextBox.BringToFront();
    }
    private void LicenseButton_Click(object sender, EventArgs e)
    {
        LicenseTextBox.BringToFront();
    }
    private void ChangelogButton_Click(object sender, EventArgs e)
    {
        ChangelogTextBox.BringToFront();
    }
    private void AboutReturnButton_Click(object sender, EventArgs e)
    {
        AboutDisplay.Visible = false;
    }
}
