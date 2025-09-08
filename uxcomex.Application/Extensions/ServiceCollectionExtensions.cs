using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uxcomex.Application.Interfaces;
using uxcomex.Application.Services;

namespace uxcomex.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IClienteService, ClientesService>();
            services.AddScoped<IProdutosService, ProdutosService>();
            services.AddScoped<IPedidosService, PedidosService>();
            services.AddScoped<IItemPedidosService, ItemPedidosService>();

            return services;
        }
    }
}
