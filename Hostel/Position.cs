using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hostel
{
    public class Position
    {
        [Key]
        public int PositionId { get; set; }
        [Required]
        public string PositionName { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numeric Field")]
        public int Salary { get; set; }
        List<Employee> Employees { get; set; }
    }
}