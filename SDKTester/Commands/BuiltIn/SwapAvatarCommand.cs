using BoneLib;
using SDKTester.ActionQueuer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTester.Commands.BuiltIn
{
    public class SwapAvatarCommand : GenericCommand
    {
        public override string Command => "swap_avatar";

        public override void HandleParameters(string[] param)
        {
            string barcode = param[1];

            ActionQueuer.ActionQueuer.SecondaryQueueAction(() =>
            {
                Player.RigManager.SwapAvatarCrate(new Il2CppSLZ.Marrow.Warehouse.Barcode(barcode));
            });
        }
    }
}
