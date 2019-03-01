using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Events.IdentityEvents
{
    public class UserAuthenticatedRejected : IEvent
    {
        public string Email { get; }

        protected UserAuthenticatedRejected() { }

        public UserAuthenticatedRejected(string email,string errorCode,string message)
        {
            Email = email;
        }

        public UserAuthenticatedRejected(string email, string message)
        {
            Email = email;
        }
    }
}
