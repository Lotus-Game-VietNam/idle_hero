using Lotus.CoreFramework;
using UnityEngine;

public class WorldSpaceCanvas : MonoUI
{
    public bool useMainCamera = true;
    public Camera worldCamera = null;


    private Transform worldCameraTrans = null;


    private void Awake()
    {
        canvas.worldCamera = useMainCamera ? Camera.main : worldCamera;
        worldCameraTrans = canvas.worldCamera.transform;
    }


    private void Update()
    {
        canvas.transform.forward = -worldCameraTrans.forward;
    }
}
