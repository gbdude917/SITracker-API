using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SITracker.Models
{
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
        public long UserId { get; set; }
        public User User { get; set; }

        [Required]
        [ForeignKey("Spirit")]
        [Column("spirit_id")]
        public long SpiritId { get; set; }
        public Spirit Spirit { get; set; }

        [Required]
        [ForeignKey("Adversary")]
        [Column("adversary_id")]
        public long AdversaryId { get; set; }
        public Adversary Adversary { get; set; }

        [Required]
        public string Board { get; set; }

        [Required]
        [Column("session_name")]
        public string SessionName { get; set; }

        [Required]
        public string Description { get; set; }

        [Column("played_on")]
        public DateTime? PlayedOn { get; set; }

        public string Result { get; set; }

        public bool IsCompleted { get; set; }
    }
}
