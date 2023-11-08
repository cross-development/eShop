using AutoMapper;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.DTOs;
using Catalog.Host.Models.Requests;

namespace Catalog.Host.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddItemRequest, CatalogItem>();
        CreateMap<CatalogItem, CatalogItemDto>()
            .ForMember("PictureUrl", options =>
                options.MapFrom<CatalogItemPictureResolver, string>(catalogItem => catalogItem.PictureFileName));
        
        CreateMap<AddBrandRequest, CatalogBrand>();
        CreateMap<CatalogBrand, CatalogBrandDto>();

        CreateMap<AddTypeRequest, CatalogType>();
        CreateMap<CatalogType, CatalogTypeDto>();
    }
}