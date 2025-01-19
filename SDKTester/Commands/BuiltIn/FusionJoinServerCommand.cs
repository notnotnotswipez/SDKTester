using LabFusion.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTester.Commands.BuiltIn
{
    public class FusionJoinServerCommand : GenericCommand
    {
        public override string Command => "join_fusion_server";

        public override void HandleParameters(string[] param)
        {
            ActionQueuer.ActionQueuer.QueueAction(() => {
                NetworkLayerManager.LogIn(NetworkLayerManager.GetTargetLayer());

                if (NetworkInfo.CurrentNetworkLayer is SteamNetworkLayer steamLayer)
                {
                    steamLayer.JoinServer(ulong.Parse(param[1]));
                }
            });

            Core.timesToLoadUntilSecondaryActionIsCalled = 1;
        }
    }
}
