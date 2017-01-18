namespace Passave
{
    class Info
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public int User_id { get; set; }
        public Info(int id, string name, string login, string password, string description, int user_id)
        {
            ID = id;
            User_id = user_id;
            Name = name;
            Login = login;
            Password = password;
            Description = description;
        }
        public Info(string url, string login, string password, string description)
        {
            if (login == "")
                Login = "No Login";
            else
                Login = login;

            if (password == "")
                Password = "No Password";
            else
                Password = password;

            if (description == "")
                Description = "No Description";
            else
                Description = description;

            Name = url;
        }
    }
}