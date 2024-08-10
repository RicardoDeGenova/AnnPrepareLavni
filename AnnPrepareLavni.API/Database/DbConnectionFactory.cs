using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace AnnPrepareLavni.API.Database;

public interface IDbConnectionFactory : IDisposable
{
    Task<IDbConnection> CreateConnectionAsync();
    Task<int> ExecuteAsync(string command);
}

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    private NpgsqlConnection? _connection;
    private bool _disposed;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            _connection ??= new NpgsqlConnection(_connectionString);
            await _connection.OpenAsync();
        }
        return _connection;
    }

    public async Task<int> ExecuteAsync(string command)
    {
        using var connection = await CreateConnectionAsync();
        using var cmd = (NpgsqlCommand)connection.CreateCommand();
        cmd.CommandText = command;
        return await cmd.ExecuteNonQueryAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); 
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        _disposed = true;
    }

    ~DbConnectionFactory()
    {
        Dispose(false);
    }
}