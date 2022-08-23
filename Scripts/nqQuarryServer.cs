using UnityEngine;
using nqPlugin.Preset;
using nqPlugin.Lang;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Networking;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Util;

namespace nqPlugin.Script
{
    public class nqQuarryServer : MonoBehaviour
    {
        public static List<string> spawningOres = new List<string>();
        
        public float repeatRate = nqPreset.REPEAT_RATE;
        public float timerSpawnOre = 0;
        public float timerNewUpgrade = 0;

        public GameObject textTimer;

        public int speedFactor = 0;
        public int countUranium = 0;

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
            spawningOres.Add("ItemUraniumOre");

            if (!textTimer)
            {
                nqTMproText.addText(ref textTimer, transform.position - transform.right * nqPreset.X_OFFSET_FOR_TEXT + new Vector3(0, nqPreset.Y_OFFSET_FOR_TEXT, 0), transform.rotation, gameObject, nqLang.POINT + nqLang.POINT + nqLang.POINT, 3, Color.red, new Vector2(100, 100), false);
                textTimer.transform.localEulerAngles = new Vector3(0, textTimer.transform.localEulerAngles.y - 90, 0);
                nqTMproText.setVisible(ref textTimer, false);
            }

            InvokeRepeating("oneSecond", 1, 1);
            InvokeRepeating("timerSpawnrate", repeatRate, repeatRate);
            InvokeRepeating("timerUpgrade", nqPreset.TIMER_FOR_FACTOR_UPGRADE, nqPreset.TIMER_FOR_FACTOR_UPGRADE);
        }

        public static string GetRandomOreName()
        {
            return spawningOres[Random.Range(0, spawningOres.Count)];
        }

        public static void SpawnRandomOre(Quarry quarry, string oreHash, int count)
        {
            if (!quarry.IsStructureCompleted || quarry.Error == 1 || !quarry.OnOff || quarry.Activate != 1 || !quarry.Powered)
            {
                quarry.GetComponent<nqQuarryServer>().resetTimerSpawnOre();
                return;
            }
            if (quarry.Exporting == 0 && quarry.ExportingThing == null && quarry.ExportSlot.Occupant == null)
            {
                Ore ore = OnServer.CreateOld(oreHash, quarry.Position, UnityEngine.Random.rotation) as Ore;
                if (ore != null)
                {
                    ore.SetQuantity(count);
                    OnServer.MoveToSlot(ore, quarry.ExportSlot);
                }
                quarry.GetComponent<nqQuarryServer>().resetTimerSpawnOre();
                if (quarry.CanBeginExport)
                {
                    OnServer.Interact(quarry.InteractExport, 1, false);
                }
            }
        }

        public void resetTimerSpawnOre()
        {
            timerSpawnOre = 0;
        }

        public void resetTimerNewUpgrade()
        {
            timerNewUpgrade = 0;
        }

        public void timerUpgrade()
        {
            Quarry quarry = GetComponent<Quarry>();
            if (!quarry.IsStructureCompleted || quarry.Error == 1 || !quarry.OnOff || quarry.Activate != 1 || !quarry.Powered)
            {
                return;
            }
            switch (speedFactor)
            {
                case 0:
                    if (countUranium >= nqPreset.NEED_URANIUM_X1)
                    {
                        speedFactor++;
                        countUranium -= nqPreset.NEED_URANIUM_X1;
                        quarry.UsedPower = nqPreset.USED_POWER_X1;
                        repeatRate = nqPreset.REPEAT_RATE / nqPreset.SPEED_FACTOR_X1;
                        CancelInvoke("timerSpawnrate");
                        InvokeRepeating("timerSpawnrate", repeatRate, repeatRate);
                        resetTimerSpawnOre();
                    }
                    break;
                case 1:
                    if (countUranium >= nqPreset.NEED_URANIUM_X2)
                    {
                        speedFactor++;
                        countUranium -= nqPreset.NEED_URANIUM_X2;
                        quarry.UsedPower = nqPreset.USED_POWER_X2;
                        repeatRate = nqPreset.REPEAT_RATE / nqPreset.SPEED_FACTOR_X2;
                        CancelInvoke("timerSpawnrate");
                        InvokeRepeating("timerSpawnrate", repeatRate, repeatRate);
                        resetTimerSpawnOre();
                    }
                    else
                    {
                        speedFactor--;
                        quarry.UsedPower = nqPreset.USED_POWER_X1;
                        repeatRate = nqPreset.REPEAT_RATE / nqPreset.SPEED_FACTOR_X0;
                        CancelInvoke("timerSpawnrate");
                        InvokeRepeating("timerSpawnrate", repeatRate, repeatRate);
                        resetTimerSpawnOre();
                    }
                    break;
                case 2:
                    if (countUranium >= nqPreset.NEED_URANIUM_X3)
                    {
                        speedFactor++;
                        countUranium -= nqPreset.NEED_URANIUM_X3;
                        quarry.UsedPower = nqPreset.USED_POWER_X3;
                        repeatRate = nqPreset.REPEAT_RATE / nqPreset.SPEED_FACTOR_X3;
                        CancelInvoke("timerSpawnrate");
                        InvokeRepeating("timerSpawnrate", repeatRate, repeatRate);
                        resetTimerSpawnOre();
                    }
                    else
                    {
                        speedFactor--;
                        quarry.UsedPower = nqPreset.USED_POWER_X2;
                        repeatRate = nqPreset.REPEAT_RATE / nqPreset.SPEED_FACTOR_X1;
                        CancelInvoke("timerSpawnrate");
                        InvokeRepeating("timerSpawnrate", repeatRate, repeatRate);
                        resetTimerSpawnOre();
                    }
                    break;
                case 3:
                    if (countUranium >= nqPreset.NEED_URANIUM_X3)
                    {
                        countUranium -= nqPreset.NEED_URANIUM_X3;
                    } 
                    else 
                    {
                        speedFactor--;
                        quarry.UsedPower = nqPreset.USED_POWER_X2;
                        repeatRate = nqPreset.REPEAT_RATE / nqPreset.SPEED_FACTOR_X2;
                        CancelInvoke("timerSpawnrate");
                        InvokeRepeating("timerSpawnrate", repeatRate, repeatRate);
                        resetTimerSpawnOre();
                    }
                    break;
            }
            resetTimerNewUpgrade();
        }

        public void oneSecond()
        {
            timerSpawnOre++;
            timerNewUpgrade++;

            Quarry quarry = GetComponent<Quarry>(); 
            
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject oneObject in allObjects)
            {
                if (!oneObject.name.Contains("ItemUraniumOre")) continue;
                Ore ore = oneObject.GetComponent<Ore>();
                if (!ore) continue;
                float dist = RocketMath.DistanceSquared(quarry.CenterPosition, ore.CenterPosition);
                if (ore.ParentSlot != null || dist > nqPreset.DISTANCE_FOR_URANIUM) continue;
                OnServer.Destroy(ore);
                countUranium += ore.Quantity;
            }

            int timerPercent = Mathf.RoundToInt((timerSpawnOre / repeatRate) * 100);

            string textStr = (timerPercent == 0) ? 
                nqLang.WAIT + "\n" + repeatRate + nqLang.ABBREVIATION_SEC : 
                timerPercent.ToString() + nqLang.PROCENT + 
                "\n" + nqLang.URANIUM + countUranium + 
                "\n" + nqLang.SPEED + "X" + speedFactor +
                "\n" + nqLang.UPGRADE + (nqPreset.TIMER_FOR_FACTOR_UPGRADE-timerNewUpgrade) + "s "
            ;

            int colorPercent = Mathf.RoundToInt(255 * timerPercent / 100);

            nqTMproText.setText(ref textTimer, textStr);
            nqTMproText.setColor(ref textTimer, new Color32((byte)(255 - colorPercent), (byte)colorPercent, 0, 255));
            
            bool flag = !quarry.IsStructureCompleted || quarry.Error == 1 || !quarry.OnOff || quarry.Activate != 1 || !quarry.Powered;
            if (!flag)
            {
                nqTMpro.setVisible(ref textTimer, true);
            }
            else
            {
                nqTMpro.setVisible(ref textTimer, false);
            }
        }

        public void timerSpawnrate()
        {
            SpawnRandomOre(GetComponent<Quarry>(), GetRandomOreName(), Random.Range(nqPreset.MIN_COUNT_SPAWN_ORE, nqPreset.MAX_COUNT_SPAWN_ORE));
        }

    }

}