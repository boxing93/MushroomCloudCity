using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MushroomCloud.Common.Mongo
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}
