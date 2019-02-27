using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using MushroomCloud.Common.Exceptions;
using MushroomCloud.Services.Activities.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MushroomCloud.Services.Identity.Domain.Models
{
    [CollectionName("Users")]
    public class User : MongoIdentityUser<Guid>
    {
        // public Guid Id { get; protected set; }
        // public string Email { get; protected set; }
        // public string UserName { get; protected set; }
        public string Password { get;  set; }
        public string Salt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        
        public User() : base()
        {
        }

        public User(string email,string name) : base(email,name)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new MushroomCloudException("empty_user_email", "User email can not be empty.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new MushroomCloudException("empty_user_name", "User name can not be empty.");
            }

            Id = Guid.NewGuid();
            Email = email;
            UserName = name;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string password, IEncrypter encrypter)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new MushroomCloudException("empty_user_password", "User password can not be empty.");
            }
            Salt = encrypter.GetSalt();
            Password = encrypter.GetHash(password, Salt);
        }

        public bool ValidatePassword(string password, IEncrypter encrypter)
            => Password.Equals(encrypter.GetHash(password,Salt));

    }
}
