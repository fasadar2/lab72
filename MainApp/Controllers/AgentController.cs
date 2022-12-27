using Grpc.Core;
using LabsDb.Agent;
using LabsDB.Entity;
using MainApp.Repositories;

namespace MainApp.Controllers;

public class AgentController : Agent.AgentBase
{
    private readonly IAgentRepository _agentRepository;

    public AgentController(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }

    public ResponseEmployee Auth(AuthRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            return new ResponseEmployee {Id = -1, Login = "", Password = ""};
        var employee = _agentRepository.AuthEmployee(request.Login, request.Password);
        return employee is null
            ? new ResponseEmployee {Id = -1, Login = "", Password = ""}
            : new ResponseEmployee {Id = employee.Id, Login = employee.Login, Password = employee.Password};
    }

    public override Task<ResponseEmployee> Auth(AuthRequest request, ServerCallContext context)
    {
        return Task.FromResult(Auth(request));
    }

    public NewResponse AddNewIndication(NewRequest indication)
    {
        if (indication is null || indication.House <= 0 || indication.Value <= 0 || indication.NowEmployee <= 0 ||
            string.IsNullOrWhiteSpace(indication.Title))
            return new NewResponse {Res = false};
        var house = _agentRepository.GetHouseById(indication.House);
        if (house is null)
            return new NewResponse {Res = false};
        var emp = _agentRepository.GetEmployeeById(indication.NowEmployee);
        if (emp is null)
            return new NewResponse {Res = false};
        var ind = new Indication(indication.Title, indication.Value, house, emp);
        var res = _agentRepository.AddNewIndication(ind);
        return new NewResponse {Res = res};
    }

    public override Task<NewResponse> AddNewIndication(NewRequest request, ServerCallContext context)
    {
        return Task.FromResult(AddNewIndication(request));
    }
}