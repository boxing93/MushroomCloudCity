using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MushroomCloud.Api.Models;
using MushroomCloud.Services.Activities.Domain.Models;

namespace MushroomCloud.Api.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly IMongoDatabase _database;

        public ActivityRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Models.Activity>> BrowseAsync(Guid userId)
            => await Collection
                .AsQueryable()
                .Where(x => x.UserId == userId)
                .ToListAsync();


        public async Task AddAsync(Models.Activity model) => await Collection.InsertOneAsync(model);


        public async Task<Models.Activity> GetAsync(Models.Activity model)
        => await Collection
                    .AsQueryable()
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

        private IMongoCollection<Models.Activity> Collection 
            => _database.GetCollection<Models.Activity>("Activities");
    }
}