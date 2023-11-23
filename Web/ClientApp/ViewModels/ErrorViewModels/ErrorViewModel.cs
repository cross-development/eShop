namespace ClientApp.ViewModels.ErrorViewModels;

public class ErrorViewModel
{
    public string RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}