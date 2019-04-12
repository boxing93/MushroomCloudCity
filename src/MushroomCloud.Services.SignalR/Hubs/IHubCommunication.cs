using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MushroomCloud.Services.SignalR.Hubs
{
    public interface IHubCommunication
    {
        Task ReceiveMessageAsync();
        Task InitiliazeAsync(string token);

        Task DisconnectAsync();
    }
}
