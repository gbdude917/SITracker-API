using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SITracker.Models
{
    [Table("spirits")]
    [Index(nameof(Name), nameof(Pathname), IsUnique = true)]
    public class Spirit
    {
        public Spirit()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("spirit_id")]
        public long id { get; set; }

        [Required]
        [StringLength(100)]
        
        public string? Name { get; set; }

        [Required]
        [StringLength(100)]
        public string? Pathname { get; set; }

        [Required]
        [StringLength(100)]
        public string? Image { get; set; }
    }
}
