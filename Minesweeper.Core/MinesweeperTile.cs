namespace Minesweeper;

public class MinesweeperTile
{
    public event EventHandler? Exploded;
    public event EventHandler? Cleared;

    public IMinesweeper? Game { get; init; }

    public int X { get; init; }
    public int Y { get; init; }

    public int SurroundingMines { get; private set; }
    public bool IsMine { get; private set; }

    public TileState TileState { get; private set; }

    public bool IsFlagged => TileState is TileState.Flagged or TileState.QuestionMark;
    public bool HasQuestionMark => TileState is TileState.QuestionMark;

    public bool IsCleared => TileState is TileState.Cleared;
    public bool IsExploded => TileState is TileState.Cleared && IsMine;

    internal void PlaceMine()
    {
        IsMine = true;

        foreach (var tile in Game?.GetAdjacentTiles(X, Y) ?? throw new InvalidOperationException())
        {
            tile.SurroundingMines++;
        }
    }

    public void Clear() => Clear(true);

    void Clear(bool triggerEvent)
    {
        if (IsCleared || IsFlagged)
        {
            return;
        }

        TileState = TileState.Cleared;

        if (!IsMine)
        {
            if (SurroundingMines == 0)
            {
                foreach (var tile in Game?.GetAdjacentTiles(X, Y) ?? throw new InvalidOperationException())
                {
                    tile.Clear(false);
                }
            }

            if (triggerEvent)
            {
                OnCleared(new());
            }
        }
        else
        {
            OnExploded(new());
        }
    }

    public void ToggleFlag() => TileState = TileState switch
    {
        TileState.Covered => TileState.Flagged,
        TileState.Flagged => Game?.EnableQuestionMarks ?? true ? TileState.QuestionMark : TileState.Covered,
        TileState.QuestionMark => TileState.Covered,
        _ => TileState
    };

    protected void OnCleared(EventArgs e)
        => Cleared?.Invoke(this, e);

    protected void OnExploded(EventArgs e)
        => Exploded?.Invoke(this, e);
}
