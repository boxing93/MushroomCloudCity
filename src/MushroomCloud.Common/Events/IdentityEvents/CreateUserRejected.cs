using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Events.IdentityEvents
{
    public class CreateUserRejected : IRejectedEvent
    {
        public string Email { get; }
        public string Reason { get; }

        public string ErrorCode { get; }

        protected CreateUserRejected()
        {

        }

        public CreateUserRejected(string email, string reason, string errorCode)
        {
            Email = email;
            Reason = reason;
            ErrorCode = errorCode;
        }
    }
}
