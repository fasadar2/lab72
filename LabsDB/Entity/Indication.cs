using System.Text.Json.Serialization;

namespace LabsDB.Entity;

public class Indication
{
    public Indication()
    {
        Title = string.Empty;
        TimeStamp = DateTime.Now;
        House = new House();
        Employee = new Employee();
    }

    public Indication(string title, double value, House house, Employee employee)
    {
        Value = value;
        TimeStamp = DateTime.Now;
        Title = title;
        House = house;
        HouseId = house.Id;
        Employee = employee;
        EmployeeId = employee.Id;
    }

    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("value")] public double Value { get; set; }
    [JsonPropertyName("timeStamp")] public DateTime TimeStamp { get; set; }
    public House House { get; set; }
    [JsonPropertyName("houseId")] public int HouseId { get; set; }
    public Employee Employee { get; set; }
    [JsonPropertyName("employeeId")] public int EmployeeId { get; set; }
}