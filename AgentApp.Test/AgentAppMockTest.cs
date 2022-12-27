using AgentApp;
using AgentApp.Repository;
using LabsDb.Agent;
using Moq;

namespace Agent.Test;

public class AgentAppMockTest
{
    [Test]
    public async Task AuthSuccess()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.Auth(It.IsNotNull<AuthRequest>())).Returns(Task.FromResult(new ResponseEmployee
            {Id = 1, Login = "test", Password = "test"}));
        var worker = new Worker(mock.Object);
        var res = await worker.Auth(new AuthRequest {Login = "test", Password = "test"});
        Assert.That(res.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task AuthWithNull()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.Auth(It.Is<AuthRequest>(r => r == null))).Returns(Task.FromResult(new ResponseEmployee
            {Id = -1, Login = "", Password = ""}));
        var worker = new Worker(mock.Object);
        var res = await worker.Auth(null);
        Assert.That(res.Id, Is.EqualTo(-1));
    }

    [TestCase("  ", "test")]
    [TestCase("test", "  ")]
    public async Task AuthWithEmptyData(string login, string password)
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s =>
            s.Auth(It.Is<AuthRequest>(
                r => string.IsNullOrWhiteSpace(r.Login) || string.IsNullOrWhiteSpace(r.Password)))).Returns(
            Task.FromResult(new ResponseEmployee
                {Id = -1, Login = "", Password = ""}));
        var worker = new Worker(mock.Object);
        var res = await worker.Auth(new AuthRequest {Login = login, Password = password});
        Assert.That(res.Id, Is.EqualTo(-1));
    }

    [TestCase(null, "test")]
    [TestCase("test", null)]
    [TestCase(null, null)]
    public void AuthWithNullData(string login, string password)
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s =>
            s.Auth(It.Is<AuthRequest>(
                r => string.IsNullOrWhiteSpace(r.Login) || string.IsNullOrWhiteSpace(r.Password)))).Returns(
            Task.FromResult(new ResponseEmployee
                {Id = -1, Login = "", Password = ""}));
        var worker = new Worker(mock.Object);
        AuthRequest? req = null;
        Assert.Catch<ArgumentNullException>(() => req = new AuthRequest {Login = login, Password = password});
    }

    [Test]
    public async Task AddNewIndicationSuccess()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewIndication(It.IsNotNull<NewRequest>()))
            .Returns(Task.FromResult(new NewResponse {Res = true}));
        var worker = new Worker(mock.Object);
        var res = await worker.AddNewIndication(new NewRequest
            {House = 1, NowEmployee = 1, Title = "Test", Value = 2.0D});
        Assert.That(res.Res, Is.True);
    }

    [Test]
    public async Task AddNewIndicationWithNull()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewIndication(It.Is<NewRequest>(r => r == null)))
            .Returns(Task.FromResult(new NewResponse {Res = false}));
        var worker = new Worker(mock.Object);
        var res = await worker.AddNewIndication(null);
        Assert.That(res.Res, Is.False);
    }

    [Test]
    public async Task AddNewIndicationWithErrorHouse()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewIndication(It.Is<NewRequest>(r => r.House > 0)))
            .Returns(Task.FromResult(new NewResponse {Res = true}));
        var worker = new Worker(mock.Object);
        var res = await worker.AddNewIndication(new NewRequest
            {House = -1, NowEmployee = 1, Title = "Test", Value = 2.0D});
        Assert.That(res.Res, Is.False);
    }

    [Test]
    public async Task AddNewIndicationWithErrorEmployee()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewIndication(It.Is<NewRequest>(r => r.NowEmployee > 0)))
            .Returns(Task.FromResult(new NewResponse {Res = true}));
        var worker = new Worker(mock.Object);
        var res = await worker.AddNewIndication(new NewRequest
            {House = 1, NowEmployee = -1, Title = "Test", Value = 2.0D});
        Assert.That(res.Res, Is.False);
    }

    [Test]
    public async Task AddNewIndicationWithErrorValue()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewIndication(It.Is<NewRequest>(r => r.Value >= 0)))
            .Returns(Task.FromResult(new NewResponse {Res = true}));
        var worker = new Worker(mock.Object);
        var res = await worker.AddNewIndication(new NewRequest
            {House = 1, NowEmployee = 1, Title = "Test", Value = -2.0D});
        Assert.That(res.Res, Is.False);
    }

    [Test]
    public async Task AddNewIndicationWithEmptyTitle()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewIndication(It.Is<NewRequest>(r => !string.IsNullOrWhiteSpace(r.Title))))
            .Returns(Task.FromResult(new NewResponse {Res = true}));
        var worker = new Worker(mock.Object);
        var res = await worker.AddNewIndication(new NewRequest
            {House = 1, NowEmployee = 1, Title = "   ", Value = 2.0D});
        Assert.That(res.Res, Is.False);
    }

    [Test]
    public void AddNewIndicationWithNullTitle()
    {
        var mock = new Mock<IAgentRepository>();
        mock.Setup(s => s.AddNewIndication(It.Is<NewRequest>(r => !string.IsNullOrWhiteSpace(r.Title))))
            .Returns(Task.FromResult(new NewResponse {Res = true}));
        var worker = new Worker(mock.Object);
        NewRequest? request = null;
        Assert.Catch<ArgumentNullException>(() => request = new NewRequest
            {House = 1, NowEmployee = 1, Title = null, Value = 2.0D});
    }
}