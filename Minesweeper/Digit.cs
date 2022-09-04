namespace Minesweeper;

public class Digit : GameControl
{
    [Flags]
    enum Segments : byte
    {
        A = 1,
        B = 2,
        C = 4,
        D = 8,
        E = 16,
        F = 32,
        G = 64
    }

    readonly static Segments Zero = Segments.A | Segments.B | Segments.C | Segments.D | Segments.E | Segments.F;
    readonly static Segments One = Segments.B | Segments.C;
    readonly static Segments Two = Segments.A | Segments.B | Segments.E | Segments.D | Segments.G;
    readonly static Segments Three = Segments.A | Segments.B | Segments.C | Segments.D | Segments.G;
    readonly static Segments Four = Segments.B | Segments.C | Segments.F | Segments.G;
    readonly static Segments Five = Segments.A | Segments.C | Segments.D |  Segments.F | Segments.G;
    readonly static Segments Six = Segments.A | Segments.C | Segments.D | Segments.E | Segments.F | Segments.G;
    readonly static Segments Seven = Segments.A | Segments.B | Segments.C;
    readonly static Segments Eight = Segments.A | Segments.B | Segments.C | Segments.D | Segments.E | Segments.F | Segments.G;
    readonly static Segments Nine = Segments.A | Segments.B | Segments.C | Segments.D | Segments.F | Segments.G;
    readonly static Segments Minus = Segments.G;

    static Segments GetSegments(char digit) => digit switch
    {
        ' ' => 0,       '-' => Minus,
        '0' => Zero,    '5' => Five,
        '1' => One,     '6' => Six,
        '2' => Two,     '7' => Seven,
        '3' => Three,   '8' => Eight,
        '4' => Four,    '9' => Nine,
        _ => throw new ArgumentOutOfRangeException(nameof(digit))
    };

    char _value = ' ';
    public char Value
    {
        get => _value;
        set
        {
            if (value is >= '0' and <= '9' or ' ' or '-')
            {
                _value = value;
                Invalidate();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }

    public Digit()
    {
        Size = Constants.DisplayDigitSize;
        PaintBackground = false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.FillRectangle(Brushes.DarkRed, 0, 0, 13, 23);

        var segments = GetSegments(Value);

        if (segments.HasFlag(Segments.A))
        {
            e.Graphics.FillPolygon(Brushes.Red, new Point[]
            {
                new(0, 0),
                new(6, 6),
                new(12, 0)
            });
        }

        if (segments.HasFlag(Segments.B))
        {
            e.Graphics.FillPolygon(Brushes.Red, new Point[]
            {
                new(12, 0),
                new(6, 6),
                new(12, 12)
            });
        }

        if (segments.HasFlag(Segments.C))
        {
            e.Graphics.FillPolygon(Brushes.Red, new Point[]
            {
                new(12, 10),
                new(6, 16),
                new(12, 22)
            });
        }

        if (segments.HasFlag(Segments.D))
        {
            e.Graphics.FillPolygon(Brushes.Red, new Point[]
            {
                new(12, 22),
                new(6, 16),
                new(0, 22)
            });
        }

        if (segments.HasFlag(Segments.E))
        {
            e.Graphics.FillPolygon(Brushes.Red, new Point[]
            {
                new(0, 22),
                new(6, 16),
                new(0, 10)
            });
        }

        if (segments.HasFlag(Segments.F))
        {
            e.Graphics.FillPolygon(Brushes.Red, new Point[]
            {
                new(0, 12),
                new(6, 6),
                new(0, 0)
            });
        }

        if (segments.HasFlag(Segments.G))
        {
            e.Graphics.FillPolygon(Brushes.Red, new Point[]
            {
                new(1, 11),
                new(6, 6),
                new(11, 11),
                new(6, 16),
            });
        }

        e.Graphics.DrawLine(Pens.Black, 0, 0, 12, 12);
        e.Graphics.DrawLine(Pens.Black, 12, 0, 0, 12);
        e.Graphics.FillRectangle(Brushes.Black, 4, 4, 5, 6);

        e.Graphics.DrawLine(Pens.Black, 0, 10, 12, 22);
        e.Graphics.DrawLine(Pens.Black, 12, 10, 0, 22);
        e.Graphics.FillRectangle(Brushes.Black, 4, 13, 5, 6);

        e.Graphics.DrawRectangle(Pens.Black, 0, 0, 12, 22);
    }
}
