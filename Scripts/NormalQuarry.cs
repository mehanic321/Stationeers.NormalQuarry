using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts;
using Assets.Scripts.Networking;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Items;
using TMPro;

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
                            if (NewQuarryServerComponent.activatedRate)
                            {
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
            if (GameManager.IsServer && !__instance.gameObject.GetComponent<NewQuarryServer>())
            {
                __instance.gameObject.AddComponent<NewQuarryServer>();
            }
            if (!__instance.gameObject.GetComponent<NewQuarryAll>())
            {
                NewQuarryAll component = __instance.gameObject.AddComponent<NewQuarryAll>();
                component.startComponent();
                Debug.Log("TEST " + GameManager.IsServer);
            }
        }
    }

    public class NewQuarryServer : MonoBehaviour
    {

        public float repeatRate = 10f;
        public bool activatedRate = false;
        public int randomOre = 0;        
        public float startTimer;        
        public bool startPercent = false;        
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
            InvokeRepeating("oneSecond", 1, 1);
        }

        public void timerRepeating()
        {
            startPercent = true;
            startTimer = NetworkTime.time;
            activatedRate = true;
            randomOre = Random.Range(0, ores.Count);            
        }

        public void oneSecond()
        {
            if (startPercent)
            {
                GetComponent<NewQuarryAll>().timerPercent = 100-Mathf.RoundToInt(((startTimer + repeatRate) - NetworkTime.time) / repeatRate * 100);
            }
        }

        public void activatedRateOff()
        {
            activatedRate = false;
        }
        
    }

    public class NewQuarryAll : NetworkBehaviour
    {
        [SyncVar]
        public int timerPercent = 0;

        public GameObject textTimer;

        public void oneSecond()
        {
            if (textTimer)
            {
                setTextStr(ref textTimer, timerPercent + "%");
                Quarry quarry = GetComponent<Quarry>();
                bool flag = quarry.Error == 1 || !quarry.OnOff || quarry.Activate != 1 || !quarry.Powered;
                if (!flag)
                {
                    setVisible(ref textTimer, true);
                }
                else
                {
                    setVisible(ref textTimer, false);
                }
            }
        }

        public void startComponent() {
            InvokeRepeating("oneSecond", 1, 1);
            if (!textTimer)
            {
                addText(ref textTimer, transform.position - transform.right * .7f + new Vector3(0, 2, 0), transform.rotation, gameObject, "...", 5, Color.red, new Vector2(100, 100), false);
                textTimer.transform.localEulerAngles = new Vector3(0, textTimer.transform.localEulerAngles.y - 90, 0);
                setVisible(ref textTimer, false);
            }
        }

        public static void addText(ref GameObject uiEmpty, Vector3 position, Quaternion rotation, GameObject parent, string textStr = "", int fontSize = 35, Color color = new Color(), Vector2 size = new Vector2(), bool seeOnPlayer = false, TextAlignmentOptions alignment = TextAlignmentOptions.CenterGeoAligned)
        {
            if (color == new Color()) color = Color.black;
            if (size == new Vector2()) size = Vector2.one;
            uiEmpty = new GameObject();
            uiEmpty.transform.position = position;
            uiEmpty.transform.rotation = rotation;
            uiEmpty.transform.SetParent(parent.transform);
            TextMeshPro text = uiEmpty.AddComponent<TextMeshPro>();
            text.color = color;
            text.text = textStr;
            text.fontSize = fontSize;
            text.alignment = alignment;
            RectTransform rectTransform = uiEmpty.GetComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            if (seeOnPlayer) uiEmpty.AddComponent<seeOnPlayerController>();
        }

        public void setTextStr(ref GameObject text, string textStr) {
            text.GetComponent<TextMeshPro>().SetText(textStr);
        }

        public void setVisible(ref GameObject text, bool active)
        {
            text.GetComponent<MeshRenderer>().enabled = active;
        }
    }
    
    public class occlusionController : MonoBehaviour
    {
        private void Update()
        {
            //GetComponent<Quarry>().IsOccluded = false;
        }
    }

    public class seeOnPlayerController : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
