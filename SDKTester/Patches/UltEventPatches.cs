using HarmonyLib;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow.Utilities;
using Il2CppUltEvents;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTester.Patches
{
    [HarmonyPatch(typeof(UltEventHolder), nameof(UltEventHolder.Invoke))]
    public class UltEventHolderInvokePatch
    {   
        public static void Prefix(UltEventHolder __instance) {
            if (Core.logUltEventCalls) {
                MelonLogger.Msg(ObjectPathExtensions.ObjectPath(__instance) + ".Invoke()");
            }
        }
    }
}
