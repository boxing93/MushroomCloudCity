using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
namespace MushroomCloud.Common.Exceptions
{
   public class MushroomCloudException : Exception
   {
        public string Code { get; set; }

        public MushroomCloudException()
        {
        }

        public MushroomCloudException(string code,string message)
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
        public class ExceptionDetails
        {
            public string Code { get; set; }
            public string MushroomCloudExceptionMessage { get; set; }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }

}
