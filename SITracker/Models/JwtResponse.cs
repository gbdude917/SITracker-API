namespace SITracker.Models
{
    public class JwtResponse
    {
        public string? Token { get; set; }

        public string? Username { get; set; }

        public DateTime? Expiration { get; set; }
    }
}
