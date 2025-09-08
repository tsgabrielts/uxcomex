using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uxcomex.Domain.Interfaces.Repositories;
using uxcomex.Infrastructure.Repositories;

namespace uxcomex.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register repositories
            services.AddScoped<IClientesRepository, ClientesRepository>();
            services.AddScoped<IProdutosRepository, ProdutosRepository>();
            services.AddScoped<IPedidosRepository, PedidosRepository>();
            services.AddScoped<IItemPedidosRepository, ItemPedidosRepository>();

            return services;
        }
    }
}
