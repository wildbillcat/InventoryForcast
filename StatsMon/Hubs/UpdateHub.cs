using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using StatsMon.Models;

namespace StatsMon.Hubs
{
    public class UpdateHub : Hub
    {
        public void SentUpdateNotification(UpdateNotification Update)
        {
            Clients.All.updateNotification(Update);
        }
    }
}