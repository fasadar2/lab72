using ClientApp.Models;
using ClientApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ClientApp.Controllers;

public class HomeController : Controller
{
    private readonly IClientRepository _clientRepository;

    public HomeController(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IActionResult> Index()
    {
        return View(new HouseViewModel {Houses = await _clientRepository.GetAllHouses()});
    }
}