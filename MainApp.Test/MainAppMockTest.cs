using LabsDb.Agent;
using LabsDB.Entity;
using MainApp.Controllers;
using MainApp.Repositories;
using Moq;

namespace Tests.MainAppTests;

public class MainAppMockTest
{
    private readonly Employee _employee;
    private readonly IEnumerable<House> _testHouses;

    public MainAppMockTest()
    {
        var testHouses = new List<House>();
        _employee = new Employee {Id = 1, Login = "Test", Password = "Test"};
        for (var i = 1; i < 3; i++)
        {
            var home = new House {Id = i};
            var ind0 = new Indication("Свет", 100 * i, home, _employee);
            var ind1 = new Indication("Вода", 200 * i, home, _employee);
            home.Indications.Add(ind0);
            home.Indications.Add(ind1);
            testHouses.Add(home);
        }

        _testHouses = testHouses;
    }

    [Test]
    public void GetAllHousesSuccess()
    {
        var mock = new Mock<IClientRepository>();
        mock.Setup(r => r.GetAllHouses()).Returns(_testHouses);
        var clientController = new ClientController(mock.Object);
        var result = clientController.GetHouses();
        Assert.That(result.Count(), Is.EqualTo(_testHouses.Count()));
    }

    [Test]
    public void AddNewIndicationSuccess()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewIndication(It.IsNotNull<Indication>())).Returns(true);
        mock.Setup(r => r.GetEmployeeById(It.IsAny<int>())).Returns(_employee);
        mock.Setup(r => r.GetHouseById(It.IsAny<int>())).Returns(_testHouses.First(h => h.Id == 1));
        var agentController = new AgentController(mock.Object);
        var res = agentController.AddNewIndication(new NewRequest {Title = "t", House = 1, NowEmployee = 1, Value = 1});
        Assert.That(res.Res, Is.True);
    }

    [Test]
    public void AddNewIndicationWithNull()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewIndication(It.IsNotNull<Indication>())).Returns(true);
        var agentController = new AgentController(mock.Object);
#pragma warning disable CS8625
        var res = agentController.AddNewIndication(null);
#pragma warning restore CS8625
        Assert.That(res.Res, Is.False);
    }

    [Test]
    public void AddNewIndicationWithErrorHouse()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewIndication(It.Is<Indication>(i => i.HouseId > 0))).Returns(true);
        var agentController = new AgentController(mock.Object);
        var res = agentController.AddNewIndication(new NewRequest {House = -1});
        Assert.That(res.Res, Is.False);
    }

    [Test]
    public void AddNewIndicationWithErrorEmployee()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewIndication(It.Is<Indication>(i => i.EmployeeId > 0))).Returns(true);
        var agentController = new AgentController(mock.Object);
        var res = agentController.AddNewIndication(new NewRequest {NowEmployee = -1});
        Assert.That(res.Res, Is.False);
    }

    [Test]
    public void AddNewIndicationWithErrorValue()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewIndication(It.Is<Indication>(i => i.Value > 0))).Returns(true);
        var agentController = new AgentController(mock.Object);
        var res = agentController.AddNewIndication(new NewRequest {Value = -1});
        Assert.That(res.Res, Is.False);
    }

    [Test]
    public void AddNewIndicationWithEmptyString()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewIndication(It.Is<Indication>(i => !string.IsNullOrWhiteSpace(i.Title)))).Returns(true);
        var agentController = new AgentController(mock.Object);
        var res = agentController.AddNewIndication(new NewRequest {Title = "  "}).Res;
        Assert.That(res, Is.False);
    }

    [Test]
    public void AddNewIndicationWithNullString()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AddNewIndication(It.Is<Indication>(i => !string.IsNullOrWhiteSpace(i.Title)))).Returns(true);
        var agentController = new AgentController(mock.Object);
        NewRequest? requst = null;
        Assert.Catch<ArgumentNullException>(() => requst = new NewRequest {Title = null});
    }

    [Test]
    public void AuthSuccess()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AuthEmployee(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new Employee {Id = 1, Login = "Test", Password = "Test"});
        var agentController = new AgentController(mock.Object);
        var res = agentController.Auth(new AuthRequest {Login = "Test", Password = "Test"});
        Assert.That(res, Is.Not.Null);
    }

    [TestCase("   ", "test")]
    [TestCase("test", "    ")]
    [TestCase("   ", "    ")]
    public void AuthWithEmptyData(string login, string password)
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AuthEmployee(It.Is<string>(s => !string.IsNullOrWhiteSpace(s)),
                It.Is<string>(s => !string.IsNullOrWhiteSpace(s))))
            .Returns(new Employee {Id = 1, Login = "Test", Password = "Test"});
        var agentController = new AgentController(mock.Object);
        var res = agentController.Auth(new AuthRequest {Login = login, Password = password});
        Assert.That(res.Id, Is.EqualTo(-1));
    }

    [TestCase(null, "test")]
    [TestCase("test", null)]
    [TestCase(null, null)]
    public void AuthWithNullData(string login, string password)
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(r => r.AuthEmployee(It.Is<string>(s => !string.IsNullOrWhiteSpace(s)),
                It.Is<string>(s => !string.IsNullOrWhiteSpace(s))))
            .Returns(new Employee {Id = 1, Login = "Test", Password = "Test"});
        var agentController = new AgentController(mock.Object);
        Assert.Catch<ArgumentNullException>(() =>
            agentController.Auth(new AuthRequest {Login = login, Password = password}));
    }
}