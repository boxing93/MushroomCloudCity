﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MushroomCloud.Common.Commands.IdentityCommands
{
    public class CreateUser : BaseCommand,ICommand
    {
        public string Name { get; set; }
    }
}
