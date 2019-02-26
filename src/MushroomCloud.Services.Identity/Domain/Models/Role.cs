using System;
using AspNetCore.Identity.MongoDbCore.Models;

namespace MushroomCloud.Services.Identity
{
    public class Role : MongoIdentityRole<Guid>
    {
        public Role() : base()
        {
        }
        public Role(string roleName) : base(roleName)
        {
        }
    }
}