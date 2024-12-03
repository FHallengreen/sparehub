namespace Repository.Interfaces;

public interface IDatabaseRepository
{
    Task<List<string>> GetDatabaseTableNamesAsync();
}
