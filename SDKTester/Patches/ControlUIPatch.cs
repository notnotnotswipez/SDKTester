using HarmonyLib;
using Il2CppSLZ.Bonelab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTester.Patches
{
    [HarmonyPatch(typeof(Control_UI_InGameData), nameof(Control_UI_InGameData.Initialize_Player))]
    public class ControlUIPatch
    {
        public static void Postfix() {
            Core.StartActionTimer(0f);
        }
    }
}
