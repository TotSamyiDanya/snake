using static snake.Game.Enums;

namespace snake.Game
{
    public class Snake
    {
        private const ConsoleColor TailColor = ConsoleColor.DarkYellow;
        private const ConsoleColor HeadColor = ConsoleColor.DarkMagenta;

        private const char SnakeSymbol = '■';

        public Snake(int initX, int initY, int length = 2)
        {
            Head = new Pixel(initX, initY, HeadColor);

            for (int i = length; i >= 0; i--)
                Tail.Enqueue(new Pixel(Head.X - i - 1, initY, TailColor));
        }

        public Pixel Head { get; private set; }
        public Queue<Pixel> Tail { get; } = new Queue<Pixel>();

        public void Print()
        {
            Head.Print(SnakeSymbol);

            foreach (Pixel pixel in Tail)
                pixel.Print(SnakeSymbol);
        }
        public void Clear()
        {
            Head.Clear();

            foreach (Pixel pixel in Tail)
                pixel.Clear();
        }
        public void Move(Direction direction, bool eat = false)
        {
            Clear();

            Tail.Enqueue(new Pixel(Head.X, Head.Y, TailColor));

            if (!eat)
                Tail.Dequeue();

            Head = direction switch
            {
                Direction.Right => new Pixel(Head.X + 1, Head.Y, HeadColor),
                Direction.Left => new Pixel(Head.X - 1, Head.Y, HeadColor),
                Direction.Up => new Pixel(Head.X, Head.Y - 1, HeadColor),
                Direction.Down => new Pixel(Head.X, Head.Y + 1, HeadColor),
                _ => Head
            };

            Print();
        }
    }
}