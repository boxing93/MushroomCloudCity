using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Events.IdentityEvents
{
    public class UserAuthenticated : IEvent
    {
        public string Email { get; }

        protected UserAuthenticated() { }

        public UserAuthenticated(string email)
        {
            Email = email;
        }
    }
}
