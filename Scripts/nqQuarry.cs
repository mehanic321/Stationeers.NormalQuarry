using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Assets.Scripts;
using Assets.Scripts.Objects.Electrical;
using nqPlugin.Preset;
using nqPlugin.Lang;

namespace nqPlugin.Script
{
    public class nqQuarry : NetworkBehaviour
    {
        [SyncVar]
        public int timerPercent = 0;

        public GameObject parentPanel;
        public GameObject textTimer;
        public GameObject button;

        private Quarry instance;

        public static void SetUpQuarryPostfix(Quarry __instance)
        {
            __instance.UsedPower = 10000f;
            if (GameManager.IsServer)
                __instance.gameObject.AddComponent<nqQuarryServer>();

            if (!__instance.gameObject.GetComponent<nqQuarry>())
                __instance.gameObject.AddComponent<nqQuarry>();
        }

        void Awake()
        {
            instance = GetComponent<Quarry>();
        }

        void Start()
        {
            if (!textTimer)
            {
                Vector3 posInfoPanel = transform.position - transform.right * nqPreset.X_OFFSET_FOR_TEXT + new Vector3(0, nqPreset.Y_OFFSET_FOR_TEXT, 0);
                parentPanel = new GameObject();
                parentPanel.transform.position = posInfoPanel;
                parentPanel.transform.rotation = transform.rotation;
                parentPanel.transform.SetParent(gameObject.transform);
                parentPanel.AddComponent<MeshRenderer>();
                parentPanel.AddComponent<Canvas>();

                nqTMproText.addText(ref textTimer, Vector3.zero, Quaternion.identity, parentPanel, nqLang.POINT + nqLang.POINT + nqLang.POINT, 5, Color.red, new Vector2(100, 100), false);
                textTimer.transform.localEulerAngles = new Vector3(0, textTimer.transform.localEulerAngles.y - 90, 0);
                nqTMproText.setVisible(ref textTimer, false);

                GameObject uiEmpty = new GameObject();
                uiEmpty.transform.SetParent(parentPanel.transform);
                uiEmpty.AddComponent<Button>();
                //nqTMproButton.addButton(ref button, posInfoPanel, transform.rotation, textTimer.transform.parent.gameObject);
            }
            InvokeRepeating("oneSecond", 1, 1);
        }

        void oneSecond()
        {
            if (textTimer)
            {
                string textStr = (timerPercent == 0) ? nqLang.WAIT + "\n" + Mathf.RoundToInt(nqPreset.REPEAT_RATE / 60) + nqLang.ABBREVIATION_MIN : timerPercent.ToString() + nqLang.PROCENT;
                int colorPercent = Mathf.RoundToInt(255 * timerPercent / 100);
                nqTMproText.setText(ref textTimer, textStr);
                nqTMproText.setColor(ref textTimer, new Color32((byte)(255 - colorPercent), (byte)colorPercent, 0, 255));
                bool flag = instance.Error == 1 || !instance.OnOff || instance.Activate != 1 || !instance.Powered;
                if (!flag)
                {
                    nqTMpro.setVisible(ref textTimer, true);
                }
                else
                {
                    nqTMpro.setVisible(ref textTimer, false);
                }
            }
        }

    }
}