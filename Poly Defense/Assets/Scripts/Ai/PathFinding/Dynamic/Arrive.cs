using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive
{ 
    public Kinematic character;
    public Transform target;

    public float maxAcceleration;
    public float maxSpeed;

    public float targetRadius;
    public float slowRadius;

    public float timeToTarget = 0.1f;

    float targetSpeed;

    public SteeringOutput GetSteering()
    {
        SteeringOutput result = new SteeringOutput();

        Vector3 direction = target.position - character.position;
        float distance = direction.magnitude;

        if (distance < targetRadius)
            return null;

        if (distance > slowRadius)
            targetSpeed = maxSpeed;
        else
            targetSpeed = maxSpeed * distance / slowRadius;

        Vector3 targetVelocity = direction.normalized;
        targetVelocity *= targetSpeed;

        result.linear = targetVelocity - character.velocity;
        result.linear /= timeToTarget;

        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear.Normalize();
            result.linear *= maxAcceleration;
        }
                
        result.angular = NewOrientation(character.velocity, character.orientation);
        return result;

    }

    float NewOrientation(Vector3 velocity, float currentOrientation)
    {
        if (velocity.magnitude > 0)
            return Mathf.Atan2(velocity.x, velocity.z);
        else
            return currentOrientation;

    }
}
