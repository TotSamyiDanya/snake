namespace snake.Game
{
    public readonly struct Pixel
    {
        public Pixel(int x, int y, ConsoleColor color)
        {
            X = x;
            Y = y;
            Color = color;
        }

        public int X { get; }
        public int Y { get; }
        public ConsoleColor Color { get; }

        public void Print(char symbol)
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(X, Y);
            Console.Write(symbol);
        }
        public void Clear()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(' ');
        }
    }
}