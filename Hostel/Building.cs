using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hostel
{
    public class Building
    {
        public int BuildingId { get; set; }
        [Required]
        public string BuildingName { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Purchase> Purchases { get; set; }
        public List<Bill> Bills { get; set; }
    }
}