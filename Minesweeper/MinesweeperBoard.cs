using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Minesweeper;

public class MinesweeperBoard : Control
{
    Minesweeper<Tile> Game { get; } = new(20, 20, 40);

    MinesweeperBoard.Board BoardControl { get; }

    public MinesweeperBoard()
    {
        DoubleBuffered = true;
        BoardControl = new(this);

        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        ClientSize = BoardControl.Size;
    }

    class Board : GameControl
    {
        public MinesweeperBoard FormsControl { get; }

        public Field Field { get; }
        public TopPanel TopPanel { get; }

        public Board(MinesweeperBoard board)
        {
            FormsControl = board;
            Border = Constants.WindowBorder;

            Field = new(board.Game)
            {
                Parent = this
            };

            TopPanel = new()
            {
                Parent = this,
                Width = Field.Width,
                Location = new(Border.Left + Constants.Margin.Left, Border.Top + Constants.Margin.Top)
            };

            Field.Location = new(Border.Left + Constants.Margin.Left, TopPanel.Bounds.Bottom + Constants.Margin.Top);
            ContentSize = new(Field.Bounds.Right + Constants.Margin.Right, Field.Bounds.Bottom + Constants.Margin.Bottom);

            FormsControl.Paint += (_, e) => InvokePaint(this, e);
            FormsControl.MouseDown += (_, e) => InvokeMouseDown(this, e);
            FormsControl.MouseMove += (_, e) => InvokeMouseMove(this, e);
            FormsControl.MouseUp += (_, e) => InvokeMouseUp(this, e);
        }

        protected override void Invalidate(Rectangle rect)
        {
            FormsControl.Invalidate(new Region(rect));
        }
    }
}
