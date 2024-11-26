using Domain.Models;
using Neo4j.Driver;

namespace Repository.Neo4J;

public class DispatchNeo4jRepository
{
    private readonly IDriver _driver;

    public DispatchNeo4jRepository(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<Dispatch> CreateDispatchAsync(Dispatch dispatch)
    {
        throw new NotImplementedException();
    }

    public async Task<Dispatch> GetDispatchByIdAsync(string dispatchId)
    {
        throw new NotImplementedException();
    }
}
