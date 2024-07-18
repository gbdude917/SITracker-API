using Newtonsoft.Json;

namespace SITracker.Dtos
{
    public class UpdateUsernameDto
    {

        [JsonProperty("new_user_name")]
        public string? NewUsername { get; set; }
    }
}
