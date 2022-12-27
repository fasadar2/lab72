using LabsDB.Entity;

namespace ClientApp.Repositories;

public interface IClientRepository
{
    Task<IEnumerable<House>> GetAllHouses();
}