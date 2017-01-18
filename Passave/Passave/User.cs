namespace Passave
{
    class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public User() { }
        public User(int id, string name, string pass)
        {
            ID = id;
            Name = name;
            Password = pass;
        }
        public User(string name, string pass)
        {
            Name = name;
            Password = pass;
        }
    }
}