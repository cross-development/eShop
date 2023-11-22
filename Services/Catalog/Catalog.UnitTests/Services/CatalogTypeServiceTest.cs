using Moq;

namespace Catalog.UnitTests.Services;

public class CatalogTypeServiceTest
{
    private readonly Mock<IApplicationDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
    private readonly Mock<ILogger<CatalogTypeService>> _logger;
    private readonly Mock<IMapper> _mapper;

    private readonly ICatalogTypeService _catalogTypeService;

    public CatalogTypeServiceTest()
    {
        _dbContextWrapper = new Mock<IApplicationDbContextWrapper<ApplicationDbContext>>();
        _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
        _logger = new Mock<ILogger<CatalogTypeService>>();
        _mapper = new Mock<IMapper>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper
            .Setup(context => context.BeginTransactionAsync(CancellationToken.None))
            .ReturnsAsync(dbContextTransaction.Object);

        _catalogTypeService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogTypesAsync_Success()
    {
        // arrange
        var catalogTypesSuccess = new List<CatalogType>
        {
            new CatalogType
            {
                Type = "TestName"
            }
        };

        var catalogType = new CatalogType
        {
            Type = "TestName"
        };

        var catalogTypeDto = new CatalogTypeDto
        {
            Type = "TestName"
        };

        _catalogTypeRepository.Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(catalogTypesSuccess);

        _mapper.Setup(mapper => mapper.Map<CatalogTypeDto>(
            It.Is<CatalogType>(type => type.Equals(catalogType)))
        ).Returns(catalogTypeDto);

        // act
        var result = await _catalogTypeService.GetCatalogTypesAsync();

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogTypesAsync_Failed()
    {
        // arrange
        _catalogTypeRepository.Setup(repository => repository.GetAllAsync())
            .Returns((Func<PaginatedResponse<CatalogTypeDto>>)null!);

        // act
        var result = await _catalogTypeService.GetCatalogTypesAsync();

        // result
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogTypeByIdAsync_Success()
    {
        // arrange
        var testTypeId = 1;
        var testTypeName = "TestName";

        var catalogType = new CatalogType
        {
            Id = testTypeId,
            Type = testTypeName
        };

        var catalogTypeDto = new CatalogTypeDto
        {
            Id = testTypeId,
            Type = testTypeName
        };

        _catalogTypeRepository.Setup(repository => repository.GetByIdAsync(
            It.Is<int>(typeId => typeId == testTypeId))
        ).ReturnsAsync(catalogType);

        _mapper.Setup(mapper => mapper.Map<CatalogTypeDto>(
            It.Is<CatalogType>(type => type.Equals(catalogType)))
        ).Returns(catalogTypeDto);

        // act
        var result = await _catalogTypeService.GetCatalogTypeByIdAsync(testTypeId);

        // assert
        result.Should().NotBeNull();
        result?.Type.Should().Be(testTypeName);
        result?.Id.Should().Be(testTypeId);
    }

    [Fact]
    public async Task GetCatalogTypeByIdAsync_Failed()
    {
        // arrange
        var testTypeId = 1;

        _catalogTypeRepository.Setup(repository => repository.GetByIdAsync(
            It.Is<int>(typeId => typeId == testTypeId))
        ).ReturnsAsync((Func<CatalogType>)null!);

        // act
        var result = await _catalogTypeService.GetCatalogTypeByIdAsync(testTypeId);

        // result
        result.Should().BeNull();
    }

    [Fact]
    public async Task FindCatalogTypeAsync_Success()
    {
        // arrange
        var testTypeId = 1;
        var testTypeName = "TestName";

        var catalogType = new CatalogType
        {
            Id = testTypeId,
            Type = testTypeName
        };

        _catalogTypeRepository.Setup(repository => repository.FindOneAsync(
            It.Is<int>(typeId => typeId == testTypeId))
        ).ReturnsAsync(catalogType);

        // act
        var result = await _catalogTypeService.FindCatalogTypeAsync(testTypeId);

        // assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(testTypeId);
        result?.Type.Should().Be(testTypeName);
    }

    [Fact]
    public async Task FindCatalogTypeAsync_Failed()
    {
        // arrange
        var testTypeId = 1;

        _catalogTypeRepository.Setup(repository => repository.FindOneAsync(
            It.Is<int>(typeId => typeId == testTypeId))
        ).ReturnsAsync((Func<CatalogType>)null!);

        // act
        var result = await _catalogTypeService.FindCatalogTypeAsync(testTypeId);

        // result
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddCatalogTypeAsync_Success()
    {
        var catalogType = new CatalogType
        {
            Id = 1,
            Type = "TestName"
        };

        var addTypeRequest = new AddTypeRequest
        {
            Type = catalogType.Type
        };

        _catalogTypeRepository.Setup(repository => repository.AddAsync(
            It.Is<CatalogType>(type => type.Equals(catalogType)))
        ).ReturnsAsync(catalogType);

        _mapper.Setup(mapper => mapper.Map<CatalogType>(
            It.Is<AddTypeRequest>(type => type.Equals(addTypeRequest)))
        ).Returns(catalogType);

        // act
        var result = await _catalogTypeService.AddCatalogTypeAsync(addTypeRequest);

        // assert
        result.Should().Be(catalogType);
    }

    [Fact]
    public async Task AddCatalogTypeAsync_Failed()
    {
        // arrange
        CatalogType? testResult = null;

        var addTypeRequest = new AddTypeRequest
        {
            Type = "TestName"
        };

        _catalogTypeRepository.Setup(repository => repository.AddAsync(
            It.Is<CatalogType>(type => type.Equals(addTypeRequest)))
        ).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.AddCatalogTypeAsync(addTypeRequest);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateCatalogTypeAsync_Success()
    {
        // arrange
        var catalogType = new CatalogType
        {
            Id = 1,
            Type = "TestName"
        };

        var updateTypeRequest = new UpdateTypeRequest
        {
            Type = "TestName UPD"
        };

        catalogType.Type = updateTypeRequest.Type ?? catalogType.Type;

        _catalogTypeRepository.Setup(repository => repository.UpdateAsync(
            It.Is<CatalogType>(type => type == catalogType))
        ).ReturnsAsync(catalogType);

        // act
        var result = await _catalogTypeService.UpdateCatalogTypeAsync(updateTypeRequest, catalogType);

        // assert
        result.Should().NotBeNull();
        result?.Should().Be(catalogType);
    }

    [Fact]
    public async Task UpdateCatalogTypeAsync_Failed()
    {
        // arrange
        CatalogType? testResult = null;

        var catalogType = new CatalogType
        {
            Id = 1,
            Type = "TestName"
        };

        var updateTypeRequest = new UpdateTypeRequest
        {
            Type = "TestName UPD"
        };

        catalogType.Type = updateTypeRequest.Type ?? catalogType.Type;

        _catalogTypeRepository.Setup(repository => repository.UpdateAsync(
            It.Is<CatalogType>(type => type == catalogType))
        ).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.UpdateCatalogTypeAsync(updateTypeRequest, catalogType);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateCatalogTypeAsync_WithDefaultValues_Success()
    {
        // arrange
        var catalogType = new CatalogType
        {
            Id = 1,
            Type = "TestName"
        };

        var updateTypeRequest = new UpdateTypeRequest();

        catalogType.Type = updateTypeRequest.Type ?? catalogType.Type;

        _catalogTypeRepository.Setup(repository => repository.UpdateAsync(
            It.Is<CatalogType>(type => type == catalogType))
        ).ReturnsAsync(catalogType);

        // act
        var result = await _catalogTypeService.UpdateCatalogTypeAsync(updateTypeRequest, catalogType);

        // assert
        result.Should().NotBeNull();
        result?.Should().Be(catalogType);
    }

    [Fact]
    public async Task DeleteCatalogTypeAsync_Success()
    {
        // arrange
        var testResult = true;

        var catalogType = new CatalogType
        {
            Id = 1,
            Type = "TestName"
        };

        _catalogTypeRepository.Setup(repository => repository.DeleteAsync(
            It.Is<CatalogType>(type => type == catalogType))
        ).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.DeleteCatalogTypeAsync(catalogType);

        // assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteCatalogTypeAsync_Failed()
    {
        // arrange
        var testResult = false;

        var catalogType = new CatalogType
        {
            Id = 1,
            Type = "TestName"
        };

        _catalogTypeRepository.Setup(repository => repository.DeleteAsync(
            It.Is<CatalogType>(type => type == catalogType))
        ).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.DeleteCatalogTypeAsync(catalogType);

        // result
        result.Should().BeFalse();
    }
}