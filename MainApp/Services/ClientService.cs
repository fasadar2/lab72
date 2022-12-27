using LabsDB.Entity;
using MainApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MainApp.Services;

public class ClientService : IClientRepository
{
    private readonly ApplicationContext _context;

    public ClientService(ApplicationContext context)
    {
        _context = context;
    }

    public IEnumerable<House> GetAllHouses()
    {
        var indications = _context.Houses.Include(h => h.Indications).ToList();
        indications = indications.Select(h =>
        {
            h.Indications = h.Indications.Select(i =>
            {
                i.House = null;
                return i;
            }).ToList();
            return h;
        }).ToList();
        return indications;
    }
}