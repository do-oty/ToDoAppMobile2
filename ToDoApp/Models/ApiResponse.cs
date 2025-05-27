using System.Text.Json.Serialization;

namespace ToDoApp.Models;

public class ApiResponse<T>
{
    [JsonPropertyName("status")]
    public int status { get; set; }

    [JsonPropertyName("success")]
    public bool success { get; set; }

    [JsonPropertyName("message")]
    public string? message { get; set; }

    [JsonPropertyName("data")]
    public T? data { get; set; }
} 