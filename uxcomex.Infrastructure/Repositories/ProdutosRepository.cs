using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using uxcomex.Domain.Entities;
using uxcomex.Domain.Interfaces.Repositories;

namespace uxcomex.Infrastructure.Repositories
{
    public class ProdutosRepository : IProdutosRepository
    {
        private readonly string _connectionString;

        public ProdutosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string não encontrada");
        }

        public async Task<IEnumerable<Produtos>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT 
                    *
                FROM produtos 
                ORDER BY pro_nome";

            return await connection.QueryAsync<Produtos>(sql);
        }

        public async Task<Produtos?> GetByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT 
*
                FROM produtos 
                WHERE pro_id = @Id";

            return await connection.QuerySingleOrDefaultAsync<Produtos>(sql, new { Id = id });
        }

        public async Task<Guid> CreateAsync(Produtos produto)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                INSERT INTO produtos (pro_id, pro_nome, pro_descricao, pro_valor, pro_quantidade_estoque)
                VALUES (@pro_id, @pro_nome, @pro_descricao, @pro_valor, @pro_quantidade_estoque);
                SELECT @pro_id;";

            return await connection.QuerySingleAsync<Guid>(sql, produto);
        }

        public async Task<bool> UpdateAsync(Produtos produto)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                UPDATE produtos 
                SET pro_nome = @pro_nome, 
                pro_descricao = @pro_descricao, 
                pro_valor = @pro_valor, 
                pro_quantidade_estoque = @pro_quantidade_estoque
                WHERE pro_id = @pro_id";

            var affectedRows = await connection.ExecuteAsync(sql, produto);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                delete produtos
                WHERE pro_id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> UpdateEstoque(List<ItemPedidos> itens)
        {
            int affectedRows = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    const string sql = @"
                    UPDATE produtos
                    SET pro_quantidade_estoque = pro_quantidade_estoque - @itp_quantidade
                    WHERE pro_id = @pro_id AND pro_quantidade_estoque >= @itp_quantidade
                ";


                    affectedRows = await connection.ExecuteAsync(sql, itens, transaction);
                    transaction.Commit();
                }
            }

            return affectedRows == itens.Count;
        }


        public async Task<bool> UpdateEstoque(List<ItemPedidos> itens, SqlConnection connection, SqlTransaction transaction)
        {
            int affectedRows = 0;
    
            string sql = @"
                    UPDATE produtos
                    SET pro_quantidade_estoque = pro_quantidade_estoque - @itp_quantidade
                    WHERE pro_id = @pro_id AND pro_quantidade_estoque >= @itp_quantidade
                ";

            affectedRows = await connection.ExecuteAsync(sql, itens, transaction);

            return affectedRows == itens.Count;
        }
    }
}