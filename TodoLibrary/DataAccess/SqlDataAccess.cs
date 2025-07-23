using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TodoLibrary.DataAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;
    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStrginName)
    {
        string connectionString = _config.GetConnectionString(connectionStrginName)!;

        using IDbConnection connection = new SqlConnection(connectionString);

        var results = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

        return results.ToList();
    }
    public async Task SaveData<T>(string storedProcedure, T parameters, string connectionStrginName)
    {
        string connectionString = _config.GetConnectionString(connectionStrginName)!;

        using IDbConnection connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}
