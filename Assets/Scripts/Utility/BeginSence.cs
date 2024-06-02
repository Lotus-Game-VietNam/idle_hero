using Lotus.CoreFramework;
using UnityEngine;

public class BeginSence : MonoBehaviour
{


    private void Start()
    {
        this.LoadSceneAsync(Utilities.GetCurrentFarmScene(), 0.5f);
    }
}
