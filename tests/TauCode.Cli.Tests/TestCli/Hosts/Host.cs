﻿using System.Collections.Generic;
using TauCode.Cli.Tests.TestCli.AddIns;

namespace TauCode.Cli.Tests.TestCli.Hosts
{
    public class Host : CliHostBase
    {
        public Host()
            : base("host", "host-1.0", true)
        {   
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new DbAddIn(),
            };
        }
    }
}