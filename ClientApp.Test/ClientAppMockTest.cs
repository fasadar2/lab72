using ClientApp.Controllers;
using ClientApp.Models;
using ClientApp.Repositories;
using LabsDB.Entity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClientApp.Test;

public class ClientAppMockTest
{
    private readonly IEnumerable<House> _testHouses;

    public ClientAppMockTest()
    {
        var testHouses = new List<House>();
        var e = new Employee {Id = 1, Login = "Test", Password = "Test"};
        for (var i = 1; i < 3; i++)
        {
            var home = new House {Id = i};
            var ind0 = new Indication("Свет", 100 * i, home, e);
            var ind1 = new Indication("Вода", 200 * i, home, e);
            home.Indications.Add(ind0);
            home.Indications.Add(ind1);
            testHouses.Add(home);
        }

        _testHouses = testHouses;
    }

    [Test]
    public async Task GetHouseSuccess()
    {
        var mock = new Mock<IClientRepository>();
        mock.Setup(s => s.GetAllHouses()).Returns(Task.FromResult(_testHouses));
        var controller = new HomeController(mock.Object);
        var actionResult = await controller.Index();
        Assert.That(actionResult, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult) actionResult;
        Assert.That(viewResult.ViewData.Model, Is.TypeOf<HouseViewModel>());
        var model = (HouseViewModel) viewResult.ViewData.Model!;
        Assert.That(model.Houses.Count(), Is.EqualTo(_testHouses.Count()));
    }

    [Test]
    public async Task GetHouseServerError()
    {
        var mock = new Mock<IClientRepository>();
        mock.Setup(s => s.GetAllHouses()).Returns(Task.FromResult(Enumerable.Empty<House>()));
        var controller = new HomeController(mock.Object);
        var actionResult = await controller.Index();
        Assert.That(actionResult, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult) actionResult;
        Assert.That(viewResult.ViewData.Model, Is.TypeOf<HouseViewModel>());
        var model = (HouseViewModel) viewResult.ViewData.Model!;
        Assert.That(model.Houses.Count(), Is.EqualTo(0));
    }
}