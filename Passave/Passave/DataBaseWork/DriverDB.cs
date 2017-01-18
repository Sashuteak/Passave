using System;
using System.IO;
using SQLite;
using System.Collections.Generic;

namespace Passave.DataBaseWork
{
    class DriverDB
    {
        protected string dbpath;
        protected SQLiteConnection connection;

        public DriverDB()
        {
            dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Passave.db3");
            connection = new SQLiteConnection(dbpath);
            connection.CreateTable<UserTable>();
            connection.CreateTable<InfoTable>();
        }
        ~DriverDB()
        {
            connection.Close();
        }
        public void InsertIntoUsers(string name, string pass)
        {
            UserTable item = new UserTable();
            item.Name = name;
            item.Password = pass;
            connection.Insert(item);
        }
        public void InsertIntoInfo(string url, string login, string password, string description, int user_id)
        {
            InfoTable item = new InfoTable();
            item.Url = url;
            item.Login = login;
            item.Password = password;
            item.Description = description;
            item.User_id = user_id;
            connection.Insert(item);
        }
        public List<User> SelectAllFromUser()
        {
            List<User> list = new List<User>();
            TableQuery<UserTable> table = connection.Table<UserTable>();
            foreach (UserTable item in table)
            {
                list.Add(new User(item.id, item.Name, item.Password));
            }
            return list;
        }
        public List<Info> SelectAllFromInfo()
        {
            List<Info> list = new List<Info>();
            TableQuery<InfoTable> table = connection.Table<InfoTable>();
            foreach (InfoTable item in table)
            {
                list.Add(new Info(item.id, item.Url, item.Login, item.Password, item.Description, item.User_id));
            }
            return list;
        }
        public void UpdateUser(int id, string name, string pass)
        {
            UserTable item = connection.Get<UserTable>(id);
            item.Name = name;
            item.Password = pass;
            connection.Update(item);
        }
        public void UpdateInfo(int id, string url, string login, string password, string description)
        {
            InfoTable item = connection.Get<InfoTable>(id);
            item.Url = url;
            item.Login = login;
            item.Password = password;
            item.Description = description;
            connection.Update(item);
        }
        public void DellFromUser(int id)
        {
            UserTable item = connection.Get<UserTable>(id);
            connection.Delete(item);
        }
        public void DelFromInfo(int id)
        {
            InfoTable item = connection.Get<InfoTable>(id);
            connection.Delete(item);
        }
    }
}