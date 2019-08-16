using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace LoginReggy.Models
{
    public class UserLog
    {
        [Required]
        public string Email {get; set;}

        [Required]
        public string Password {get; set;}
    }
}