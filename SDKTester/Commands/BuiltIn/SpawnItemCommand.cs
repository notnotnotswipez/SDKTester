using BoneLib;
using SDKTester.ActionQueuer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDKTester.Commands.BuiltIn
{
    public class SpawnItemCommand : GenericCommand
    {
        public override string Command => "spawn_object";

        public override void HandleParameters(string[] param)
        {
            string barcode = param[1];

            bool useFrontOfPlayer = false;

            float x = 0;
            float y = 0;
            float z = 0;

            if (param[2] == "front_of_player")
            {
                useFrontOfPlayer = true;
            }
            else {
                x = float.Parse(param[2]);
                y = float.Parse(param[3]);
                z = float.Parse(param[4]);
            }
            

            ActionQueuer.ActionQueuer.SecondaryQueueAction(() =>
            {
                Vector3 target = new Vector3(x, y, z);

                if (useFrontOfPlayer) {
                    target = Player.RigManager.physicsRig.m_head.transform.position + Player.RigManager.physicsRig.m_head.transform.forward;
                }

                HelperMethods.SpawnCrate(barcode, target, UnityEngine.Quaternion.identity, UnityEngine.Vector3.one, false);
            });
        }
    }
}
