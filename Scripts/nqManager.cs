using UnityEngine;
using Assets.Scripts.Networking;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Items;

namespace nqPlugin.Script
{
    public class nqManager : MonoBehaviour
    {

        public static void createItemOreByQuarryRandomQuantity(ref Quarry quarry, string oreHash, int count)
        {
            Ore ore = OnServer.Create(oreHash, quarry.DrillHead.position, Random.rotation) as Ore;
            bool flag3 = ore != null;
            if (flag3)
            {
                ore.NetworkQuantity = Mathf.CeilToInt(Mathf.Clamp(count, 1f, (float)ore.MaxQuantity));
                OnServer.MoveToSlot(ore, quarry.ExportSlot);
            }
        }

    }
}