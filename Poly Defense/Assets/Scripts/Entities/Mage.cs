using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Character
{
    public GameObject bullet;

    // Update is called once per frame



    public override void Attack()
    {
        GameObject newB = Instantiate(bullet, transform.position + new Vector3(0, 1, 0), Camera.main.transform.rotation);
        Destroy(newB, 5f);
    }
}
