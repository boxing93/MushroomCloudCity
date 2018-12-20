using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Events.IdentityEvents
{
    interface IAuthenticatedEvent : IEvent
    {
        Guid UserId { get; }
    }
}
