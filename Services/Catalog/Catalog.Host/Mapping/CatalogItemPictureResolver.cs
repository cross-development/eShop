using Microsoft.Extensions.Options;
using AutoMapper;
using Catalog.Host.Models.DTOs;
using Catalog.Host.Data.Entities;
using Catalog.Host.Configurations;

namespace Catalog.Host.Mapping;

public class CatalogItemPictureResolver : IMemberValueResolver<CatalogItem, CatalogItemDto, string, object>
{
    private readonly CatalogConfiguration _config;

    public CatalogItemPictureResolver(IOptions<CatalogConfiguration> config)
    {
        _config = config.Value;
    }

    public object Resolve(CatalogItem source, CatalogItemDto destination, string sourceMember, object destMember,
        ResolutionContext context)
    {
        return $"{_config.Host}/{_config.ImgUrl}/{sourceMember}";
    }
}