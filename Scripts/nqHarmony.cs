using HarmonyLib;
using Assets.Scripts.Objects.Electrical;

namespace nqPlugin.Script
{
    [HarmonyPatch(typeof(Quarry))]
    [HarmonyPatch("OnServerExportTick")]
    public class OnServerExportTick_nqPatch
    {
        public static void Postfix(Quarry __instance)
        {
            nqQuarryServer.OnServerExportTickPostfix(__instance);
        }
    }

    [HarmonyPatch(typeof(Quarry), "SetUpQuarry")]
    sealed class SetUpQuarry_nqPatch
    {
        static void Postfix(Quarry __instance)
        {
            nqQuarry.SetUpQuarryPostfix(__instance);
        }
    }
}