namespace Minesweeper;

internal static class Constants
{
    public static Size ButtonSize { get; } = new(16, 16);
    public static Padding ButtonBorder { get; } = new(2);

    public static Padding FieldBorder { get; } = new(3);

    public static int TopPanelHeight { get; } = 37;
    public static Padding TopPanelBorder { get; } = new(2);

    public static Padding WindowBorder { get; } = new(3);

    public static Padding Margin { get; } = new(6);

    public static Size DisplayDigitSize { get; } = new(13, 23);
    public static Padding DisplayBorder { get; } = new(1);
    public static Padding DisplayMargin { get; } = new(5, 4, 7, 4);

    public static Size FaceButtonSize { get; } = new(26, 26);
    public static Padding FaceButtonBorder { get; } = new(3);
}
