using Microsoft.AspNetCore.SignalR;
using MushroomCloud.Common.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MushroomCloud.Services.SignalR.Hubs
{
    public class MushroomCloudHub : Hub
    {
        private readonly IJwtHandler _jwtHandler;

        public MushroomCloudHub(IJwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }
        public async Task InitiliazeAsync(string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                await DisconnectAsync();
            }
            try
            {
                var payload = _jwtHandler.GetTokenPayload(token);
                if (payload == null)
                {
                    await DisconnectAsync();

                    return;
                }
                await ConnectAsync();
            }
            catch
            {
                await DisconnectAsync();
            }
        }
        private async Task ConnectAsync()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("Connected!");
        }
        private async Task DisconnectAsync()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("Disconnected!");
        }
    }
}
