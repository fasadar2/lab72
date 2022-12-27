using LabsDB.Entity;

namespace MainApp.Repositories;

public interface IClientRepository
{
    IEnumerable<House> GetAllHouses();
}