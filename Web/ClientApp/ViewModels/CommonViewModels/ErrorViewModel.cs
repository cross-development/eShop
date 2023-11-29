namespace ClientApp.ViewModels.CommonViewModels;

public sealed class ErrorViewModel
{
    public string RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}