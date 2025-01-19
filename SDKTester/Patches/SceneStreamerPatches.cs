using HarmonyLib;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow.SceneStreaming;
using Il2CppSLZ.Marrow.Warehouse;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDKTester.Patches
{

    [HarmonyPatch(typeof(SceneBootstrapper_Bonelab), nameof(SceneBootstrapper_Bonelab.UpdateLog))]
    public class SceneStreamerPatchesCrateLoad
    {
        public static void Prefix(SceneBootstrapper_Bonelab __instance, string msg)
        {
            if (Core.loadLevelOverrideBarcode != "default") {
                __instance.VoidG114CrateRef = new LevelCrateReference(new Barcode(Core.loadLevelOverrideBarcode));
            }
        }
    }

    // Cancel fusions initial reload of the scene when a server starts.
    [HarmonyPatch(typeof(SceneStreamer), nameof(SceneStreamer.Reload))]
    public class SceneStreamerPatchesReload
    {
        public static bool Prefix()
        {
            if (Core.reloadNeedsPrevention)
            {
                Core.reloadNeedsPrevention = false;
                return false;
            }

            return true;
        }
    }
}
