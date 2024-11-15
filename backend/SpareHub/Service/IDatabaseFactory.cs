namespace Service;

public interface IDatabaseFactory
{
    T GetService<T>() where T : class;
}