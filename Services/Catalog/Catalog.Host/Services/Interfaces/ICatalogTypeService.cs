using Catalog.Host.Data.Entities;
using Catalog.Host.Models.DTOs;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogTypeService
{
    Task<DataResponse<CatalogTypeDto>> GetCatalogTypesAsync();

    Task<CatalogTypeDto> GetCatalogTypeByIdAsync(int id);

    Task<CatalogType> FindCatalogTypeAsync(int id);

    Task<CatalogType> AddCatalogTypeAsync(AddTypeRequest request);

    Task<CatalogType> UpdateCatalogTypeAsync(UpdateTypeRequest request, CatalogType catalogType);

    Task<bool> DeleteCatalogTypeAsync(CatalogType catalogType);
}