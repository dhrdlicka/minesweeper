namespace Minesweeper;

public class FaceButton : GameControl
{
    bool _pressed = false;
    public bool IsPressed
    {
        get => _pressed;
        set
        {
            _pressed = value;
            Invalidate();
        }
    }

    bool LeftButtonDown { get; set; }

    public FaceButton()
    {
        Size = Constants.FaceButtonSize;
        Border = Constants.FaceButtonBorder;
        PaintBackground = false;

        MouseDown += (_, e) => IsPressed = LeftButtonDown |= e.Button is MouseButtons.Left;
        MouseMove += (_, e) => IsPressed = LeftButtonDown && Rectangle.Contains(e.Location);
        MouseUp += (_, e) => IsPressed = LeftButtonDown &= e.Button is not MouseButtons.Left;
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        if (Rectangle.Contains(e.Location))
            base.OnMouseClick(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (!IsPressed)
        {
            e.Graphics.Draw3DControl(new(), Size, Border, SunkenBorder, true);
        }
        else
        {
            e.Graphics.DrawRectangle(Pens.Gray, 0, 0, Width - 2, Height - 2);
            e.Graphics.FillRectangle(Brushes.LightGray, 1, 1, Width - 2, Height - 2);
            e.Graphics.DrawRectangle(Pens.Gray, 1, 1, Width - 1, Height - 1);
        }
    }
}
