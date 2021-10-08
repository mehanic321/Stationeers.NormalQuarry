using UnityEngine;
using TMPro;

namespace nqPlugin.Script
{
    public class nqTMproText : nqTMpro
    {
        
        public static void setText(ref GameObject textObject, string text)
        {
            textObject.GetComponent<TextMeshPro>().SetText(text);            
        }

    }
}
