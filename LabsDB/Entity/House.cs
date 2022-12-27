using System.Text.Json.Serialization;

namespace LabsDB.Entity;

public class House
{
    public House()
    {
        Indications = new List<Indication>();
    }

    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("indications")] public List<Indication> Indications { get; set; }
}