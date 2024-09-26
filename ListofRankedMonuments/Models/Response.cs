using System.Text.Json.Serialization;

public class Response
{
    [JsonPropertyName("Status")]
    public int Status { get; set; }

    [JsonPropertyName("Message")]
    public string Message { get; set; }

    [JsonPropertyName("Data")]
    public object Data { get; set; }

    [JsonPropertyName("PageNumber")]
    public int? PageNumber { get; set; }

    [JsonPropertyName("PageSize")]
    public int? PageSize { get; set; }

    [JsonPropertyName("TotalPages")]
    public int? TotalPages { get; set; }

    [JsonPropertyName("TotalRecords")]
    public int? TotalRecords { get; set; }
}

public class ResponseRefreshToken : Response
{
    [JsonPropertyName("RefreshToken")]
    public string RefreshToken { get; set; }
}
public class RefreshTokenRequest
{
    [JsonPropertyName("RefreshToken")]
    public string RefreshToken { get; set; }
}

