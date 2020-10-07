using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 7f;
    public float maxSpeed = 10f;
    public float mass = 1;
    public float maxVel = 10f;
    public float maxForce = 10;
    public float maxAccel = 6f;
    public float targetRadius;
    public float slowRadius = 2f;
    public float timeToTarget = 0.1f;

    private Transform target;
    private int wavepointIndex = 0;
    [SerializeField]
    private Vector3 vel;

    void Start()
    {
        target = Waypoints.waypoints[0];
        vel = Vector3.zero;
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position; // direction to target
        Vector3 steering = dir - vel;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        steering /= mass;

        vel = Vector3.ClampMagnitude(vel + steering, maxVel);   // velocity cliped to a max speed
        transform.position += vel * Time.deltaTime;             // position += velocity * time

        transform.forward = transform.forward.normalized;
        transform.forward *= maxAccel;

        if (Vector3.Distance(transform.position, target.position) <= 0.8f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.waypoints.Length - 1)
        {
            Destroy(gameObject);
        }
        wavepointIndex++;
        target = Waypoints.waypoints[wavepointIndex];
    }
}

/*
     
        
        ////////////////////////////////////////////////////

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
               // transform.forward = vel.normalized;

        var desiredVelocity = target.transform.position - transform.position;
        desiredVelocity = desiredVelocity.normalized * MaxVelocity;

        var steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, MaxForce);
        steering /= Mass;

        velocity = Vector3.ClampMagnitude(velocity + steering, MaxVelocity);
        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity.normalized;
     */
