using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{


    public float SPD { get; private set; }


    public void Initialized(float SPD)
    {
        this.SPD = SPD;
    }
}
