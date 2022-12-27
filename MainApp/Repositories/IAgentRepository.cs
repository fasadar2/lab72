using LabsDB.Entity;

namespace MainApp.Repositories;

public interface IAgentRepository
{
    bool AddNewIndication(Indication indication);
    Employee? AuthEmployee(string login, string password);
    House? GetHouseById(int id);
    Employee? GetEmployeeById(int id);
}