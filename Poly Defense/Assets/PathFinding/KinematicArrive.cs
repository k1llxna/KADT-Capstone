﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicArrive
{    
    public Static character;
    public Vector3 target;
	public float maxSpeed;
    public float radius = 1f;
    public float timeToTarget = 0.1f;

   public KinematicSteeringOutput getSteering()
    {
		KinematicSteeringOutput result =  new KinematicSteeringOutput();
		result.velocity = target - character.position;

		if (result.velocity.magnitude < radius)
		{
			return result;
		}

		// Trick to slow down near the target
		result.velocity /= timeToTarget;

		// clip to max speed
		if (result.velocity.magnitude > maxSpeed)
		{
			result.velocity.Normalize();
			result.velocity *= maxSpeed;

		}

		result.rotation = 0;
		return result;
	}
}