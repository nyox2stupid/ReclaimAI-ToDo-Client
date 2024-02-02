using SQLite;

namespace TodoSQLite.Models;

public class Settings
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Token { get; set; }
}

