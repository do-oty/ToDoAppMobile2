using System.Text.Json.Serialization;

namespace ToDoApp.Models
{
    public class UserData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("fname")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lname")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("timemodified")]
        public string TimeModified { get; set; } = string.Empty;
    }
} 