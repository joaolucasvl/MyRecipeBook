using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static void Migrate(DatabaseType databaseType, string connectionString)
    {
        if(databaseType == DatabaseType.SqlServer)
        {
            EnsureDatabaseCreatedSqlServer(connectionString);
        }
    }

    private static void EnsureDatabaseCreatedSqlServer(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

        var databaseName = connectionStringBuilder.InitialCatalog;

        connectionStringBuilder.Remove("Database");

        using var dbConnection = new SqlConnection(connectionStringBuilder.ConnectionString);

        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);

        var records = dbConnection.Query("SELECT * FROM sys.databases WHERE name = @name", parameters);

        if(records.Any() == false)
            dbConnection.Execute($"CREATE DATABASE [{databaseName}]");
 
    }
}