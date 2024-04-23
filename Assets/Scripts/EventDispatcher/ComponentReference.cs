using System;
using UnityEngine;


namespace Lotus.CoreFramework
{
    public static class ComponentReference
    {
        public static Func<RectTransform> MainRect = null;

        public static Func<Transform> BossRenderParent = null;
    }
}

