namespace Service.Interfaces;

public interface IDatabaseService
{
    Task<List<string>> GetTableNames();
}
