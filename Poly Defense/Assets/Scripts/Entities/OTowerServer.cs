using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OTowerServer : OTower
{

    private void Start()
    {
        Placed();
    }
    // Update is called once per frame

    public override void Upgrade()
    {
        level++;
        cost += 100 * (level - 1);


        maxHealth += Mathf.RoundToInt(maxHealth * 0.25f);
        maxHealth *= 2;
        health = maxHealth;

        damage += Mathf.RoundToInt(maxHealth * 0.12f);
        damage *= 2;

        transform.localScale *= 1.2f;

        Placed();
    }

    public override void Sell(Character player)
    {
        //Player gets 65% back upon selling
        player.GiveMoney(Mathf.RoundToInt(cost * 0.65f));

        //Kill object
        photonView.RPC("RPC_Die", RpcTarget.AllBufferedViaServer);
    }

    public override void Repair()
    {
        if(health < maxHealth)
        {
            health += 10;

            if(health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    public override void Placed()
    {
        Debug.Log("Disabaling Nodes");
        NavMesh graph = FindObjectOfType<NavMesh>();

        graph.DisableBounds(bounds.bounds);
    }

    public override void Die()
    {
        photonView.RPC("RPC_Die", RpcTarget.AllBufferedViaServer);
    }



    [PunRPC]
    void RPC_SetTransform(Transform tower)
    {
        transform.position = tower.position;
        transform.rotation = tower.rotation;
    }
    [PunRPC]
    void RPC_Die()
    {
        base.Die();
    }
    [PunRPC]
    void RPC_Repair()
    {
        base.Repair();
    }
    [PunRPC]
    void RPC_Upgrade()
    {
        base.Upgrade();
    }



}
