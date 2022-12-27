using LabsDB.Entity;
using MainApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MainApp.Controllers;

[ApiController]
[Route("/client")]
public class ClientController : ControllerBase
{
    private readonly IClientRepository _clientService;

    public ClientController(IClientRepository clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("get")]
    public IEnumerable<House> GetHouses()
    {
        return _clientService.GetAllHouses();
    }
}