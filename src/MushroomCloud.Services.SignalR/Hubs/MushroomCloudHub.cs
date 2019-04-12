using Microsoft.AspNetCore.SignalR;
using MushroomCloud.Common.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MushroomCloud.Services.SignalR.Hubs
{
    public class MushroomCloudHub : Hub<IHubCommunication>
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
                await SendMessageAsync();
            }
            catch
            {
                await DisconnectAsync();
            }
        }
        private async Task SendMessageAsync()
        {
            await Clients.All.ReceiveMessageAsync();
        }
        private async Task DisconnectAsync()
        {
            await Clients.All.DisconnectAsync();
        }
    }
}
