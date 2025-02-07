using UnityEngine;
using UnityEngine.VFX;

public class LaserVFXController : MonoBehaviour
{
    public VisualEffect laserVFX;
    public Transform startPoint;
    public Transform endPoint;

    void Update()
    {
        laserVFX.SetVector3("Start Point", startPoint.position);
        laserVFX.SetVector3("End Point", endPoint.position);
    }
}
