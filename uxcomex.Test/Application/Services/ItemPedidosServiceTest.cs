using Moq;
using uxcomex.Application.Services;
using uxcomex.Domain.Entities;
using uxcomex.Domain.Interfaces.Repositories;
using uxcomex.Domain.ViewModel;
using Xunit;

namespace uxcomex.Tests.Application.Services
{
    public class ItemPedidosServiceTests
    {
        private readonly Mock<IItemPedidosRepository> _mockRepo;
        private readonly ItemPedidosService _service;

        public ItemPedidosServiceTests()
        {
            _mockRepo = new Mock<IItemPedidosRepository>();
            _service = new ItemPedidosService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnItem()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new ItemPedidos { ped_id = id };
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(expected);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.ped_id);
            _mockRepo.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallRepository()
        {
            // Arrange
            var items = new List<ItemPedidos>
            {
                new ItemPedidos { ped_id = Guid.NewGuid() }
            };

            _mockRepo.Setup(r => r.CreateAsync(items)).Returns(Task.CompletedTask);

            // Act
            await _service.CreateAsync(items);

            // Assert
            _mockRepo.Verify(r => r.CreateAsync(items), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
        {
            // Arrange
            var items = new List<ItemPedidos> { new ItemPedidos { ped_id = Guid.NewGuid() } };
            _mockRepo.Setup(r => r.UpdateAsync(items)).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateAsync(items);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.UpdateAsync(items), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsyncViewModel_ShouldReturnList()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new List<ItemPedidoViewModel>
            {
                new ItemPedidoViewModel { ProdutoId = Guid.NewGuid(), Quantidade = 2, PrecoUnitario = 100 }
            };

            _mockRepo.Setup(r => r.GetByIdAsyncViewModel(id)).ReturnsAsync(expected);

            // Act
            var result = await _service.GetByIdAsyncViewModel(id);

            // Assert
            Assert.Single(result);
            Assert.Equal(expected[0].ProdutoId, result[0].ProdutoId);
            _mockRepo.Verify(r => r.GetByIdAsyncViewModel(id), Times.Once);
        }
    }
}
