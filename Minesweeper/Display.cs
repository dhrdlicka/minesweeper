namespace Minesweeper;

public partial class Display : GameControl
{
    Digit[] _digits = new Digit[0];
    public int Digits
    {
        get => _digits.Length;
        set
        {
            foreach (var digit in _digits)
            {
                Children.Remove(digit);
            }

            _digits = new Digit[value];
            ContentSize = new(value * DigitSize.Width, DigitSize.Height);

            var x = Border.Left;
            for (var i = 0; i < _digits.Length; i++)
            {
                _digits[i] = new()
                {
                    Parent = this,
                    Location = new(x, Border.Top)
                };

                x += _digits[i].Width;

                Children.Add(_digits[i]);
            }

            PopulateDigits();
        }
    }

    int _value;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            PopulateDigits();
        }
    }
    public Size DigitSize { get; } = Constants.DisplayDigitSize;

    public Display()
    {
        Border = Constants.DisplayBorder;
        Digits = 3;
        SunkenBorder = true;
    }

    public void PopulateDigits()
    {
        var text = (Value % 100).ToString($"D{(Digits >= 0 ? Digits : Digits - 1)}");

        for (var i = 0; i < text.Length; i++)
        {
            _digits[i].Value = text[i];
        }
    }
}
