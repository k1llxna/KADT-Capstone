using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align
{

    public Kinematic character;
    public Kinematic target;

    public float maxAngularAccelleration;
    public float maxRotation;

    public float targetRadius;
    public float slowRadius;

    float timeToTarget = 0.1f;

   public SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        float rotation = target.orientation - character.orientation;
        rotation = (rotation + Mathf.PI) % Mathf.PI;
        float rotationSize = Mathf.Abs(rotation);

        float targetRotation;

        if (rotationSize < targetRadius)
            return null;
        if (rotationSize > slowRadius)
            targetRotation = maxRotation;
        else
            targetRotation = maxRotation * rotationSize / slowRadius;

        targetRotation *= rotation / rotationSize;

        result.angular = targetRotation - character.rotation;
        result.angular /= timeToTarget;

        float angularAcceleration = Mathf.Abs(result.angular);
        if(angularAcceleration > maxAngularAccelleration)
        {
            result.angular /= angularAcceleration;
            result.angular *= maxAngularAccelleration;
        }

        result.linear = Vector3.zero;
        return result;
    }
}
