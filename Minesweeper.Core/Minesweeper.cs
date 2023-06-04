namespace Minesweeper;

public class Minesweeper<T> : IMinesweeper where T : MinesweeperTile, new()
{
    public event EventHandler? AreaCleared;
    public event EventHandler? GameOver;

    T[,] tiles;

    public int FieldWidth { get; private set; }
    public int FieldHeight { get; private set; }
    public int MineCount { get; private set; }

    public bool EnableQuestionMarks { get; set; } = true;

    public bool GameLost { get; private set; }

    public Minesweeper(int width, int height, int mineCount)
    {
        FieldWidth = width;
        FieldHeight = height;
        MineCount = mineCount;

        tiles = new T[width, height];

        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            var tile = new T()
            {
                X = x,
                Y = y,
                Game = this
            };

            tile.Cleared += tile_Cleared;
            tile.Exploded += tile_Exploded;

            tiles[x, y] = tile;
        }

        GenerateMines();
    }

    private void tile_Cleared(object? sender, EventArgs e)
        => OnAreaCleared(e);

    private void tile_Exploded(object? sender, EventArgs e)
    {
        GameLost = true;
        OnGameOver(e);
    }

    MinesweeperTile IMinesweeper.GetTile(int x, int y) => GetTileBase(x, y);
    public T GetTile(int x, int y) => GetTileBase(x, y);
    T GetTileBase(int x, int y)
        => tiles[x, y];

    IEnumerable<MinesweeperTile> IMinesweeper.GetAdjacentTiles(int x, int y) => GetAdjacentTilesBase(x, y);
    public IEnumerable<T> GetAdjacentTiles(int x, int y) => GetAdjacentTilesBase(x, y);
    IEnumerable<T> GetAdjacentTilesBase(int x, int y)
    {
        for (int dx = -1; dx < 2; dx++)
        {
            if (x + dx < 0 || x + dx >= FieldWidth) continue;

            for (int dy = -1; dy < 2; dy++)
            {
                if (y + dy < 0 || y + dy >= FieldHeight) continue;

                if (dx == 0 && dy == 0) continue;

                yield return tiles[x + dx, y + dy];
            }
        }
    }

    void GenerateMines()
    {
        var rnd = new Random();

        for (int i = 0; i < MineCount; i++)
        {
            int x, y;
            do
            {
                x = rnd.Next(0, FieldWidth);
                y = rnd.Next(0, FieldHeight);
            } while (tiles[x, y].IsMine);

            tiles[x, y].PlaceMine();
        }
    }

    protected void OnAreaCleared(EventArgs e)
        => AreaCleared?.Invoke(this, e);

    protected void OnGameOver(EventArgs e)
        => GameOver?.Invoke(this, e);
}
