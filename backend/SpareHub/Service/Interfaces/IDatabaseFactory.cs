namespace Service.Interfaces;

public interface IDatabaseFactory
{
    T GetRepository<T>() where T : class;
}