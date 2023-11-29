using Microsoft.AspNetCore.Mvc.Rendering;
using ClientApp.Models;

namespace ClientApp.ViewModels.HomeViewModels;

public sealed class CatalogViewModel
{
    public IEnumerable<CatalogItem> CatalogItems { get; set; }

    public int? BrandId { get; set; }

    public IEnumerable<SelectListItem> Brands { get; set; }

    public int? TypeId { get; set; }

    public IEnumerable<SelectListItem> Types { get; set; }

    public PaginationWithFilterViewModel PaginationViewModel { get; set; }
}