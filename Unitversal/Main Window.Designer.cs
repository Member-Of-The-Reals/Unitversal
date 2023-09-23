namespace Unitversal;

//Disables automatically copying text when double clicking labels
public class LabelNoCopy : Label
{
    //It appears that double click copy relies on base.Text. The fix is to use a different variable.
    private string text;
    public override string Text
    {
        get => text;
        set
        {
            text = value;
            OnTextChanged(EventArgs.Empty);
        }
    }
}
//Text box with custom inner padding
//Fixes issue with button in text box blocking text
public class TextBoxPadded : TextBox
{
    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);
    private const int EM_SETMARGINS = 0xd3;
    private const int EC_RIGHTMARGIN = 2;
    public int PadSize = 0;
    public TextBoxPadded() : base()
    {
    }
    public void SetMargin()
    {
        SendMessage(Handle, EM_SETMARGINS, EC_RIGHTMARGIN, PadSize << 16);
    }
}
partial class MainWindow
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
        CloseButton = new Button();
        MaximizeButton = new Button();
        MinimizeButton = new Button();
        Title = new LabelNoCopy();
        TitleBar = new Panel();
        SearchBox = new TextBoxPadded();
        RightClickMenu = new ContextMenuStrip(components);
        RightClickOpen = new ToolStripMenuItem();
        RightClickCut = new ToolStripMenuItem();
        RightClickCopy = new ToolStripMenuItem();
        RightClickPaste = new ToolStripMenuItem();
        RightClickSelectAll = new ToolStripMenuItem();
        ClearSearchButton = new Button();
        InterpretLabel = new LabelNoCopy();
        InterpretToolTip = new ToolTip(components);
        SettingsButton = new Button();
        SortButton = new Button();
        SortMenu = new ContextMenuStrip(components);
        SortAscending = new ToolStripMenuItem();
        SortDescending = new ToolStripMenuItem();
        SortSeparator = new ToolStripSeparator();
        SortUnit = new ToolStripMenuItem();
        SortMagnitude = new ToolStripMenuItem();
        SearchView = new ListView();
        InfoDisplay = new Panel();
        DescriptionText = new RichTextBox();
        InfoCloseButton = new Button();
        SettingsPanel = new Panel();
        SettingsLabel = new LabelNoCopy();
        GeneralSettingsLabel = new LabelNoCopy();
        PositionCheckbox = new CheckBox();
        SizeCheckbox = new CheckBox();
        CurrencyCheckbox = new CheckBox();
        ConversionSettingsLabel = new LabelNoCopy();
        SignificantFiguresLabel = new LabelNoCopy();
        SignificantFiguresEntry = new NumericUpDown();
        DecimalSeparatorLabel = new LabelNoCopy();
        DecimalSeparatorEntry = new TextBox();
        IntegerGroupSeparatorLabel = new LabelNoCopy();
        IntegerGroupSeparatorEntry = new TextBox();
        IntegerGroupSizeLabel = new LabelNoCopy();
        IntegerGroupSizeEntry = new NumericUpDown();
        DecimalGroupSeparatorLabel = new LabelNoCopy();
        DecimalGroupSeparatorEntry = new TextBox();
        DecimalGroupSizeLabel = new LabelNoCopy();
        DecimalGroupSizeEntry = new NumericUpDown();
        ScientificNotationLabel = new LabelNoCopy();
        LargeMagnitudeLabel = new LabelNoCopy();
        LargeMagnitudeEntry = new NumericUpDown();
        LargeExponentLabel = new LabelNoCopy();
        LargeExponentEntry = new NumericUpDown();
        SmallMagnitudeLabel = new LabelNoCopy();
        SmallMagnitudeEntry = new NumericUpDown();
        SmallExponentLabel = new LabelNoCopy();
        SmallExponentEntry = new NumericUpDown();
        AppearLabel = new LabelNoCopy();
        LightMode = new RadioButton();
        DarkMode = new RadioButton();
        SystemMode = new RadioButton();
        HelpLabel = new LabelNoCopy();
        ExploreButton = new Button();
        UpdateCurrencyButton = new Button();
        CurrencyUpdateText = new LabelNoCopy();
        AboutLabel = new LabelNoCopy();
        AppAuthorText = new TextBox();
        AboutButton = new Button();
        SaveButton = new Button();
        CancButton = new Button();
        AboutDisplay = new Panel();
        NoticeButton = new Button();
        NoticeTextBox = new TextBox();
        LicenseButton = new Button();
        LicenseTextBox = new TextBox();
        ChangelogButton = new Button();
        ChangelogTextBox = new TextBox();
        AboutCloseButton = new Button();
        Explorer = new SplitContainer();
        ExploreMenuButton = new Button();
        AllButton = new Button();
        CategoryButton = new Button();
        BinaryButton = new Button();
        SIButton = new Button();
        EquivalentsButton = new Button();
        TemperatureButton = new Button();
        BackButton = new Button();
        ExitButton = new Button();
        ExplorerLabel = new LabelNoCopy();
        ExploreSort = new Button();
        AllView = new ListView();
        CategoryView = new ListView();
        BinaryView = new ListView();
        SIView = new ListView();
        EquivalentsView = new ListView();
        TemperatureView = new ListView();
        TitleBar.SuspendLayout();
        RightClickMenu.SuspendLayout();
        SortMenu.SuspendLayout();
        InfoDisplay.SuspendLayout();
        SettingsPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)SignificantFiguresEntry).BeginInit();
        ((System.ComponentModel.ISupportInitialize)IntegerGroupSizeEntry).BeginInit();
        ((System.ComponentModel.ISupportInitialize)DecimalGroupSizeEntry).BeginInit();
        ((System.ComponentModel.ISupportInitialize)LargeMagnitudeEntry).BeginInit();
        ((System.ComponentModel.ISupportInitialize)LargeExponentEntry).BeginInit();
        ((System.ComponentModel.ISupportInitialize)SmallMagnitudeEntry).BeginInit();
        ((System.ComponentModel.ISupportInitialize)SmallExponentEntry).BeginInit();
        AboutDisplay.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)Explorer).BeginInit();
        Explorer.Panel1.SuspendLayout();
        Explorer.Panel2.SuspendLayout();
        Explorer.SuspendLayout();
        SuspendLayout();
        // 
        // CloseButton
        // 
        CloseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        CloseButton.FlatAppearance.BorderSize = 0;
        CloseButton.FlatStyle = FlatStyle.Flat;
        CloseButton.Font = new Font("Segoe MDL2 Assets", 10F, FontStyle.Regular, GraphicsUnit.Point);
        CloseButton.Location = new Point(348, 0);
        CloseButton.Name = "CloseButton";
        CloseButton.Size = new Size(50, 30);
        CloseButton.TabIndex = 0;
        CloseButton.Text = "";
        CloseButton.UseVisualStyleBackColor = true;
        CloseButton.Click += CloseButton_Click;
        // 
        // MaximizeButton
        // 
        MaximizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        MaximizeButton.FlatAppearance.BorderSize = 0;
        MaximizeButton.FlatStyle = FlatStyle.Flat;
        MaximizeButton.Font = new Font("Segoe MDL2 Assets", 10F, FontStyle.Regular, GraphicsUnit.Point);
        MaximizeButton.Location = new Point(298, 0);
        MaximizeButton.Name = "MaximizeButton";
        MaximizeButton.Size = new Size(50, 30);
        MaximizeButton.TabIndex = 1;
        MaximizeButton.Text = "";
        MaximizeButton.UseVisualStyleBackColor = true;
        MaximizeButton.Click += MaximizeButton_Click;
        // 
        // MinimizeButton
        // 
        MinimizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        MinimizeButton.FlatAppearance.BorderSize = 0;
        MinimizeButton.FlatStyle = FlatStyle.Flat;
        MinimizeButton.Font = new Font("Segoe MDL2 Assets", 10F, FontStyle.Regular, GraphicsUnit.Point);
        MinimizeButton.Location = new Point(248, 0);
        MinimizeButton.Name = "MinimizeButton";
        MinimizeButton.Size = new Size(50, 30);
        MinimizeButton.TabIndex = 2;
        MinimizeButton.Text = "";
        MinimizeButton.UseVisualStyleBackColor = true;
        MinimizeButton.Click += MinimizeButton_Click;
        // 
        // Title
        // 
        Title.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        Title.AutoSize = true;
        Title.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
        Title.Location = new Point(16, 6);
        Title.Name = "Title";
        Title.Size = new Size(65, 17);
        Title.TabIndex = 3;
        Title.Text = "Unitversal";
        Title.TextAlign = ContentAlignment.MiddleCenter;
        Title.MouseDown += Title_MouseDown;
        // 
        // TitleBar
        // 
        TitleBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TitleBar.BackColor = SystemColors.Window;
        TitleBar.Controls.Add(Title);
        TitleBar.Controls.Add(MinimizeButton);
        TitleBar.Controls.Add(MaximizeButton);
        TitleBar.Controls.Add(CloseButton);
        TitleBar.Location = new Point(1, 1);
        TitleBar.Name = "TitleBar";
        TitleBar.Size = new Size(398, 30);
        TitleBar.TabIndex = 4;
        TitleBar.MouseDown += TitleBar_MouseDown;
        // 
        // SearchBox
        // 
        SearchBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        SearchBox.ContextMenuStrip = RightClickMenu;
        SearchBox.Font = new Font("Segoe UI", 11.8F, FontStyle.Regular, GraphicsUnit.Point);
        SearchBox.Location = new Point(20, 40);
        SearchBox.MaximumSize = new Size(1000000000, 1000000000);
        SearchBox.MaxLength = 43679;
        SearchBox.Name = "SearchBox";
        SearchBox.Size = new Size(314, 28);
        SearchBox.TabIndex = 5;
        SearchBox.TextChanged += SearchBox_TextChanged;
        SearchBox.MouseUp += SearchBox_MouseUp;
        // 
        // RightClickMenu
        // 
        RightClickMenu.Items.AddRange(new ToolStripItem[] { RightClickOpen, RightClickCut, RightClickCopy, RightClickPaste, RightClickSelectAll });
        RightClickMenu.Name = "RightClickMenu";
        RightClickMenu.RenderMode = ToolStripRenderMode.Professional;
        RightClickMenu.Size = new Size(123, 114);
        // 
        // RightClickOpen
        // 
        RightClickOpen.Name = "RightClickOpen";
        RightClickOpen.Size = new Size(122, 22);
        RightClickOpen.Text = "Open";
        RightClickOpen.Click += RightClickOpen_Click;
        // 
        // RightClickCut
        // 
        RightClickCut.Name = "RightClickCut";
        RightClickCut.Size = new Size(122, 22);
        RightClickCut.Text = "Cut";
        RightClickCut.Click += RightClickCut_Click;
        // 
        // RightClickCopy
        // 
        RightClickCopy.Name = "RightClickCopy";
        RightClickCopy.Size = new Size(122, 22);
        RightClickCopy.Text = "Copy";
        RightClickCopy.Click += RightClickCopy_Click;
        // 
        // RightClickPaste
        // 
        RightClickPaste.Name = "RightClickPaste";
        RightClickPaste.Size = new Size(122, 22);
        RightClickPaste.Text = "Paste";
        RightClickPaste.Click += RightClickPaste_Click;
        // 
        // RightClickSelectAll
        // 
        RightClickSelectAll.Name = "RightClickSelectAll";
        RightClickSelectAll.Size = new Size(122, 22);
        RightClickSelectAll.Text = "Select All";
        RightClickSelectAll.Click += RightClickSelectAll_Click;
        // 
        // ClearSearchButton
        // 
        ClearSearchButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        ClearSearchButton.FlatAppearance.BorderSize = 0;
        ClearSearchButton.FlatStyle = FlatStyle.Flat;
        ClearSearchButton.Font = new Font("Segoe MDL2 Assets", 7F, FontStyle.Regular, GraphicsUnit.Point);
        ClearSearchButton.Location = new Point(20, 40);
        ClearSearchButton.Name = "ClearSearchButton";
        ClearSearchButton.Size = new Size(29, 29);
        ClearSearchButton.TabIndex = 0;
        ClearSearchButton.TabStop = false;
        ClearSearchButton.Text = "";
        ClearSearchButton.UseVisualStyleBackColor = true;
        ClearSearchButton.Visible = false;
        ClearSearchButton.Click += ClearSearchButton_Click;
        // 
        // InterpretLabel
        // 
        InterpretLabel.AutoSize = true;
        InterpretLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        InterpretLabel.Location = new Point(20, 76);
        InterpretLabel.MaximumSize = new Size(297, 15);
        InterpretLabel.MinimumSize = new Size(0, 15);
        InterpretLabel.Name = "InterpretLabel";
        InterpretLabel.Size = new Size(0, 15);
        InterpretLabel.TabIndex = 7;
        InterpretLabel.Text = null;
        InterpretLabel.TextAlign = ContentAlignment.MiddleLeft;
        InterpretLabel.TextChanged += InterpretLabel_TextChanged;
        InterpretLabel.DoubleClick += InterpretLabel_DoubleClick;
        InterpretLabel.MouseLeave += InterpretLabel_MouseLeave;
        InterpretLabel.MouseHover += InterpretLabel_MouseHover;
        // 
        // InterpretToolTip
        // 
        InterpretToolTip.AutomaticDelay = 0;
        InterpretToolTip.BackColor = SystemColors.Window;
        InterpretToolTip.ForeColor = SystemColors.ControlText;
        InterpretToolTip.OwnerDraw = true;
        InterpretToolTip.Draw += InterpretToolTip_Draw;
        // 
        // SettingsButton
        // 
        SettingsButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        SettingsButton.FlatAppearance.BorderSize = 0;
        SettingsButton.FlatStyle = FlatStyle.Flat;
        SettingsButton.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        SettingsButton.Location = new Point(340, 40);
        SettingsButton.Name = "SettingsButton";
        SettingsButton.Size = new Size(40, 28);
        SettingsButton.TabIndex = 6;
        SettingsButton.Text = "⚙";
        SettingsButton.Click += SettingsButton_Click;
        // 
        // SortButton
        // 
        SortButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        SortButton.FlatAppearance.BorderSize = 0;
        SortButton.FlatStyle = FlatStyle.Flat;
        SortButton.Location = new Point(317, 72);
        SortButton.Name = "SortButton";
        SortButton.Size = new Size(63, 25);
        SortButton.TabIndex = 8;
        SortButton.Text = "↕ Sort By";
        SortButton.UseVisualStyleBackColor = true;
        SortButton.Click += SortButton_Click;
        // 
        // SortMenu
        // 
        SortMenu.Items.AddRange(new ToolStripItem[] { SortAscending, SortDescending, SortSeparator, SortUnit, SortMagnitude });
        SortMenu.Name = "SortMenu";
        SortMenu.ShowCheckMargin = true;
        SortMenu.ShowImageMargin = false;
        SortMenu.Size = new Size(137, 98);
        SortMenu.Closed += SortMenu_Closed;
        // 
        // SortAscending
        // 
        SortAscending.Checked = true;
        SortAscending.CheckOnClick = true;
        SortAscending.CheckState = CheckState.Checked;
        SortAscending.Name = "SortAscending";
        SortAscending.Size = new Size(136, 22);
        SortAscending.Text = "Ascending";
        SortAscending.Click += SortAscending_Click;
        // 
        // SortDescending
        // 
        SortDescending.CheckOnClick = true;
        SortDescending.Name = "SortDescending";
        SortDescending.Size = new Size(136, 22);
        SortDescending.Text = "Descending";
        SortDescending.Click += SortDescending_Click;
        // 
        // SortSeparator
        // 
        SortSeparator.Name = "SortSeparator";
        SortSeparator.Size = new Size(133, 6);
        // 
        // SortUnit
        // 
        SortUnit.Checked = true;
        SortUnit.CheckOnClick = true;
        SortUnit.CheckState = CheckState.Checked;
        SortUnit.Name = "SortUnit";
        SortUnit.Size = new Size(136, 22);
        SortUnit.Text = "Unit";
        SortUnit.Click += SortUnit_Click;
        // 
        // SortMagnitude
        // 
        SortMagnitude.CheckOnClick = true;
        SortMagnitude.Name = "SortMagnitude";
        SortMagnitude.Size = new Size(136, 22);
        SortMagnitude.Text = "Magnitude";
        SortMagnitude.Click += SortMagnitude_Click;
        // 
        // SearchView
        // 
        SearchView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        SearchView.BorderStyle = BorderStyle.None;
        SearchView.ContextMenuStrip = RightClickMenu;
        SearchView.FullRowSelect = true;
        SearchView.HeaderStyle = ColumnHeaderStyle.None;
        SearchView.Location = new Point(20, 101);
        SearchView.Name = "SearchView";
        SearchView.Size = new Size(360, 232);
        SearchView.TabIndex = 9;
        SearchView.UseCompatibleStateImageBehavior = false;
        SearchView.View = View.Details;
        SearchView.ColumnWidthChanged += SearchView_ColumnWidthChanged;
        SearchView.ItemActivate += SearchView_ItemActivate;
        SearchView.KeyDown += SearchView_KeyDown;
        SearchView.MouseUp += SearchView_MouseUp;
        // 
        // InfoDisplay
        // 
        InfoDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        InfoDisplay.Controls.Add(DescriptionText);
        InfoDisplay.Controls.Add(InfoCloseButton);
        InfoDisplay.Location = new Point(11, 36);
        InfoDisplay.Name = "InfoDisplay";
        InfoDisplay.Size = new Size(378, 310);
        InfoDisplay.TabIndex = 10;
        InfoDisplay.Visible = false;
        // 
        // DescriptionText
        // 
        DescriptionText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        DescriptionText.BackColor = Color.White;
        DescriptionText.BorderStyle = BorderStyle.None;
        DescriptionText.ContextMenuStrip = RightClickMenu;
        DescriptionText.Location = new Point(10, 10);
        DescriptionText.Name = "DescriptionText";
        DescriptionText.ReadOnly = true;
        DescriptionText.Size = new Size(358, 266);
        DescriptionText.TabIndex = 11;
        DescriptionText.Text = "";
        DescriptionText.LinkClicked += DescriptionText_LinkClicked;
        DescriptionText.MouseUp += DescriptionText_MouseUp;
        // 
        // InfoCloseButton
        // 
        InfoCloseButton.Anchor = AnchorStyles.Bottom;
        InfoCloseButton.FlatStyle = FlatStyle.Flat;
        InfoCloseButton.Location = new Point(162, 282);
        InfoCloseButton.Name = "InfoCloseButton";
        InfoCloseButton.Size = new Size(54, 24);
        InfoCloseButton.TabIndex = 12;
        InfoCloseButton.Text = "Close";
        InfoCloseButton.UseVisualStyleBackColor = true;
        InfoCloseButton.Click += InfoCloseButton_Click;
        // 
        // SettingsPanel
        // 
        SettingsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        SettingsPanel.AutoScroll = true;
        SettingsPanel.AutoScrollMinSize = new Size(0, 800);
        SettingsPanel.Controls.Add(SettingsLabel);
        SettingsPanel.Controls.Add(GeneralSettingsLabel);
        SettingsPanel.Controls.Add(PositionCheckbox);
        SettingsPanel.Controls.Add(SizeCheckbox);
        SettingsPanel.Controls.Add(CurrencyCheckbox);
        SettingsPanel.Controls.Add(ConversionSettingsLabel);
        SettingsPanel.Controls.Add(SignificantFiguresLabel);
        SettingsPanel.Controls.Add(SignificantFiguresEntry);
        SettingsPanel.Controls.Add(DecimalSeparatorLabel);
        SettingsPanel.Controls.Add(DecimalSeparatorEntry);
        SettingsPanel.Controls.Add(IntegerGroupSeparatorLabel);
        SettingsPanel.Controls.Add(IntegerGroupSeparatorEntry);
        SettingsPanel.Controls.Add(IntegerGroupSizeLabel);
        SettingsPanel.Controls.Add(IntegerGroupSizeEntry);
        SettingsPanel.Controls.Add(DecimalGroupSeparatorLabel);
        SettingsPanel.Controls.Add(DecimalGroupSeparatorEntry);
        SettingsPanel.Controls.Add(DecimalGroupSizeLabel);
        SettingsPanel.Controls.Add(DecimalGroupSizeEntry);
        SettingsPanel.Controls.Add(ScientificNotationLabel);
        SettingsPanel.Controls.Add(LargeMagnitudeLabel);
        SettingsPanel.Controls.Add(LargeMagnitudeEntry);
        SettingsPanel.Controls.Add(LargeExponentLabel);
        SettingsPanel.Controls.Add(LargeExponentEntry);
        SettingsPanel.Controls.Add(SmallMagnitudeLabel);
        SettingsPanel.Controls.Add(SmallMagnitudeEntry);
        SettingsPanel.Controls.Add(SmallExponentLabel);
        SettingsPanel.Controls.Add(SmallExponentEntry);
        SettingsPanel.Controls.Add(AppearLabel);
        SettingsPanel.Controls.Add(LightMode);
        SettingsPanel.Controls.Add(DarkMode);
        SettingsPanel.Controls.Add(SystemMode);
        SettingsPanel.Controls.Add(HelpLabel);
        SettingsPanel.Controls.Add(ExploreButton);
        SettingsPanel.Controls.Add(UpdateCurrencyButton);
        SettingsPanel.Controls.Add(CurrencyUpdateText);
        SettingsPanel.Controls.Add(AboutLabel);
        SettingsPanel.Controls.Add(AppAuthorText);
        SettingsPanel.Controls.Add(AboutButton);
        SettingsPanel.Controls.Add(SaveButton);
        SettingsPanel.Controls.Add(CancButton);
        SettingsPanel.Location = new Point(1, 31);
        SettingsPanel.Name = "SettingsPanel";
        SettingsPanel.Size = new Size(398, 318);
        SettingsPanel.TabIndex = 13;
        SettingsPanel.Visible = false;
        // 
        // SettingsLabel
        // 
        SettingsLabel.Anchor = AnchorStyles.Top;
        SettingsLabel.AutoSize = true;
        SettingsLabel.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
        SettingsLabel.Location = new Point(21, 6);
        SettingsLabel.Name = "SettingsLabel";
        SettingsLabel.Size = new Size(86, 28);
        SettingsLabel.TabIndex = 0;
        SettingsLabel.Text = "Settings";
        // 
        // GeneralSettingsLabel
        // 
        GeneralSettingsLabel.Anchor = AnchorStyles.Top;
        GeneralSettingsLabel.AutoSize = true;
        GeneralSettingsLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
        GeneralSettingsLabel.Location = new Point(22, 45);
        GeneralSettingsLabel.Name = "GeneralSettingsLabel";
        GeneralSettingsLabel.Size = new Size(66, 21);
        GeneralSettingsLabel.TabIndex = 1;
        GeneralSettingsLabel.Text = "General";
        // 
        // PositionCheckbox
        // 
        PositionCheckbox.Anchor = AnchorStyles.Top;
        PositionCheckbox.AutoSize = true;
        PositionCheckbox.Location = new Point(28, 80);
        PositionCheckbox.Name = "PositionCheckbox";
        PositionCheckbox.Size = new Size(177, 19);
        PositionCheckbox.TabIndex = 2;
        PositionCheckbox.Text = "Remember Window Position";
        PositionCheckbox.UseVisualStyleBackColor = true;
        // 
        // SizeCheckbox
        // 
        SizeCheckbox.Anchor = AnchorStyles.Top;
        SizeCheckbox.AutoSize = true;
        SizeCheckbox.Location = new Point(212, 80);
        SizeCheckbox.Name = "SizeCheckbox";
        SizeCheckbox.Size = new Size(154, 19);
        SizeCheckbox.TabIndex = 3;
        SizeCheckbox.Text = "Remember Window Size";
        SizeCheckbox.UseVisualStyleBackColor = true;
        // 
        // CurrencyCheckbox
        // 
        CurrencyCheckbox.Anchor = AnchorStyles.Top;
        CurrencyCheckbox.AutoSize = true;
        CurrencyCheckbox.Location = new Point(28, 115);
        CurrencyCheckbox.Name = "CurrencyCheckbox";
        CurrencyCheckbox.Size = new Size(178, 19);
        CurrencyCheckbox.TabIndex = 4;
        CurrencyCheckbox.Text = "Update currencies on startup";
        CurrencyCheckbox.UseVisualStyleBackColor = true;
        // 
        // ConversionSettingsLabel
        // 
        ConversionSettingsLabel.Anchor = AnchorStyles.Top;
        ConversionSettingsLabel.AutoSize = true;
        ConversionSettingsLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
        ConversionSettingsLabel.Location = new Point(22, 150);
        ConversionSettingsLabel.Name = "ConversionSettingsLabel";
        ConversionSettingsLabel.Size = new Size(99, 21);
        ConversionSettingsLabel.TabIndex = 5;
        ConversionSettingsLabel.Text = "Conversions";
        // 
        // SignificantFiguresLabel
        // 
        SignificantFiguresLabel.Anchor = AnchorStyles.Top;
        SignificantFiguresLabel.AutoSize = true;
        SignificantFiguresLabel.Location = new Point(22, 185);
        SignificantFiguresLabel.Name = "SignificantFiguresLabel";
        SignificantFiguresLabel.Size = new Size(99, 15);
        SignificantFiguresLabel.TabIndex = 6;
        SignificantFiguresLabel.Text = "Significant Digits:";
        // 
        // SignificantFiguresEntry
        // 
        SignificantFiguresEntry.Anchor = AnchorStyles.Top;
        SignificantFiguresEntry.Location = new Point(122, 183);
        SignificantFiguresEntry.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        SignificantFiguresEntry.Name = "SignificantFiguresEntry";
        SignificantFiguresEntry.Size = new Size(45, 23);
        SignificantFiguresEntry.TabIndex = 7;
        SignificantFiguresEntry.TextAlign = HorizontalAlignment.Center;
        SignificantFiguresEntry.Value = new decimal(new int[] { 10, 0, 0, 0 });
        // 
        // DecimalSeparatorLabel
        // 
        DecimalSeparatorLabel.Anchor = AnchorStyles.Top;
        DecimalSeparatorLabel.AutoSize = true;
        DecimalSeparatorLabel.Location = new Point(179, 185);
        DecimalSeparatorLabel.Name = "DecimalSeparatorLabel";
        DecimalSeparatorLabel.Size = new Size(106, 15);
        DecimalSeparatorLabel.TabIndex = 8;
        DecimalSeparatorLabel.Text = "Decimal Separator:";
        // 
        // DecimalSeparatorEntry
        // 
        DecimalSeparatorEntry.Anchor = AnchorStyles.Top;
        DecimalSeparatorEntry.Location = new Point(288, 183);
        DecimalSeparatorEntry.MaxLength = 1;
        DecimalSeparatorEntry.Name = "DecimalSeparatorEntry";
        DecimalSeparatorEntry.Size = new Size(15, 23);
        DecimalSeparatorEntry.TabIndex = 9;
        DecimalSeparatorEntry.Text = ".";
        DecimalSeparatorEntry.TextAlign = HorizontalAlignment.Center;
        // 
        // IntegerGroupSeparatorLabel
        // 
        IntegerGroupSeparatorLabel.Anchor = AnchorStyles.Top;
        IntegerGroupSeparatorLabel.AutoSize = true;
        IntegerGroupSeparatorLabel.Location = new Point(22, 220);
        IntegerGroupSeparatorLabel.Name = "IntegerGroupSeparatorLabel";
        IntegerGroupSeparatorLabel.Size = new Size(153, 15);
        IntegerGroupSeparatorLabel.TabIndex = 10;
        IntegerGroupSeparatorLabel.Text = "Integer Grouping Separator:";
        // 
        // IntegerGroupSeparatorEntry
        // 
        IntegerGroupSeparatorEntry.Anchor = AnchorStyles.Top;
        IntegerGroupSeparatorEntry.Location = new Point(183, 218);
        IntegerGroupSeparatorEntry.MaxLength = 1;
        IntegerGroupSeparatorEntry.Name = "IntegerGroupSeparatorEntry";
        IntegerGroupSeparatorEntry.Size = new Size(15, 23);
        IntegerGroupSeparatorEntry.TabIndex = 11;
        IntegerGroupSeparatorEntry.Text = ",";
        IntegerGroupSeparatorEntry.TextAlign = HorizontalAlignment.Center;
        // 
        // IntegerGroupSizeLabel
        // 
        IntegerGroupSizeLabel.Anchor = AnchorStyles.Top;
        IntegerGroupSizeLabel.AutoSize = true;
        IntegerGroupSizeLabel.Location = new Point(212, 220);
        IntegerGroupSizeLabel.Name = "IntegerGroupSizeLabel";
        IntegerGroupSizeLabel.Size = new Size(106, 15);
        IntegerGroupSizeLabel.TabIndex = 12;
        IntegerGroupSizeLabel.Text = "Integer Group Size:";
        // 
        // IntegerGroupSizeEntry
        // 
        IntegerGroupSizeEntry.Anchor = AnchorStyles.Top;
        IntegerGroupSizeEntry.Location = new Point(327, 218);
        IntegerGroupSizeEntry.Maximum = new decimal(new int[] { 9, 0, 0, 0 });
        IntegerGroupSizeEntry.Name = "IntegerGroupSizeEntry";
        IntegerGroupSizeEntry.Size = new Size(29, 23);
        IntegerGroupSizeEntry.TabIndex = 13;
        IntegerGroupSizeEntry.TextAlign = HorizontalAlignment.Center;
        IntegerGroupSizeEntry.Value = new decimal(new int[] { 3, 0, 0, 0 });
        // 
        // DecimalGroupSeparatorLabel
        // 
        DecimalGroupSeparatorLabel.Anchor = AnchorStyles.Top;
        DecimalGroupSeparatorLabel.AutoSize = true;
        DecimalGroupSeparatorLabel.Location = new Point(22, 255);
        DecimalGroupSeparatorLabel.Name = "DecimalGroupSeparatorLabel";
        DecimalGroupSeparatorLabel.Size = new Size(159, 15);
        DecimalGroupSeparatorLabel.TabIndex = 14;
        DecimalGroupSeparatorLabel.Text = "Decimal Grouping Separator:";
        // 
        // DecimalGroupSeparatorEntry
        // 
        DecimalGroupSeparatorEntry.Anchor = AnchorStyles.Top;
        DecimalGroupSeparatorEntry.Location = new Point(183, 253);
        DecimalGroupSeparatorEntry.MaxLength = 1;
        DecimalGroupSeparatorEntry.Name = "DecimalGroupSeparatorEntry";
        DecimalGroupSeparatorEntry.Size = new Size(15, 23);
        DecimalGroupSeparatorEntry.TabIndex = 15;
        DecimalGroupSeparatorEntry.Text = " ";
        DecimalGroupSeparatorEntry.TextAlign = HorizontalAlignment.Center;
        // 
        // DecimalGroupSizeLabel
        // 
        DecimalGroupSizeLabel.Anchor = AnchorStyles.Top;
        DecimalGroupSizeLabel.AutoSize = true;
        DecimalGroupSizeLabel.Location = new Point(212, 255);
        DecimalGroupSizeLabel.Name = "DecimalGroupSizeLabel";
        DecimalGroupSizeLabel.Size = new Size(112, 15);
        DecimalGroupSizeLabel.TabIndex = 16;
        DecimalGroupSizeLabel.Text = "Decimal Group Size:";
        // 
        // DecimalGroupSizeEntry
        // 
        DecimalGroupSizeEntry.Anchor = AnchorStyles.Top;
        DecimalGroupSizeEntry.Location = new Point(327, 253);
        DecimalGroupSizeEntry.Maximum = new decimal(new int[] { 9, 0, 0, 0 });
        DecimalGroupSizeEntry.Name = "DecimalGroupSizeEntry";
        DecimalGroupSizeEntry.Size = new Size(29, 23);
        DecimalGroupSizeEntry.TabIndex = 17;
        DecimalGroupSizeEntry.TextAlign = HorizontalAlignment.Center;
        DecimalGroupSizeEntry.Value = new decimal(new int[] { 5, 0, 0, 0 });
        // 
        // ScientificNotationLabel
        // 
        ScientificNotationLabel.Anchor = AnchorStyles.Top;
        ScientificNotationLabel.AutoSize = true;
        ScientificNotationLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
        ScientificNotationLabel.Location = new Point(22, 290);
        ScientificNotationLabel.Name = "ScientificNotationLabel";
        ScientificNotationLabel.Size = new Size(146, 21);
        ScientificNotationLabel.TabIndex = 18;
        ScientificNotationLabel.Text = "Scientific Notation";
        // 
        // LargeMagnitudeLabel
        // 
        LargeMagnitudeLabel.Anchor = AnchorStyles.Top;
        LargeMagnitudeLabel.AutoSize = true;
        LargeMagnitudeLabel.Location = new Point(22, 325);
        LargeMagnitudeLabel.Name = "LargeMagnitudeLabel";
        LargeMagnitudeLabel.Size = new Size(185, 15);
        LargeMagnitudeLabel.TabIndex = 19;
        LargeMagnitudeLabel.Text = "Use For Magnitudes Greater Than:";
        // 
        // LargeMagnitudeEntry
        // 
        LargeMagnitudeEntry.Anchor = AnchorStyles.Top;
        LargeMagnitudeEntry.Location = new Point(213, 323);
        LargeMagnitudeEntry.Maximum = new decimal(new int[] { -1304428545, 434162106, 542, 0 });
        LargeMagnitudeEntry.Name = "LargeMagnitudeEntry";
        LargeMagnitudeEntry.Size = new Size(90, 23);
        LargeMagnitudeEntry.TabIndex = 20;
        LargeMagnitudeEntry.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // LargeExponentLabel
        // 
        LargeExponentLabel.Anchor = AnchorStyles.Top;
        LargeExponentLabel.AutoSize = true;
        LargeExponentLabel.Location = new Point(307, 325);
        LargeExponentLabel.Name = "LargeExponentLabel";
        LargeExponentLabel.Size = new Size(24, 15);
        LargeExponentLabel.TabIndex = 21;
        LargeExponentLabel.Text = "E +";
        // 
        // LargeExponentEntry
        // 
        LargeExponentEntry.Anchor = AnchorStyles.Top;
        LargeExponentEntry.Location = new Point(331, 323);
        LargeExponentEntry.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        LargeExponentEntry.Name = "LargeExponentEntry";
        LargeExponentEntry.Size = new Size(45, 23);
        LargeExponentEntry.TabIndex = 22;
        LargeExponentEntry.Value = new decimal(new int[] { 10, 0, 0, 0 });
        // 
        // SmallMagnitudeLabel
        // 
        SmallMagnitudeLabel.Anchor = AnchorStyles.Top;
        SmallMagnitudeLabel.AutoSize = true;
        SmallMagnitudeLabel.Location = new Point(22, 360);
        SmallMagnitudeLabel.Name = "SmallMagnitudeLabel";
        SmallMagnitudeLabel.Size = new Size(169, 15);
        SmallMagnitudeLabel.TabIndex = 23;
        SmallMagnitudeLabel.Text = "Use For Magnitudes Less Than:";
        // 
        // SmallMagnitudeEntry
        // 
        SmallMagnitudeEntry.Anchor = AnchorStyles.Top;
        SmallMagnitudeEntry.Location = new Point(213, 358);
        SmallMagnitudeEntry.Maximum = new decimal(new int[] { -1304428545, 434162106, 542, 0 });
        SmallMagnitudeEntry.Name = "SmallMagnitudeEntry";
        SmallMagnitudeEntry.Size = new Size(90, 23);
        SmallMagnitudeEntry.TabIndex = 24;
        SmallMagnitudeEntry.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // SmallExponentLabel
        // 
        SmallExponentLabel.Anchor = AnchorStyles.Top;
        SmallExponentLabel.AutoSize = true;
        SmallExponentLabel.Location = new Point(307, 360);
        SmallExponentLabel.Name = "SmallExponentLabel";
        SmallExponentLabel.Size = new Size(21, 15);
        SmallExponentLabel.TabIndex = 25;
        SmallExponentLabel.Text = "E -";
        // 
        // SmallExponentEntry
        // 
        SmallExponentEntry.Anchor = AnchorStyles.Top;
        SmallExponentEntry.Location = new Point(331, 358);
        SmallExponentEntry.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        SmallExponentEntry.Name = "SmallExponentEntry";
        SmallExponentEntry.Size = new Size(45, 23);
        SmallExponentEntry.TabIndex = 26;
        SmallExponentEntry.Value = new decimal(new int[] { 10, 0, 0, 0 });
        // 
        // AppearLabel
        // 
        AppearLabel.Anchor = AnchorStyles.Top;
        AppearLabel.AutoSize = true;
        AppearLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
        AppearLabel.Location = new Point(22, 395);
        AppearLabel.Name = "AppearLabel";
        AppearLabel.Size = new Size(98, 21);
        AppearLabel.TabIndex = 27;
        AppearLabel.Text = "Appearance";
        // 
        // LightMode
        // 
        LightMode.Anchor = AnchorStyles.Top;
        LightMode.AutoSize = true;
        LightMode.Location = new Point(28, 430);
        LightMode.Name = "LightMode";
        LightMode.Size = new Size(52, 19);
        LightMode.TabIndex = 28;
        LightMode.TabStop = true;
        LightMode.Text = "Light";
        LightMode.UseVisualStyleBackColor = true;
        // 
        // DarkMode
        // 
        DarkMode.Anchor = AnchorStyles.Top;
        DarkMode.AutoSize = true;
        DarkMode.Location = new Point(28, 465);
        DarkMode.Name = "DarkMode";
        DarkMode.Size = new Size(49, 19);
        DarkMode.TabIndex = 29;
        DarkMode.TabStop = true;
        DarkMode.Text = "Dark";
        DarkMode.UseVisualStyleBackColor = true;
        // 
        // SystemMode
        // 
        SystemMode.Anchor = AnchorStyles.Top;
        SystemMode.AutoSize = true;
        SystemMode.Checked = true;
        SystemMode.Location = new Point(28, 500);
        SystemMode.Name = "SystemMode";
        SystemMode.Size = new Size(63, 19);
        SystemMode.TabIndex = 30;
        SystemMode.TabStop = true;
        SystemMode.Text = "System";
        SystemMode.UseVisualStyleBackColor = true;
        // 
        // HelpLabel
        // 
        HelpLabel.Anchor = AnchorStyles.Top;
        HelpLabel.AutoSize = true;
        HelpLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
        HelpLabel.Location = new Point(22, 535);
        HelpLabel.Name = "HelpLabel";
        HelpLabel.Size = new Size(45, 21);
        HelpLabel.TabIndex = 31;
        HelpLabel.Text = "Help";
        // 
        // ExploreButton
        // 
        ExploreButton.Anchor = AnchorStyles.Top;
        ExploreButton.FlatStyle = FlatStyle.Flat;
        ExploreButton.Location = new Point(26, 570);
        ExploreButton.Name = "ExploreButton";
        ExploreButton.Size = new Size(95, 24);
        ExploreButton.TabIndex = 32;
        ExploreButton.Text = "Explore Units";
        ExploreButton.UseVisualStyleBackColor = true;
        ExploreButton.Click += ExploreButton_Click;
        // 
        // UpdateCurrencyButton
        // 
        UpdateCurrencyButton.Anchor = AnchorStyles.Top;
        UpdateCurrencyButton.FlatStyle = FlatStyle.Flat;
        UpdateCurrencyButton.Location = new Point(26, 605);
        UpdateCurrencyButton.Name = "UpdateCurrencyButton";
        UpdateCurrencyButton.Size = new Size(122, 24);
        UpdateCurrencyButton.TabIndex = 33;
        UpdateCurrencyButton.Text = "Update Currencies";
        UpdateCurrencyButton.UseVisualStyleBackColor = true;
        UpdateCurrencyButton.Click += UpdateCurrencyButton_Click;
        // 
        // CurrencyUpdateText
        // 
        CurrencyUpdateText.Anchor = AnchorStyles.Top;
        CurrencyUpdateText.AutoSize = true;
        CurrencyUpdateText.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        CurrencyUpdateText.Location = new Point(153, 610);
        CurrencyUpdateText.Name = "CurrencyUpdateText";
        CurrencyUpdateText.Size = new Size(0, 15);
        CurrencyUpdateText.TabIndex = 39;
        CurrencyUpdateText.Text = null;
        // 
        // AboutLabel
        // 
        AboutLabel.Anchor = AnchorStyles.Top;
        AboutLabel.AutoSize = true;
        AboutLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
        AboutLabel.Location = new Point(22, 640);
        AboutLabel.Name = "AboutLabel";
        AboutLabel.Size = new Size(56, 21);
        AboutLabel.TabIndex = 35;
        AboutLabel.Text = "About";
        // 
        // AppAuthorText
        // 
        AppAuthorText.Anchor = AnchorStyles.Top;
        AppAuthorText.BorderStyle = BorderStyle.None;
        AppAuthorText.Location = new Point(22, 670);
        AppAuthorText.Multiline = true;
        AppAuthorText.Name = "AppAuthorText";
        AppAuthorText.ReadOnly = true;
        AppAuthorText.Size = new Size(163, 31);
        AppAuthorText.TabIndex = 41;
        AppAuthorText.Text = "Unitversal\r\n© 2022 Member Of The Reals";
        AppAuthorText.GotFocus += AppAuthorText_GotFocus;
        // 
        // AboutButton
        // 
        AboutButton.Anchor = AnchorStyles.Top;
        AboutButton.FlatStyle = FlatStyle.Flat;
        AboutButton.Location = new Point(26, 715);
        AboutButton.Name = "AboutButton";
        AboutButton.Size = new Size(65, 24);
        AboutButton.TabIndex = 38;
        AboutButton.Text = "About";
        AboutButton.UseVisualStyleBackColor = true;
        AboutButton.Click += AboutButton_Click;
        // 
        // SaveButton
        // 
        SaveButton.Anchor = AnchorStyles.Top;
        SaveButton.FlatStyle = FlatStyle.Flat;
        SaveButton.Location = new Point(127, 756);
        SaveButton.Name = "SaveButton";
        SaveButton.Size = new Size(75, 24);
        SaveButton.TabIndex = 39;
        SaveButton.Text = "Save";
        SaveButton.UseVisualStyleBackColor = true;
        SaveButton.Click += SaveButton_Click;
        // 
        // CancButton
        // 
        CancButton.Anchor = AnchorStyles.Top;
        CancButton.FlatStyle = FlatStyle.Flat;
        CancButton.Location = new Point(212, 756);
        CancButton.Name = "CancButton";
        CancButton.Size = new Size(75, 24);
        CancButton.TabIndex = 40;
        CancButton.Text = "Cancel";
        CancButton.UseVisualStyleBackColor = true;
        CancButton.Click += CancButton_Click;
        // 
        // AboutDisplay
        // 
        AboutDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        AboutDisplay.Controls.Add(NoticeButton);
        AboutDisplay.Controls.Add(NoticeTextBox);
        AboutDisplay.Controls.Add(LicenseButton);
        AboutDisplay.Controls.Add(LicenseTextBox);
        AboutDisplay.Controls.Add(ChangelogButton);
        AboutDisplay.Controls.Add(ChangelogTextBox);
        AboutDisplay.Controls.Add(AboutCloseButton);
        AboutDisplay.Location = new Point(1, 31);
        AboutDisplay.Name = "AboutDisplay";
        AboutDisplay.Size = new Size(398, 318);
        AboutDisplay.TabIndex = 41;
        AboutDisplay.Visible = false;
        // 
        // NoticeButton
        // 
        NoticeButton.FlatStyle = FlatStyle.Flat;
        NoticeButton.Location = new Point(0, 0);
        NoticeButton.Name = "NoticeButton";
        NoticeButton.Size = new Size(75, 28);
        NoticeButton.TabIndex = 14;
        NoticeButton.Text = "Notice";
        NoticeButton.UseVisualStyleBackColor = true;
        NoticeButton.Click += NoticeButton_Click;
        // 
        // NoticeTextBox
        // 
        NoticeTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        NoticeTextBox.BackColor = SystemColors.Window;
        NoticeTextBox.BorderStyle = BorderStyle.None;
        NoticeTextBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
        NoticeTextBox.Location = new Point(10, 35);
        NoticeTextBox.Multiline = true;
        NoticeTextBox.Name = "NoticeTextBox";
        NoticeTextBox.ReadOnly = true;
        NoticeTextBox.ScrollBars = ScrollBars.Both;
        NoticeTextBox.Size = new Size(378, 255);
        NoticeTextBox.TabIndex = 0;
        NoticeTextBox.Text = resources.GetString("NoticeTextBox.Text");
        NoticeTextBox.WordWrap = false;
        // 
        // LicenseButton
        // 
        LicenseButton.FlatStyle = FlatStyle.Flat;
        LicenseButton.Location = new Point(74, 0);
        LicenseButton.Name = "LicenseButton";
        LicenseButton.Size = new Size(75, 28);
        LicenseButton.TabIndex = 15;
        LicenseButton.Text = "License";
        LicenseButton.UseVisualStyleBackColor = true;
        LicenseButton.Click += LicenseButton_Click;
        // 
        // LicenseTextBox
        // 
        LicenseTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        LicenseTextBox.BackColor = SystemColors.Window;
        LicenseTextBox.BorderStyle = BorderStyle.None;
        LicenseTextBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
        LicenseTextBox.Location = new Point(10, 35);
        LicenseTextBox.Multiline = true;
        LicenseTextBox.Name = "LicenseTextBox";
        LicenseTextBox.ReadOnly = true;
        LicenseTextBox.ScrollBars = ScrollBars.Both;
        LicenseTextBox.Size = new Size(378, 255);
        LicenseTextBox.TabIndex = 1;
        LicenseTextBox.Text = resources.GetString("LicenseTextBox.Text");
        LicenseTextBox.WordWrap = false;
        // 
        // ChangelogButton
        // 
        ChangelogButton.FlatStyle = FlatStyle.Flat;
        ChangelogButton.Location = new Point(148, 0);
        ChangelogButton.Name = "ChangelogButton";
        ChangelogButton.Size = new Size(75, 28);
        ChangelogButton.TabIndex = 16;
        ChangelogButton.Text = "Changelog";
        ChangelogButton.UseVisualStyleBackColor = true;
        ChangelogButton.Click += ChangelogButton_Click;
        // 
        // ChangelogTextBox
        // 
        ChangelogTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        ChangelogTextBox.BackColor = SystemColors.Window;
        ChangelogTextBox.BorderStyle = BorderStyle.None;
        ChangelogTextBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
        ChangelogTextBox.Location = new Point(10, 35);
        ChangelogTextBox.Multiline = true;
        ChangelogTextBox.Name = "ChangelogTextBox";
        ChangelogTextBox.ReadOnly = true;
        ChangelogTextBox.ScrollBars = ScrollBars.Both;
        ChangelogTextBox.Size = new Size(378, 255);
        ChangelogTextBox.TabIndex = 2;
        ChangelogTextBox.Text = resources.GetString("ChangelogTextBox.Text");
        ChangelogTextBox.WordWrap = false;
        // 
        // AboutCloseButton
        // 
        AboutCloseButton.Anchor = AnchorStyles.Bottom;
        AboutCloseButton.FlatStyle = FlatStyle.Flat;
        AboutCloseButton.Location = new Point(172, 292);
        AboutCloseButton.Name = "AboutCloseButton";
        AboutCloseButton.Size = new Size(54, 24);
        AboutCloseButton.TabIndex = 13;
        AboutCloseButton.Text = "Close";
        AboutCloseButton.UseVisualStyleBackColor = true;
        AboutCloseButton.Click += AboutCloseButton_Click;
        // 
        // Explorer
        // 
        Explorer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        Explorer.IsSplitterFixed = true;
        Explorer.Location = new Point(1, 31);
        Explorer.Name = "Explorer";
        // 
        // Explorer.Panel1
        // 
        Explorer.Panel1.Controls.Add(ExploreMenuButton);
        Explorer.Panel1.Controls.Add(AllButton);
        Explorer.Panel1.Controls.Add(CategoryButton);
        Explorer.Panel1.Controls.Add(BinaryButton);
        Explorer.Panel1.Controls.Add(SIButton);
        Explorer.Panel1.Controls.Add(EquivalentsButton);
        Explorer.Panel1.Controls.Add(TemperatureButton);
        Explorer.Panel1.Controls.Add(BackButton);
        Explorer.Panel1.Controls.Add(ExitButton);
        // 
        // Explorer.Panel2
        // 
        Explorer.Panel2.Controls.Add(ExplorerLabel);
        Explorer.Panel2.Controls.Add(ExploreSort);
        Explorer.Panel2.Controls.Add(AllView);
        Explorer.Panel2.Controls.Add(CategoryView);
        Explorer.Panel2.Controls.Add(BinaryView);
        Explorer.Panel2.Controls.Add(SIView);
        Explorer.Panel2.Controls.Add(EquivalentsView);
        Explorer.Panel2.Controls.Add(TemperatureView);
        Explorer.Size = new Size(398, 318);
        Explorer.SplitterDistance = 150;
        Explorer.TabIndex = 17;
        Explorer.Visible = false;
        Explorer.Resize += Explorer_Resize;
        // 
        // ExploreMenuButton
        // 
        ExploreMenuButton.FlatAppearance.BorderSize = 0;
        ExploreMenuButton.FlatStyle = FlatStyle.Flat;
        ExploreMenuButton.Font = new Font("Segoe MDL2 Assets", 12F, FontStyle.Regular, GraphicsUnit.Point);
        ExploreMenuButton.Location = new Point(0, 0);
        ExploreMenuButton.Name = "ExploreMenuButton";
        ExploreMenuButton.Size = new Size(30, 30);
        ExploreMenuButton.TabIndex = 16;
        ExploreMenuButton.Text = "";
        ExploreMenuButton.UseVisualStyleBackColor = true;
        ExploreMenuButton.Click += ExploreMenuButton_Click;
        // 
        // AllButton
        // 
        AllButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        AllButton.FlatAppearance.BorderSize = 0;
        AllButton.FlatStyle = FlatStyle.Flat;
        AllButton.Location = new Point(0, 31);
        AllButton.Name = "AllButton";
        AllButton.Size = new Size(148, 28);
        AllButton.TabIndex = 15;
        AllButton.Text = "All Units";
        AllButton.TextAlign = ContentAlignment.MiddleLeft;
        AllButton.UseVisualStyleBackColor = true;
        AllButton.Click += AllButton_Click;
        // 
        // CategoryButton
        // 
        CategoryButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        CategoryButton.FlatAppearance.BorderSize = 0;
        CategoryButton.FlatStyle = FlatStyle.Flat;
        CategoryButton.Location = new Point(0, 59);
        CategoryButton.Name = "CategoryButton";
        CategoryButton.Size = new Size(148, 28);
        CategoryButton.TabIndex = 17;
        CategoryButton.Text = "Units By Category";
        CategoryButton.TextAlign = ContentAlignment.MiddleLeft;
        CategoryButton.UseVisualStyleBackColor = true;
        CategoryButton.Click += CategoryButton_Click;
        // 
        // BinaryButton
        // 
        BinaryButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        BinaryButton.FlatAppearance.BorderSize = 0;
        BinaryButton.FlatStyle = FlatStyle.Flat;
        BinaryButton.Location = new Point(0, 87);
        BinaryButton.Name = "BinaryButton";
        BinaryButton.Size = new Size(148, 28);
        BinaryButton.TabIndex = 18;
        BinaryButton.Text = "Binary Prefixes";
        BinaryButton.TextAlign = ContentAlignment.MiddleLeft;
        BinaryButton.UseVisualStyleBackColor = true;
        BinaryButton.Click += BinaryButton_Click;
        // 
        // SIButton
        // 
        SIButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        SIButton.FlatAppearance.BorderSize = 0;
        SIButton.FlatStyle = FlatStyle.Flat;
        SIButton.Location = new Point(0, 115);
        SIButton.Name = "SIButton";
        SIButton.Size = new Size(148, 28);
        SIButton.TabIndex = 19;
        SIButton.Text = "SI Prefixes";
        SIButton.TextAlign = ContentAlignment.MiddleLeft;
        SIButton.UseVisualStyleBackColor = true;
        SIButton.Click += SIButton_Click;
        // 
        // EquivalentsButton
        // 
        EquivalentsButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        EquivalentsButton.FlatAppearance.BorderSize = 0;
        EquivalentsButton.FlatStyle = FlatStyle.Flat;
        EquivalentsButton.Location = new Point(0, 143);
        EquivalentsButton.Name = "EquivalentsButton";
        EquivalentsButton.Size = new Size(148, 28);
        EquivalentsButton.TabIndex = 20;
        EquivalentsButton.Text = "SI Equivalents";
        EquivalentsButton.TextAlign = ContentAlignment.MiddleLeft;
        EquivalentsButton.UseVisualStyleBackColor = true;
        EquivalentsButton.Click += EquivalentsButton_Click;
        // 
        // TemperatureButton
        // 
        TemperatureButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TemperatureButton.FlatAppearance.BorderSize = 0;
        TemperatureButton.FlatStyle = FlatStyle.Flat;
        TemperatureButton.Location = new Point(0, 171);
        TemperatureButton.Name = "TemperatureButton";
        TemperatureButton.Size = new Size(148, 28);
        TemperatureButton.TabIndex = 21;
        TemperatureButton.Text = "Temperature Formulas";
        TemperatureButton.TextAlign = ContentAlignment.MiddleLeft;
        TemperatureButton.UseVisualStyleBackColor = true;
        TemperatureButton.Click += TemperatureButton_Click;
        // 
        // BackButton
        // 
        BackButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        BackButton.FlatAppearance.BorderSize = 0;
        BackButton.FlatStyle = FlatStyle.Flat;
        BackButton.Location = new Point(0, 262);
        BackButton.Name = "BackButton";
        BackButton.Size = new Size(148, 28);
        BackButton.TabIndex = 23;
        BackButton.Text = "Back";
        BackButton.TextAlign = ContentAlignment.MiddleLeft;
        BackButton.UseVisualStyleBackColor = true;
        BackButton.Visible = false;
        BackButton.Click += BackButton_Click;
        // 
        // ExitButton
        // 
        ExitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        ExitButton.FlatAppearance.BorderSize = 0;
        ExitButton.FlatStyle = FlatStyle.Flat;
        ExitButton.Location = new Point(0, 290);
        ExitButton.Name = "ExitButton";
        ExitButton.Size = new Size(148, 28);
        ExitButton.TabIndex = 22;
        ExitButton.Text = "Exit";
        ExitButton.TextAlign = ContentAlignment.MiddleLeft;
        ExitButton.UseVisualStyleBackColor = true;
        ExitButton.Click += ExitButton_Click;
        // 
        // ExplorerLabel
        // 
        ExplorerLabel.AutoSize = true;
        ExplorerLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
        ExplorerLabel.Location = new Point(0, 6);
        ExplorerLabel.Name = "ExplorerLabel";
        ExplorerLabel.Size = new Size(66, 20);
        ExplorerLabel.TabIndex = 11;
        ExplorerLabel.Text = "All Units";
        // 
        // ExploreSort
        // 
        ExploreSort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        ExploreSort.FlatAppearance.BorderSize = 0;
        ExploreSort.FlatStyle = FlatStyle.Flat;
        ExploreSort.Location = new Point(195, 4);
        ExploreSort.Name = "ExploreSort";
        ExploreSort.Size = new Size(45, 25);
        ExploreSort.TabIndex = 17;
        ExploreSort.Text = "↕ Sort";
        ExploreSort.UseVisualStyleBackColor = true;
        ExploreSort.Click += ExploreSort_Click;
        // 
        // AllView
        // 
        AllView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        AllView.BorderStyle = BorderStyle.None;
        AllView.ContextMenuStrip = RightClickMenu;
        AllView.FullRowSelect = true;
        AllView.HeaderStyle = ColumnHeaderStyle.None;
        AllView.Location = new Point(0, 33);
        AllView.Name = "AllView";
        AllView.Size = new Size(244, 285);
        AllView.TabIndex = 10;
        AllView.UseCompatibleStateImageBehavior = false;
        AllView.View = View.Details;
        AllView.ColumnWidthChanged += AllView_ColumnWidthChanged;
        AllView.ItemActivate += AllView_ItemActivate;
        AllView.KeyDown += AllView_KeyDown;
        AllView.MouseUp += AllView_MouseUp;
        // 
        // CategoryView
        // 
        CategoryView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        CategoryView.BorderStyle = BorderStyle.None;
        CategoryView.ContextMenuStrip = RightClickMenu;
        CategoryView.FullRowSelect = true;
        CategoryView.HeaderStyle = ColumnHeaderStyle.None;
        CategoryView.Location = new Point(0, 33);
        CategoryView.Name = "CategoryView";
        CategoryView.Size = new Size(244, 285);
        CategoryView.TabIndex = 12;
        CategoryView.UseCompatibleStateImageBehavior = false;
        CategoryView.View = View.Details;
        CategoryView.ColumnWidthChanged += CategoryView_ColumnWidthChanged;
        CategoryView.ItemActivate += CategoryView_ItemActivate;
        CategoryView.KeyDown += CategoryView_KeyDown;
        CategoryView.MouseUp += CategoryView_MouseUp;
        // 
        // BinaryView
        // 
        BinaryView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        BinaryView.BorderStyle = BorderStyle.None;
        BinaryView.ContextMenuStrip = RightClickMenu;
        BinaryView.FullRowSelect = true;
        BinaryView.HeaderStyle = ColumnHeaderStyle.None;
        BinaryView.Location = new Point(0, 33);
        BinaryView.Name = "BinaryView";
        BinaryView.Size = new Size(244, 285);
        BinaryView.TabIndex = 13;
        BinaryView.UseCompatibleStateImageBehavior = false;
        BinaryView.View = View.Details;
        BinaryView.ColumnWidthChanged += BinaryView_ColumnWidthChanged;
        BinaryView.ItemActivate += BinaryView_ItemActivate;
        BinaryView.KeyDown += BinaryView_KeyDown;
        BinaryView.MouseUp += BinaryView_MouseUp;
        // 
        // SIView
        // 
        SIView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        SIView.BorderStyle = BorderStyle.None;
        SIView.ContextMenuStrip = RightClickMenu;
        SIView.FullRowSelect = true;
        SIView.HeaderStyle = ColumnHeaderStyle.None;
        SIView.Location = new Point(0, 33);
        SIView.Name = "SIView";
        SIView.Size = new Size(244, 285);
        SIView.TabIndex = 14;
        SIView.UseCompatibleStateImageBehavior = false;
        SIView.View = View.Details;
        SIView.ColumnWidthChanged += SIView_ColumnWidthChanged;
        SIView.ItemActivate += SIView_ItemActivate;
        SIView.KeyDown += SIView_KeyDown;
        SIView.MouseUp += SIView_MouseUp;
        // 
        // EquivalentsView
        // 
        EquivalentsView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        EquivalentsView.BorderStyle = BorderStyle.None;
        EquivalentsView.ContextMenuStrip = RightClickMenu;
        EquivalentsView.FullRowSelect = true;
        EquivalentsView.HeaderStyle = ColumnHeaderStyle.None;
        EquivalentsView.Location = new Point(0, 33);
        EquivalentsView.Name = "EquivalentsView";
        EquivalentsView.Size = new Size(244, 285);
        EquivalentsView.TabIndex = 15;
        EquivalentsView.UseCompatibleStateImageBehavior = false;
        EquivalentsView.View = View.Details;
        EquivalentsView.ColumnWidthChanged += EquivalentsView_ColumnWidthChanged;
        EquivalentsView.ItemActivate += EquivalentsView_ItemActivate;
        EquivalentsView.KeyDown += EquivalentsView_KeyDown;
        EquivalentsView.MouseUp += EquivalentsView_MouseUp;
        // 
        // TemperatureView
        // 
        TemperatureView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        TemperatureView.BorderStyle = BorderStyle.None;
        TemperatureView.ContextMenuStrip = RightClickMenu;
        TemperatureView.FullRowSelect = true;
        TemperatureView.HeaderStyle = ColumnHeaderStyle.None;
        TemperatureView.Location = new Point(0, 33);
        TemperatureView.Name = "TemperatureView";
        TemperatureView.Size = new Size(244, 285);
        TemperatureView.TabIndex = 16;
        TemperatureView.UseCompatibleStateImageBehavior = false;
        TemperatureView.View = View.Details;
        TemperatureView.ColumnWidthChanged += TemperatureView_ColumnWidthChanged;
        TemperatureView.ItemActivate += TemperatureView_ItemActivate;
        TemperatureView.KeyDown += TemperatureView_KeyDown;
        TemperatureView.MouseUp += TemperatureView_MouseUp;
        // 
        // MainWindow
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = SystemColors.Window;
        ClientSize = new Size(400, 350);
        ControlBox = false;
        Controls.Add(Explorer);
        Controls.Add(AboutDisplay);
        Controls.Add(SettingsPanel);
        Controls.Add(InfoDisplay);
        Controls.Add(SearchView);
        Controls.Add(SortButton);
        Controls.Add(SettingsButton);
        Controls.Add(InterpretLabel);
        Controls.Add(ClearSearchButton);
        Controls.Add(SearchBox);
        Controls.Add(TitleBar);
        DoubleBuffered = true;
        FormBorderStyle = FormBorderStyle.None;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MinimumSize = new Size(400, 350);
        Name = "MainWindow";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Unitversal";
        FormClosing += MainWindow_FormClosing;
        Load += MainWindow_Load;
        LocationChanged += MainWindow_LocationChanged;
        MouseClick += MainWindow_MouseClick;
        Resize += MainWindow_Resize;
        TitleBar.ResumeLayout(false);
        TitleBar.PerformLayout();
        RightClickMenu.ResumeLayout(false);
        SortMenu.ResumeLayout(false);
        InfoDisplay.ResumeLayout(false);
        SettingsPanel.ResumeLayout(false);
        SettingsPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)SignificantFiguresEntry).EndInit();
        ((System.ComponentModel.ISupportInitialize)IntegerGroupSizeEntry).EndInit();
        ((System.ComponentModel.ISupportInitialize)DecimalGroupSizeEntry).EndInit();
        ((System.ComponentModel.ISupportInitialize)LargeMagnitudeEntry).EndInit();
        ((System.ComponentModel.ISupportInitialize)LargeExponentEntry).EndInit();
        ((System.ComponentModel.ISupportInitialize)SmallMagnitudeEntry).EndInit();
        ((System.ComponentModel.ISupportInitialize)SmallExponentEntry).EndInit();
        AboutDisplay.ResumeLayout(false);
        AboutDisplay.PerformLayout();
        Explorer.Panel1.ResumeLayout(false);
        Explorer.Panel2.ResumeLayout(false);
        Explorer.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)Explorer).EndInit();
        Explorer.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Button CloseButton;
    private System.Windows.Forms.Button MaximizeButton;
    private System.Windows.Forms.Button MinimizeButton;
    private LabelNoCopy Title;
    private System.Windows.Forms.Panel TitleBar;
    private TextBoxPadded SearchBox;
    private System.Windows.Forms.Button ClearSearchButton;
    private System.Windows.Forms.ContextMenuStrip RightClickMenu;
    private System.Windows.Forms.ToolStripMenuItem RightClickOpen;
    private System.Windows.Forms.ToolStripMenuItem RightClickCut;
    private System.Windows.Forms.ToolStripMenuItem RightClickCopy;
    private System.Windows.Forms.ToolStripMenuItem RightClickPaste;
    private System.Windows.Forms.ToolStripMenuItem RightClickSelectAll;
    private LabelNoCopy InterpretLabel;
    private System.Windows.Forms.ToolTip InterpretToolTip;
    private System.Windows.Forms.Button SettingsButton;
    private System.Windows.Forms.Button SortButton;
    private System.Windows.Forms.ContextMenuStrip SortMenu;
    private System.Windows.Forms.ToolStripMenuItem SortAscending;
    private System.Windows.Forms.ToolStripMenuItem SortDescending;
    private System.Windows.Forms.ToolStripSeparator SortSeparator;
    private System.Windows.Forms.ToolStripMenuItem SortUnit;
    private System.Windows.Forms.ToolStripMenuItem SortMagnitude;
    private System.Windows.Forms.ListView SearchView;
    private System.Windows.Forms.Panel InfoDisplay;
    private System.Windows.Forms.RichTextBox DescriptionText;
    private System.Windows.Forms.Button InfoCloseButton;
    private System.Windows.Forms.Panel SettingsPanel;
    private LabelNoCopy SettingsLabel;
    private LabelNoCopy GeneralSettingsLabel;
    private System.Windows.Forms.CheckBox PositionCheckbox;
    private System.Windows.Forms.CheckBox SizeCheckbox;
    private System.Windows.Forms.CheckBox CurrencyCheckbox;
    private LabelNoCopy ConversionSettingsLabel;
    private LabelNoCopy SignificantFiguresLabel;
    private System.Windows.Forms.NumericUpDown SignificantFiguresEntry;
    private LabelNoCopy DecimalSeparatorLabel;
    private System.Windows.Forms.TextBox DecimalSeparatorEntry;
    private LabelNoCopy IntegerGroupSeparatorLabel;
    private System.Windows.Forms.TextBox IntegerGroupSeparatorEntry;
    private LabelNoCopy IntegerGroupSizeLabel;
    private System.Windows.Forms.NumericUpDown IntegerGroupSizeEntry;
    private LabelNoCopy DecimalGroupSeparatorLabel;
    private System.Windows.Forms.TextBox DecimalGroupSeparatorEntry;
    private LabelNoCopy DecimalGroupSizeLabel;
    private System.Windows.Forms.NumericUpDown DecimalGroupSizeEntry;
    private LabelNoCopy ScientificNotationLabel;
    private LabelNoCopy LargeMagnitudeLabel;
    private System.Windows.Forms.NumericUpDown LargeMagnitudeEntry;
    private LabelNoCopy LargeExponentLabel;
    private System.Windows.Forms.NumericUpDown LargeExponentEntry;
    private LabelNoCopy SmallMagnitudeLabel;
    private System.Windows.Forms.NumericUpDown SmallMagnitudeEntry;
    private LabelNoCopy SmallExponentLabel;
    private System.Windows.Forms.NumericUpDown SmallExponentEntry;
    private LabelNoCopy AppearLabel;
    private System.Windows.Forms.RadioButton LightMode;
    private System.Windows.Forms.RadioButton DarkMode;
    private System.Windows.Forms.RadioButton SystemMode;
    private LabelNoCopy HelpLabel;
    private System.Windows.Forms.Button ExploreButton;
    private System.Windows.Forms.Button UpdateCurrencyButton;
    private LabelNoCopy CurrencyUpdateText;
    private LabelNoCopy AboutLabel;
    private System.Windows.Forms.TextBox AppAuthorText;
    private System.Windows.Forms.Button SaveButton;
    private System.Windows.Forms.Button CancButton;
    private System.Windows.Forms.Button AboutButton;
    private System.Windows.Forms.Panel AboutDisplay;
    private System.Windows.Forms.Button NoticeButton;
    private System.Windows.Forms.TextBox NoticeTextBox;
    private System.Windows.Forms.Button LicenseButton;
    private System.Windows.Forms.TextBox LicenseTextBox;
    private System.Windows.Forms.Button ChangelogButton;
    private System.Windows.Forms.TextBox ChangelogTextBox;
    private System.Windows.Forms.Button AboutCloseButton;
    private SplitContainer Explorer;
    private Button ExploreMenuButton;
    private Button AllButton;
    private Button CategoryButton;
    private Button BinaryButton;
    private Button SIButton;
    private Button EquivalentsButton;
    private Button TemperatureButton;
    private LabelNoCopy ExplorerLabel;
    private ListView AllView;
    private ListView CategoryView;
    private ListView BinaryView;
    private ListView SIView;
    private ListView EquivalentsView;
    private ListView TemperatureView;
    private Button ExploreSort;
    private Button ExitButton;
    private Button BackButton;
}
