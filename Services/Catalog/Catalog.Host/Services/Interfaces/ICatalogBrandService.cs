using Catalog.Host.Data.Entities;
using Catalog.Host.Models.DTOs;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogBrandService
{
    Task<DataResponse<CatalogBrandDto>> GetCatalogBrandsAsync();

    Task<CatalogBrandDto> GetCatalogBrandByIdAsync(int id);

    Task<CatalogBrand> FindCatalogBrandAsync(int id);

    Task<CatalogBrand> AddCatalogBrandAsync(AddBrandRequest request);

    Task<CatalogBrand> UpdateCatalogBrandAsync(UpdateBrandRequest request, CatalogBrand catalogBrand);

    Task<bool> DeleteCatalogBrandAsync(CatalogBrand catalogBrand);
}