using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek
{
    public Kinematic character;
    public Transform target;

    public float maxAcceleration;

    public SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        result.linear = target.position - character.position;

        result.linear.Normalize();
        result.linear *= maxAcceleration;

        result.angular = 0;
        return result;
    }
    

}
