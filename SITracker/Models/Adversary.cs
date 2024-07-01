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
            Name = "";
            Pathname = "";
            Flag = "";
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("adversary_id")]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Pathname { get; set; }

        [Required]
        [StringLength(100)]
        public string Flag { get; set; }

    }
}
