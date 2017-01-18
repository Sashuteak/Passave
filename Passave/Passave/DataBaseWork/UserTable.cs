using SQLite;

namespace Passave.DataBaseWork
{
    [Table("Users")]
    public class UserTable
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
    }
}