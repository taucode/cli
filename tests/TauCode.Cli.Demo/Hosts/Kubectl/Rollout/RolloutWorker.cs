﻿using System;
using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Demo.Hosts.Kubectl.Rollout
{
    public class RolloutWorker : CliWorkerBase
    {
        public RolloutWorker()
            : base(
                typeof(RolloutWorker).Assembly.GetResourceText(".Kubectl.Rollout.NoName.lisp", true),
                null,
                false)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}