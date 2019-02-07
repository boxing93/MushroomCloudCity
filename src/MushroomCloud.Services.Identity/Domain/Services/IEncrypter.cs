using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MushroomCloud.Services.Activities.Domain.Services
{
    public interface IEncrypter
    {
        string GetSalt();
        string GetHash(string value, string salt);
    }
}
