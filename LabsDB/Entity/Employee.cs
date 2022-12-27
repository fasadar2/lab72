namespace LabsDB.Entity;

public class Employee
{
    public Employee()
    {
        Login = string.Empty;
        Password = string.Empty;
        Indications = new List<Indication>();
    }

    public Employee(string login, string password)
    {
        Login = login;
        Password = password;
        Indications = new List<Indication>();
    }

    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public List<Indication> Indications { get; set; }
}