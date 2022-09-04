namespace Minesweeper;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        var board = new MinesweeperBoard()
        {
            Location = new(0, 0),
            Parent = this
        };

        ClientSize = board.ClientSize;
    }
}
