
namespace Unitversal
{
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.CloseButton = new System.Windows.Forms.Button();
            this.MaximizeButton = new System.Windows.Forms.Button();
            this.MinimizeButton = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.TitleBar = new System.Windows.Forms.Panel();
            this.SearchBox = new System.Windows.Forms.TextBox();
            this.RightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RightClickOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.RightClickCut = new System.Windows.Forms.ToolStripMenuItem();
            this.RightClickCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.RightClickPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.RightClickSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.InterpretLabel = new System.Windows.Forms.Label();
            this.InterpretToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SortButton = new System.Windows.Forms.Button();
            this.SortMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SortAscending = new System.Windows.Forms.ToolStripMenuItem();
            this.SortDescending = new System.Windows.Forms.ToolStripMenuItem();
            this.SortSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.SortUnit = new System.Windows.Forms.ToolStripMenuItem();
            this.SortMagnitude = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchView = new System.Windows.Forms.ListView();
            this.AboutDisplay = new System.Windows.Forms.Panel();
            this.DescriptionText = new System.Windows.Forms.RichTextBox();
            this.ReturnButton = new System.Windows.Forms.Button();
            this.SettingsPanel = new System.Windows.Forms.Panel();
            this.SettingsLabel = new System.Windows.Forms.Label();
            this.GeneralSettingsLabel = new System.Windows.Forms.Label();
            this.PositionCheckbox = new System.Windows.Forms.CheckBox();
            this.SizeCheckbox = new System.Windows.Forms.CheckBox();
            this.CurrencyCheckbox = new System.Windows.Forms.CheckBox();
            this.ConversionSettingsLabel = new System.Windows.Forms.Label();
            this.SignificantFiguresLabel = new System.Windows.Forms.Label();
            this.SignificantFiguresEntry = new System.Windows.Forms.NumericUpDown();
            this.DecimalSeparatorLabel = new System.Windows.Forms.Label();
            this.DecimalSeparatorEntry = new System.Windows.Forms.TextBox();
            this.IntegerGroupSeparatorLabel = new System.Windows.Forms.Label();
            this.IntegerGroupSeparatorEntry = new System.Windows.Forms.TextBox();
            this.IntegerGroupSizeLabel = new System.Windows.Forms.Label();
            this.IntegerGroupSizeEntry = new System.Windows.Forms.NumericUpDown();
            this.DecimalGroupSeparatorLabel = new System.Windows.Forms.Label();
            this.DecimalGroupSeparatorEntry = new System.Windows.Forms.TextBox();
            this.DecimalGroupSizeLabel = new System.Windows.Forms.Label();
            this.DecimalGroupSizeEntry = new System.Windows.Forms.NumericUpDown();
            this.ScientificNotationLabel = new System.Windows.Forms.Label();
            this.LargeMagnitudeLabel = new System.Windows.Forms.Label();
            this.LargeMagnitudeEntry = new System.Windows.Forms.NumericUpDown();
            this.LargeExponentLabel = new System.Windows.Forms.Label();
            this.LargeExponentEntry = new System.Windows.Forms.NumericUpDown();
            this.SmallMagnitudeLabel = new System.Windows.Forms.Label();
            this.SmallMagnitudeEntry = new System.Windows.Forms.NumericUpDown();
            this.SmallExponentLabel = new System.Windows.Forms.Label();
            this.SmallExponentEntry = new System.Windows.Forms.NumericUpDown();
            this.AppearLabel = new System.Windows.Forms.Label();
            this.LightMode = new System.Windows.Forms.RadioButton();
            this.DarkMode = new System.Windows.Forms.RadioButton();
            this.SystemMode = new System.Windows.Forms.RadioButton();
            this.HelpLabel = new System.Windows.Forms.Label();
            this.ExploreButton = new System.Windows.Forms.Button();
            this.UpdateCurrencyButton = new System.Windows.Forms.Button();
            this.CurrencyUpdateText = new System.Windows.Forms.Label();
            this.AboutLabel = new System.Windows.Forms.Label();
            this.AppNameLabel = new System.Windows.Forms.Label();
            this.AuthorLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CancButton = new System.Windows.Forms.Button();
            this.TitleBar.SuspendLayout();
            this.RightClickMenu.SuspendLayout();
            this.SortMenu.SuspendLayout();
            this.AboutDisplay.SuspendLayout();
            this.SettingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SignificantFiguresEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IntegerGroupSizeEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DecimalGroupSizeEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMagnitudeEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeExponentEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMagnitudeEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallExponentEntry)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.FlatAppearance.BorderSize = 0;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CloseButton.Location = new System.Drawing.Point(347, 0);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(50, 30);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // MaximizeButton
            // 
            this.MaximizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaximizeButton.FlatAppearance.BorderSize = 0;
            this.MaximizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MaximizeButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MaximizeButton.Location = new System.Drawing.Point(297, 0);
            this.MaximizeButton.Name = "MaximizeButton";
            this.MaximizeButton.Size = new System.Drawing.Size(50, 30);
            this.MaximizeButton.TabIndex = 1;
            this.MaximizeButton.Text = "";
            this.MaximizeButton.UseVisualStyleBackColor = true;
            this.MaximizeButton.Click += new System.EventHandler(this.MaximizeButton_Click);
            // 
            // MinimizeButton
            // 
            this.MinimizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinimizeButton.FlatAppearance.BorderSize = 0;
            this.MinimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MinimizeButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MinimizeButton.Location = new System.Drawing.Point(247, 0);
            this.MinimizeButton.Name = "MinimizeButton";
            this.MinimizeButton.Size = new System.Drawing.Size(50, 30);
            this.MinimizeButton.TabIndex = 2;
            this.MinimizeButton.Text = "";
            this.MinimizeButton.UseVisualStyleBackColor = true;
            this.MinimizeButton.Click += new System.EventHandler(this.MinimizeButton_Click);
            // 
            // Title
            // 
            this.Title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Title.Location = new System.Drawing.Point(16, 6);
            this.Title.Margin = new System.Windows.Forms.Padding(0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(65, 17);
            this.Title.TabIndex = 3;
            this.Title.Text = "Unitversal";
            this.Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Title_MouseDown);
            // 
            // TitleBar
            // 
            this.TitleBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TitleBar.BackColor = System.Drawing.SystemColors.Window;
            this.TitleBar.Controls.Add(this.Title);
            this.TitleBar.Controls.Add(this.MinimizeButton);
            this.TitleBar.Controls.Add(this.MaximizeButton);
            this.TitleBar.Controls.Add(this.CloseButton);
            this.TitleBar.Location = new System.Drawing.Point(1, 1);
            this.TitleBar.Name = "TitleBar";
            this.TitleBar.Size = new System.Drawing.Size(398, 30);
            this.TitleBar.TabIndex = 4;
            this.TitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            // 
            // SearchBox
            // 
            this.SearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchBox.ContextMenuStrip = this.RightClickMenu;
            this.SearchBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SearchBox.Location = new System.Drawing.Point(20, 40);
            this.SearchBox.MaximumSize = new System.Drawing.Size(9999, 28);
            this.SearchBox.MaxLength = 43679;
            this.SearchBox.MinimumSize = new System.Drawing.Size(50, 28);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(314, 28);
            this.SearchBox.TabIndex = 5;
            this.SearchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
            this.SearchBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SearchBox_MouseUp);
            // 
            // RightClickMenu
            // 
            this.RightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RightClickOpen,
            this.RightClickCut,
            this.RightClickCopy,
            this.RightClickPaste,
            this.RightClickSelectAll});
            this.RightClickMenu.Name = "RightClickMenu";
            this.RightClickMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.RightClickMenu.Size = new System.Drawing.Size(123, 114);
            // 
            // RightClickOpen
            // 
            this.RightClickOpen.Name = "RightClickOpen";
            this.RightClickOpen.Size = new System.Drawing.Size(122, 22);
            this.RightClickOpen.Text = "Open";
            this.RightClickOpen.Click += new System.EventHandler(this.RightClickOpen_Click);
            // 
            // RightClickCut
            // 
            this.RightClickCut.Name = "RightClickCut";
            this.RightClickCut.Size = new System.Drawing.Size(122, 22);
            this.RightClickCut.Text = "Cut";
            this.RightClickCut.Click += new System.EventHandler(this.RightClickCut_Click);
            // 
            // RightClickCopy
            // 
            this.RightClickCopy.Name = "RightClickCopy";
            this.RightClickCopy.Size = new System.Drawing.Size(122, 22);
            this.RightClickCopy.Text = "Copy";
            this.RightClickCopy.Click += new System.EventHandler(this.RightClickCopy_Click);
            // 
            // RightClickPaste
            // 
            this.RightClickPaste.Name = "RightClickPaste";
            this.RightClickPaste.Size = new System.Drawing.Size(122, 22);
            this.RightClickPaste.Text = "Paste";
            this.RightClickPaste.Click += new System.EventHandler(this.RightClickPaste_Click);
            // 
            // RightClickSelectAll
            // 
            this.RightClickSelectAll.Name = "RightClickSelectAll";
            this.RightClickSelectAll.Size = new System.Drawing.Size(122, 22);
            this.RightClickSelectAll.Text = "Select All";
            this.RightClickSelectAll.Click += new System.EventHandler(this.RightClickSelectAll_Click);
            // 
            // SettingsButton
            // 
            this.SettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsButton.FlatAppearance.BorderSize = 0;
            this.SettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SettingsButton.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SettingsButton.Location = new System.Drawing.Point(340, 40);
            this.SettingsButton.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsButton.MaximumSize = new System.Drawing.Size(40, 28);
            this.SettingsButton.MinimumSize = new System.Drawing.Size(40, 28);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(40, 28);
            this.SettingsButton.TabIndex = 6;
            this.SettingsButton.Text = "⚙";
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // InterpretLabel
            // 
            this.InterpretLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InterpretLabel.AutoSize = true;
            this.InterpretLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.InterpretLabel.Location = new System.Drawing.Point(20, 76);
            this.InterpretLabel.Margin = new System.Windows.Forms.Padding(0);
            this.InterpretLabel.MaximumSize = new System.Drawing.Size(297, 15);
            this.InterpretLabel.MinimumSize = new System.Drawing.Size(0, 15);
            this.InterpretLabel.Name = "InterpretLabel";
            this.InterpretLabel.Size = new System.Drawing.Size(0, 15);
            this.InterpretLabel.TabIndex = 7;
            this.InterpretLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.InterpretLabel.TextChanged += new System.EventHandler(this.InterpretLabel_TextChanged);
            this.InterpretLabel.DoubleClick += new System.EventHandler(this.InterpretLabel_DoubleClick);
            this.InterpretLabel.MouseLeave += new System.EventHandler(this.InterpretLabel_MouseLeave);
            this.InterpretLabel.MouseHover += new System.EventHandler(this.InterpretLabel_MouseHover);
            // 
            // InterpretToolTip
            // 
            this.InterpretToolTip.AutomaticDelay = 0;
            this.InterpretToolTip.BackColor = System.Drawing.SystemColors.Window;
            this.InterpretToolTip.ForeColor = System.Drawing.SystemColors.ControlText;
            this.InterpretToolTip.OwnerDraw = true;
            this.InterpretToolTip.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.InterpretToolTip_Draw);
            // 
            // SortButton
            // 
            this.SortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SortButton.FlatAppearance.BorderSize = 0;
            this.SortButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SortButton.Location = new System.Drawing.Point(317, 72);
            this.SortButton.Name = "SortButton";
            this.SortButton.Size = new System.Drawing.Size(63, 25);
            this.SortButton.TabIndex = 8;
            this.SortButton.Text = "↕ Sort By";
            this.SortButton.UseVisualStyleBackColor = true;
            this.SortButton.Click += new System.EventHandler(this.SortButton_Click);
            // 
            // SortMenu
            // 
            this.SortMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SortAscending,
            this.SortDescending,
            this.SortSeparator,
            this.SortUnit,
            this.SortMagnitude});
            this.SortMenu.Name = "SortMenu";
            this.SortMenu.ShowCheckMargin = true;
            this.SortMenu.ShowImageMargin = false;
            this.SortMenu.Size = new System.Drawing.Size(181, 120);
            this.SortMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.SortMenu_Closed);
            // 
            // SortAscending
            // 
            this.SortAscending.CheckOnClick = true;
            this.SortAscending.Name = "SortAscending";
            this.SortAscending.Size = new System.Drawing.Size(180, 22);
            this.SortAscending.Text = "Ascending";
            this.SortAscending.Click += new System.EventHandler(this.SortAscending_Click);
            // 
            // SortDescending
            // 
            this.SortDescending.CheckOnClick = true;
            this.SortDescending.Name = "SortDescending";
            this.SortDescending.Size = new System.Drawing.Size(180, 22);
            this.SortDescending.Text = "Descending";
            this.SortDescending.Click += new System.EventHandler(this.SortDescending_Click);
            // 
            // SortSeparator
            // 
            this.SortSeparator.Name = "SortSeparator";
            this.SortSeparator.Size = new System.Drawing.Size(177, 6);
            // 
            // SortUnit
            // 
            this.SortUnit.CheckOnClick = true;
            this.SortUnit.Name = "SortUnit";
            this.SortUnit.Size = new System.Drawing.Size(180, 22);
            this.SortUnit.Text = "Unit";
            this.SortUnit.Click += new System.EventHandler(this.SortUnit_Click);
            // 
            // SortMagnitude
            // 
            this.SortMagnitude.CheckOnClick = true;
            this.SortMagnitude.Name = "SortMagnitude";
            this.SortMagnitude.Size = new System.Drawing.Size(180, 22);
            this.SortMagnitude.Text = "Magnitude";
            this.SortMagnitude.Click += new System.EventHandler(this.SortMagnitude_Click);
            // 
            // SearchView
            // 
            this.SearchView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SearchView.ContextMenuStrip = this.RightClickMenu;
            this.SearchView.FullRowSelect = true;
            this.SearchView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.SearchView.Location = new System.Drawing.Point(20, 101);
            this.SearchView.Margin = new System.Windows.Forms.Padding(0);
            this.SearchView.Name = "SearchView";
            this.SearchView.Size = new System.Drawing.Size(360, 232);
            this.SearchView.TabIndex = 9;
            this.SearchView.UseCompatibleStateImageBehavior = false;
            this.SearchView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.SearchView_ColumnWidthChanged);
            this.SearchView.ItemActivate += new System.EventHandler(this.SearchView_ItemActivate);
            this.SearchView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchView_KeyDown);
            this.SearchView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SearchView_MouseUp);
            // 
            // AboutDisplay
            // 
            this.AboutDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AboutDisplay.Controls.Add(this.DescriptionText);
            this.AboutDisplay.Controls.Add(this.ReturnButton);
            this.AboutDisplay.Location = new System.Drawing.Point(20, 101);
            this.AboutDisplay.Name = "AboutDisplay";
            this.AboutDisplay.Size = new System.Drawing.Size(360, 232);
            this.AboutDisplay.TabIndex = 10;
            this.AboutDisplay.Visible = false;
            // 
            // DescriptionText
            // 
            this.DescriptionText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionText.BackColor = System.Drawing.Color.White;
            this.DescriptionText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DescriptionText.ContextMenuStrip = this.RightClickMenu;
            this.DescriptionText.Location = new System.Drawing.Point(10, 10);
            this.DescriptionText.Name = "DescriptionText";
            this.DescriptionText.ReadOnly = true;
            this.DescriptionText.Size = new System.Drawing.Size(340, 188);
            this.DescriptionText.TabIndex = 11;
            this.DescriptionText.Text = "";
            this.DescriptionText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DescriptionText_MouseUp);
            // 
            // ReturnButton
            // 
            this.ReturnButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ReturnButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReturnButton.Location = new System.Drawing.Point(153, 204);
            this.ReturnButton.Margin = new System.Windows.Forms.Padding(0);
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.Size = new System.Drawing.Size(54, 24);
            this.ReturnButton.TabIndex = 12;
            this.ReturnButton.Text = "Return";
            this.ReturnButton.UseVisualStyleBackColor = true;
            this.ReturnButton.Click += new System.EventHandler(this.ReturnButton_Click);
            // 
            // SettingsPanel
            // 
            this.SettingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsPanel.AutoScroll = true;
            this.SettingsPanel.AutoScrollMinSize = new System.Drawing.Size(0, 770);
            this.SettingsPanel.Controls.Add(this.SettingsLabel);
            this.SettingsPanel.Controls.Add(this.GeneralSettingsLabel);
            this.SettingsPanel.Controls.Add(this.PositionCheckbox);
            this.SettingsPanel.Controls.Add(this.SizeCheckbox);
            this.SettingsPanel.Controls.Add(this.CurrencyCheckbox);
            this.SettingsPanel.Controls.Add(this.ConversionSettingsLabel);
            this.SettingsPanel.Controls.Add(this.SignificantFiguresLabel);
            this.SettingsPanel.Controls.Add(this.SignificantFiguresEntry);
            this.SettingsPanel.Controls.Add(this.DecimalSeparatorLabel);
            this.SettingsPanel.Controls.Add(this.DecimalSeparatorEntry);
            this.SettingsPanel.Controls.Add(this.IntegerGroupSeparatorLabel);
            this.SettingsPanel.Controls.Add(this.IntegerGroupSeparatorEntry);
            this.SettingsPanel.Controls.Add(this.IntegerGroupSizeLabel);
            this.SettingsPanel.Controls.Add(this.IntegerGroupSizeEntry);
            this.SettingsPanel.Controls.Add(this.DecimalGroupSeparatorLabel);
            this.SettingsPanel.Controls.Add(this.DecimalGroupSeparatorEntry);
            this.SettingsPanel.Controls.Add(this.DecimalGroupSizeLabel);
            this.SettingsPanel.Controls.Add(this.DecimalGroupSizeEntry);
            this.SettingsPanel.Controls.Add(this.ScientificNotationLabel);
            this.SettingsPanel.Controls.Add(this.LargeMagnitudeLabel);
            this.SettingsPanel.Controls.Add(this.LargeMagnitudeEntry);
            this.SettingsPanel.Controls.Add(this.LargeExponentLabel);
            this.SettingsPanel.Controls.Add(this.LargeExponentEntry);
            this.SettingsPanel.Controls.Add(this.SmallMagnitudeLabel);
            this.SettingsPanel.Controls.Add(this.SmallMagnitudeEntry);
            this.SettingsPanel.Controls.Add(this.SmallExponentLabel);
            this.SettingsPanel.Controls.Add(this.SmallExponentEntry);
            this.SettingsPanel.Controls.Add(this.AppearLabel);
            this.SettingsPanel.Controls.Add(this.LightMode);
            this.SettingsPanel.Controls.Add(this.DarkMode);
            this.SettingsPanel.Controls.Add(this.SystemMode);
            this.SettingsPanel.Controls.Add(this.HelpLabel);
            this.SettingsPanel.Controls.Add(this.ExploreButton);
            this.SettingsPanel.Controls.Add(this.UpdateCurrencyButton);
            this.SettingsPanel.Controls.Add(this.CurrencyUpdateText);
            this.SettingsPanel.Controls.Add(this.AboutLabel);
            this.SettingsPanel.Controls.Add(this.AppNameLabel);
            this.SettingsPanel.Controls.Add(this.AuthorLabel);
            this.SettingsPanel.Controls.Add(this.SaveButton);
            this.SettingsPanel.Controls.Add(this.CancButton);
            this.SettingsPanel.Location = new System.Drawing.Point(1, 31);
            this.SettingsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsPanel.Name = "SettingsPanel";
            this.SettingsPanel.Size = new System.Drawing.Size(398, 318);
            this.SettingsPanel.TabIndex = 13;
            this.SettingsPanel.Visible = false;
            // 
            // SettingsLabel
            // 
            this.SettingsLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SettingsLabel.AutoSize = true;
            this.SettingsLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.SettingsLabel.Location = new System.Drawing.Point(21, 6);
            this.SettingsLabel.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsLabel.Name = "SettingsLabel";
            this.SettingsLabel.Size = new System.Drawing.Size(86, 28);
            this.SettingsLabel.TabIndex = 0;
            this.SettingsLabel.Text = "Settings";
            // 
            // GeneralSettingsLabel
            // 
            this.GeneralSettingsLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.GeneralSettingsLabel.AutoSize = true;
            this.GeneralSettingsLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.GeneralSettingsLabel.Location = new System.Drawing.Point(22, 45);
            this.GeneralSettingsLabel.Margin = new System.Windows.Forms.Padding(0);
            this.GeneralSettingsLabel.Name = "GeneralSettingsLabel";
            this.GeneralSettingsLabel.Size = new System.Drawing.Size(66, 21);
            this.GeneralSettingsLabel.TabIndex = 1;
            this.GeneralSettingsLabel.Text = "General";
            // 
            // PositionCheckbox
            // 
            this.PositionCheckbox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PositionCheckbox.AutoSize = true;
            this.PositionCheckbox.Location = new System.Drawing.Point(28, 80);
            this.PositionCheckbox.Margin = new System.Windows.Forms.Padding(0);
            this.PositionCheckbox.Name = "PositionCheckbox";
            this.PositionCheckbox.Size = new System.Drawing.Size(177, 19);
            this.PositionCheckbox.TabIndex = 2;
            this.PositionCheckbox.Text = "Remember Window Position";
            this.PositionCheckbox.UseVisualStyleBackColor = true;
            // 
            // SizeCheckbox
            // 
            this.SizeCheckbox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SizeCheckbox.AutoSize = true;
            this.SizeCheckbox.Location = new System.Drawing.Point(212, 80);
            this.SizeCheckbox.Margin = new System.Windows.Forms.Padding(0);
            this.SizeCheckbox.Name = "SizeCheckbox";
            this.SizeCheckbox.Size = new System.Drawing.Size(154, 19);
            this.SizeCheckbox.TabIndex = 3;
            this.SizeCheckbox.Text = "Remember Window Size";
            this.SizeCheckbox.UseVisualStyleBackColor = true;
            // 
            // CurrencyCheckbox
            // 
            this.CurrencyCheckbox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CurrencyCheckbox.AutoSize = true;
            this.CurrencyCheckbox.Location = new System.Drawing.Point(28, 115);
            this.CurrencyCheckbox.Margin = new System.Windows.Forms.Padding(0);
            this.CurrencyCheckbox.Name = "CurrencyCheckbox";
            this.CurrencyCheckbox.Size = new System.Drawing.Size(178, 19);
            this.CurrencyCheckbox.TabIndex = 4;
            this.CurrencyCheckbox.Text = "Update currencies on startup";
            this.CurrencyCheckbox.UseVisualStyleBackColor = true;
            // 
            // ConversionSettingsLabel
            // 
            this.ConversionSettingsLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ConversionSettingsLabel.AutoSize = true;
            this.ConversionSettingsLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ConversionSettingsLabel.Location = new System.Drawing.Point(22, 150);
            this.ConversionSettingsLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ConversionSettingsLabel.Name = "ConversionSettingsLabel";
            this.ConversionSettingsLabel.Size = new System.Drawing.Size(99, 21);
            this.ConversionSettingsLabel.TabIndex = 5;
            this.ConversionSettingsLabel.Text = "Conversions";
            // 
            // SignificantFiguresLabel
            // 
            this.SignificantFiguresLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SignificantFiguresLabel.AutoSize = true;
            this.SignificantFiguresLabel.Location = new System.Drawing.Point(22, 185);
            this.SignificantFiguresLabel.Margin = new System.Windows.Forms.Padding(0);
            this.SignificantFiguresLabel.Name = "SignificantFiguresLabel";
            this.SignificantFiguresLabel.Size = new System.Drawing.Size(99, 15);
            this.SignificantFiguresLabel.TabIndex = 6;
            this.SignificantFiguresLabel.Text = "Significant Digits:";
            // 
            // SignificantFiguresEntry
            // 
            this.SignificantFiguresEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SignificantFiguresEntry.Location = new System.Drawing.Point(122, 183);
            this.SignificantFiguresEntry.Margin = new System.Windows.Forms.Padding(0);
            this.SignificantFiguresEntry.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SignificantFiguresEntry.Name = "SignificantFiguresEntry";
            this.SignificantFiguresEntry.Size = new System.Drawing.Size(45, 23);
            this.SignificantFiguresEntry.TabIndex = 7;
            this.SignificantFiguresEntry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SignificantFiguresEntry.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // DecimalSeparatorLabel
            // 
            this.DecimalSeparatorLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DecimalSeparatorLabel.AutoSize = true;
            this.DecimalSeparatorLabel.Location = new System.Drawing.Point(179, 185);
            this.DecimalSeparatorLabel.Name = "DecimalSeparatorLabel";
            this.DecimalSeparatorLabel.Size = new System.Drawing.Size(106, 15);
            this.DecimalSeparatorLabel.TabIndex = 8;
            this.DecimalSeparatorLabel.Text = "Decimal Separator:";
            // 
            // DecimalSeparatorEntry
            // 
            this.DecimalSeparatorEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DecimalSeparatorEntry.Location = new System.Drawing.Point(288, 183);
            this.DecimalSeparatorEntry.Margin = new System.Windows.Forms.Padding(0);
            this.DecimalSeparatorEntry.MaxLength = 1;
            this.DecimalSeparatorEntry.Name = "DecimalSeparatorEntry";
            this.DecimalSeparatorEntry.Size = new System.Drawing.Size(15, 23);
            this.DecimalSeparatorEntry.TabIndex = 9;
            this.DecimalSeparatorEntry.Text = ".";
            this.DecimalSeparatorEntry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // IntegerGroupSeparatorLabel
            // 
            this.IntegerGroupSeparatorLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IntegerGroupSeparatorLabel.AutoSize = true;
            this.IntegerGroupSeparatorLabel.Location = new System.Drawing.Point(22, 220);
            this.IntegerGroupSeparatorLabel.Margin = new System.Windows.Forms.Padding(0);
            this.IntegerGroupSeparatorLabel.Name = "IntegerGroupSeparatorLabel";
            this.IntegerGroupSeparatorLabel.Size = new System.Drawing.Size(153, 15);
            this.IntegerGroupSeparatorLabel.TabIndex = 10;
            this.IntegerGroupSeparatorLabel.Text = "Integer Grouping Separator:";
            // 
            // IntegerGroupSeparatorEntry
            // 
            this.IntegerGroupSeparatorEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IntegerGroupSeparatorEntry.Location = new System.Drawing.Point(183, 218);
            this.IntegerGroupSeparatorEntry.Margin = new System.Windows.Forms.Padding(0);
            this.IntegerGroupSeparatorEntry.MaxLength = 1;
            this.IntegerGroupSeparatorEntry.Name = "IntegerGroupSeparatorEntry";
            this.IntegerGroupSeparatorEntry.Size = new System.Drawing.Size(15, 23);
            this.IntegerGroupSeparatorEntry.TabIndex = 11;
            this.IntegerGroupSeparatorEntry.Text = ",";
            this.IntegerGroupSeparatorEntry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // IntegerGroupSizeLabel
            // 
            this.IntegerGroupSizeLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IntegerGroupSizeLabel.AutoSize = true;
            this.IntegerGroupSizeLabel.Location = new System.Drawing.Point(212, 220);
            this.IntegerGroupSizeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.IntegerGroupSizeLabel.Name = "IntegerGroupSizeLabel";
            this.IntegerGroupSizeLabel.Size = new System.Drawing.Size(106, 15);
            this.IntegerGroupSizeLabel.TabIndex = 12;
            this.IntegerGroupSizeLabel.Text = "Integer Group Size:";
            // 
            // IntegerGroupSizeEntry
            // 
            this.IntegerGroupSizeEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IntegerGroupSizeEntry.Location = new System.Drawing.Point(327, 218);
            this.IntegerGroupSizeEntry.Margin = new System.Windows.Forms.Padding(0);
            this.IntegerGroupSizeEntry.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.IntegerGroupSizeEntry.Name = "IntegerGroupSizeEntry";
            this.IntegerGroupSizeEntry.Size = new System.Drawing.Size(29, 23);
            this.IntegerGroupSizeEntry.TabIndex = 13;
            this.IntegerGroupSizeEntry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IntegerGroupSizeEntry.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // DecimalGroupSeparatorLabel
            // 
            this.DecimalGroupSeparatorLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DecimalGroupSeparatorLabel.AutoSize = true;
            this.DecimalGroupSeparatorLabel.Location = new System.Drawing.Point(22, 255);
            this.DecimalGroupSeparatorLabel.Margin = new System.Windows.Forms.Padding(0);
            this.DecimalGroupSeparatorLabel.Name = "DecimalGroupSeparatorLabel";
            this.DecimalGroupSeparatorLabel.Size = new System.Drawing.Size(159, 15);
            this.DecimalGroupSeparatorLabel.TabIndex = 14;
            this.DecimalGroupSeparatorLabel.Text = "Decimal Grouping Separator:";
            // 
            // DecimalGroupSeparatorEntry
            // 
            this.DecimalGroupSeparatorEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DecimalGroupSeparatorEntry.Location = new System.Drawing.Point(183, 253);
            this.DecimalGroupSeparatorEntry.Margin = new System.Windows.Forms.Padding(0);
            this.DecimalGroupSeparatorEntry.MaxLength = 1;
            this.DecimalGroupSeparatorEntry.Name = "DecimalGroupSeparatorEntry";
            this.DecimalGroupSeparatorEntry.Size = new System.Drawing.Size(15, 23);
            this.DecimalGroupSeparatorEntry.TabIndex = 15;
            this.DecimalGroupSeparatorEntry.Text = " ";
            this.DecimalGroupSeparatorEntry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DecimalGroupSizeLabel
            // 
            this.DecimalGroupSizeLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DecimalGroupSizeLabel.AutoSize = true;
            this.DecimalGroupSizeLabel.Location = new System.Drawing.Point(212, 255);
            this.DecimalGroupSizeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.DecimalGroupSizeLabel.Name = "DecimalGroupSizeLabel";
            this.DecimalGroupSizeLabel.Size = new System.Drawing.Size(112, 15);
            this.DecimalGroupSizeLabel.TabIndex = 16;
            this.DecimalGroupSizeLabel.Text = "Decimal Group Size:";
            // 
            // DecimalGroupSizeEntry
            // 
            this.DecimalGroupSizeEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DecimalGroupSizeEntry.Location = new System.Drawing.Point(327, 253);
            this.DecimalGroupSizeEntry.Margin = new System.Windows.Forms.Padding(0);
            this.DecimalGroupSizeEntry.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.DecimalGroupSizeEntry.Name = "DecimalGroupSizeEntry";
            this.DecimalGroupSizeEntry.Size = new System.Drawing.Size(29, 23);
            this.DecimalGroupSizeEntry.TabIndex = 17;
            this.DecimalGroupSizeEntry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DecimalGroupSizeEntry.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // ScientificNotationLabel
            // 
            this.ScientificNotationLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ScientificNotationLabel.AutoSize = true;
            this.ScientificNotationLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ScientificNotationLabel.Location = new System.Drawing.Point(22, 290);
            this.ScientificNotationLabel.Name = "ScientificNotationLabel";
            this.ScientificNotationLabel.Size = new System.Drawing.Size(146, 21);
            this.ScientificNotationLabel.TabIndex = 18;
            this.ScientificNotationLabel.Text = "Scientific Notation";
            // 
            // LargeMagnitudeLabel
            // 
            this.LargeMagnitudeLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LargeMagnitudeLabel.AutoSize = true;
            this.LargeMagnitudeLabel.Location = new System.Drawing.Point(22, 325);
            this.LargeMagnitudeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.LargeMagnitudeLabel.Name = "LargeMagnitudeLabel";
            this.LargeMagnitudeLabel.Size = new System.Drawing.Size(185, 15);
            this.LargeMagnitudeLabel.TabIndex = 19;
            this.LargeMagnitudeLabel.Text = "Use For Magnitudes Greater Than:";
            // 
            // LargeMagnitudeEntry
            // 
            this.LargeMagnitudeEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LargeMagnitudeEntry.Location = new System.Drawing.Point(213, 323);
            this.LargeMagnitudeEntry.Margin = new System.Windows.Forms.Padding(0);
            this.LargeMagnitudeEntry.Maximum = new decimal(new int[] {
            -1304428545,
            434162106,
            542,
            0});
            this.LargeMagnitudeEntry.Name = "LargeMagnitudeEntry";
            this.LargeMagnitudeEntry.Size = new System.Drawing.Size(90, 23);
            this.LargeMagnitudeEntry.TabIndex = 20;
            this.LargeMagnitudeEntry.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // LargeExponentLabel
            // 
            this.LargeExponentLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LargeExponentLabel.AutoSize = true;
            this.LargeExponentLabel.Location = new System.Drawing.Point(307, 325);
            this.LargeExponentLabel.Name = "LargeExponentLabel";
            this.LargeExponentLabel.Size = new System.Drawing.Size(24, 15);
            this.LargeExponentLabel.TabIndex = 21;
            this.LargeExponentLabel.Text = "E +";
            // 
            // LargeExponentEntry
            // 
            this.LargeExponentEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LargeExponentEntry.Location = new System.Drawing.Point(331, 323);
            this.LargeExponentEntry.Margin = new System.Windows.Forms.Padding(0);
            this.LargeExponentEntry.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.LargeExponentEntry.Name = "LargeExponentEntry";
            this.LargeExponentEntry.Size = new System.Drawing.Size(45, 23);
            this.LargeExponentEntry.TabIndex = 22;
            this.LargeExponentEntry.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // SmallMagnitudeLabel
            // 
            this.SmallMagnitudeLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SmallMagnitudeLabel.AutoSize = true;
            this.SmallMagnitudeLabel.Location = new System.Drawing.Point(22, 360);
            this.SmallMagnitudeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.SmallMagnitudeLabel.Name = "SmallMagnitudeLabel";
            this.SmallMagnitudeLabel.Size = new System.Drawing.Size(169, 15);
            this.SmallMagnitudeLabel.TabIndex = 23;
            this.SmallMagnitudeLabel.Text = "Use For Magnitudes Less Than:";
            // 
            // SmallMagnitudeEntry
            // 
            this.SmallMagnitudeEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SmallMagnitudeEntry.Location = new System.Drawing.Point(213, 358);
            this.SmallMagnitudeEntry.Margin = new System.Windows.Forms.Padding(0);
            this.SmallMagnitudeEntry.Maximum = new decimal(new int[] {
            -1304428545,
            434162106,
            542,
            0});
            this.SmallMagnitudeEntry.Name = "SmallMagnitudeEntry";
            this.SmallMagnitudeEntry.Size = new System.Drawing.Size(90, 23);
            this.SmallMagnitudeEntry.TabIndex = 24;
            this.SmallMagnitudeEntry.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SmallExponentLabel
            // 
            this.SmallExponentLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SmallExponentLabel.AutoSize = true;
            this.SmallExponentLabel.Location = new System.Drawing.Point(307, 360);
            this.SmallExponentLabel.Name = "SmallExponentLabel";
            this.SmallExponentLabel.Size = new System.Drawing.Size(21, 15);
            this.SmallExponentLabel.TabIndex = 25;
            this.SmallExponentLabel.Text = "E -";
            // 
            // SmallExponentEntry
            // 
            this.SmallExponentEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SmallExponentEntry.Location = new System.Drawing.Point(331, 358);
            this.SmallExponentEntry.Margin = new System.Windows.Forms.Padding(0);
            this.SmallExponentEntry.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.SmallExponentEntry.Name = "SmallExponentEntry";
            this.SmallExponentEntry.Size = new System.Drawing.Size(45, 23);
            this.SmallExponentEntry.TabIndex = 26;
            this.SmallExponentEntry.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // AppearLabel
            // 
            this.AppearLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AppearLabel.AutoSize = true;
            this.AppearLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.AppearLabel.Location = new System.Drawing.Point(22, 395);
            this.AppearLabel.Margin = new System.Windows.Forms.Padding(0);
            this.AppearLabel.Name = "AppearLabel";
            this.AppearLabel.Size = new System.Drawing.Size(98, 21);
            this.AppearLabel.TabIndex = 27;
            this.AppearLabel.Text = "Appearance";
            // 
            // LightMode
            // 
            this.LightMode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LightMode.AutoSize = true;
            this.LightMode.Location = new System.Drawing.Point(28, 430);
            this.LightMode.Margin = new System.Windows.Forms.Padding(0);
            this.LightMode.Name = "LightMode";
            this.LightMode.Size = new System.Drawing.Size(52, 19);
            this.LightMode.TabIndex = 28;
            this.LightMode.TabStop = true;
            this.LightMode.Text = "Light";
            this.LightMode.UseVisualStyleBackColor = true;
            // 
            // DarkMode
            // 
            this.DarkMode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DarkMode.AutoSize = true;
            this.DarkMode.Location = new System.Drawing.Point(28, 465);
            this.DarkMode.Margin = new System.Windows.Forms.Padding(0);
            this.DarkMode.Name = "DarkMode";
            this.DarkMode.Size = new System.Drawing.Size(49, 19);
            this.DarkMode.TabIndex = 29;
            this.DarkMode.TabStop = true;
            this.DarkMode.Text = "Dark";
            this.DarkMode.UseVisualStyleBackColor = true;
            // 
            // SystemMode
            // 
            this.SystemMode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SystemMode.AutoSize = true;
            this.SystemMode.Checked = true;
            this.SystemMode.Location = new System.Drawing.Point(28, 500);
            this.SystemMode.Margin = new System.Windows.Forms.Padding(0);
            this.SystemMode.Name = "SystemMode";
            this.SystemMode.Size = new System.Drawing.Size(63, 19);
            this.SystemMode.TabIndex = 30;
            this.SystemMode.TabStop = true;
            this.SystemMode.Text = "System";
            this.SystemMode.UseVisualStyleBackColor = true;
            // 
            // HelpLabel
            // 
            this.HelpLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.HelpLabel.AutoSize = true;
            this.HelpLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.HelpLabel.Location = new System.Drawing.Point(22, 535);
            this.HelpLabel.Margin = new System.Windows.Forms.Padding(0);
            this.HelpLabel.Name = "HelpLabel";
            this.HelpLabel.Size = new System.Drawing.Size(45, 21);
            this.HelpLabel.TabIndex = 31;
            this.HelpLabel.Text = "Help";
            // 
            // ExploreButton
            // 
            this.ExploreButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ExploreButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExploreButton.Location = new System.Drawing.Point(26, 570);
            this.ExploreButton.Name = "ExploreButton";
            this.ExploreButton.Size = new System.Drawing.Size(109, 24);
            this.ExploreButton.TabIndex = 32;
            this.ExploreButton.Text = "Explore All Units";
            this.ExploreButton.UseVisualStyleBackColor = true;
            this.ExploreButton.Click += new System.EventHandler(this.ExploreButton_Click);
            // 
            // UpdateCurrencyButton
            // 
            this.UpdateCurrencyButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.UpdateCurrencyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateCurrencyButton.Location = new System.Drawing.Point(26, 605);
            this.UpdateCurrencyButton.Name = "UpdateCurrencyButton";
            this.UpdateCurrencyButton.Size = new System.Drawing.Size(122, 24);
            this.UpdateCurrencyButton.TabIndex = 34;
            this.UpdateCurrencyButton.Text = "Update Currencies";
            this.UpdateCurrencyButton.UseVisualStyleBackColor = true;
            this.UpdateCurrencyButton.Click += new System.EventHandler(this.UpdateCurrencyButton_Click);
            // 
            // CurrencyUpdateText
            // 
            this.CurrencyUpdateText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CurrencyUpdateText.AutoSize = true;
            this.CurrencyUpdateText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CurrencyUpdateText.Location = new System.Drawing.Point(153, 610);
            this.CurrencyUpdateText.Name = "CurrencyUpdateText";
            this.CurrencyUpdateText.Size = new System.Drawing.Size(0, 15);
            this.CurrencyUpdateText.TabIndex = 39;
            // 
            // AboutLabel
            // 
            this.AboutLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AboutLabel.AutoSize = true;
            this.AboutLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.AboutLabel.Location = new System.Drawing.Point(22, 640);
            this.AboutLabel.Margin = new System.Windows.Forms.Padding(0);
            this.AboutLabel.Name = "AboutLabel";
            this.AboutLabel.Size = new System.Drawing.Size(56, 21);
            this.AboutLabel.TabIndex = 40;
            this.AboutLabel.Text = "About";
            // 
            // AppNameLabel
            // 
            this.AppNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AppNameLabel.AutoSize = true;
            this.AppNameLabel.Location = new System.Drawing.Point(22, 669);
            this.AppNameLabel.Margin = new System.Windows.Forms.Padding(0);
            this.AppNameLabel.Name = "AppNameLabel";
            this.AppNameLabel.Size = new System.Drawing.Size(59, 15);
            this.AppNameLabel.TabIndex = 41;
            this.AppNameLabel.Text = "Unitversal";
            // 
            // AuthorLabel
            // 
            this.AuthorLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AuthorLabel.AutoSize = true;
            this.AuthorLabel.Location = new System.Drawing.Point(22, 689);
            this.AuthorLabel.Margin = new System.Windows.Forms.Padding(0);
            this.AuthorLabel.Name = "AuthorLabel";
            this.AuthorLabel.Size = new System.Drawing.Size(161, 15);
            this.AuthorLabel.TabIndex = 42;
            this.AuthorLabel.Text = "© 2022 Member Of The Reals";
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Location = new System.Drawing.Point(126, 725);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 24);
            this.SaveButton.TabIndex = 35;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CancButton
            // 
            this.CancButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CancButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancButton.Location = new System.Drawing.Point(211, 725);
            this.CancButton.Name = "CancButton";
            this.CancButton.Size = new System.Drawing.Size(75, 24);
            this.CancButton.TabIndex = 36;
            this.CancButton.Text = "Cancel";
            this.CancButton.UseVisualStyleBackColor = true;
            this.CancButton.Click += new System.EventHandler(this.CancButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.ControlBox = false;
            this.Controls.Add(this.SettingsPanel);
            this.Controls.Add(this.AboutDisplay);
            this.Controls.Add(this.SearchView);
            this.Controls.Add(this.SortButton);
            this.Controls.Add(this.InterpretLabel);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.TitleBar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 350);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unitversal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.LocationChanged += new System.EventHandler(this.MainWindow_LocationChanged);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseClick);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.TitleBar.ResumeLayout(false);
            this.TitleBar.PerformLayout();
            this.RightClickMenu.ResumeLayout(false);
            this.SortMenu.ResumeLayout(false);
            this.AboutDisplay.ResumeLayout(false);
            this.SettingsPanel.ResumeLayout(false);
            this.SettingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SignificantFiguresEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IntegerGroupSizeEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DecimalGroupSizeEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeMagnitudeEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LargeExponentEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallMagnitudeEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmallExponentEntry)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button MaximizeButton;
        private System.Windows.Forms.Button MinimizeButton;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Panel TitleBar;
        private System.Windows.Forms.TextBox SearchBox;
        private System.Windows.Forms.ContextMenuStrip RightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem RightClickOpen;
        private System.Windows.Forms.ToolStripMenuItem RightClickCut;
        private System.Windows.Forms.ToolStripMenuItem RightClickCopy;
        private System.Windows.Forms.ToolStripMenuItem RightClickPaste;
        private System.Windows.Forms.ToolStripMenuItem RightClickSelectAll;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.Label InterpretLabel;
        private System.Windows.Forms.ToolTip InterpretToolTip;
        private System.Windows.Forms.Button SortButton;
        private System.Windows.Forms.ContextMenuStrip SortMenu;
        private System.Windows.Forms.ToolStripMenuItem SortAscending;
        private System.Windows.Forms.ToolStripMenuItem SortDescending;
        private System.Windows.Forms.ToolStripSeparator SortSeparator;
        private System.Windows.Forms.ToolStripMenuItem SortUnit;
        private System.Windows.Forms.ToolStripMenuItem SortMagnitude;
        private System.Windows.Forms.ListView SearchView;
        private System.Windows.Forms.Panel AboutDisplay;
        private System.Windows.Forms.RichTextBox DescriptionText;
        private System.Windows.Forms.Button ReturnButton;
        private System.Windows.Forms.Panel SettingsPanel;
        private System.Windows.Forms.Label SettingsLabel;
        private System.Windows.Forms.Label GeneralSettingsLabel;
        private System.Windows.Forms.CheckBox PositionCheckbox;
        private System.Windows.Forms.CheckBox SizeCheckbox;
        private System.Windows.Forms.CheckBox CurrencyCheckbox;
        private System.Windows.Forms.Label ConversionSettingsLabel;
        private System.Windows.Forms.Label SignificantFiguresLabel;
        private System.Windows.Forms.NumericUpDown SignificantFiguresEntry;
        private System.Windows.Forms.Label DecimalSeparatorLabel;
        private System.Windows.Forms.TextBox DecimalSeparatorEntry;
        private System.Windows.Forms.Label IntegerGroupSeparatorLabel;
        private System.Windows.Forms.TextBox IntegerGroupSeparatorEntry;
        private System.Windows.Forms.Label IntegerGroupSizeLabel;
        private System.Windows.Forms.NumericUpDown IntegerGroupSizeEntry;
        private System.Windows.Forms.Label DecimalGroupSeparatorLabel;
        private System.Windows.Forms.TextBox DecimalGroupSeparatorEntry;
        private System.Windows.Forms.Label DecimalGroupSizeLabel;
        private System.Windows.Forms.NumericUpDown DecimalGroupSizeEntry;
        private System.Windows.Forms.Label ScientificNotationLabel;
        private System.Windows.Forms.Label LargeMagnitudeLabel;
        private System.Windows.Forms.NumericUpDown LargeMagnitudeEntry;
        private System.Windows.Forms.Label LargeExponentLabel;
        private System.Windows.Forms.NumericUpDown LargeExponentEntry;
        private System.Windows.Forms.Label SmallMagnitudeLabel;
        private System.Windows.Forms.NumericUpDown SmallMagnitudeEntry;
        private System.Windows.Forms.Label SmallExponentLabel;
        private System.Windows.Forms.NumericUpDown SmallExponentEntry;
        private System.Windows.Forms.Label AppearLabel;
        private System.Windows.Forms.RadioButton LightMode;
        private System.Windows.Forms.RadioButton DarkMode;
        private System.Windows.Forms.RadioButton SystemMode;
        private System.Windows.Forms.Label HelpLabel;
        private System.Windows.Forms.Button ExploreButton;
        private System.Windows.Forms.Button UpdateCurrencyButton;
        private System.Windows.Forms.Label CurrencyUpdateText;
        private System.Windows.Forms.Label AboutLabel;
        private System.Windows.Forms.Label AppNameLabel;
        private System.Windows.Forms.Label AuthorLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CancButton;
    }
}
