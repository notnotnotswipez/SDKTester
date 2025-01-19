using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTester.Commands
{
    public abstract class GenericCommand
    {
        public abstract string Command { get; }

        public abstract void HandleParameters(string[] param);

    }
}
