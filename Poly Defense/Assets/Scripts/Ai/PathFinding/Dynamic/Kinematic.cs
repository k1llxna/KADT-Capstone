using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematic
{
    public Vector3 position;
    public float orientation;

    public Vector3 velocity;
    public float rotation;

    public float rotSpeed;

    public void Update(SteeringOutput steering, float time)
    {
        position += velocity * time;
        orientation += rotation * time;

        velocity += steering.linear * time;
        //rotation += steering.angular * time;

        orientation = Mathf.Lerp(orientation, steering.angular, time * rotSpeed);
    }

}
