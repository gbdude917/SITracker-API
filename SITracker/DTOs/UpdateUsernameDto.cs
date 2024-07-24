using Newtonsoft.Json;

namespace SITracker.Dtos
{
    public class UpdateUsernameDto
    {

        [JsonProperty("new_username")]
        public string? NewUsername { get; set; }
    }
}
