using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using uxcomex.Domain.Entities;
using uxcomex.Domain.Interfaces.Repositories;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Infrastructure.Repositories
{
    public class PedidosRepository : IPedidosRepository
    {
        private readonly string _connectionString;
        private readonly IProdutosRepository _produtosRepository;
        private readonly IItemPedidosRepository _itemPedidosRepository;
        public PedidosRepository(IConfiguration configuration, IProdutosRepository produtosRepository, IItemPedidosRepository itemPedidosRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string não encontrada");

            _itemPedidosRepository = itemPedidosRepository;
            _produtosRepository = produtosRepository;
        }

        public async Task<IEnumerable<Pedidos>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT 
                    *
                FROM pedidos 
                ORDER BY cli_id";

            return await connection.QueryAsync<Pedidos>(sql);
        }


        public async Task<IEnumerable<PedidoViewModel>> GetAllAsyncViewModel()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT 
                        ped_id,
                        cli_nome,
                        ped_valor_total,
                        ped_status,
                        ped_data
                        from pedidos p
                        join clientes c on p.cli_id = c.cli_id
                ORDER BY ped_data";

            return await connection.QueryAsync<PedidoViewModel>(sql);
        }

        public async Task<Pedidos?> GetByIdAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT 
                *
                FROM pedidos 
                WHERE ped_id = @Id";

            return await connection.QuerySingleOrDefaultAsync<Pedidos>(sql, new { Id = id });
        }


        public async Task<PedidoViewModel> GetByIdAsyncViewModel(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT 
                        ped_id,
                        cli_nome,
                        ped_valor_total,
                        ped_status,
                        ped_data
                        from pedidos p
                        join clientes c on p.cli_id = c.cli_id
                where ped_id = @Id
                ORDER BY ped_data";

            return await connection.QuerySingleOrDefaultAsync<PedidoViewModel>(sql, new {Id = id});
        }

        public async Task<Guid> CreateAsync(Pedidos pedidos)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
            INSERT INTO pedidos
                (ped_id, ped_valor_total, ped_status, cli_id)
            VALUES
                (@ped_id, @ped_valor_total, @ped_status, @cli_id);
            SELECT @ped_id;";

            return await connection.QuerySingleAsync<Guid>(sql, pedidos);
        }

        public async Task<Guid> CreateAsync(Pedidos pedidos, SqlConnection connection, SqlTransaction transaction)
        {
            var sql = @"
            INSERT INTO pedidos
                (ped_id, ped_valor_total, ped_status, cli_id)
            VALUES
                (@ped_id, @ped_valor_total, @ped_status, @cli_id);
            SELECT @ped_id;";

            return await connection.QuerySingleAsync<Guid>(sql, pedidos, transaction);
        }

        public async Task<bool> UpdateAsync(Pedidos pedidos)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                UPDATE pedidos 
                SET 
                ped_valor_total = @ped_valor_total, 
                ped_status = @ped_status , 
                cli_id = @cli_status
                WHERE ped_id = @ped_id";

            var affectedRows = await connection.ExecuteAsync(sql, pedidos);
            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                delete pedidos
                WHERE ped_id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task FluxoRegistroPedido(Pedidos pedidos, List<ItemPedidos> itens)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                await CreateAsync(pedidos, connection, transaction);
                bool temEstoque = await _produtosRepository.UpdateEstoque(itens, connection, transaction);

                if (temEstoque)
                {

                    await _itemPedidosRepository.CreateAsync(itens, connection, transaction);
                    transaction.Commit();
                } else
                {
                    throw new Exception("Sem estoque");
                }
            }
            catch (Exception ex) {
                transaction.Rollback();
                throw;
            }

        }

        public async Task<bool> UpdateStatusAsync(Guid ped_id, string status)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                string sql = @"update pedidos set ped_status = @Status where ped_id = @Ped_id";

                var affectedRows = await connection.ExecuteAsync(sql, new { Ped_id = ped_id,  Status = status }, transaction);

                if (affectedRows > 0) {
                    await InserirNotificacaoAsync(ped_id, status, connection, transaction);

                }

                transaction.Commit();

                return affectedRows > 0;
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task InserirNotificacaoAsync(Guid ped_id, string status_novo, SqlConnection connection, SqlTransaction transaction)
        {
            var sql = @"
            INSERT INTO notificacao (ped_id, ped_status)
            VALUES (@PedId, @PedStatusNovo)";

            await connection.ExecuteAsync(sql, new
            {
                PedId = ped_id,
                PedStatusNovo = status_novo
            }, transaction);
        }
    }
}
