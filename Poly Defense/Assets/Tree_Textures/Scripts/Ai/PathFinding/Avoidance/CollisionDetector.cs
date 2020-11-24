using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class CollisionDetector
{
    public Collision GetCollision(Vector3 position, Vector3 direction)
    {
        LayerMask notGround = 1 << 9;
        notGround = ~notGround;
        Collision result = new Collision();
        RaycastHit hit;

        if (Physics.Raycast(position, direction.normalized, out hit, direction.magnitude, notGround))
        {
            result.position = hit.point;
            return result;
        }
        else
        {
            return null;
        }       
    }
}
