using TMPro;
using UnityEngine;



namespace Lotus.CoreFramework
{
    public class UI_Resource : MonoBehaviour
    {
        public ResourceType type;
        public TMP_Text valueTxt = null;


        private void Awake()
        {
            OnResourceChanged(type, ResourceManager.Resources[type]);
        }

        private void OnEnable()
        {
            ResourceManager.OnResourceChanged.AddListener(OnResourceChanged);
        }

        private void OnDisable()
        {
            ResourceManager.OnResourceChanged.RemoveListener(OnResourceChanged);
        }

        private void OnResourceChanged(ResourceType type, float newValue)
        {
            if (this.type != type)
                return;

            valueTxt.text = newValue.ToString();
        }
    }
}

