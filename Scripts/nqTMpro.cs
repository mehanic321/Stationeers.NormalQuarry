using UnityEngine;
using TMPro;

namespace nqPlugin.Script
{
    public class nqTMpro : TextMeshPro
    {
        public bool isLookAtTheCamera = false;

        void Update()
        {
            lookAtTheCamera();
        }

        void lookAtTheCamera()
        {
            if (!isLookAtTheCamera) return;
            transform.rotation = Camera.main.transform.rotation;
        }

        public static void setVisible(ref GameObject obj, bool active)
        {
            obj.GetComponent<MeshRenderer>().enabled = active;
        }

        public static void addText(ref GameObject uiEmpty, Vector3 position, Quaternion rotation, GameObject parent, string textStr = "", int fontSize = 35, Color color = new Color(), Vector2 size = new Vector2(), bool isLookAtTheCamera = false, TextAlignmentOptions alignment = TextAlignmentOptions.CenterGeoAligned)
        {
            if (color == new Color()) color = Color.black;
            if (size == new Vector2()) size = Vector2.one;
            uiEmpty = new GameObject();
            uiEmpty.transform.position = position;
            uiEmpty.transform.rotation = rotation;
            uiEmpty.transform.SetParent(parent.transform);
            nqTMproText text = uiEmpty.AddComponent<nqTMproText>();
            text.color = color;
            text.SetText(textStr);
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.isLookAtTheCamera = isLookAtTheCamera;
            RectTransform rectTransform = uiEmpty.GetComponent<RectTransform>();
            rectTransform.sizeDelta = size;
        }

        public static void setColor(ref GameObject textObject, Color color)
        {
            textObject.GetComponent<TextMeshPro>().color = color;
        }

    }
}
