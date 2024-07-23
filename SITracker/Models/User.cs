using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SITracker.Models
{
    [Table("users")]
    [Index(nameof(Email), nameof(Username), IsUnique = true)]
    public class User
    {
        public User()
        {
            GameSessions = new List<GameSession>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public long Id { get; set; }

        [Required]
        [Column("email")]
        public string? Email { get; set; }

        [Required]
        [Column("username")]
        public string? Username { get; set; }

        [Required]
        [JsonIgnore]
        [Column("password")]
        public string? Password { get; set; }

        [Required]
        [Column("registration_date")]
        public DateTime RegistrationDate { get; set; }

        [JsonIgnore]
        public ICollection<GameSession> GameSessions { get; set; }

        // [JsonIgnore]
        // public ICollection<Authority> Authorities { get; set; }


    }
}
