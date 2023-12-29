namespace snake.Game
{
    public class User
    {
        public User(string name)
        {
            Name = name;
        }
        public string Name { get; }
        public int Score { get; set; }
    }
}