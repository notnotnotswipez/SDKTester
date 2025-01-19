using LabFusion.Network;
using SDKTester.ActionQueuer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTester.Commands.BuiltIn
{
    public class FusionStartServerCommand : GenericCommand
    {
        public override string Command => "start_fusion_server";

        public override void HandleParameters(string[] param)
        {
            ActionQueuer.ActionQueuer.QueueAction(() => {
                NetworkLayerManager.LogIn(NetworkLayerManager.GetTargetLayer());
                NetworkHelper.StartServer();
            });

            Core.fusionServerQueued = true;
        }
    }
}
