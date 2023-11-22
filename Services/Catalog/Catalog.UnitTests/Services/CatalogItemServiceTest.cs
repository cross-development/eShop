using Microsoft.EntityFrameworkCore;
using Moq;

namespace Catalog.UnitTests.Services;

public class CatalogItemServiceTest
{
    private readonly Mock<IApplicationDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<ILogger<CatalogItemService>> _logger;
    private readonly Mock<IMapper> _mapper;

    private readonly ICatalogItemService _catalogItemService;

    private readonly CatalogItem _testItem = new CatalogItem
    {
        Name = "Name",
        Description = "Description",
        Price = 100,
        AvailableStock = 100,
        CatalogBrandId = 1,
        CatalogTypeId = 1,
        PictureFileName = "1.png"
    };

    public CatalogItemServiceTest()
    {
        _dbContextWrapper = new Mock<IApplicationDbContextWrapper<ApplicationDbContext>>();
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _logger = new Mock<ILogger<CatalogItemService>>();
        _mapper = new Mock<IMapper>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper
            .Setup(context => context.BeginTransactionAsync(CancellationToken.None))
            .ReturnsAsync(dbContextTransaction.Object);

        _catalogItemService = new CatalogItemService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testTotalCount = 1;

        var catalogItem = new CatalogItem
        {
            Name = "TestName",
            CatalogBrandId = 1,
            CatalogTypeId = 1
        };

        var itemsListSuccess = new List<CatalogItem> { catalogItem };

        var paginatedFilterRequest = new PaginatedItemRequest
        {
            Limit = 10,
            Page = 1,
            BrandId = 1,
            TypeId = 1
        };

        var catalogItemDto = new CatalogItemDto
        {
            Name = catalogItem.Name
        };

        _catalogItemRepository.Setup(repository => repository.GetAllAsync(
            It.Is<PaginatedItemRequest>(item => item.Equals(paginatedFilterRequest)))
        ).ReturnsAsync(itemsListSuccess);

        _catalogItemRepository.Setup(repository => repository.GetCountAsync()
        ).ReturnsAsync(testTotalCount);

        _mapper.Setup(mapper => mapper.Map<CatalogItemDto>(
            It.Is<CatalogItem>(item => item.Equals(catalogItem)))
        ).Returns(catalogItemDto);

        // act
        var result = await _catalogItemService.GetCatalogItemsAsync(paginatedFilterRequest);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.Page.Should().Be(paginatedFilterRequest.Page);
        result?.Limit.Should().Be(paginatedFilterRequest.Limit);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPage = 10000;
        var testLimit = 10000;
        var testBrandId = 10000;
        var testTypeId = 10000;

        var paginatedFilterRequest = new PaginatedItemRequest
        {
            Limit = testLimit,
            Page = testPage,
            BrandId = testBrandId,
            TypeId = testTypeId
        };

        _catalogItemRepository.Setup(repository => repository.GetAllAsync(
            It.Is<PaginatedItemRequest>(item => item.Equals(paginatedFilterRequest)))
        ).Returns((Func<PaginatedResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogItemService.GetCatalogItemsAsync(paginatedFilterRequest);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogItemByIdAsync_Success()
    {
        // arrange
        var testItemId = 1;
        var testItemName = "TestName";

        var catalogItem = new CatalogItem
        {
            Id = testItemId,
            Name = testItemName,
            CatalogBrandId = 1,
            CatalogTypeId = 1
        };

        var catalogItemDto = new CatalogItemDto
        {
            Id = testItemId,
            Name = testItemName,
        };

        _catalogItemRepository.Setup(repository => repository.GetByIdAsync(
            It.Is<int>(itemId => itemId == testItemId))
        ).ReturnsAsync(catalogItem);

        _mapper.Setup(mapper => mapper.Map<CatalogItemDto>(
            It.Is<CatalogItem>(item => item.Equals(catalogItem)))
        ).Returns(catalogItemDto);

        // act
        var result = await _catalogItemService.GetCatalogItemByIdAsync(testItemId);

        // assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(testItemId);
        result?.Name.Should().Be(testItemName);
    }

    [Fact]
    public async Task GetCatalogItemByIdAsync_Failed()
    {
        // arrange
        var testItemId = 1;

        _catalogItemRepository.Setup(repository => repository.GetByIdAsync(
            It.Is<int>(itemId => itemId == testItemId))
        ).ReturnsAsync((Func<CatalogItem>)null!);

        // act
        var result = await _catalogItemService.GetCatalogItemByIdAsync(testItemId);

        // result
        result.Should().BeNull();
    }

    [Fact]
    public async Task FindCatalogItemAsync_Success()
    {
        // arrange
        var testItemId = 1;
        var testItemName = "TestName";

        var catalogItem = new CatalogItem
        {
            Id = testItemId,
            Name = testItemName
        };

        _catalogItemRepository.Setup(repository => repository.FindOneAsync(
            It.Is<int>(itemId => itemId == testItemId))
        ).ReturnsAsync(catalogItem);

        // act
        var result = await _catalogItemService.FindCatalogItemAsync(testItemId);

        // assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(testItemId);
        result?.Name.Should().Be(testItemName);
    }

    [Fact]
    public async Task FindCatalogItemAsync_Failed()
    {
        // arrange
        var testItemId = 1;

        _catalogItemRepository.Setup(repository => repository.FindOneAsync(
            It.Is<int>(itemId => itemId == testItemId))
        ).ReturnsAsync((Func<CatalogItem>)null!);

        // act
        var result = await _catalogItemService.FindCatalogItemAsync(testItemId);

        // result
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddCatalogItemAsync_Success()
    {
        // arrange
        var addItemRequest = new AddItemRequest
        {
            Name = _testItem.Name,
            Description = _testItem.Description,
            Price = _testItem.Price,
            PictureFileName = _testItem.PictureFileName,
            AvailableStock = _testItem.AvailableStock,
            CatalogBrandId = _testItem.CatalogBrandId,
            CatalogTypeId = _testItem.CatalogTypeId
        };

        _catalogItemRepository.Setup(repository => repository.AddAsync(
            It.Is<CatalogItem>(item => item.Equals(_testItem)))
        ).ReturnsAsync(_testItem);

        _mapper.Setup(mapper => mapper.Map<CatalogItem>(
            It.Is<AddItemRequest>(item => item.Equals(addItemRequest)))
        ).Returns(_testItem);

        // act
        var result = await _catalogItemService.AddCatalogItemAsync(addItemRequest);

        // assert
        result.Should().Be(_testItem);
    }

    [Fact]
    public async Task AddCatalogItemAsync_Failed()
    {
        // arrange
        CatalogItem? testResult = null;

        var addItemRequest = new AddItemRequest
        {
            Name = _testItem.Name,
            Description = _testItem.Description,
            Price = _testItem.Price,
            PictureFileName = _testItem.PictureFileName,
            AvailableStock = _testItem.AvailableStock,
            CatalogBrandId = _testItem.CatalogBrandId,
            CatalogTypeId = _testItem.CatalogTypeId
        };

        _catalogItemRepository.Setup(repository => repository.AddAsync(
           It.Is<CatalogItem>(item => item.Equals(addItemRequest)))
        ).ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.AddCatalogItemAsync(addItemRequest);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_Success()
    {
        // arrange
        var catalogItem = new CatalogItem
        {
            Id = 1,
            Name = _testItem.Name,
            Description = _testItem.Description,
            Price = _testItem.Price,
            PictureFileName = _testItem.PictureFileName,
            AvailableStock = _testItem.AvailableStock,
            CatalogBrandId = _testItem.CatalogBrandId,
            CatalogTypeId = _testItem.CatalogTypeId
        };

        var updateItemRequest = new UpdateItemRequest
        {
            Name = "Name UPD",
            Description = "Description UPD",
            Price = 2,
            PictureFileName = "2.png",
            AvailableStock = 2,
            CatalogBrandId = 2,
            CatalogTypeId = 2
        };

        catalogItem.Name = updateItemRequest.Name ?? catalogItem.Name;
        catalogItem.Description = updateItemRequest.Description ?? catalogItem.Description;
        catalogItem.Price = updateItemRequest.Price ?? catalogItem.Price;
        catalogItem.PictureFileName = updateItemRequest.PictureFileName ?? catalogItem.PictureFileName;
        catalogItem.AvailableStock = updateItemRequest.AvailableStock ?? catalogItem.AvailableStock;
        catalogItem.CatalogBrandId = updateItemRequest.CatalogBrandId ?? catalogItem.CatalogBrandId;
        catalogItem.CatalogTypeId = updateItemRequest.CatalogTypeId ?? catalogItem.CatalogTypeId;

        _catalogItemRepository.Setup(repository => repository.UpdateAsync(
            It.Is<CatalogItem>(brand => brand == catalogItem))
        ).ReturnsAsync(catalogItem);

        // act
        var result = await _catalogItemService.UpdateCatalogItemAsync(updateItemRequest, catalogItem);

        // assert
        result.Should().NotBeNull();
        result?.Should().Be(catalogItem);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_Failed()
    {
        // arrange
        CatalogItem? testResult = null;

        var catalogItem = new CatalogItem
        {
            Id = 1,
            Name = _testItem.Name,
            Description = _testItem.Description,
            Price = _testItem.Price,
            PictureFileName = _testItem.PictureFileName,
            AvailableStock = _testItem.AvailableStock,
            CatalogBrandId = _testItem.CatalogBrandId,
            CatalogTypeId = _testItem.CatalogTypeId
        };

        var updateItemRequest = new UpdateItemRequest
        {
            Name = "Name UPD",
            Description = "Description UPD",
            Price = 2,
            PictureFileName = "2.png",
            AvailableStock = 2,
            CatalogBrandId = 2,
            CatalogTypeId = 2
        };

        catalogItem.Name = updateItemRequest.Name ?? catalogItem.Name;
        catalogItem.Description = updateItemRequest.Description ?? catalogItem.Description;
        catalogItem.Price = updateItemRequest.Price ?? catalogItem.Price;
        catalogItem.PictureFileName = updateItemRequest.PictureFileName ?? catalogItem.PictureFileName;
        catalogItem.AvailableStock = updateItemRequest.AvailableStock ?? catalogItem.AvailableStock;
        catalogItem.CatalogBrandId = updateItemRequest.CatalogBrandId ?? catalogItem.CatalogBrandId;
        catalogItem.CatalogTypeId = updateItemRequest.CatalogTypeId ?? catalogItem.CatalogTypeId;

        _catalogItemRepository.Setup(repository => repository.UpdateAsync(
            It.Is<CatalogItem>(item => item == catalogItem))
        ).ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.UpdateCatalogItemAsync(updateItemRequest, catalogItem);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_WithDefaultValues_Success()
    {
        // arrange
        var catalogItem = new CatalogItem
        {
            Id = 1,
            Name = _testItem.Name,
            Description = _testItem.Description,
            Price = _testItem.Price,
            PictureFileName = _testItem.PictureFileName,
            AvailableStock = _testItem.AvailableStock,
            CatalogBrandId = _testItem.CatalogBrandId,
            CatalogTypeId = _testItem.CatalogTypeId
        };

        var updateItemRequest = new UpdateItemRequest();

        catalogItem.Name = updateItemRequest.Name ?? catalogItem.Name;
        catalogItem.Description = updateItemRequest.Description ?? catalogItem.Description;
        catalogItem.Price = updateItemRequest.Price ?? catalogItem.Price;
        catalogItem.PictureFileName = updateItemRequest.PictureFileName ?? catalogItem.PictureFileName;
        catalogItem.AvailableStock = updateItemRequest.AvailableStock ?? catalogItem.AvailableStock;
        catalogItem.CatalogBrandId = updateItemRequest.CatalogBrandId ?? catalogItem.CatalogBrandId;
        catalogItem.CatalogTypeId = updateItemRequest.CatalogTypeId ?? catalogItem.CatalogTypeId;

        _catalogItemRepository.Setup(repository => repository.UpdateAsync(
            It.Is<CatalogItem>(brand => brand == catalogItem))
        ).ReturnsAsync(catalogItem);

        // act
        var result = await _catalogItemService.UpdateCatalogItemAsync(updateItemRequest, catalogItem);

        // assert
        result.Should().NotBeNull();
        result?.Should().Be(catalogItem);
    }

    [Fact]
    public async Task DeleteCatalogItemAsync_Success()
    {
        // arrange
        var testResult = true;

        var catalogItem = new CatalogItem
        {
            Id = 1,
            Name = "TestName"
        };

        _catalogItemRepository.Setup(repository => repository.DeleteAsync(
            It.Is<CatalogItem>(item => item == catalogItem))
        ).ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.DeleteCatalogItemAsync(catalogItem);

        // assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteCatalogItemAsync_Failed()
    {
        // arrange
        var testResult = false;

        var catalogItem = new CatalogItem
        {
            Id = 1,
            Name = "TestName"
        };

        _catalogItemRepository.Setup(repository => repository.DeleteAsync(
            It.Is<CatalogItem>(item => item == catalogItem))
        ).ReturnsAsync(testResult);

        // act
        var result = await _catalogItemService.DeleteCatalogItemAsync(catalogItem);

        // result
        result.Should().BeFalse();
    }
}