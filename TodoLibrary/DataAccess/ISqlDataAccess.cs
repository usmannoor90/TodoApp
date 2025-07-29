namespace TodoLibrary.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStrginName);
        Task SaveData<T>(string storedProcedure, T parameters, string connectionStrginName);
        Task<List<T>> EFLoadData<T>(string storedProcedure, object[] sqlParams);
    }
}