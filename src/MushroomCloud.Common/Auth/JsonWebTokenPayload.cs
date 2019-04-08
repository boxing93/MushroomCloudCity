using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Auth
{
    public class JsonWebTokenPayload
    {
        public string Subject { get; set; }
        public string Role { get; set; }
        public DateTime Expires { get; set; }
        public IDictionary<string, string> Claims { get; set; }
    }
}
