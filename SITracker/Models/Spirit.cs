using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SITracker.Models
{
    [Table("spirits")]
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
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Pathname { get; set; }

        [Required]
        [StringLength(100)]
        public string Image { get; set; }
    }
}
