using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MushroomCloud.Services.Identity.Domain.Models;

namespace MushroomCloud.Services.Identity.Domain.Repositories
{
    public class UserRepository<TUser> : IUserRepository<TUser>
    where TUser : User
    {
        private readonly IMongoDatabase _database;
        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(User user)
            => await Collection
            .InsertOneAsync(user);

        public async Task<User> GetAsync(Guid id)
            => await Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<User> GetAsync(string email)
            => await Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Email == email);


        private IMongoCollection<User> Collection
            => _database.GetCollection<User>("Users");
    }

}
