using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotus.CoreFramework
{
    public class MonoUI : MonoBehaviour
    {


        private RectTransform _rect = null;
        public RectTransform rect => this.TryGetComponent(ref _rect);
    }
}

