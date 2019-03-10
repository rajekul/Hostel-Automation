using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hostel
{
    public class MyHub : Hub
    {
        public void AddMember()
        {

            //HostelDBContext context = new HostelDBContext();
            //string total = context.Members.Count().ToString();
            Clients.All.TotalMember("a");
        }

    }
}