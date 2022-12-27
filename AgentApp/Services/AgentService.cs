using AgentApp.Repository;
using Grpc.Net.Client;
using LabsDb.Agent;

namespace AgentApp.Services;

public class AgentService : IAgentRepository
{
    public async Task<ResponseEmployee> Auth(AuthRequest request)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5174");
        var client = new Agent.AgentClient(channel);
        return await client.AuthAsync(request);
    }

    public async Task<NewResponse> AddNewIndication(NewRequest request)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5174");
        var client = new Agent.AgentClient(channel);
        return await client.AddNewIndicationAsync(request);
    }
}