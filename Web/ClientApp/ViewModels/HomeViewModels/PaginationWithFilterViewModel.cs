using ClientApp.ViewModels.CommonViewModels;

namespace ClientApp.ViewModels.HomeViewModels;

public sealed class PaginationWithFilterViewModel : PaginationViewModel
{
    public int? BrandId { get; set; }

    public int? TypeId { get; set; }
}