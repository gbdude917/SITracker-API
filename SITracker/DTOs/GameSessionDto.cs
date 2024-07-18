using Newtonsoft.Json;

namespace SITracker.Dtos
{
    public class GameSessionDto
    {
        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("spirit_id")]
        public long SpiritId { get; set; }

        [JsonProperty("adversary_id")]
        public long AdversaryId { get; set; }

        [JsonProperty("board")]
        public string? Board { get; set; }

        [JsonProperty("session_name")]
        public string? SessionName { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("played_on")]
        public DateTime? PlayedOn { get; set; }

        [JsonProperty("result")]
        public string? Result { get; set; }

        [JsonProperty("is_completed")]
        public bool IsCompleted { get; set; }
    }
}
