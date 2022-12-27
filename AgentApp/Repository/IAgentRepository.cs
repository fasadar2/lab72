using LabsDb.Agent;

namespace AgentApp.Repository;

public interface IAgentRepository
{
    public Task<ResponseEmployee> Auth(AuthRequest request);

    public Task<NewResponse> AddNewIndication(NewRequest request);
}