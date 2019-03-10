using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hostel
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        [Required]
        public string RoomNo { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numeric Field")]
        public int TotalSeat { get; set; }
        public int BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        public Building Building { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public RoomCategory RoomCategory { get; set; }
        public List<Member> Members { get; set; }
    }
}