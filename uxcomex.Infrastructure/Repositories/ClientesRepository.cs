using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using uxcomex.Domain.Entities;
using uxcomex.Domain.Interfaces.Repositories;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Infrastructure.Repositories
{
    internal class ClientesRepository : IClientesRepository
    {
        private readonly string _connectionString;

        public ClientesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string não encontrada");
        }

        public async Task<IEnumerable<ClienteViewModel>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT 
                    cli_id, 
                    cli_nome, 
                    cli_email, 
                    cli_telefone, 
                    cli_data_cadastro
                FROM clientes 
                ORDER BY cli_nome";

            return await connection.QueryAsync<ClienteViewModel>(sql);
        }

        public async Task<ClienteViewModel?> GetByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT 
                    cli_id, 
                    cli_nome, 
                    cli_email, 
                    cli_telefone
                FROM clientes 
                WHERE cli_id = @Id";

            return await connection.QuerySingleOrDefaultAsync<ClienteViewModel>(sql, new { Id = id });
        }

        public async Task<Guid> CreateAsync(Clientes cliente)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                INSERT INTO clientes (cli_id, cli_nome, cli_email, cli_telefone)
                VALUES (@cli_id, @cli_nome, @cli_email, @cli_telefone);
                SELECT @cli_id;";

            return await connection.QuerySingleAsync<Guid>(sql, cliente);
        }

        public async Task<bool> UpdateAsync(Clientes cliente)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                UPDATE clientes 
                SET cli_nome = @cli_nome, 
                    cli_email = @cli_email, 
                    cli_telefone = @cli_telefone
                WHERE cli_id = @cli_id";

            var affectedRows = await connection.ExecuteAsync(sql, cliente);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                delete clientes
                WHERE cli_id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

    }
}
