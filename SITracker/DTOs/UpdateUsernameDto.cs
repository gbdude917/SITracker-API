using Newtonsoft.Json;

namespace SITracker.DTOs
{
    public class UpdateUsernameDto
    {

        [JsonProperty("new_user_name")]
        public string? NewUsername { get; set; }
    }
}
