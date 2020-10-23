using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicSeek
{
	public Static character;
	public Vector3 target;
	public float maxSpeed;

	public KinematicSteeringOutput getSteering()
	{
		KinematicSteeringOutput result = new KinematicSteeringOutput();

		result.velocity = target - character.position;

		result.velocity *= maxSpeed;

		result.rotation = 0;
		return result;
	}
}
