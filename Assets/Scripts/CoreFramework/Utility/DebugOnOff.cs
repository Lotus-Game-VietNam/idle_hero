using UnityEngine;

public class DebugOnOff : MonoBehaviour
{
#if UNITY_EDITOR
    // This function is called when the object becomes enabled and active.
    protected void OnEnable()
    {
        Debug.Log($"GameObject: <b>{name}</b> is Actived");
    }

    // This function is called when the behaviour becomes disabled () or inactive.
    protected void OnDisable()
    {
        Debug.Log($"GameObject: <b>{name}</b> is Deactived");
    }
    // This function is called when the MonoBehaviour will be destroyed.
    protected void OnDestroy()
    {
        Debug.Log($"GameObject: <b>{name}</b> is Destroy");
    }
#endif
}