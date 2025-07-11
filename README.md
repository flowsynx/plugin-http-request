# FlowSynx HTTP Request Plugin

The HTTP Request Plugin is a pre-packaged, plug-and-play integration component for the FlowSynx engine. It enables making HTTP requests to REST APIs and web services, supporting a variety of operations such as GET, POST, PUT, and DELETE. Designed for FlowSynx’s no-code/low-code automation workflows, this plugin simplifies API integration and external service communication.

This plugin is automatically installed by the FlowSynx engine when selected within the platform. It is not intended for manual installation or standalone developer use outside the FlowSynx environment.

---

## Purpose

The HTTP Request Plugin allows FlowSynx users to:

- Send HTTP requests to external APIs.
- Perform CRUD operations (GET, POST, PUT, DELETE) within workflows.
- Configure headers, content types, and request bodies dynamically.
- Handle JSON and plain text payloads in workflows without writing code.

---

## Supported Operations

- **get**: Sends an HTTP GET request to the specified URL.
- **post**: Sends an HTTP POST request with an optional body payload.
- **put**: Sends an HTTP PUT request with an optional body payload.
- **delete**: Sends an HTTP DELETE request to the specified URL.

---

## Plugin Specifications

The plugin requires the following configuration:

This plugin does not require global configuration. All request details are provided as input parameters.

---

## Input Parameters

Each operation accepts specific parameters:

| Parameter     | Type                         | Required | Description                                                              |
|---------------|-------------------------------|----------|--------------------------------------------------------------------------|
| `Operation`   | string (`get`, `post`, `put`, `delete`) | Yes      | The HTTP method to use for the request.                                  |
| `Url`         | string                        | Yes      | The URL of the API endpoint.                                             |
| `Headers`     | dictionary (string, string)   | No       | A key-value map of HTTP headers to include in the request.               |
| `Body`        | string                        | No       | The body payload for POST and PUT requests (JSON or plain text).         |
| `ContentType` | string                        | No       | The content type of the body (e.g., `application/json`, `text/plain`).   |

### Example input (POST)

```json
{
  "Operation": "post",
  "Url": "https://api.example.com/v1/items",
  "Headers": {
    "Authorization": "Bearer abc123",
    "Accept": "application/json"
  },
  "Body": "{"name": "Sample Item", "price": 19.99}",
  "ContentType": "application/json"
}
```

### Example input (GET)

```json
{
  "Operation": "get",
  "Url": "https://api.example.com/v1/items",
  "Headers": {
    "Authorization": "Bearer abc123"
  }
}
```

---

## Debugging Tips

- Verify the `Url` is correct and the API endpoint is accessible from the FlowSynx environment.
- Ensure the `Headers` include proper authorization if required (e.g., API keys, Bearer tokens).
- For POST and PUT requests, confirm the `Body` is properly formatted JSON if using `application/json`.
- Check response codes and payloads using FlowSynx’s built-in debugging tools.

---

## HTTP Considerations

- **Timeouts**: Requests are subject to FlowSynx’s default timeout settings.
- **Redirects**: Follow redirects are enabled by default for GET requests.
- **Security**: HTTPS is recommended for all requests to ensure encryption of data in transit.

---

## Security Notes

- No credentials or request data are persisted outside the execution scope unless explicitly configured.
- Only authorized FlowSynx platform users can view or modify plugin configurations.

---

## License

© FlowSynx. All rights reserved.
