
using uxcomex.Application.Interfaces;
using uxcomex.Domain.Entities;
using uxcomex.Domain.Interfaces.Repositories;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Application.Services
{
    public class ClientesService : IClienteService
    {
        private readonly IClientesRepository _clienteRepository;

        public ClientesService(IClientesRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ClienteViewModel?> GetClienteByIdAsync(Guid id)
        {
            return await _clienteRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ClienteViewModel>> GetAllClientesAsync()
        {
            return await _clienteRepository.GetAllAsync();
        }


        public async Task<Guid> CreateClienteAsync(ClienteViewModel clienteViewModel)
        {
            Clientes cliente = ConverteViewModelEmEntidade(clienteViewModel);

            return await _clienteRepository.CreateAsync(cliente);
        }

        public async Task<bool> UpdateClienteAsync(ClienteViewModel clienteViewModel)
        {
            Clientes cliente = ConverteViewModelEmEntidade(clienteViewModel);

            var existingCliente = await _clienteRepository.GetByIdAsync(cliente.cli_id);
            if (existingCliente == null)
            {
                throw new InvalidOperationException("Cliente não encontrado.");
            }

            return await _clienteRepository.UpdateAsync(cliente);
        }

        public async Task<bool> DeleteClienteAsync(Guid id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                throw new InvalidOperationException("Cliente não encontrado.");
            }

            return await _clienteRepository.DeleteAsync(id);
        }

        private Clientes ConverteViewModelEmEntidade(ClienteViewModel clienteViewModel)
        {
            var cliente = new Clientes()
            {
                cli_id =  clienteViewModel.cli_id != Guid.Empty ? clienteViewModel.cli_id : Guid.NewGuid(),
                cli_nome = clienteViewModel.cli_nome,
                cli_email = clienteViewModel.cli_email,
                cli_telefone = clienteViewModel.cli_telefone
            };

            return cliente;
        }
    }
}
