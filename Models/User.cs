using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltExam.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name="Name:")]
        [MinLength(2)]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        [Display(Name="Email Address:")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",ErrorMessage="Password Must Contain at least 1 Number, 1 Letter Capital, 1 Letter Lowercase, and 1 Special Character")]
        [Display(Name="Password:")]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password:")]
        public string Confirm { get; set; }

        List<Event> MyEvents {get;set;}
        List<RSVP> Attending {get;set;}
    }
}