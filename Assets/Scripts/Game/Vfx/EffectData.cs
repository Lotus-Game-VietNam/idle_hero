using UnityEngine;

public class EffectData
{
    public Vector3 showPoint;
    public Transform followTarget;

    public float[] floatData = null;
    public string[] strData = null;
    public object[] objectData = null;


    public EffectData()
    {
        
    }

    public EffectData(Vector3 point, Transform followTarget)
    {
        this.showPoint = point;
        this.followTarget = followTarget;
    }

    public EffectData(float[] floatData = null, string[] strData = null, object[] objectData = null)
    {
        this.floatData = floatData;
        this.strData = strData;
        this.objectData = objectData;
    }
}
