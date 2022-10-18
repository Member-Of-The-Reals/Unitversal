using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Unitversal
{
    public partial class MainWindow : Form
    {
        //Custom context menu colors
        public class ContextMenuColor : ProfessionalColorTable
        {
            //Makes the context menu margin color the same as background color
            public override Color ImageMarginGradientBegin
            {
                get { return AppState.BackgroundColor; }
            }
            public override Color ImageMarginGradientMiddle
            {
                get { return AppState.BackgroundColor; }
            }
            public override Color ImageMarginGradientEnd
            {
                get { return AppState.BackgroundColor; }
            }
            //Context menu highlight color
            public override Color MenuItemSelected
            {
                get { return AppState.MenuHighlight; }
            }
            public override Color MenuItemSelectedGradientBegin
            {
                get { return AppState.MenuHighlight; }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return AppState.MenuHighlight; }
            }
        }
        //Custom context menu renderer
        public class ContextMenuRenderer : ToolStripProfessionalRenderer
        {
            public ContextMenuRenderer() : base(new ContextMenuColor())
            {
            }
            //Custom checkmark image with flat appearance
            protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
            {
                //Anti-alias checkmark image
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //Image rectangle
                var R = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
                //Make rectangle smaller
                R.Inflate(-3, -4);
                //Drawing checkmark
                e.Graphics.DrawLines(AppState.SystemColorPen, new Point[]{
                    new Point(R.Left, R.Bottom - R.Height/2),
                    new Point(R.Left + R.Width/3,  R.Bottom),
                    new Point(R.Right, R.Top)
                });
            }
        }
        //Change color of all controls in a window
        private void ChangeColor(Control Window)
        {
            //Change color of every control
            foreach (Control Item in Window.Controls)
            {
                Item.BackColor = AppState.BackgroundColor;
                Item.ForeColor = AppState.ForegroundColor;
                //Recursively change all items in a panel
                if (Item is Panel)
                {
                    ChangeColor(Item);
                }
            }
        }
        private void ChangeTheme(string Theme)
        {
            //System mode
            if (Theme == "SYSTEM")
            {
                //Try getting registry key
                try
                {
                    using (RegistryKey Key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize"))
                    {
                        if (Key is not null)
                        {
                            int? KeyValue = Key.GetValue("AppsUseLightTheme") as int?;
                            if (KeyValue is not null)
                            {
                                //0 means dark
                                if (KeyValue == 0)
                                {
                                    Theme = "DARK";
                                }
                                //1 means light; any other value should default to light
                                else
                                {
                                    Theme = "LIGHT";
                                }
                            }
                        }
                    }
                }
                //Any exception default to light
                catch (Exception)
                {
                    Theme = "LIGHT";
                }
            }
            //Define colors
            AppState.BackgroundColor = Theme == "DARK" ? Color.FromArgb(30, 30, 30) : SystemColors.Window;
            AppState.ForegroundColor = Theme == "DARK" ? SystemColors.Window : SystemColors.ControlText;
            AppState.MenuHighlight = Theme == "DARK" ? Color.FromArgb(0, 0, 50) : Color.FromArgb(179, 215, 243);
            AppState.SystemColorPen = Theme == "DARK" ? Pens.White : Pens.Black;
            //Change color of the main window
            BackColor = AppState.BackgroundColor;
            ForeColor = AppState.ForegroundColor;
            //Change color of right click menu
            RightClickMenu.BackColor = AppState.BackgroundColor;
            RightClickMenu.ForeColor = AppState.ForegroundColor;
            //Change color of sort menu
            SortMenu.BackColor = AppState.BackgroundColor;
            SortMenu.ForeColor = AppState.ForegroundColor;
            //Change color of tooltip
            InterpretToolTip.BackColor = AppState.BackgroundColor;
            InterpretToolTip.ForeColor = AppState.ForegroundColor;
            //Change color of all controls in main window
            ChangeColor(this);
        }
    }
}