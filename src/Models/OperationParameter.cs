namespace FlowSynx.Plugins.HttpRequest.Models;

internal class OperationParameter
{
    public string Operation { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public Dictionary<string, string>? Headers { get; set; }
    public string? Body { get; set; }
    public string? ContentType { get; set; }
}