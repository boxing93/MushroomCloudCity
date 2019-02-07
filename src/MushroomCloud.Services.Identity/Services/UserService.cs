using MushroomCloud.Common.Exceptions;
using MushroomCloud.Services.Activities.Domain.Services;
using MushroomCloud.Services.Identity.Domain.Models;
using MushroomCloud.Services.Identity.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MushroomCloud.Services.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;

        public UserService(IUserRepository repository, IEncrypter encrypter)
        {
            _userRepository = repository;
            _encrypter = encrypter;
        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new MushroomCloudException("invalid_credentials",
                    $"Invalid credentials.");
            }
            if (!user.ValidatePassword(password, _encrypter))
            {
                throw new MushroomCloudException("invalid_credentials",
                    $"Invalid credentials.");
            }
            //TO DO: 
            //Json web token handler who create token by user id. 
        }

        public async Task RegisterAsync(string email, string password, string name)
        {
            var user = await _userRepository.GetAsync(email);
            if (user != null)
            {
                throw new MushroomCloudException("email_in_use",
                    $"Email: '{email}' is already in use.");
            }
            user = new User(email, name);
            user.SetPassword(password,_encrypter);
            await _userRepository.AddAsync(user);
        }
    }
}
