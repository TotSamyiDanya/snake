using snake.Game;

namespace snake.Core
{
    public class GameManager
    {
        private Field _field = Field.GetInstance();
        private UserManager _userManager = UserManager.GetInstance();

        public GameManager()
        {
            ShowMenu();
        }

        private void ShowMenu()
        {
            Console.SetCursorPosition((Console.WindowWidth - "Привет! Введите ваше имя".Length) / 2, Console.WindowHeight / 2);
            Console.Write("Привет! Введите ваше имя");
            Console.SetCursorPosition(Console.WindowWidth / 2, (Console.WindowHeight / 2) + 2);
            string userName = GetUserName();

            while (userName == "")
            {
                userName = GetUserName();
            }

            _userManager.RegisterUser(userName);

            Console.Clear();
            Console.CursorVisible = false;

            int score = _field.Run();
            _userManager.WriteUser(score);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            _userManager.ShowUsers();

            Console.ReadKey();
        }
        private string GetUserName()
        {
            string inputText = "";

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                    break;
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (inputText.Length > 0)
                    {
                        inputText = inputText.Substring(0, inputText.Length - 1);
                        UpdateCenteredText(inputText);
                    }
                }
                else if (Char.IsLetterOrDigit(key.KeyChar) || Char.IsPunctuation(key.KeyChar) || Char.IsSymbol(key.KeyChar) || Char.IsWhiteSpace(key.KeyChar))
                {
                    inputText += key.KeyChar;
                    UpdateCenteredText(inputText);
                }
            }
            return inputText;
        }
        private void UpdateCenteredText(string text)
        {
            Console.Clear();
            int screenWidth = Console.WindowWidth;
            int stringLength = text.Length;
            int leftMargin = (screenWidth - stringLength) / 2;
            Console.SetCursorPosition((Console.WindowWidth - "Привет! Введите ваше имя".Length) / 2, Console.WindowHeight / 2);
            Console.Write("Привет! Введите ваше имя\n");
            Console.SetCursorPosition(leftMargin, (Console.WindowHeight / 2) + 2);
            Console.Write(text);
        }
    }
}