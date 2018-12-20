using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Events.IdentityEvents
{
    interface IRejectedEvent : IEvent
    {
        string Reason { get; }

        string ErrorCode { get; }
    }
}
