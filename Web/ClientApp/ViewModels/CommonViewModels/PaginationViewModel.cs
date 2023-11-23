namespace ClientApp.ViewModels.CommonViewModels;

public class PaginationViewModel
{
    public long TotalItems { get; set; }

    public int ItemsPerPage { get; set; }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }

    public bool IsPreviousDisabled { get; set; }

    public bool IsNextDisabled { get; set; }
}