using MongoDB.Driver;
using MushroomCloud.Common.Mongo;
using MushroomCloud.Services.Activities.Domain.Models;
using MushroomCloud.Services.Activities.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MushroomCloud.Services.Activities.Services
{
    public class CustomSeeder : MongoSeeder
    {
        private readonly ICategoryRepository _categoryRepository;
        public CustomSeeder(IMongoDatabase database, ICategoryRepository categoryRepository) : base(database)
        {
            _categoryRepository = categoryRepository;

        }
        protected override async Task CustomSeedAsync()
        {
            var categories = new List<string>
            {
                "work",
                "sport",
                "hobby"
            };
            await Task.WhenAll(categories.Select(x => _categoryRepository.AddAsync(new Category(x))));
        }

    }
}
