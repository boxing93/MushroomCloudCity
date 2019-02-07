using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MushroomCloud.Services.Identity.Services
{
    public interface IUserService
    {
        Task RegisterAsync(string email, string password, string name);
        Task LoginAsync(string email, string password);
    }
}
