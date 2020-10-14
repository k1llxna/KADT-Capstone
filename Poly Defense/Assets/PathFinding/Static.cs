using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static
{
    public Vector3 position;
    public float orientation;

    public void UpdateObject(KinematicSteeringOutput steering, float time)
    {
        position += (Vector3)steering.velocity * time;
        orientation += steering.rotation * time;
    }
}
