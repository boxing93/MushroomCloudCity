using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MushroomCloud.Common.Mongo
{
    public class MongoInitializer : IDatabaseInitializer
    {
        private bool _initialized;
        private readonly bool _seed;
        private readonly IMongoDatabase _database;
        private readonly IDatabaseSeeder _seeder;

        public MongoInitializer(IMongoDatabase database, IOptions<MongoOptions> options,IDatabaseSeeder seeder)
        {
            _seeder = seeder;
            _database = database;
            _seed = options.Value.Seed;
        }

        public MongoInitializer()
        {
        }

        public async Task InitializeAsync()
        {
            if (_initialized)
            {
                return;
            }
            RegisterConventions();
            _initialized = true;
            if (!_seed)
            {
                return;
            }

            await _seeder.SeedAsync();
        }

        private void RegisterConventions()
        {
            ConventionRegistry.Register("MushroomCloudConvention", new MongoConvention(), x => true);
        }

        private class MongoConvention : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(MongoDB.Bson.BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}
