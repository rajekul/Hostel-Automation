using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hostel
{
    public class RoomCategory
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string RoomType { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numeric Field")]
        public int Rent { get; set; }
        [DataType(DataType.MultilineText)]
        public string Facility { get; set; }
        public List<Room> Rooms { get; set; }
    }
}