namespace Minesweeper;

public interface IMinesweeper
{
    int FieldWidth { get; }
    int FieldHeight { get; }
    int MineCount { get; }
    bool GameLost { get; }
    bool EnableQuestionMarks { get; }

    MinesweeperTile GetTile(int x, int y);
    IEnumerable<MinesweeperTile> GetAdjacentTiles(int x, int y);
}
