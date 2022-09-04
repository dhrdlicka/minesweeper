namespace Minesweeper;

public class TopPanel : GameControl
{
    public Display LeftDisplay { get; }
    public Display RightDisplay { get; }
    public FaceButton FaceButton { get; }

    public TopPanel()
    {
        Border = Constants.TopPanelBorder;

        LeftDisplay = new()
        {
            Parent = this,
            Digits = 3,
            Value = 69,
            Location = new(Border.Left + Constants.DisplayMargin.Left, Border.Top + Constants.DisplayMargin.Top)
        };

        RightDisplay = new()
        {
            Parent = this,
            Digits = 3,
            Value = 69
        };

        FaceButton = new()
        {
            Parent = this
        };

        Height = Constants.TopPanelHeight;
        SunkenBorder = true;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        RightDisplay.Location = new(Size.Width - Border.Right - Constants.DisplayMargin.Right - RightDisplay.Size.Width + 1, Border.Top + Constants.DisplayMargin.Left);
        FaceButton.Location = new(Size.Width / 2 - FaceButton.Width / 2, Size.Height / 2 - FaceButton.Height / 2);
        base.OnSizeChanged(e);
    }
}
