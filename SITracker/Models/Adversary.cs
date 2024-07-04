using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SITracker.Models
{
    [Table("adversaries")]
    [Index(nameof(Name), nameof(Pathname), IsUnique = true)]
    public class Adversary
    {
        public Adversary()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("adversary_id")]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("name")]
        public string? Name { get; set; }

        [Required]
        [StringLength(100)]
        [Column("pathname")]
        public string? Pathname { get; set; }

        [Required]
        [StringLength(100)]
        [Column("flag")]
        public string? Flag { get; set; }

    }
}
