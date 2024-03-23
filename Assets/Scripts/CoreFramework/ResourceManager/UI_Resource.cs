using DG.Tweening;
using Doozy.Runtime.Reactor.Animators;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;



namespace Lotus.CoreFramework
{
    public class UI_Resource : MonoBehaviour
    {
        [Title("Configuration")]
        public ResourceType type;

        private TMP_Text _valueTxt = null;
        public TMP_Text valueTxt => this.TryGetComponentInChildren(ref _valueTxt);

        private UIAnimator _ator = null;
        public UIAnimator ator => this.TryGetComponentInChildren(ref _ator);


        private Tween textTween = null;



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

            textTween.Stop();
            ator.Play();

            int.TryParse(valueTxt.text, out int startValue);
            textTween = DOTween.To(() => startValue, x => valueTxt.text = x.ToString(), (int)newValue, 0.5f);
        }
    }
}

