using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    Static body = new Static();
    public Vector3 target;
    public float speed;

    KinematicSeek seek = new KinematicSeek();

    KinematicArrive arrive = new KinematicArrive();

    public bool Arrive;

    private void Start()
    {
        seek.character = body;
        seek.maxSpeed = speed;
        seek.target = target;

        arrive.character = body;
        arrive.maxSpeed = speed;
        arrive.target = target;

        body.position = transform.position;

    }
    // Update is called once per frame
    void Update()
    {
        UpdatePosition();       
    }

    void UpdatePosition()
    {
        if (Arrive)
        {
            body.UpdateObject(arrive.getSteering(), Time.deltaTime);
        }
        else
        {
            body.UpdateObject(seek.getSteering(), Time.deltaTime);
        }

        transform.position = body.position;
    }

}
