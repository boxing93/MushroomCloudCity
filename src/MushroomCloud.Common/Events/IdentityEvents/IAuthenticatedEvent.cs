using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Events.IdentityEvents
{
    public interface IAuthenticatedEvent : IEvent
    {
        Guid UserId { get; }
    }
}
