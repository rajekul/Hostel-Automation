using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hostel
{
    public class MemberPayment
    {
        [Key]
        public int PayId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public float Debit { get; set; }
        [Required]
        public float Credit { get; set; }
        [Required]
        public float Balance { get; set; }
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }
        public int BuildingId { get; set; }
    }
}