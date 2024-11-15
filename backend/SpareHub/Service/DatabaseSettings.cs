namespace Service;

public enum DatabaseType
{
    MySql,
    MongoDb,
    Neo4j
}

public class DatabaseSettings
{
    public DatabaseType DefaultDatabaseType { get; set; }
}
