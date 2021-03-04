using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapToPlayer : MonoBehaviour
{
    Rigidbody rb;
    public float explosionForce;
    public float attractionForce;

    public int worth;

    Vector3 offset = new Vector3(0, 1, 0);

    public AudioClip pickupClip;

    //In seconds
    public float pickUpTime;
    float time;

    bool pickupable;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        time += Time.deltaTime;

        if(time > pickUpTime)
        {
            pickupable = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pickupable)
        {
            if (other.tag.Equals("Player"))
            {
                Character player = other.GetComponent<Character>();
                if (player.money < player.maxMoney /*&& player.photonView.IsMine*/)
                {
                    Vector3 force = (other.transform.position + offset - transform.position).normalized * explosionForce;
                    rb.AddForce(force, ForceMode.Impulse);
                }
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (pickupable)
        {
            if (other.tag.Equals("Player"))
            {

                Character player = other.GetComponent<Character>();
                if (player.money < player.maxMoney /*&& player.photonView.IsMine*/)
                {
                    Vector3 force = (other.transform.position + offset - transform.position).normalized * attractionForce;
                    rb.AddForce(force, ForceMode.Acceleration);

                    if ((other.transform.position + offset - transform.position).magnitude <= 1)
                    {
                        other.gameObject.GetComponent<Character>().GiveMoney(worth);
                        player.GetComponent<AudioSource>().PlayOneShot(pickupClip);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

}
