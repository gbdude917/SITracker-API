using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SITracker.Models
{
    [Table("game_sessions")]

    public class GameSession
    {
        public GameSession()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("game_session_id")]
        public long Id { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        [JsonIgnore]
        public long UserId { get; set; }

        public User? User { get; set; }

        [Required]
        [ForeignKey("Spirit")]
        [Column("spirit_id")]
        [JsonIgnore]
        public long SpiritId { get; set; }

        public Spirit? Spirit { get; set; }

        [Required]
        [ForeignKey("Adversary")]
        [Column("adversary_id")]
        [JsonIgnore]
        public long AdversaryId { get; set; }

        public Adversary? Adversary { get; set; }

        [Required]
        [Column("board")]
        public string? Board { get; set; }

        [Required]
        [Column("session_name")]
        public string? SessionName { get; set; }

        [Required]
        [Column("description")]
        public string? Description { get; set; }

        [Column("played_on")]
        public DateTime? PlayedOn { get; set; }

        [Column("result")]
        public string? Result { get; set; }

        [Column("is_completed")]
        public bool IsCompleted { get; set; }
    }
}
