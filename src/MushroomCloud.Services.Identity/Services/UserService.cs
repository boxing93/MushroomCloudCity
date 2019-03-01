using Microsoft.AspNetCore.Identity;
using MushroomCloud.Common.Auth;
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
        private readonly IUserRepository<User> _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IEncrypter _encrypter;
        private readonly UserManager<User> _userManager;


        public UserService(IUserRepository<User> repository, IEncrypter encrypter, IJwtHandler jwtHandler, UserManager<User> userManager)
        {
            _jwtHandler = jwtHandler;
            _userRepository = repository;
            _encrypter = encrypter;
            _userManager = userManager;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                throw new MushroomCloudException("invalid_credentials", $"userId and code cannot be null.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new MushroomCloudException("invalid_credentials",$"user is null.");
            }
            return await _userManager.ConfirmEmailAsync(user, code);
        }

        public async Task<JsonWebToken> LoginAsync(string email, string password)
        {
            
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new MushroomCloudException("invalid_credentials",
                    $"Invalid credentials.");
            }
            if (await _userManager.CheckPasswordAsync(user, user.Password))/*(!user.ValidatePassword(password, _encrypter))*/
            {
                throw new MushroomCloudException("invalid_credentials",
                    $"Invalid credentials.");
            }
            return _jwtHandler.Create(user.Id);
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
            user.SetPassword(password, _encrypter);
            await _userManager.CreateAsync(user, password);
        }

        
    }
}
