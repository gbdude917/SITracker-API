using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;



namespace SITracker.Models
{
    [Index(nameof(Email), nameof(Username), IsUnique = true)]
    public class User
    {
        public User()
        {
            Email = "";
            Username = "";
            Password = "";
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public long Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [JsonIgnore]
        public string Password { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        // [JsonIgnore]
        // public ICollection<Authority> Authorities { get; set; }

        
    }
}
