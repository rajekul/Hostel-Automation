using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hostel
{
    public class EmployeeSalary
    {
        [Key]
        public int SalaryId { get; set; }
        public DateTime PayDate { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public float AmmountPaid { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

    }
}