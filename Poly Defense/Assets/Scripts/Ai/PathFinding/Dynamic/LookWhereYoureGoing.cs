using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class LookWhereYoureGoing : Align
{
    public SteeringOutput getSteering()
    {
        Vector3 velocity = character.velocity;

        if (velocity.magnitude == 0)
            return null;

        target.orientation = Mathf.Atan2(-velocity.x, velocity.z);

        return  base.getSteering();
    }
}
