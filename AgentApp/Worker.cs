using AgentApp.Repository;
using LabsDb.Agent;
using LabsDB.Entity;

namespace AgentApp;

public class Worker : BackgroundService
{
    private static readonly List<string> _titles =
        new(new[] {"Газ", "Электричество", "Интернет", "Вода", null, "  "}!);

    private readonly IAgentRepository _agentRepository;
    private Employee? _employee;

    public Worker(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var res = await Auth(new AuthRequest {Login = "Test", Password = "Test"});
        if (res.Id == -1) return;
        _employee = new Employee {Id = res.Id, Login = res.Login, Password = res.Password};
        while (!stoppingToken.IsCancellationRequested)
            try
            {
                var req = new NewRequest
                {
                    Title = _titles[new Random().Next(0, _titles.Count)], House = new Random().Next(-5, 10), NowEmployee = _employee.Id,
                    Value = new Random().NextDouble() * new Random().Next(-10, 100)
                };
                var resp = await AddNewIndication(req);
                if (resp.Res)
                    await Task.Delay(3600000, stoppingToken);
                else
                    Console.WriteLine("Don't added");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Null title");
            }
    }

    public async Task<ResponseEmployee> Auth(AuthRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            return new ResponseEmployee {Id = -1, Password = "", Login = ""};

        return await _agentRepository.Auth(request);
    }

    public async Task<NewResponse> AddNewIndication(NewRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Title) || request.House <= 0 ||
            request.NowEmployee <= 0 || request.Value < 0) return new NewResponse {Res = false};
        return await _agentRepository.AddNewIndication(request);
    }
}