using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollEffects : MonoBehaviour
{

   public  List<Rigidbody> rbs;
    public List<BoxCollider> bcs;
    public List<CapsuleCollider> ccs;
    public SphereCollider head;

    public CapsuleCollider enemyCollider;

    private void Awake()
    {
        TurnOff();
    }

    public void TurnOn()
    {
        Debug.Log("RagDoll Effects Turned on");

        foreach(Rigidbody rb in rbs)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        foreach(BoxCollider bc in bcs)
        {
            bc.enabled = true;
        } 
        
        foreach(CapsuleCollider cc in ccs)
        {
            cc.enabled = true;
        }

        head.enabled = true;

        enemyCollider.enabled = false;
    }
    public void TurnOff()
    {
        foreach(Rigidbody rb in rbs)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        foreach(BoxCollider bc in bcs)
        {
            bc.enabled = false;
        }

        foreach (CapsuleCollider cc in ccs)
        {
            cc.enabled = false;
        }

        head.enabled = false;

        enemyCollider.enabled = true;
    }
}
