using Lotus.CoreFramework;
using UnityEngine;

public class BeginSence : MonoBehaviour
{


    private void Start()
    {
        this.LoadSceneAsync(SceneName.Farm_1, 0.5f);
    }
}
