using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTester.Commands.BuiltIn
{
    public class LoadLevelCommand : GenericCommand
    {
        public override string Command => "load_level";

        
        public override void HandleParameters(string[] param)
        {
            string barcode = param[1];

            Core.loadLevelOverrideBarcode = barcode;
        }
    }
}
