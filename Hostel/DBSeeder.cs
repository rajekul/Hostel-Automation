using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Hostel
{
    public class DBSeeder : DropCreateDatabaseIfModelChanges<HostelDBContext>
    {
        protected override void Seed(HostelDBContext context)
        {

            User user = new User()
            {
                Username = "Admin",
                Password = "admin",
                Type = "Admin",
                status = "Active",
            };
            Hostel hostel = new Hostel()
            {
                HostelName = "HostelBD",
                HostelTitle = "HostelBD",
                Email = "hostel@abc.com",
                Phone = "01762506794",
                City = "Dhaka",
                Area = "Nikuja-2",
                Road = "10",
                House = "24",
                HostelLogo = "hostellogo.png",
            };
            context.Users.Add(user);
            context.Hostels.Add(hostel);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}