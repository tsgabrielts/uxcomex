using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using uxcomex.Domain.Entities;
using uxcomex.Domain.Interfaces.Repositories;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Infrastructure.Repositories
{
    public class ItemPedidosRepository : IItemPedidosRepository
    {

        private readonly string _connectionString;

        public ItemPedidosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string não encontrada");
        }

        public async Task<ItemPedidos?> GetByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT 
                *
                FROM item_pedidos 
                WHERE ped_id = @Id";

            return await connection.QuerySingleOrDefaultAsync<ItemPedidos>(sql, new { Id = id });
        }

        public async Task<List<ItemPedidoViewModel>> GetByIdAsyncViewModel(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
            select ped_id, pro_nome, p.pro_id as ProdutoId, itp_quantidade as Quantidade, itp_preco_unitario as PrecoUnitario from item_pedidos ip join produtos p on ip.pro_id = p.pro_id
where ped_id = @Id";

            var result = await connection.QueryAsync<ItemPedidoViewModel>(sql, new { Id = id });
            return result.ToList();
        }

        public async Task CreateAsync(List<ItemPedidos> itemPedidos)
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"
                    INSERT INTO item_pedidos (itp_id, ped_id, pro_id, itp_quantidade, itp_preco_unitario)
                    VALUES (newid(), @ped_id, @pro_id, @itp_quantidade, @itp_preco_unitario);";

                    await connection.ExecuteAsync(sql, itemPedidos);
        }

        public async Task CreateAsync(List<ItemPedidos> itemPedidos, SqlConnection connection, SqlTransaction transaction)
        {
            var sql = @"
                    INSERT INTO item_pedidos (itp_id, ped_id, pro_id, itp_quantidade, itp_preco_unitario)
                    VALUES (newid(), @ped_id, @pro_id, @itp_quantidade, @itp_preco_unitario);";

            await connection.ExecuteAsync(sql, itemPedidos, transaction);
        }

        public async Task<bool> UpdateAsync(List<ItemPedidos> itemPedidos)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                UPDATE produtos 
                SET pro_nome = @pro_nome, 
                pro_descricao = @pro_descricao, 
                pro_valor = @pro_valor, 
                pro_quantidade_estoque = @pro_quantidade_estoque
                WHERE pro_id = @pro_id";

            var affectedRows = await connection.ExecuteAsync(sql, itemPedidos);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                delete item_pedidos
                WHERE ped_id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}
