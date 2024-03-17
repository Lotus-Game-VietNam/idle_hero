using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBrain : CharacterBrain
{
    public override CharacterType CharacterType => CharacterType.Hero;




    private void OnAutoFarm()
    {

    }


    protected override void Update()
    {
        base.Update();
        OnAutoFarm();
    }
}
