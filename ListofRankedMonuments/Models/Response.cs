using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

// Tạo một lớp để cấu hình đối tượng JSON
public class Response
{
    [JsonPropertyName("Status")]
    public int Status { get; set; }

    public string Token { get; set; }

    [JsonPropertyName("Message")]
    public string Message { get; set; }
}

