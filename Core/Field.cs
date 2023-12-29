using snake.Game;
using System.Diagnostics;
using static snake.Game.Enums;

namespace snake.Core
{
    public class Field
    {
        #region consts
        private const int FieldWidth = 71;
        private const int FieldHeight = 30;

        private const ConsoleColor HeartColor = ConsoleColor.DarkRed;
        private const ConsoleColor BorderColor = ConsoleColor.White;
        private const ConsoleColor FoodColor = ConsoleColor.DarkGreen;
        private const ConsoleColor BoostColor = ConsoleColor.DarkCyan;

        private const char HeartSymbol = '♥';
        private const char BorderSymbol = '*';
        private const char FoodSymbol = 'ó';
        private const char BoostSymbol = '?';
        #endregion

        #region fields
        private Pixel[] _hearts = new Pixel[]
        {
            new Pixel(FieldWidth - 1, 1, HeartColor),
            new Pixel(FieldWidth - 3, 1, HeartColor),
            new Pixel(FieldWidth - 5, 1, HeartColor)
        };
        private int _frameMilliseconds = 200;
        private int _score = 0;
        private Snake _snake = new Snake(10, 5);
        private Direction _currentMovement = Direction.Right;
        private Stopwatch _stopwatch = new Stopwatch();
        private Pixel _food;
        private Pixel _boost;
        #endregion

        public Field()
        {
            Console.SetWindowSize(FieldWidth, FieldHeight);
            Console.SetBufferSize(FieldWidth, FieldHeight);
        }

        public int Run()
        {
            PrintScore(0);
            PrintHearts();
            PrintBorder();

            _snake.Print();

            int lagMs = 0;
            int lifes = 3;
            _food = GenFood(_snake);
            _food.Print(FoodSymbol);

            while (true)
            {
                _stopwatch.Restart();
                Direction oldMovement = _currentMovement;

                while (_stopwatch.ElapsedMilliseconds <= _frameMilliseconds - lagMs)
                {
                    if (_currentMovement == oldMovement)
                        _currentMovement = ReadMovement(_currentMovement);
                }

                _stopwatch.Restart();
                _snake.Move(_currentMovement);

                if (_snake.Head.X == _food.X && _snake.Head.Y == _food.Y)
                {
                    _snake.Move(_currentMovement, true);
                    _food = GenFood(_snake);
                    _food.Print(FoodSymbol);

                    _score++;
                    PrintScore(_score);

                    if (_score % 4 == 0)
                    {
                        _boost = GenBoost(_snake, _food);
                        _boost.Print(BoostSymbol);
                    }

                    Task.Run(() => Console.Beep(1200, 200));
                }

                if (_snake.Head.X == _boost.X && _snake.Head.Y == _boost.Y)
                {
                    Random random = new Random();
                    Array boosts = Enum.GetValues(typeof(Boost));
                    Boost currentBoost = (Boost)boosts.GetValue(random.Next(boosts.Length));

                    switch (currentBoost)
                    {
                        case Boost.Scores:
                            _score = _score + 5;
                            PrintScore(_score);
                            break;
                        case Boost.Life:
                            if (lifes < 3)
                            {
                                lifes++;
                                PrintHeart(lifes);
                            }
                            break;
                        case Boost.Speed:
                            if (_frameMilliseconds > 100)
                                _frameMilliseconds = _frameMilliseconds - 5;
                            break;
                    }

                    Task.Run(() => Console.Beep(1200, 200));
                }

                if (IsCollision(_snake))
                {
                    if (lifes == 1)
                    {
                        ClearHeart(0);
                        break;
                    }
                    else
                    {
                        lifes--;
                        ClearHeart(lifes);

                        _snake.Clear();
                        _snake = new Snake(10, 5);

                        _currentMovement = Direction.Right;
                    }
                }

                lagMs = (int)_stopwatch.ElapsedMilliseconds;
            }

            return _score;
        }

        private void PrintScore(int score)
        {
            Console.ForegroundColor = BorderColor;
            Console.SetCursorPosition(0, 1);
            Console.Write($"Счёт: {score}");
        }
        private void PrintHearts()
        {
            foreach (var heart in _hearts)
                heart.Print(HeartSymbol);
        }
        private void PrintHeart(int lifes)
        {
            _hearts[lifes - 1].Print(HeartSymbol);
        }
        private void ClearHeart(int lifes)
        {
            _hearts[lifes].Clear();
        }
        private void PrintBorder()
        {
            Console.ForegroundColor = BorderColor;

            for (int x = 0; x < FieldWidth; x = x + 2)
            {
                Console.SetCursorPosition(x, 3);
                Console.Write(BorderSymbol);
                Console.SetCursorPosition(x, FieldHeight - 1);
                Console.Write(BorderSymbol);
            }

            for (int y = 3; y < FieldHeight; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write(BorderSymbol);
                Console.SetCursorPosition(FieldWidth - 1, y);
                Console.Write(BorderSymbol);
            }
        }
        private bool IsCollision(Snake snake)
        {
            bool result = false;
            if (snake.Head.X == FieldWidth - 1 || snake.Head.X == 0 || snake.Head.Y == FieldHeight - 1 || snake.Head.Y == 3 || snake.Tail.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                result = true;
            return result;
        }
        private Direction ReadMovement(Direction currentDirection)
        {
            if (!Console.KeyAvailable)
                return currentDirection;

            ConsoleKey key = Console.ReadKey(true).Key;
            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                _ => currentDirection
            };
            return currentDirection;
        }
        private Pixel GenFood(Snake snake)
        {
            Pixel food;
            do
            {
                Random random = new Random();
                food = new Pixel(random.Next(1, FieldWidth - 2), random.Next(4, FieldHeight - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y || snake.Tail.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }
        private Pixel GenBoost(Snake snake, Pixel food)
        {
            Pixel boost;
            do
            {
                Random random = new Random();
                boost = new Pixel(random.Next(1, FieldWidth - 2), random.Next(4, FieldHeight - 2), BoostColor);
            } while (snake.Head.X == boost.X && snake.Head.Y == boost.Y || snake.Tail.Any(b => b.X == boost.X && b.Y == boost.Y) || food.X == boost.X && food.Y == boost.Y);

            return boost;
        }
    }
}