using System.Text.Json.Serialization;

namespace WebBff.API.DTOs
{
    public class UserDto
    {
        [JsonPropertyName("id")] 
        public int UserId { get; set; }

        [JsonPropertyName("username")] 
        public string UserName { get; set; }
    }
}
