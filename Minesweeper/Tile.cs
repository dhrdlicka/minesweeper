namespace Minesweeper;

public class Tile : MinesweeperTile
{
    Tile.TileControl? _control;
    public GameControl GameControl => _control ??= new(this);

    bool _isPeeked = false;
    public bool IsPeeked
    {
        get => _isPeeked;
        set
        {
            if (!IsCleared && !IsFlagged)
            {
                _isPeeked = value;
            }
        }
    }


    class TileControl : GameControl
    {
        public Tile Tile { get; }

        public Font Font { get; } = new("Tahoma", 8, FontStyle.Bold);

        public TileControl(Tile tile)
        {
            Tile = tile;
            Border = Constants.ButtonBorder;
            Size = Constants.ButtonSize;
            Location = new(Size.Width * Tile.X, Size.Height * Tile.Y);
        }

        string Text => Tile switch
        {
            { IsFlagged: true, HasQuestionMark: false } => "F",
            { IsFlagged: true, HasQuestionMark: true } => "?",
            { IsMine: true } => "*",
            { SurroundingMines: > 0 } => Tile.SurroundingMines.ToString(),
            _ => ""
        };

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Tile.IsCleared || Tile.IsExploded || Tile.IsPeeked)
            {
                var brush = Tile.IsExploded ? Brushes.Red : Brushes.LightGray;

                e.Graphics.FillRectangle(Brushes.Gray, 0, 0, Width, Height);
                e.Graphics.FillRectangle(brush, Border.Left / 2, Border.Top / 2, Width - Border.Left / 2, Height - Border.Top / 2);
            }

            var textBrush = Tile switch
            {
                { IsMine: true, Game.GameLost: true } => Brushes.Black,
                { IsFlagged: true } => Brushes.Gray,
                { IsCleared: true } => Tile.SurroundingMines switch
                {
                    1 => Brushes.Blue,
                    2 => Brushes.Green,
                    3 => Brushes.Red,
                    4 => Brushes.Purple,
                    5 => Brushes.Maroon,
                    6 => Brushes.Turquoise,
                    7 => Brushes.Black,
                    8 => Brushes.Gray,
                    _ => null
                },
                _ => null
            };

            if (textBrush is not null)
            {
                var textSize = e.Graphics.MeasureString(Text, Font);

                var x = Width / 2 - textSize.Width / 2;
                var y = Height / 2 - textSize.Height / 2;

                e.Graphics.DrawString(Text, Font, textBrush, x, y);
            }

            base.OnPaint(e);
        }
    }
}
