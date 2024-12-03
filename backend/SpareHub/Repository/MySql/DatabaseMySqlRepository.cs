using Microsoft.EntityFrameworkCore;
using Persistence.MySql.SparehubDbContext;
using Repository.Interfaces;

namespace Repository.MySql;

public class DatabaseMySqlRepository(SpareHubDbContext dbContext) : IDatabaseRepository
{
    public async Task<List<string>> GetDatabaseTableNamesAsync()
    {
        var connection = dbContext.Database.GetDbConnection();

        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        var command = connection.CreateCommand();
        // Query to fetch only tables from the current schema
        command.CommandText = @"
        SELECT TABLE_NAME 
        FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_SCHEMA = DATABASE() 
        AND TABLE_NAME NOT LIKE 'INNODB%' -- Exclude specific patterns if needed
        AND TABLE_NAME NOT IN (
            'ADMINISTRABLE_ROLE_AUTHORIZATIONS', 
            'APPLICABLE_ROLES', 
            'CHARACTER_SETS',
            -- Add other system tables you want to exclude here
            'VIEWS', 'TABLES_EXTENSIONS'
        )";

        var tableNames = new List<string>();
        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                tableNames.Add(reader.GetString(0));
            }
        }

        return tableNames;
    }
}
