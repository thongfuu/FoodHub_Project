using System.Drawing.Drawing2D;

namespace FoodHubCustomerUI
{
    internal static class CustomerUiTheme
    {
        public static readonly Color Background = Color.FromArgb(247, 248, 245);
        public static readonly Color Surface = Color.FromArgb(255, 255, 252);
        public static readonly Color SurfaceAlt = Color.FromArgb(241, 245, 239);
        public static readonly Color Primary = Color.FromArgb(45, 101, 82);
        public static readonly Color PrimaryHover = Color.FromArgb(34, 83, 66);
        public static readonly Color Accent = Color.FromArgb(189, 139, 72);
        public static readonly Color Danger = Color.FromArgb(178, 65, 65);
        public static readonly Color Text = Color.FromArgb(35, 45, 43);
        public static readonly Color MutedText = Color.FromArgb(103, 115, 111);
        public static readonly Color Border = Color.FromArgb(220, 226, 218);
        public static readonly Color Selection = Color.FromArgb(220, 235, 226);

        public static readonly Font TitleFont = new("Segoe UI", 20F, FontStyle.Bold);
        public static readonly Font SectionFont = new("Segoe UI", 13F, FontStyle.Bold);
        public static readonly Font BodyFont = new("Segoe UI", 10F);
        public static readonly Font SmallFont = new("Segoe UI", 9F);

        public static void ApplyForm(Form form, Size size, string title)
        {
            form.Text = title;
            form.BackColor = Background;
            form.Font = BodyFont;
            form.ClientSize = size;
            form.MinimumSize = new Size(Math.Min(size.Width, 900), Math.Min(size.Height + 39, 560));
            form.StartPosition = FormStartPosition.CenterScreen;
        }

        public static Panel CreateCard(string name, Rectangle bounds)
        {
            return new Panel
            {
                Name = name,
                BackColor = Surface,
                Location = bounds.Location,
                Size = bounds.Size,
                Padding = new Padding(18)
            };
        }

        public static Label CreateTitle(string text, Point location, int width)
        {
            return new Label
            {
                AutoSize = false,
                Text = text,
                Location = location,
                Size = new Size(width, 42),
                Font = TitleFont,
                ForeColor = Text
            };
        }

        public static Label CreateSubtitle(string text, Point location, int width)
        {
            return new Label
            {
                AutoSize = false,
                Text = text,
                Location = location,
                Size = new Size(width, 28),
                Font = BodyFont,
                ForeColor = MutedText
            };
        }

        public static void StylePrimaryButton(Button button)
        {
            StyleButton(button, Primary, Color.White);
            button.FlatAppearance.MouseOverBackColor = PrimaryHover;
        }

        public static void StyleSecondaryButton(Button button)
        {
            StyleButton(button, SurfaceAlt, Text);
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 237, 229);
        }

        public static void StyleDangerButton(Button button)
        {
            StyleButton(button, Danger, Color.White);
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(150, 50, 50);
        }

        public static void StyleInput(Control control)
        {
            control.Font = BodyFont;
            control.ForeColor = Text;
            control.BackColor = Color.White;
        }

        public static void StyleLabel(Label label, bool section = false)
        {
            label.Font = section ? SectionFont : BodyFont;
            label.ForeColor = section ? Text : MutedText;
        }

        public static void StyleMenu(MenuStrip menu)
        {
            menu.BackColor = Surface;
            menu.ForeColor = Text;
            menu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            menu.Padding = new Padding(24, 7, 0, 7);
            menu.Renderer = new ToolStripProfessionalRenderer(new SoftMenuColors());
        }

        public static void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = Surface;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Border;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            grid.ColumnHeadersHeight = 40;
            grid.RowTemplate.Height = 38;
            grid.DefaultCellStyle.BackColor = Surface;
            grid.DefaultCellStyle.ForeColor = Text;
            grid.DefaultCellStyle.SelectionBackColor = Selection;
            grid.DefaultCellStyle.SelectionForeColor = Text;
            grid.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 248);
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToResizeRows = false;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public static void PaintCard(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel) return;

            using var borderPen = new Pen(Border);
            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawRectangle(borderPen, rect);
        }

        private static void StyleButton(Button button, Color backColor, Color foreColor)
        {
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button.Height = Math.Max(button.Height, 40);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
        }

        private sealed class SoftMenuColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected => SurfaceAlt;
            public override Color MenuItemBorder => Border;
            public override Color MenuBorder => Border;
            public override Color ToolStripDropDownBackground => Surface;
            public override Color ImageMarginGradientBegin => Surface;
            public override Color ImageMarginGradientMiddle => Surface;
            public override Color ImageMarginGradientEnd => Surface;
        }
    }
}
