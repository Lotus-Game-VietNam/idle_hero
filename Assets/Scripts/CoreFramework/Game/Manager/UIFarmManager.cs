using Lotus.CoreFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIFarmManager : MonoUI
{



    private void Awake()
    {
        ComponentReference.MainRect = () => rect;
    }
}
