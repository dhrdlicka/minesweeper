using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Minesweeper;

public class Field : GameControl
{
    private Minesweeper<Tile> Game { get; }

    private bool LeftButtonDown { get; set; } = false;
    private bool RightButtonDown { get; set; } = false;
    private bool MiddleButtonDown { get; set; } = false;

    private bool PeekDisabled { get; set; } = false;

    public Field(Minesweeper<Tile> game)
    {
        Game = game;
        Border = Constants.FieldBorder;
        ContentSize = new(Game.FieldWidth * Constants.ButtonSize.Width, Game.FieldHeight * Constants.ButtonSize.Width);
        SunkenBorder = true;

        Game.GameOver += (_, _) => Enabled = false;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        LeftButtonDown |= e.Button is MouseButtons.Left;
        RightButtonDown |= e.Button is MouseButtons.Right;
        MiddleButtonDown |= e.Button is MouseButtons.Middle;

        if (ContentRectangle.Contains(e.Location))
        {
            var (x, y) = PointToButton(e.X, e.Y);

            if (RightButtonDown && !LeftButtonDown && !MiddleButtonDown)
            {
                Game.GetTile(x, y).ToggleFlag();
            }
            else
            {
                PeekDisabled = false;
                Game.BigPeek = RightButtonDown | MiddleButtonDown;
                Game.MovePeek(x, y);
            }

            Invalidate();
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if ((LeftButtonDown || MiddleButtonDown) && (Game.PeekedTile is not null || !PeekDisabled))
        {
            if (ContentRectangle.Contains(e.Location))
            {
                var (x, y) = PointToButton(e.X, e.Y);
                Game.MovePeek(x, y);
            }
            else
            {
                Game.CancelPeek();
            }
            Invalidate();
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (LeftButtonDown || MiddleButtonDown)
        {
            if (e.Button is MouseButtons.Left && Game.PeekedTile is not null && !Game.BigPeek)
            {
                Game.PeekedTile.Clear();
            }

            PeekDisabled = true;
            Game.CancelPeek();
            Invalidate();
        }

        LeftButtonDown &= e.Button is not MouseButtons.Left;
        RightButtonDown &= e.Button is not MouseButtons.Right;
        MiddleButtonDown &= e.Button is not MouseButtons.Middle;

        Game.BigPeek = RightButtonDown | MiddleButtonDown;
        base.OnMouseUp(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.TranslateTransform(Border.Left, Border.Top);

        if (ContentRectangle.Contains(e.ClipRectangle))
        {
            var topleft = PointToButton(e.ClipRectangle.Left, e.ClipRectangle.Top);
            var bottomright = PointToButton(e.ClipRectangle.Right - 1, e.ClipRectangle.Bottom - 1);

            for (var x = topleft.X; x <= bottomright.X; x++)
            for (var y = topleft.Y; y <= bottomright.Y; y++)
            {
                InvokePaint(Game.GetTile(x, y).GameControl, e);
            }
        }
        else
        {
            for (var x = 0; x < Game.FieldWidth; x++)
            for (var y = 0; y < Game.FieldHeight; y++)
            {
                InvokePaint(Game.GetTile(x, y).GameControl, e);
            }
        }
    }

    private (int X, int Y) PointToButton(int x, int y)
        => ((x - Border.Left) / Constants.ButtonSize.Width, (y - Border.Top) / Constants.ButtonSize.Height);

    private (int X, int Y) ButtonToPoint(int x, int y)
        => (x * Constants.ButtonSize.Width + Border.Left, y * Constants.ButtonSize.Height + Border.Top);
}
