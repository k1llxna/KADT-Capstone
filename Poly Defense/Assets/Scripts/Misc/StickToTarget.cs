using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToTarget : MonoBehaviour
{

    public Transform target;
    public Vector3 offset = new Vector3(0, 0, 0);


    Vector3 truePosition;


     void Update()
    {
        truePosition = target.localPosition + offset;


        if (target == null)
        {
            foreach (Character player in GameObject.FindObjectsOfType<Character>())
            {
                target = player.transform;
            }
        }
        else
        {
            transform.localPosition = truePosition;     
        }
    }
}
