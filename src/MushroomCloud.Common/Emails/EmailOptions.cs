using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Emails
{
    public class EmailOptions
    {
        public string Host { get; protected set; }
        public int Port { get; protected set; }
        public string Username { get; protected set; }
        public string Password { get; protected set; }
    }
}
