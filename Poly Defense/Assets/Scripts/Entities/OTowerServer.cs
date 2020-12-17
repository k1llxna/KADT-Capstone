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
        photonView.RPC("RPC_Upgrade", RpcTarget.AllBufferedViaServer);
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
        photonView.RPC("RPC_Repair", RpcTarget.AllBufferedViaServer);
    }

    public override void Die()
    {
        photonView.RPC("RPC_Die", RpcTarget.AllBufferedViaServer);
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
