using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static
{
    public Vector3 position;
    public float orientation;
    public float rotSpeed = 10;

    public void UpdateObject(KinematicSteeringOutput steering, float time)
    {
        position += (Vector3)steering.velocity * time;
        orientation = Mathf.Lerp(orientation, steering.rotation, time * rotSpeed);

        /*
        if (Mathf.Abs(orientation) < Mathf.Abs(steering.rotation))
            orientation += steering.rotation * time;
        else if(Mathf.Abs(orientation) > Mathf.Abs(steering.rotation))
            orientation -= steering.rotation * time;*/
    }
}
