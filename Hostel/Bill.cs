using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hostel
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }
        [Required]
        public DateTime BilledDate { get; set; }
        [Required]
        public string BillDetails { get; set; }
        [Required]
        public float Ammount { get; set; }
        public string SubmitBy { get; set; }
        public int BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        public Building Building { get; set; }
    }
}