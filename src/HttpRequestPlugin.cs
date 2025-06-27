using FlowSynx.PluginCore;
using FlowSynx.PluginCore.Extensions;
using FlowSynx.PluginCore.Helpers;
using FlowSynx.Plugins.HttpRequest.Models;
using System.Text;

namespace FlowSynx.Plugins.HttpRequest;

public class HttpRequestPlugin : IPlugin
{
    private IPluginLogger? _logger;
    private static readonly HttpClient _httpClient = new HttpClient();
    private bool _isInitialized;

    public PluginMetadata Metadata
    {
        get
        {
            return new PluginMetadata
            {
                Id = Guid.Parse("fa9e852c-d81d-4f29-9a79-28c091c7bd22"),
                Name = "HttpRequest",
                CompanyName = "FlowSynx",
                Description = Resources.PluginDescription,
                Version = new PluginVersion(1, 1, 0),
                Category = PluginCategory.Web,
                Authors = new List<string> { "FlowSynx" },
                Copyright = "© FlowSynx. All rights reserved.",
                Icon = "flowsynx.png",
                ReadMe = "README.md",
                RepositoryUrl = "https://github.com/flowsynx/plugin-http-request",
                ProjectUrl = "https://flowsynx.io",
                Tags = new List<string>() { "flowSynx", "http", "http-request", "rest", "restful" }
            };
        }
    }

    public PluginSpecifications? Specifications { get; set; }

    public Type SpecificationsType => typeof(HttpRequestPluginSpecifications);

    public IReadOnlyCollection<string> SupportedOperations => new[] { "get", "post", "put", "delete" };

    public Task Initialize(IPluginLogger logger)
    {
        if (ReflectionHelper.IsCalledViaReflection())
            throw new InvalidOperationException(Resources.ReflectionBasedAccessIsNotAllowed);

        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
        _isInitialized = true;
        return Task.CompletedTask;
    }

    public async Task<object?> ExecuteAsync(PluginParameters parameters, CancellationToken cancellationToken)
    {
        if (ReflectionHelper.IsCalledViaReflection())
            throw new InvalidOperationException(Resources.ReflectionBasedAccessIsNotAllowed);

        if (!_isInitialized)
            throw new InvalidOperationException($"Plugin '{Metadata.Name}' v{Metadata.Version} is not initialized.");

        var operationParameter = parameters.ToObject<OperationParameter>();
        var operation = operationParameter.Operation.ToLowerInvariant();
        var url = operationParameter.Url;
        var headers = operationParameter.Headers ?? new Dictionary<string, string>();
        var body = operationParameter.Body;
        var contentType = operationParameter.ContentType ?? "application/json";

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL must be specified in specifications.");

        var request = new HttpRequestMessage
        {
            Method = operation switch
            {
                "get" => HttpMethod.Get,
                "post" => HttpMethod.Post,
                "put" => HttpMethod.Put,
                "delete" => HttpMethod.Delete,
                _ => throw new NotSupportedException($"HttpRequest plugin: Operation '{operation}' is not supported.")
            },
            RequestUri = new Uri(url)
        };

        foreach (var header in headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (operation is "post" or "put" && !string.IsNullOrEmpty(body))
        {
            request.Content = new StringContent(body, Encoding.UTF8, contentType);
        }

        _logger?.LogInfo($"Sending {operation.ToUpper()} request to {url}");

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync();

        _logger?.LogInfo($"Response: {(int)response.StatusCode} {response.ReasonPhrase}");

        var result = new PluginContext(Guid.NewGuid().ToString(), "Data")
        {
            Format = "Json",
            Content = responseBody
        };

        result.Metadata["StatusCode"] = (int)response.StatusCode;
        if (response.ReasonPhrase is not null)
            result.Metadata["ReasonPhrase"] = response.ReasonPhrase;

        return result;
    }
}