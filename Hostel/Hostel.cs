using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hostel
{
    public class Hostel
    {
        public int HostelId { get; set; }
        [Required]
        public string HostelName { get; set; }
        [Required]
        public string HostelTitle { get; set; }
        [Required][RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",ErrorMessage ="Invalid Email")]
        public string Email { get; set; }
        [Required][MaxLength(12)][RegularExpression("^[0-9]*$",ErrorMessage ="Invalid Number")]
        public string Phone { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public string Road { get; set; }
        [Required]
        public string House { get; set; }
        public string HostelLogo { get; set; }
    }
}