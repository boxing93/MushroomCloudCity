using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MushroomCloud.Common.Services
{
   public interface IServiceHost
    {
        Task RunAsync();

        void Run();
    }
}
