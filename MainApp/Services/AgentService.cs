using LabsDB.Entity;
using MainApp.Repositories;

namespace MainApp.Services;

public class AgentService : IAgentRepository
{
    private readonly ApplicationContext _context;

    public AgentService(ApplicationContext context)
    {
        _context = context;
    }

    public bool AddNewIndication(Indication indication)
    {
        if (indication is null || string.IsNullOrWhiteSpace(indication.Title) ||
            !_context.Houses.Any(h => h.Id == indication.HouseId) ||
            !_context.Employees.Any(e => e.Id == indication.EmployeeId) || indication.Value <= 0) return false;
        try
        {
            _context.Add(indication);
            _context.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public Employee? AuthEmployee(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password)) return null;
        return _context.Employees.FirstOrDefault(e => e.Login == login && e.Password == password);
    }

    public House? GetHouseById(int id)
    {
        if (id <= 0) return null;
        return _context.Houses.FirstOrDefault(h => h.Id == id);
    }

    public Employee? GetEmployeeById(int id)
    {
        if (id <= 0) return null;
        return _context.Employees.FirstOrDefault(e => e.Id == id);
    }
}