using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Exceptions
{
   public class MushroomCloudException : Exception
   {
        public string Code { get; }

        public MushroomCloudException()
        {
        }

        public MushroomCloudException(string code)
        {
            Code = code;
        }

        public MushroomCloudException(string message, params object[] args) : this(string.Empty, message, args)
        {
        }

        public MushroomCloudException(string code, string message, params object[] args) : this(null, code, message, args)
        {
        }

        public MushroomCloudException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public MushroomCloudException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
