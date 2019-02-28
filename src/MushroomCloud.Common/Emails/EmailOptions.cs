using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Emails
{
    public class EmailOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
