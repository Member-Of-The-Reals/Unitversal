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
        AppAuthorText.Text = $"{Application.ProductName} {Application.ProductVersion}\r\n© 2023 Member Of The Reals";
        //Scaling for high resolution displays
        ClearSearchButton.Size = new Size(SearchBox.Height - 3, SearchBox.Height - 4);
        ClearSearchButton.Location = new Point(SearchBox.Location.X + (SearchBox.Width - ClearSearchButton.Width - 2), SearchBox.Location.Y + 2);
        SettingsButton.Height = SearchBox.Height;
        Size AuthorTextSize = TextRenderer.MeasureText(AppAuthorText.Text, AppAuthorText.Font);
        AppAuthorText.Width = AuthorTextSize.Width;
        AppAuthorText.Height = AuthorTextSize.Height;
        //Redraw when resized to avoid visual artifacts
        SetStyle(ControlStyles.ResizeRedraw, true);
        //Change renderer for right click and sort menu
        RightClickMenu.Renderer = new ContextMenuRenderer();
        SortMenu.Renderer = new ContextMenuRenderer();
        RightClickMenu.PerformLayout();
        SortMenu.PerformLayout();
        //Add autosizing width columns to list views
        SearchView.Columns.Add("Result", -1, HorizontalAlignment.Center);
        AllView.Columns.Add("Result", -1, HorizontalAlignment.Center);
        CategoryView.Columns.Add("Result", -1, HorizontalAlignment.Center);
        BinaryView.Columns.Add("Result", -1, HorizontalAlignment.Center);
        SIView.Columns.Add("Result", -1, HorizontalAlignment.Center);
        EquivalentsView.Columns.Add("Result", -1, HorizontalAlignment.Center);
        TemperatureView.Columns.Add("Result", -1, HorizontalAlignment.Center);
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
        if (DatabaseStatus == 0)
        {
            MessageBox.Show(
                "Unable to find the units database! Please redownload the database, make sure it is placed in the same directory as the app, then restart.",
                "Database Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );
        }
        else
        {
            if (DatabaseStatus == -1)
            {
                MessageBox.Show(
                    "The units database is corrupted! Please redownload the database, make sure it is placed in the same directory as the app, then restart.",
                    "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
            }
            //Fault tolerance; enable as many features regardless of database corruption
            Database.GetAllUnits(AppState.DatabasePath);
            Database.GetBinaryPrefixes(AppState.DatabasePath);
            Database.GetCaseSensitive(AppState.DatabasePath);
            Database.GetCurrencyAliases(AppState.DatabasePath);
            Database.GetSIEquivalents(AppState.DatabasePath);
            Database.GetSIPrefixes(AppState.DatabasePath);
            Database.GetSpecialUnits(AppState.DatabasePath);
            Database.GetTemperatureFormulas(AppState.DatabasePath);
            Database.CalculateInexactValues();
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
        if (WindowState != FormWindowState.Minimized)
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
            //Resize interpret label
            if (SearchView.Items.Count > 0)
            {
                SetLabel(InterpretLabel, SortButton.Location.X - InterpretLabel.Location.X, AppState.Interpretation);
            }
            //Resize explorer label
            if (AppState.Explore)
            {
                SetLabel(ExplorerLabel, ExploreSort.Location.X - ExplorerLabel.Location.X, AppState.CurrentView);
            }
            //Resize column width to fit longest item
            if (SearchView.Columns.Count > 0)
            {
                SearchView.Columns[0].Width = -1;
            }
            if (AllView.Columns.Count > 0)
            {
                AllView.Columns[0].Width = -1;
            }
            if (CategoryView.Columns.Count > 0)
            {
                CategoryView.Columns[0].Width = -1;
            }
            if (BinaryView.Columns.Count > 0)
            {
                BinaryView.Columns[0].Width = -1;
            }
            if (SIView.Columns.Count > 0)
            {
                SIView.Columns[0].Width = -1;
            }
            if (EquivalentsView.Columns.Count > 0)
            {
                EquivalentsView.Columns[0].Width = -1;
            }
            if (TemperatureView.Columns.Count > 0)
            {
                TemperatureView.Columns[0].Width = -1;
            }
            ScrollTextContents();
            Settings.WindowSize = Size;
        }
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
        switch (AppState.RightClickLocation)
        {
            case "SearchView":
                SearchView_ItemActivate(this, EventArgs.Empty);
                break;
            case "AllView":
                AllView_ItemActivate(this, EventArgs.Empty);
                break;
            case "CategoryView":
                CategoryView_ItemActivate(this, EventArgs.Empty);
                break;
            case "BinaryView":
                BinaryView_ItemActivate(this, EventArgs.Empty);
                break;
            case "SIView":
                SIView_ItemActivate(this, EventArgs.Empty);
                break;
            case "EquivalentsView":
                EquivalentsView_ItemActivate(this, EventArgs.Empty);
                break;
            case "TemperatureView":
                TemperatureView_ItemActivate(this, EventArgs.Empty);
                break;
        }
    }
    private void RightClickCut_Click(object sender, EventArgs e)
    {
        SearchBox.Cut();
    }
    private void RightClickCopy_Click(object sender, EventArgs e)
    {
        switch (AppState.RightClickLocation)
        {
            case "SearchBox":
                SearchBox.Copy();
                break;
            case "DescriptionText":
                DescriptionText.Copy();
                break;
            default:
                ListViewCopy(Controls.Find(AppState.RightClickLocation, true)[0] as ListView);
                break;
        }
    }
    private void RightClickPaste_Click(object sender, EventArgs e)
    {
        SearchBox.Focus();
        SearchBox.Paste();
    }
    private void RightClickSelectAll_Click(object sender, EventArgs e)
    {
        switch (AppState.RightClickLocation)
        {
            case "SearchBox":
                SearchBox.Focus();
                SearchBox.SelectAll();
                break;
            case "DescriptionText":
                DescriptionText.Focus();
                DescriptionText.SelectAll();
                break;
            default:
                ListViewSelectAll(Controls.Find(AppState.RightClickLocation, true)[0] as ListView);
                break;
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
            AppState.RightClickLocation = "SearchBox";
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
            SearchBox.PadSize = ClearSearchButton.Width + 1; //Prevent clear button from blocking text
            SearchBox.SetMargin();
        }
        else
        {
            ClearSearchButton.Visible = false;
            SearchBox.PadSize = 0;
            SearchBox.SetMargin();
        }
        AppState.Recognized = false;
        ScrollTextContents();
        string CurrentQuery = SearchBox.Text.Trim();
        if (CurrentQuery.Length == 0)
        {
            ClearSearchView();
            return;
        }
        else if (CurrentQuery.Equals("ABOUT", StringComparison.OrdinalIgnoreCase))
        {
            SearchBox.Clear();
            AboutButton_Click(this, EventArgs.Empty);
            return;
        }
        else if (CurrentQuery.Equals("EXPLORE", StringComparison.OrdinalIgnoreCase))
        {
            SearchBox.Clear();
            ExploreButton_Click(this, EventArgs.Empty);
            return;
        }
        else if (CurrentQuery.Equals("SETTINGS", StringComparison.OrdinalIgnoreCase))
        {
            SearchBox.Clear();
            SettingsButton_Click(this, EventArgs.Empty);
            return;
        }
        else
        {
            string Unit1 = "";
            string Unit2 = "";
            int UnitsIndex = Search.UnitNameIndex(CurrentQuery, BigDecimal.Separators);
            UnitsIndex = UnitsIndex == -1 ? 0 : UnitsIndex;
            string Magnitude = CurrentQuery.Substring(0, UnitsIndex).Trim();
            string[] TokenizedQuery = CurrentQuery.Substring(UnitsIndex).Split(" ");
            //Remove integer and decimal group separators from magnitude
            Magnitude = Magnitude.Replace(Settings.IntegerGroupSeparator, "");
            Magnitude = Magnitude.Replace(Settings.DecimalGroupSeparator, "");
            //Check for and get valid magnitude
            if (BigDecimal.TryParse(Magnitude, out Calculate.Magnitude, char.Parse(Settings.DecimalSeparator)))
            {
                int ToIndex = -1;
                //Find all "to" strings
                List<int> ToStrings = new List<int>();
                for (int i = 1; i < TokenizedQuery.Length; i++)
                {
                    if (i != ToIndex && TokenizedQuery[i].Equals("to", StringComparison.OrdinalIgnoreCase))
                    {
                        ToIndex = i;
                        ToStrings.Add(i);
                    }
                }
                //Get middle "to" string
                ToIndex = ToStrings.Count == 0 ? -1 : ToStrings[ToStrings.Count / 2];
                //Get units from query
                if (ToIndex != -1)
                {
                    Unit1 = string.Join(" ", TokenizedQuery, 0, ToIndex);
                    Unit2 = string.Join(" ", TokenizedQuery, ToIndex + 1, TokenizedQuery.Length - ToIndex - 1);
                }
                else
                {
                    Unit1 = string.Join(" ", TokenizedQuery);
                }
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
                        AppState.Recognized = true;
                    }
                    //Check unit 2 recognition
                    if (Calculate.QueryType == "CONVERT" && AppState.Recognized)
                    {
                        //Get best matches of unit 2
                        Calculate.Unit2BestMatches = Search.BestMatches(Unit2, Calculate.Unit1.Type);
                        if (Calculate.Unit2BestMatches.Count > 0)
                        {
                            Calculate.Unit2 = Database.GetUnitFromCache(Database.UnitList[Calculate.Unit2BestMatches[0]]);
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
                Unit1 = string.Join(" ", TokenizedQuery);
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
                AppState.Explore //Breaks out of explore
            )
            {
                PreviousState.Magnitude = Calculate.Magnitude;
                PreviousState.Unit1BestMatches = new List<string>(Calculate.Unit1BestMatches);
                PreviousState.Unit2BestMatches = new List<string>(Calculate.Unit2BestMatches);
                PreviousState.QueryType = Calculate.QueryType;
                UpdateResults();
            }
            //Display new alternate name matches if any
            else
            {
                UpdateInterpretation();
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
            ShowUnitInfo(Calculate.Unit1.Unit);
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
            //Following items are disabled for explorer sort
            SortSeparator.Visible = true;
            SortUnit.Visible = true;
            SortMagnitude.Visible = true;
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
        bool SortButtonClicked = SortButton.ClientRectangle.Contains(SortButton.PointToClient(Cursor.Position));
        bool ExploreButtonClicked = ExploreSort.ClientRectangle.Contains(ExploreSort.PointToClient(Cursor.Position));
        //Set sort shown to false if sort menu closed without button
        if (!SortButtonClicked)
        {
            AppState.SortShown = false;
        }
        if (!ExploreButtonClicked)
        {
            AppState.ExploreSortShown = false;
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
        if (AppState.Explore)
        {
            LoadExplorer();
        }
        else if (SearchView.Items.Count > 0 && Settings.SortOrder != PreviousState.SortOrder)
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
        if (AppState.Explore)
        {
            LoadExplorer();
        }
        else if (SearchView.Items.Count > 0 && Settings.SortOrder != PreviousState.SortOrder)
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
    //Search view minimum column width
    private void SearchView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
        //-19 adjustment prevents horizontal scrollbar from appearing and accounts for width of vertical scrollbar
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
            AppState.RightClickLocation = "SearchView";
            //Open is added for list view
            RightClickOpen.Visible = true;
            //Cut is removed for list view
            RightClickCut.Visible = false;
            //Paste is removed for list view
            RightClickPaste.Visible = false;
            //Enable or disable open and copy depending on selection
            if (SearchView.SelectedItems.Count > 0)
            {
                RightClickOpen.Enabled = true;
                RightClickCopy.Enabled = true;
            }
            else
            {
                RightClickOpen.Enabled = false;
                RightClickCopy.Enabled = false;
            }
        }
    }
    //Search view key binds
    private void SearchView_KeyDown(object sender, KeyEventArgs e)
    {
        string KeysPressed = e.KeyData.ToString();
        if (KeysPressed == "C, Control")
        {
            ListViewCopy(SearchView);
        }
        if (KeysPressed == "A, Control")
        {
            ListViewSelectAll(SearchView);
        }
    }
    //Double click item for more information
    private void SearchView_ItemActivate(object sender, EventArgs e)
    {
        string Item;
        if (SearchView.SelectedItems.Count > 0)
        {
            if (Calculate.QueryType == "INFO")
            {
                Item = SearchView.SelectedItems[0].Text;
            }
            else
            {
                Item = Calculate.Results[SearchView.Items.IndexOf(SearchView.SelectedItems[0])].Item2;
                Item = Database.UnitList[Item];
            }
            if (Database.UnitList.ContainsKey(Item))
            {
                ShowUnitInfo(Item);
            }
            else if (Database.SIPrefixes.ContainsKey(Item) || Database.BinaryPrefixes.ContainsKey(Item))
            {
                ShowPrefixInfo(Item);
            }
        }
    }
    //
    //————————————————————Info Display————————————————————
    //
    //Description text right click menu behavior
    private void DescriptionText_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            //Track click location
            AppState.RightClickLocation = "DescriptionText";
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
    //Description text open clicked links
    private void DescriptionText_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        System.Diagnostics.Process.Start("explorer.exe", e.LinkText);
    }
    private void InfoCloseButton_Click(object sender, EventArgs e)
    {
        InfoDisplay.Visible = false;
        if (AppState.Explore)
        {
            Explorer.Visible = true;
        }
    }
    //
    //————————————————————Settings Window————————————————————
    //
    private void SettingsButton_Click(object sender, EventArgs e)
    {
        SettingsPanel.Visible = true;
        SettingsPanel.Focus();
    }
    private void ExploreButton_Click(object sender, EventArgs e)
    {
        CancButton_Click(this, EventArgs.Empty);
        AppState.Explore = true;
        Explorer.Visible = true;
        Explorer.Panel1.Focus();
        AppState.CurrentView = "All Units";
        LoadExplorer();
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
        CancButton_Click(this, EventArgs.Empty);
        AboutDisplay.Visible = true;
        AboutDisplay.Focus();
    }
    private void SaveButton_Click(object sender, EventArgs e)
    {
        if (!SeparatorCheck())
        {
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
            UpdateResults();
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
    private void AboutCloseButton_Click(object sender, EventArgs e)
    {
        AboutDisplay.Visible = false;
    }
    //
    //————————————————————Explorer————————————————————
    //
    private void ExploreMenuButton_Click(object sender, EventArgs e)
    {
        if (Explorer.SplitterDistance != ExploreMenuButton.Width)
        {
            Explorer.SplitterDistance = ExploreMenuButton.Width;
            AllButton.Visible = false;
            CategoryButton.Visible = false;
            BinaryButton.Visible = false;
            SIButton.Visible = false;
            EquivalentsButton.Visible = false;
            TemperatureButton.Visible = false;
            BackButton.Visible = false;
            ExitButton.Visible = false;
        }
        else
        {
            Explorer.SplitterDistance = (int)(TextRenderer.MeasureText(TemperatureButton.Text, TemperatureButton.Font).Width * 1.2);
            AllButton.Visible = true;
            CategoryButton.Visible = true;
            BinaryButton.Visible = true;
            SIButton.Visible = true;
            EquivalentsButton.Visible = true;
            TemperatureButton.Visible = true;
            ExitButton.Visible = true;
            if (
                (AppState.SecondMenu != "" && Explorer.Panel2.Controls.GetChildIndex(CategoryView) == 0)
                ||
                (AppState.FirstUnit != "" && Explorer.Panel2.Controls.GetChildIndex(TemperatureView) == 0)
            )
            {
                BackButton.Visible = true;
            }
        }
        //Resize column width to fit longest item
        foreach (ListView View in new ListView[] { AllView, CategoryView, BinaryView, SIView, EquivalentsView, TemperatureView })
        {
            View.BeginUpdate();
            View.Columns[0].Width = -1;
            View.EndUpdate();
        }
        SetLabel(ExplorerLabel, ExploreSort.Location.X - ExplorerLabel.Location.X, AppState.CurrentView);
    }
    //Manages width of buttons when window resizes
    private void Explorer_Resize(object sender, EventArgs e)
    {
        int Width = (int)(TextRenderer.MeasureText(TemperatureButton.Text, TemperatureButton.Font).Width * 1.2);
        Explorer.SplitterDistance = Width;
        AllButton.Width = Width;
        CategoryButton.Width = Width;
        BinaryButton.Width = Width;
        SIButton.Width = Width;
        EquivalentsButton.Width = Width;
        TemperatureButton.Width = Width;
        BackButton.Width = Width;
        ExitButton.Width = Width;
    }
    private void AllButton_Click(object sender, EventArgs e)
    {
        AllView.BringToFront();
        ExplorerLabel.Text = "All Units";
        AppState.CurrentView = ExplorerLabel.Text;
        BackButton.Visible = false;
    }
    private void CategoryButton_Click(object sender, EventArgs e)
    {
        CategoryView.BringToFront();
        AppState.CurrentView = AppState.SecondMenu == "" ? "Units By Category" : $"{AppState.SecondMenu} Units";
        SetLabel(ExplorerLabel, ExploreSort.Location.X - ExplorerLabel.Location.X, AppState.CurrentView);
        if (AppState.SecondMenu != "" && Explorer.Panel2.Controls.GetChildIndex(CategoryView) == 0)
        {
            BackButton.Visible = true;
            return;
        }
        BackButton.Visible = false;
    }
    private void BinaryButton_Click(object sender, EventArgs e)
    {
        BinaryView.BringToFront();
        ExplorerLabel.Text = "Binary Prefixes";
        AppState.CurrentView = ExplorerLabel.Text;
        BackButton.Visible = false;
    }
    private void SIButton_Click(object sender, EventArgs e)
    {
        SIView.BringToFront();
        ExplorerLabel.Text = "SI Prefixes";
        AppState.CurrentView = ExplorerLabel.Text;
        BackButton.Visible = false;
    }
    private void EquivalentsButton_Click(object sender, EventArgs e)
    {
        EquivalentsView.BringToFront();
        ExplorerLabel.Text = "SI Equivalents";
        AppState.CurrentView = ExplorerLabel.Text;
        BackButton.Visible = false;
    }
    private void TemperatureButton_Click(object sender, EventArgs e)
    {
        TemperatureView.BringToFront();
        ExplorerLabel.Text = AppState.FirstUnit == "" ? "Temperature Formulas" : $"{AppState.FirstUnit} Formulas";
        AppState.CurrentView = ExplorerLabel.Text;
        if (AppState.FirstUnit != "" && Explorer.Panel2.Controls.GetChildIndex(TemperatureView) == 0)
        {
            BackButton.Visible = true;
            return;
        }
        BackButton.Visible = false;
    }
    private void BackButton_Click(object sender, EventArgs e)
    {
        if (Explorer.Panel2.Controls.GetChildIndex(CategoryView) == 0)
        {
            AppState.SecondMenu = "";
            ExplorerLabel.Text = "Units By Category";
        }
        else if (Explorer.Panel2.Controls.GetChildIndex(TemperatureView) == 0)
        {
            AppState.FirstUnit = "";
            ExplorerLabel.Text = "Temperature Formulas";
        }
        AppState.CurrentView = ExplorerLabel.Text;
        LoadExplorer();
        PreviousState.SecondMenu = AppState.SecondMenu;
        PreviousState.FirstUnit = AppState.FirstUnit;
        BackButton.Visible = false;
    }
    private void ExitButton_Click(object sender, EventArgs e)
    {
        Explorer.Visible = false;
        AppState.Explore = false;
    }
    //Sort button
    private void ExploreSort_Click(object sender, EventArgs e)
    {
        //Show sort menu if not shown
        if (!AppState.ExploreSortShown)
        {
            AppState.ExploreSortShown = true;
            SortSeparator.Visible = false;
            SortUnit.Visible = false;
            SortMagnitude.Visible = false;
            SortMenu.Show(ExploreSort.PointToScreen(new Point(0, ExploreSort.Height)));
        }
        //Hide sort menu if shown
        else
        {
            AppState.ExploreSortShown = false;
            SortMenu.Hide();
        }
    }
    //Minimum column width for list views
    private void AllView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
        //-19 adjustment prevents horizontal scrollbar from appearing and accounts for width of vertical scrollbar
        if (AllView.Columns[0].Width < AllView.Width - 19)
        {
            AllView.Columns[0].Width = AllView.Width - 19;
        }
    }
    private void CategoryView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
        //-19 adjustment prevents horizontal scrollbar from appearing and accounts for width of vertical scrollbar
        if (CategoryView.Columns[0].Width < CategoryView.Width - 19)
        {
            CategoryView.Columns[0].Width = CategoryView.Width - 19;
        }
    }
    private void BinaryView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
        //-19 adjustment prevents horizontal scrollbar from appearing and accounts for width of vertical scrollbar
        if (BinaryView.Columns[0].Width < BinaryView.Width - 19)
        {
            BinaryView.Columns[0].Width = BinaryView.Width - 19;
        }
    }
    private void SIView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
        //-19 adjustment prevents horizontal scrollbar from appearing and accounts for width of vertical scrollbar
        if (SIView.Columns[0].Width < SIView.Width - 19)
        {
            SIView.Columns[0].Width = SIView.Width - 19;
        }
    }
    private void EquivalentsView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
        //-19 adjustment prevents horizontal scrollbar from appearing and accounts for width of vertical scrollbar
        if (EquivalentsView.Columns[0].Width < EquivalentsView.Width - 19)
        {
            EquivalentsView.Columns[0].Width = EquivalentsView.Width - 19;
        }
    }
    private void TemperatureView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
        //-19 adjustment prevents horizontal scrollbar from appearing and accounts for width of vertical scrollbar
        if (TemperatureView.Columns[0].Width < TemperatureView.Width - 19)
        {
            TemperatureView.Columns[0].Width = TemperatureView.Width - 19;
        }
    }
    //List view key binds
    private void AllView_KeyDown(object sender, KeyEventArgs e)
    {
        string KeysPressed = e.KeyData.ToString();
        if (KeysPressed == "C, Control")
        {
            ListViewCopy(AllView);
        }
        if (KeysPressed == "A, Control")
        {
            ListViewSelectAll(AllView);
        }
    }
    private void CategoryView_KeyDown(object sender, KeyEventArgs e)
    {
        string KeysPressed = e.KeyData.ToString();
        if (KeysPressed == "C, Control")
        {
            ListViewCopy(CategoryView);
        }
        if (KeysPressed == "A, Control")
        {
            ListViewSelectAll(CategoryView);
        }
    }
    private void BinaryView_KeyDown(object sender, KeyEventArgs e)
    {
        string KeysPressed = e.KeyData.ToString();
        if (KeysPressed == "C, Control")
        {
            ListViewCopy(BinaryView);
        }
        if (KeysPressed == "A, Control")
        {
            ListViewSelectAll(BinaryView);
        }
    }
    private void SIView_KeyDown(object sender, KeyEventArgs e)
    {
        string KeysPressed = e.KeyData.ToString();
        if (KeysPressed == "C, Control")
        {
            ListViewCopy(SIView);
        }
        if (KeysPressed == "A, Control")
        {
            ListViewSelectAll(SIView);
        }
    }
    private void EquivalentsView_KeyDown(object sender, KeyEventArgs e)
    {
        string KeysPressed = e.KeyData.ToString();
        if (KeysPressed == "C, Control")
        {
            ListViewCopy(EquivalentsView);
        }
        if (KeysPressed == "A, Control")
        {
            ListViewSelectAll(EquivalentsView);
        }
    }
    private void TemperatureView_KeyDown(object sender, KeyEventArgs e)
    {
        string KeysPressed = e.KeyData.ToString();
        if (KeysPressed == "C, Control")
        {
            ListViewCopy(TemperatureView);
        }
        if (KeysPressed == "A, Control")
        {
            ListViewSelectAll(TemperatureView);
        }
    }
    //List view right click menu
    private void AllView_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            //Track click location
            AppState.RightClickLocation = "AllView";
            //Open is added for list view
            RightClickOpen.Visible = true;
            //Cut is removed for list view
            RightClickCut.Visible = false;
            //Paste is removed for list view
            RightClickPaste.Visible = false;
            //Enable or disable open and copy depending on selection
            if (AllView.SelectedItems.Count > 0)
            {
                RightClickOpen.Enabled = true;
                RightClickCopy.Enabled = true;
            }
            else
            {
                RightClickOpen.Enabled = false;
                RightClickCopy.Enabled = false;
            }
        }
    }
    private void CategoryView_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            //Track click location
            AppState.RightClickLocation = "CategoryView";
            //Open is added for list view
            RightClickOpen.Visible = true;
            //Cut is removed for list view
            RightClickCut.Visible = false;
            //Paste is removed for list view
            RightClickPaste.Visible = false;
            //Enable or disable open and copy depending on selection
            if (CategoryView.SelectedItems.Count > 0)
            {
                RightClickOpen.Enabled = true;
                RightClickCopy.Enabled = true;
            }
            else
            {
                RightClickOpen.Enabled = false;
                RightClickCopy.Enabled = false;
            }
        }
    }
    private void BinaryView_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            //Track click location
            AppState.RightClickLocation = "BinaryView";
            //Open is added for list view
            RightClickOpen.Visible = true;
            //Cut is removed for list view
            RightClickCut.Visible = false;
            //Paste is removed for list view
            RightClickPaste.Visible = false;
            //Enable or disable open and copy depending on selection
            if (BinaryView.SelectedItems.Count > 0)
            {
                RightClickOpen.Enabled = true;
                RightClickCopy.Enabled = true;
            }
            else
            {
                RightClickOpen.Enabled = false;
                RightClickCopy.Enabled = false;
            }
        }
    }
    private void SIView_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            //Track click location
            AppState.RightClickLocation = "SIView";
            //Open is added for list view
            RightClickOpen.Visible = true;
            //Cut is removed for list view
            RightClickCut.Visible = false;
            //Paste is removed for list view
            RightClickPaste.Visible = false;
            //Enable or disable open and copy depending on selection
            if (SIView.SelectedItems.Count > 0)
            {
                RightClickOpen.Enabled = true;
                RightClickCopy.Enabled = true;
            }
            else
            {
                RightClickOpen.Enabled = false;
                RightClickCopy.Enabled = false;
            }
        }
    }
    private void EquivalentsView_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            //Track click location
            AppState.RightClickLocation = "EquivalentsView";
            //Open is added for list view
            RightClickOpen.Visible = true;
            //Cut is removed for list view
            RightClickCut.Visible = false;
            //Paste is removed for list view
            RightClickPaste.Visible = false;
            //Enable or disable open and copy depending on selection
            if (EquivalentsView.SelectedItems.Count > 0)
            {
                RightClickOpen.Enabled = true;
                RightClickCopy.Enabled = true;
            }
            else
            {
                RightClickOpen.Enabled = false;
                RightClickCopy.Enabled = false;
            }
        }
    }
    private void TemperatureView_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            //Track click location
            AppState.RightClickLocation = "TemperatureView";
            //Open is added for list view
            RightClickOpen.Visible = true;
            //Cut is removed for list view
            RightClickCut.Visible = false;
            //Paste is removed for list view
            RightClickPaste.Visible = false;
            //Enable or disable open and copy depending on selection
            if (TemperatureView.SelectedItems.Count > 0)
            {
                RightClickOpen.Enabled = true;
                RightClickCopy.Enabled = true;
            }
            else
            {
                RightClickOpen.Enabled = false;
                RightClickCopy.Enabled = false;
            }
        }
    }
    //List view item activate
    private void AllView_ItemActivate(object sender, EventArgs e)
    {
        ShowUnitInfo(AllView.SelectedItems[0].Text);
        Explorer.Visible = false;
    }
    private void CategoryView_ItemActivate(object sender, EventArgs e)
    {
        if (AppState.SecondMenu == "")
        {
            BackButton.Visible = true;
            AppState.SecondMenu = CategoryView.SelectedItems[0].Text;
            AppState.CurrentView = $"{AppState.SecondMenu} Units";
            SetLabel(ExplorerLabel, ExploreSort.Location.X - ExplorerLabel.Location.X, AppState.CurrentView);
            LoadExplorer();
            PreviousState.SecondMenu = AppState.SecondMenu;
        }
        else
        {
            ShowUnitInfo(CategoryView.SelectedItems[0].Text);
            Explorer.Visible = false;
        }
    }
    private void BinaryView_ItemActivate(object sender, EventArgs e)
    {
        ShowPrefixInfo(BinaryView.SelectedItems[0].Text);
        Explorer.Visible = false;
    }
    private void SIView_ItemActivate(object sender, EventArgs e)
    {
        ShowPrefixInfo(SIView.SelectedItems[0].Text);
        Explorer.Visible = false;
    }
    private void EquivalentsView_ItemActivate(object sender, EventArgs e)
    {
        ShowUnitInfo(Database.SIEquivalents[EquivalentsView.SelectedItems[0].Text]);
        Explorer.Visible = false;
    }
    private void TemperatureView_ItemActivate(object sender, EventArgs e)
    {
        if (AppState.FirstUnit == "")
        {
            BackButton.Visible = true;
            AppState.FirstUnit = TemperatureView.SelectedItems[0].Text;
            ExplorerLabel.Text = $"{AppState.FirstUnit} Formulas";
            AppState.CurrentView = ExplorerLabel.Text;
            LoadExplorer();
            PreviousState.FirstUnit = AppState.FirstUnit;
        }
        else
        {
            ShowTemperatureFormula(AppState.FirstUnit, TemperatureView.SelectedItems[0].Text);
            Explorer.Visible = false;
        }
    }
}
