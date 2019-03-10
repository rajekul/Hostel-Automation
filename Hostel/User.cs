using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hostel
{
    public class User
    {
        [Key][Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string status { get; set; }
    }
}