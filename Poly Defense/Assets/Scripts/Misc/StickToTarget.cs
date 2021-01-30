using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToTarget : MonoBehaviour
{

    public Transform target;
    public Vector3 offset = new Vector3(0, 0, 0);


     void Update()
    {
        if (target == null)
        {
            foreach (Character player in GameObject.FindObjectsOfType<Character>())
            {
                //if (player.photonView.IsMine)
                //{
                    target = player.transform;
                //}
            }
        }
        else
        {
            transform.localPosition = target.localPosition + offset;
        }
    }
}
