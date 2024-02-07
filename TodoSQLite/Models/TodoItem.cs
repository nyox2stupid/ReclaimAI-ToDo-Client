using SQLite;
using System.Text.Json.Serialization;

namespace TodoSQLite.Models;

public class TodoItem
{
    [PrimaryKey, AutoIncrement]

    public int TaskID { get; set; }
    [JsonPropertyName("id")]
    public int ReclaimID { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("notes")]
    public string Notes { get; set; }
    public bool Done { get; set; }
    [JsonPropertyName("eventCategory")]
    public string EventCategory { get; set; }
    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
    public string Priority { get; set; }
}
