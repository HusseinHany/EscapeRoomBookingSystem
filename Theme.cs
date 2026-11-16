using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace EscapeRoomMS
{
    /// <summary>
    /// One look for every screen: a dark navy palette, rounded buttons with a
    /// border that lights up on hover and darkens on press, and tidy grids.
    /// Each form calls Theme.Apply(this) right after InitializeComponent().
    /// </summary>
    public static class Theme
    {
        // Palette
        public static readonly Color Background = Color.FromArgb(11, 16, 28);
        public static readonly Color Surface = Color.FromArgb(19, 28, 46);
        public static readonly Color SurfaceAlt = Color.FromArgb(25, 37, 60);
        public static readonly Color Border = Color.FromArgb(44, 62, 92);
        public static readonly Color Ink = Color.FromArgb(232, 238, 248);
        public static readonly Color InkMuted = Color.FromArgb(150, 168, 192);
        public static readonly Color Accent = Color.FromArgb(0, 168, 132);
        public static readonly Color AccentLight = Color.FromArgb(64, 224, 186);
        public static readonly Color Edit = Color.FromArgb(214, 150, 30);
        public static readonly Color Danger = Color.FromArgb(206, 60, 60);
        public static readonly Color Neutral = Color.FromArgb(38, 54, 82);

        private const int Radius = 10;

        public static void Apply(Form form)
        {
            form.BackColor = Background;
            EnableDoubleBuffering(form);
            ApplyToChildren(form.Controls);
        }

        private static void ApplyToChildren(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c is Button)
                {
                    StyleButton((Button)c);
                }
                else if (c is DataGridView)
                {
                    StyleGrid((DataGridView)c);
                }
                else if (c is TextBox)
                {
                    c.BackColor = SurfaceAlt;
                    c.ForeColor = Ink;
                }
                else if (c is ComboBox)
                {
                    ComboBox combo = (ComboBox)c;
                    combo.FlatStyle = FlatStyle.Flat;
                    combo.BackColor = SurfaceAlt;
                    combo.ForeColor = Ink;
                }
                else if (c is NumericUpDown)
                {
                    c.BackColor = SurfaceAlt;
                    c.ForeColor = Ink;
                }
                else if (c is Label)
                {
                    StyleLabel((Label)c);
                }
                else if (c is Panel || c is GroupBox)
                {
                    if (c.BackColor != Color.Transparent)
                    {
                        c.BackColor = Surface;
                        if (c is Panel)
                        {
                            c.Paint += PaintCardOutline;
                        }
                    }
                    if (c is GroupBox)
                    {
                        c.ForeColor = InkMuted;
                    }
                    EnableDoubleBuffering(c);
                }

                if (c.HasChildren)
                {
                    ApplyToChildren(c.Controls);
                }
            }
        }

        // Labels

        private static void StyleLabel(Label label)
        {
            // Large text (titles and headline numbers) gets the accent colour,
            // everything else the normal text colour.
            if (label.Font.Size >= 16f)
            {
                label.ForeColor = AccentLight;
            }
            else
            {
                label.ForeColor = Ink;
            }
        }

        // Panels

        private static void PaintCardOutline(object sender, PaintEventArgs e)
        {
            Control c = (Control)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(Border, 1f))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, c.Width - 1, c.Height - 1);
            }
        }

        // Buttons

        private static void StyleButton(Button b)
        {
            string name = b.Name.ToLowerInvariant();

            // The small "show password" eye sits inside the password box.
            if (name.Contains("show"))
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.FlatAppearance.MouseOverBackColor = SurfaceAlt;
                b.FlatAppearance.MouseDownBackColor = Border;
                b.BackColor = SurfaceAlt;
                b.ForeColor = Ink;
                b.Cursor = Cursors.Hand;
                return;
            }

            Color fill;
            if (name.Contains("delete") || name.Contains("logout") || name.Contains("remove"))
            {
                fill = Danger;
            }
            else if (name.Contains("update") || name.Contains("edit"))
            {
                fill = Edit;
            }
            else if (name.Contains("clear") || name.Contains("back") || name.Contains("refresh") || name.Contains("cancel"))
            {
                fill = Neutral;
            }
            else
            {
                fill = Accent;
            }

            b.UseVisualStyleBackColor = false;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = fill;
            b.ForeColor = fill == Edit ? Color.FromArgb(20, 20, 20) : Color.White;
            b.FlatAppearance.MouseOverBackColor = Mix(fill, Color.White, 0.14);
            b.FlatAppearance.MouseDownBackColor = Mix(fill, Color.Black, 0.22);
            b.Font = new Font("Segoe UI", b.Font.Size, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            b.TabStop = true;

            SetRoundedRegion(b);
            b.Resize += (s, e) => SetRoundedRegion((Button)s);
            b.Paint += PaintButtonBorder;
            b.MouseEnter += (s, e) => ((Control)s).Invalidate();
            b.MouseLeave += (s, e) => ((Control)s).Invalidate();
            b.GotFocus += (s, e) => ((Control)s).Invalidate();
            b.LostFocus += (s, e) => ((Control)s).Invalidate();
        }

        private static void PaintButtonBorder(object sender, PaintEventArgs e)
        {
            Button b = (Button)sender;
            bool hover = b.ClientRectangle.Contains(b.PointToClient(Cursor.Position));
            bool pressed = hover && (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left;

            Color border = Mix(b.BackColor, Color.White, hover ? 0.55 : 0.32);
            if (pressed)
            {
                border = Mix(b.BackColor, Color.Black, 0.35);
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(1, 1, b.Width - 3, b.Height - 3);
            using (GraphicsPath path = RoundedRect(r, Radius))
            using (Pen pen = new Pen(border, b.Focused ? 2.5f : 2f))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private static void SetRoundedRegion(Button b)
        {
            if (b.Width < 4 || b.Height < 4)
            {
                return;
            }
            using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, b.Width, b.Height), Radius + 1))
            {
                Region old = b.Region;
                b.Region = new Region(path);
                if (old != null)
                {
                    old.Dispose();
                }
            }
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            GraphicsPath path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // Grids

        private static void StyleGrid(DataGridView g)
        {
            EnableDoubleBuffering(g);

            g.EnableHeadersVisualStyles = false;
            g.BorderStyle = BorderStyle.None;
            g.BackgroundColor = Surface;
            g.GridColor = Border;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.RowHeadersVisible = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.AllowUserToResizeRows = false;
            g.RowTemplate.Height = 32;

            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 38;
            g.ColumnHeadersDefaultCellStyle.BackColor = SurfaceAlt;
            g.ColumnHeadersDefaultCellStyle.ForeColor = AccentLight;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = SurfaceAlt;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);

            g.DefaultCellStyle.BackColor = Surface;
            g.DefaultCellStyle.ForeColor = Ink;
            g.DefaultCellStyle.SelectionBackColor = Accent;
            g.DefaultCellStyle.SelectionForeColor = Color.White;
            g.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            g.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);

            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(22, 33, 54);
            g.AlternatingRowsDefaultCellStyle.ForeColor = Ink;
            g.AlternatingRowsDefaultCellStyle.SelectionBackColor = Accent;
            g.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
        }

        // Helpers

        /// <summary>
        /// Painting off-screen first stops grids and panels flickering when
        /// they redraw, which makes scrolling and searching feel faster.
        /// </summary>
        private static void EnableDoubleBuffering(Control c)
        {
            PropertyInfo property = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                property.SetValue(c, true, null);
            }
        }

        private static Color Mix(Color from, Color to, double amount)
        {
            int r = (int)Math.Round(from.R + (to.R - from.R) * amount);
            int g = (int)Math.Round(from.G + (to.G - from.G) * amount);
            int b = (int)Math.Round(from.B + (to.B - from.B) * amount);
            return Color.FromArgb(Clamp(r), Clamp(g), Clamp(b));
        }

        private static int Clamp(int value)
        {
            if (value < 0) return 0;
            if (value > 255) return 255;
            return value;
        }
    }
}
