using Repository.Interfaces;
using Service.Interfaces;

namespace Service.MySql.Database;

public class DatabaseService(IDatabaseRepository databaseRepository) : IDatabaseService
{
    public Task<List<string>> GetTableNames()
    {
        return databaseRepository.GetDatabaseTableNamesAsync();
    }
}
