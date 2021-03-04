using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    bool isOpen = false;

    public Transform[] spawnPoints;

    public GameObject[] jewels;

    //Based on level and round
    public int power; //determines the amount and rarity of the jewels


    void Open()
    {
        isOpen = true;

        GetComponent<Animator>().SetTrigger("Open");

        //play chest opening sound
        //add particle effects

        for(int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, jewels.Length);

            Instantiate(jewels[rand], spawnPoints[i].position, Quaternion.identity);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if(!isOpen)
        {
            if (Input.GetKeyDown("f"))
            {
                Open();
            }
        }
    }
}
