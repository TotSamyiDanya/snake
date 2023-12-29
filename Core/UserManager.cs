using snake.Game;
using System.Text.Json;

namespace snake.Core
{
    public class UserManager
    {
        #region singleton
        private static UserManager? _instance;
        private UserManager() 
        {
            ReadUsers();
        }
        public static UserManager GetInstance() => _instance ??= new();
        #endregion

        private List<User> _users = new List<User>();
        private User _user;

        public void RegisterUser(string name)
        {
            _user = new User(name);
        }

        private void ReadUsers()
        {
            var users = File.ReadAllText("..\\..\\..\\Game\\scores.json");
            if (users != "")
                _users = JsonSerializer.Deserialize<List<User>>(users)!;
        }
        public void WriteUser(int score)
        {
            _user.Score = score;

            if (_users.Any(u => u.Name == _user.Name))
            {
                var userIndex = _users.FindIndex(u => u.Name == _user.Name);
                if (_users[userIndex].Score < score)
                    _users[userIndex].Score = score;
            }
            else
                _users.Add(_user);

            File.WriteAllText("..\\..\\..\\Game\\scores.json", JsonSerializer.Serialize(_users));
        }
        public void ShowUsers()
        {
            _users = _users.OrderByDescending(u => u.Score).ToList();
            int leftMargin = (Console.BufferWidth - "Таблица результатов".Length) / 2;
            Console.SetCursorPosition(leftMargin, 0);
            Console.WriteLine("Таблица результатов");

            for (int i = 0; i < _users.Count; i++)
            {
                var row = $"{_users[i].Name}: {_users[i].Score}";
                leftMargin = (Console.BufferWidth - row.Length) / 2;
                Console.SetCursorPosition(leftMargin, i + 1);
                Console.Write(row);
            }
        }
    }
}