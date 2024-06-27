using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SITracker.Models
{
    [Table("adversaries")]
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
        [indexer(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Pathname { get; set; }

        [Required]
        [StringLength(100)]
        public string Flag { get; set; }

    }
}
