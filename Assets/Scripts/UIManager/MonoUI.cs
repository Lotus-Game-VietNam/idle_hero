using UnityEngine;

namespace Lotus.CoreFramework
{
    public class MonoUI : MonoBehaviour
    {


        private RectTransform _rect = null;
        public RectTransform rect => this.TryGetComponent(ref _rect);


        private Canvas _canvas = null;
        public Canvas canvas => this.TryGetComponent(ref _canvas);
    }
}

