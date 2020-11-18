using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : ServerPlayer
{
    public GameObject bullet;

    protected override void Attack()
    {
        Vector3 offset = new Vector3(0, 1, 0);
        Instantiate(bullet, transform.position + offset, Camera.main.transform.rotation);
    }


}
