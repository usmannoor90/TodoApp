using Dapper;
using Microsoft.EntityFrameworkCore


using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TodoLibrary.DataAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;

    public SqlDataAccess(IConfiguration config, AppDbContext context)
    {
        _config = config;
        _context = context;
    }

    public async Task<List<T>> EFLoadData<T>(string storedProcedure, object[] sqlParams) where T : class
    {

        return await _context.Set<T>().FromSqlRaw(storedProcedure, sqlParams).ToListAsync();
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
