using System.Text.Json.Serialization;

namespace ToDoApp.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    public class TodoItem
    {
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        [JsonPropertyName("item_name")]
        public string ItemName { get; set; } = string.Empty;

        [JsonPropertyName("item_description")]
        public string ItemDescription { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("timemodified")]
        public string TimeModified { get; set; } = string.Empty;
    }

    public class TodoItemsResponse : ApiResponse
    {
        [JsonPropertyName("data")]
        public Dictionary<string, TodoItem>? Data { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

    public class AddTodoResponse : ApiResponse
    {
        [JsonPropertyName("data")]
        public TodoItem? Data { get; set; }
    }

    public class SignInResponse : ApiResponse
    {
        [JsonPropertyName("data")]
        public UserData? Data { get; set; }
    }
} 