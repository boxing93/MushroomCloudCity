using MushroomCloud.Common.Events.IdentityEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Events.ActivityEvents
{
    public class RejectCreateActivity : IRejectedEvent
    {
        public Guid UserId { get; }
        public Guid ActivityId { get; }
        public string Reason { get; }
        public string ErrorCode { get; }
        protected RejectCreateActivity()
        { }

        public RejectCreateActivity(Guid userId, Guid activityId, string reason, string errorCode)
        {
            UserId = userId;
            ActivityId = activityId;
            Reason = reason;
            ErrorCode = errorCode;
        }
    }
}
