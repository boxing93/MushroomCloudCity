using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Commands.IdentityCommands
{
    interface IAuthenticatedCommand : ICommand
    {
        Guid UserId { get; set; }
    }
}
