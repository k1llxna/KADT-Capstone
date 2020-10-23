using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : Arrive
{
    CollisionDetector detector = new CollisionDetector();

    public float avoidDistance;

    public float lookAhead;

    public SteeringOutput getSteering()
    {

        Vector3 direction = character.velocity.normalized* lookAhead;

        Collision collision = detector.GetCollision(character.position, direction);

        if (collision != null)
            target.position = collision.position + collision.normal * avoidDistance;

        return base.getSteering();
    }
}
