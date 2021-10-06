using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Networking;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Items;


namespace NormalQuarry.Scripts
{
    [HarmonyPatch(typeof(Quarry))]
    [HarmonyPatch("OnServerExportTick")]
    public class OnServerExportTick_NormalQuarryPatch
    {
        public static void Postfix(Quarry __instance)
        {
            bool flag = __instance.Error == 1 || !__instance.OnOff || __instance.Activate != 1 || !__instance.Powered;
            if (!flag)
            {
                bool flag2 = __instance.IsNextExportReady;
                if (flag2)
                {
                    if (GameManager.IsServer)
                    {
                        if (__instance.TryGetComponent(out NewQuarryServer NewQuarryServerComponent))
                        {
                            if (NewQuarryServerComponent.activatedRate) {
                                string randOre = NewQuarryServer.ores[NewQuarryServerComponent.randomOre];
                                Ore ore = OnServer.Create(randOre, __instance.DrillHead.position, UnityEngine.Random.rotation) as Ore;
                                bool flag3 = ore != null;
                                if (flag3)
                                {
                                    ore.NetworkQuantity = Mathf.CeilToInt(Mathf.Clamp(Random.Range(0, 75), 1f, (float)ore.MaxQuantity));
                                    OnServer.MoveToSlot(ore, __instance.ExportSlot);
                                }
                                NewQuarryServerComponent.activatedRateOff();                                
                            }
                        }                        
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Quarry), "SetUpQuarry")]
    sealed class SetUpQuarry_NormalQuarryPatch
    {
        static void Postfix(Quarry __instance)
        {
            if (GameManager.IsServer)
            {
                __instance.gameObject.AddComponent<NewQuarryServer>();
            }
        }
    }

    public class NewQuarryServer:MonoBehaviour {

        public float repeatRate = 120f;
        public bool activatedRate = false;
        public int randomOre = 0;
        public static List<string> ores = new List<string>();

        void Start()
        {
            ores.Add("ItemCoalOre");
            ores.Add("ItemGoldOre");
            ores.Add("ItemIronOre");
            ores.Add("ItemLeadOre");
            ores.Add("ItemNickelOre");
            ores.Add("ItemSiliconOre");
            ores.Add("ItemSilverOre");
            //ores.Add("ItemUraniumOre");
            ores.Add("ItemCobaltOre");
            ores.Add("ItemCopperOre");

            InvokeRepeating("timerRepeating", repeatRate, repeatRate);
        }

        public void timerRepeating() {
            activatedRate = true;
            randomOre = Random.Range(0, ores.Count);
        }

        public void activatedRateOff()
        {
            activatedRate = false;
        }
    }

}
