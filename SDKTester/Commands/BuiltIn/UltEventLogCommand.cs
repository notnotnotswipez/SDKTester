using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTester.Commands.BuiltIn
{
    public class UltEventLogCommand : GenericCommand
    {
        public override string Command => "log_ult_events";

        
        public override void HandleParameters(string[] param)
        {
            bool bools = bool.Parse(param[1]);

            Core.logUltEventCalls = bools;
        }
    }
}
