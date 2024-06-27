using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SITracker.Models
{
    public class User
    {
        public User()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public long Id { get; set; }

        [Required]
        [Column(unique: true, nullable: false)]
        public string Email { get; set; }

        [Required]
        [Column(unique: true, nullable: false)]
        public string Username { get; set; }

        [Required]
        [JsonIgnore]
        public string Password { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [JsonIgnore]
        public ICollection<Authority> Authorities { get; set; }

        
    }
}
