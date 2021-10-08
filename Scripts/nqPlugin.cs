using Stationeers.Addons;
using UnityEngine;

namespace nqPlugin.Script
{
    public class nqPlugin : IPlugin
    {
        public void OnLoad()
        {
            Debug.Log("nq: Hello, World!");
            var gameObject = new GameObject("nq");
            Object.DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<nqManager>();
        }

        public void OnUnload()
        {

        }
    }
}