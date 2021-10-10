using UnityEngine;
using nqPlugin.Preset;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Networking;
using Assets.Scripts.Objects.Electrical;

namespace nqPlugin.Script
{
    public class nqQuarryServer : MonoBehaviour
    {
        public int randomOre = 0;

        public bool startPercent = false;
        public bool isSpawningOre = false;

        public float startTimer;
        public float repeatRate = nqPreset.REPEAT_RATE;

        public static List<string> spawningOres = new List<string>();

        public static void OnRegisteredPostfix(Quarry __instance)
        {
            __instance.UsedPower = nqPreset.USED_POWER;
        }

        public static void OnServerExportTickPostfix(Quarry __instance)
        {
            bool flag = __instance.Error == 1 || !__instance.OnOff || __instance.Activate != 1 || !__instance.Powered;
            if (!flag)
            {
                bool flag2 = __instance.IsNextExportReady;
                if (flag2)
                {
                    if (GameManager.IsServer)
                    {
                        if (__instance.TryGetComponent(out nqQuarryServer nqQuarryServerComponent))
                        {
                            if (nqQuarryServerComponent.isSpawningOre)
                            {
                                string randOre = nqQuarryServer.spawningOres[nqQuarryServerComponent.randomOre];
                                nqManager.createItemOreByQuarryRandomQuantity(ref __instance, randOre, Random.Range(nqPreset.MIN_COUNT_SPAWN_ORE, nqPreset.MAX_COUNT_SPAWN_ORE));
                                nqQuarryServerComponent.offSpawnOre();
                            }
                        }
                    }
                }
            }
        }

        void Start()
        {
            spawningOres.Add("ItemCoalOre");
            spawningOres.Add("ItemGoldOre");
            spawningOres.Add("ItemIronOre");
            spawningOres.Add("ItemLeadOre");
            spawningOres.Add("ItemNickelOre");
            spawningOres.Add("ItemSiliconOre");
            spawningOres.Add("ItemSilverOre");
            spawningOres.Add("ItemCobaltOre");
            spawningOres.Add("ItemCopperOre");

            InvokeRepeating("timerRepeating", repeatRate, repeatRate);
            InvokeRepeating("oneSecond", 1, 1);
        }

        public void oneSecond()
        {
            if (startPercent)
            {
                GetComponent<nqQuarry>().timerPercent = 100 - Mathf.RoundToInt(((startTimer + repeatRate) - NetworkTime.time) / repeatRate * 100);
            }
        }

        public void timerRepeating()
        {
            startPercent = true;
            startTimer = NetworkTime.time;
            randomOre = Random.Range(0, spawningOres.Count);
            isSpawningOre = true;
        }

        public void offSpawnOre()
        {
            isSpawningOre = false;
        }

    }

}