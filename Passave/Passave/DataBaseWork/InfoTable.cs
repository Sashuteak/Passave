using SQLite;

namespace Passave.DataBaseWork
{
    [Table("Info")]
    class InfoTable
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int id { get; set; }
        [MaxLength(50)]
        public string Url { get; set; }
        [MaxLength(50)]
        public string Login { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public int User_id { get; set; }
    }
}