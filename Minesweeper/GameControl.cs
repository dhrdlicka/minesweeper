using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Minesweeper;

public abstract partial class GameControl : TreeNodeBase<GameControl>
{
    public event EventHandler<MouseEventArgs>? MouseClick;
    public event EventHandler<MouseEventArgs>? MouseUp;
    public event EventHandler<MouseEventArgs>? MouseDown;
    public event EventHandler<MouseEventArgs>? MouseMove;
    public event EventHandler? SizeChanged;
    public event EventHandler<PaintEventArgs>? Paint;

    private Size _size = default;
    public Size Size
    {
        get => _size;
        set
        {
            _size = value;
            OnSizeChanged(new());
        }
    }

    public Point Location { get; set; }

    public Size ContentSize
    {
        get => new(Size.Width - Border.Horizontal, Size.Height - Border.Vertical);
        set => Size = new(value.Width + Border.Horizontal, value.Height + Border.Vertical);
    }

    public int Width
    {
        get => Size.Width;
        set => Size = new(value, Size.Height);
    }

    public int Height
    {
        get => Size.Height;
        set => Size = new(Size.Width, value);
    }

    public Rectangle Bounds
    {
        get => new(Location, Size);
        set
        {
            Location = value.Location;
            Size = value.Size;
        }
    }

    public Rectangle Rectangle => new(0, 0, Width, Height);
    public Rectangle ContentRectangle => new(Border.Left, Border.Top, ContentSize.Width, ContentSize.Height);

    public Padding Border { get; set; }
    public bool SunkenBorder { get; set; } = false;
    public bool Enabled { get; set; } = true;
    protected bool PaintBackground { get; set; } = true;

    protected void InvokePaint(GameControl control, PaintEventArgs e)
    {
        var container = e.Graphics.BeginContainer();
        e.Graphics.TranslateTransform(control.Location.X, control.Location.Y);
        e.Graphics.SmoothingMode = SmoothingMode.None;
        e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

        if (PaintBackground)
        {
            e.Graphics.Draw3DControl(new(), control.Size, control.Border, control.SunkenBorder);
        }

        control.OnPaint(e);

        foreach (var item in control.Children)
        {
            if (e.ClipRectangle.IntersectsWith(item.Bounds))
            {
                InvokePaint(item, new(e.Graphics, new(
                    x: Math.Max(e.ClipRectangle.X - item.Location.X, 0),
                    y: Math.Max(e.ClipRectangle.Y - item.Location.Y, 0),
                    width: Math.Min(item.Width, e.ClipRectangle.Width),
                    height: Math.Min(item.Height, e.ClipRectangle.Height)
                )));
            }
        }

        e.Graphics.EndContainer(container);
    }

    static GameControl? ControlLockingMouse { get; set; }
    static MouseButtons PressedButtons { get; set; }

    protected void InvokeMouseUp(GameControl control, MouseEventArgs e)
    {
        PressedButtons &= ~e.Button;

        foreach (var item in control.Children)
        {
            if (item.Enabled && (item.Bounds.Contains(e.Location) || item == ControlLockingMouse))
            {
                InvokeMouseUp(item, new(e.Button, e.Clicks, e.Location.X - item.Location.X, e.Location.Y - item.Location.Y, e.Delta));
                return;
            }
        }

        if (e.Button == MouseButtons.Left)
        {
            control.OnMouseClick(e);
        }

        control.OnMouseUp(e);

        if (PressedButtons == MouseButtons.None)
        {
            ControlLockingMouse = null;
        }
    }

    protected void InvokeMouseDown(GameControl control, MouseEventArgs e)
    {
        PressedButtons |= e.Button;

        foreach (var item in control.Children)
        {
            if (item.Enabled && (item.Bounds.Contains(e.Location) || item == ControlLockingMouse))
            {
                InvokeMouseDown(item, new(e.Button, e.Clicks, e.Location.X - item.Location.X, e.Location.Y - item.Location.Y, e.Delta));
                return;
            }
        }

        ControlLockingMouse ??= control;
        control.OnMouseDown(e);
    }

    protected void InvokeMouseMove(GameControl control, MouseEventArgs e)
    {
        foreach (var item in control.Children)
        {
            if (item.Enabled && (item.Bounds.Contains(e.Location) || item == ControlLockingMouse))
            {
                InvokeMouseMove(item, new(e.Button, e.Clicks, e.Location.X - item.Location.X, e.Location.Y - item.Location.Y, e.Delta));
                return;
            }
        }
        
        control.OnMouseMove(e);
    }

    protected void Invalidate()
        => Invalidate(new(new(), Size));

    protected virtual void Invalidate(Rectangle rect)
        => Parent?.Invalidate(new(
            x: rect.X + Location.X,
            y: rect.Y + Location.Y,
            width: rect.Width, height: rect.Height
        ));

    protected virtual void OnMouseClick(MouseEventArgs e)
        => MouseClick?.Invoke(this, e);

    protected virtual void OnMouseDown(MouseEventArgs e)
        => MouseDown?.Invoke(this, e);

    protected virtual void OnMouseUp(MouseEventArgs e)
        => MouseUp?.Invoke(this, e);

    protected virtual void OnMouseMove(MouseEventArgs e)
        => MouseMove?.Invoke(this, e);

    protected virtual void OnSizeChanged(EventArgs e)
        => SizeChanged?.Invoke(this, e);

    protected virtual void OnPaint(PaintEventArgs e)
        => Paint?.Invoke(this, e);
}
