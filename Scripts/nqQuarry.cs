using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Assets.Scripts;
using Assets.Scripts.Objects.Electrical;
using nqPlugin.Preset;
using nqPlugin.Lang;

namespace nqPlugin.Script
{
    public class nqQuarry:MonoBehaviour { 
        public static void SetUpQuarryPostfix(Quarry __instance)
        {
            if (GameManager.RunSimulation && !__instance.gameObject.GetComponent<nqQuarryServer>())
            {
                __instance.gameObject.AddComponent<nqQuarryServer>();
            }
        }
    }
}