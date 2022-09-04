namespace Minesweeper;

internal static class GraphicsExtensions
{
    public static void Draw3DControl(this Graphics g, int x, int y, int width, int height, Padding border, bool emboss = false, bool outline = false)
        => Draw3DControl(g, new(x, y, width, height), border, emboss, outline);

    public static void Draw3DControl(this Graphics g, Point location, Size size, Padding border, bool emboss = false, bool outline = false)
        => Draw3DControl(g, new(location, size), border, emboss, outline);

    public static void Draw3DControl(this Graphics g, Rectangle rectangle, Padding border, bool emboss = false, bool outline = false)
    {
        var regularBrush = Brushes.LightGray;
        var lightBrush = Brushes.White;
        var darkBrush = Brushes.Gray;

        var regularPen = new Pen(regularBrush);
        var darkPen = new Pen(darkBrush);

        if (border is { Top: > 0 } or { Right: > 0 } or { Bottom: > 0 } or { Left: > 0 })
        {
            g.FillRectangle(emboss ? lightBrush : darkBrush, rectangle);

            g.FillPolygon(emboss ? darkBrush : lightBrush, new Point[]
            { 
                new(rectangle.X, rectangle.Y),
                new(rectangle.Right, rectangle.Y),
                new(rectangle.Right - border.Right, rectangle.Y + border.Top),
                new(rectangle.X + border.Left, rectangle.Y + border.Top),
                new(rectangle.X + border.Left, rectangle.Bottom - border.Bottom),
                new(rectangle.X, rectangle.Bottom),
            });
        }

        g.FillRectangle(regularBrush, rectangle.X + border.Left, rectangle.Y + border.Top, rectangle.Width - border.Vertical, rectangle.Height - border.Horizontal);

        if (outline)
        {
            g.DrawRectangle(darkPen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
        }

        g.DrawLine(regularPen, rectangle.X, rectangle.Bottom - 1, rectangle.X + border.Left - 1, rectangle.Bottom - border.Bottom);
        g.DrawLine(regularPen, rectangle.Right - border.Right, rectangle.Y + border.Top - 1, rectangle.Right - 1, rectangle.Y);
    }
}
