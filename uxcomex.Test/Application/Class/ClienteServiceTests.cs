using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Xunit;
using FluentAssertions;
using uxcomex.Application.Services;
using uxcomex.Domain.Entities;
using uxcomex.Domain.Interfaces.Repositories;

namespace uxcomex.Tests.Application.Class
{
    public class ClienteServiceTests
        {
            private readonly Mock<IClientesRepository> _repositoryMock;
            private readonly ClientesService _clienteService;

            public ClienteServiceTests()
            {
                _repositoryMock = new Mock<IClientesRepository>();
                _clienteService = new ClientesService(_repositoryMock.Object);
            }

            #region GetByIdAsync Tests

            [Fact]
            public async Task GetByIdAsync_ValidId_ReturnsCliente()
            {
                var clienteId = Guid.NewGuid();
                var expectedCliente = new Clientes
                {
                    cli_id = clienteId,
                    cli_nome = "João Silva",
                    cli_email = "joao@teste.com",
                    cli_telefone = "11999999999",
                    cli_data_cadastro = DateTime.Now
                };

                _repositoryMock
                    .Setup(r => r.GetByIdAsync(clienteId))
                    .ReturnsAsync(expectedCliente);

                // Act
                var result = await _clienteService.GetClienteByIdAsync(clienteId);

                // Assert
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(expectedCliente);
                _repositoryMock.Verify(r => r.GetByIdAsync(clienteId), Times.Once);
            }

            [Fact]
            public async Task GetByIdAsync_ClienteNotFound_ReturnsNull()
            {
            // Arrange

            var emptyId = Guid.Empty;
            _repositoryMock
                    .Setup(r => r.GetByIdAsync(emptyId))
                    .ReturnsAsync((Clientes?)null);

                // Act
                var result = await _clienteService.GetClienteByIdAsync(emptyId);

                // Assert
                result.Should().BeNull();
            }

        #endregion

        #region CreateAsync Tests
        [Fact]
        public async Task CreateClienteAsync_ValidCliente_ReturnsGuid()
        {
            // Arrange
            var cliente = new Clientes
            {
                cli_nome = "Maria Santos",
                cli_email = "maria@teste.com",
                cli_telefone = "11888888888"
            };

            var expectedId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Clientes>()))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _clienteService.CreateClienteAsync(cliente);

            // Assert
            result.Should().Be(expectedId);           // compara o Guid retornado
            result.Should().NotBe(Guid.Empty);       // garante que não é vazio

            _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Clientes>()), Times.Once);
        }            

            #endregion

            #region UpdateAsync Tests

            [Fact]
            public async Task UpdateAsync_ValidCliente_ReturnsUpdatedCliente()
            {
            // Arrange

            Guid id = Guid.NewGuid();
                var cliente = new Clientes
                {
                    cli_id = id,
                    cli_nome = "João Silva Atualizado",
                    cli_email = "joao.novo@teste.com",
                    cli_telefone = "11777777777",
                };

                var existingCliente = new Clientes { cli_id = id, cli_nome = "João Silva" };

                _repositoryMock
                    .Setup(r => r.GetByIdAsync(cliente.cli_id))
                    .ReturnsAsync(existingCliente);

            _repositoryMock
.Setup(r => r.UpdateAsync(cliente))
.ReturnsAsync(true); 

            // Act
            var result = await _clienteService.UpdateClienteAsync(cliente);

                // Assert
                result.Should().BeTrue();
                _repositoryMock.Verify(r => r.GetByIdAsync(cliente.cli_id), Times.Once);
                _repositoryMock.Verify(r => r.UpdateAsync(cliente), Times.Once);
            }

            [Fact]
            public async Task UpdateAsync_ClienteNotFound_ThrowsInvalidOperationException()
            {
                // Arrange
                var cliente = new Clientes { cli_id = Guid.NewGuid(), cli_nome = "Não Existe" };

                _repositoryMock
                    .Setup(r => r.GetByIdAsync(cliente.cli_id))
                    .ReturnsAsync((Clientes?)null);

                // Act & Assert
                var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                    () => _clienteService.UpdateClienteAsync(cliente));

                exception.Message.Should().Contain("Cliente não encontrado");
                _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Clientes>()), Times.Never);
            }

            #endregion

            #region DeleteAsync Tests

            [Fact]
            public async Task DeleteAsync_ValidId_DeletesCliente()
            {
                // Arrange
                var clienteId = Guid.Parse("B135761A-7B18-4681-B21E-12D832108ED2");
                var existingCliente = new Clientes { cli_id = clienteId, cli_nome = "João Silva" };

                _repositoryMock
                    .Setup(r => r.GetByIdAsync(clienteId))
                    .ReturnsAsync(existingCliente);

            _repositoryMock
    .Setup(r => r.DeleteAsync(clienteId))
    .ReturnsAsync(true); // true = deletado com sucesso

            // Act
            await _clienteService.DeleteClienteAsync(clienteId);

                // Assert
                _repositoryMock.Verify(r => r.GetByIdAsync(clienteId), Times.Once);
                _repositoryMock.Verify(r => r.DeleteAsync(clienteId), Times.Once);
            }

            [Fact]
            public async Task DeleteAsync_ClienteNotFound_ThrowsInvalidOperationException()
            {
                // Arrange
                var clienteId = Guid.Parse("B135761A-7B18-4681-B21E-12D832108ED2");

                _repositoryMock
                    .Setup(r => r.GetByIdAsync(clienteId))
                    .ReturnsAsync((Clientes?)null);

                // Act & Assert
                var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                    () => _clienteService.DeleteClienteAsync(clienteId));

                exception.Message.Should().Contain("Cliente não encontrado");
                _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            }

            #endregion
        }
    }

