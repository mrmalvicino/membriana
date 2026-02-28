namespace Mvc.ViewModels;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrWhiteSpace(RequestId);
    public int? StatusCode { get; set; }
    public string? Path { get; set; }
}
