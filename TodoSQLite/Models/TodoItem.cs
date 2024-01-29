using SQLite;

namespace TodoSQLite.Models;

public class TodoItem
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int ReclaimID { get; set; }
    public string Title { get; set; }
    public string Notes { get; set; }
    public bool Done { get; set; }
    public string EventCategory { get; set; }
    public bool Deleted { get; set; }
    public string Priority { get; set; }
    public string Token { get; set; }
}
