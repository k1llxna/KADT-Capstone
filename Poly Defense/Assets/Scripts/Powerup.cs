using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public GameObject bullet;
    Vector3 offset = new Vector3(0, 1, 0);

    public float waitTime;
    public float length;

    Quaternion righOffset = new Quaternion(0, 45, 0,0);

    public GameObject mesh;
    public GameObject lights;
    BoxCollider boxCollider;

    AudioSource audio;

    bool inUse = false;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        StartCoroutine(Spawn());
        audio = GetComponent<AudioSource>();
    }

    //Only avaiable for 10 seconds
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(10f);

        Destroy(gameObject);
    }

    IEnumerator Use(Character character)
    {
        inUse = true;
        float time = 0;

        while (time < length)
        {

            Debug.Log("shooting ball");

            //Instantiates bullets in a cone shape infront of the player
            Instantiate(bullet, character.transform.position + offset, Camera.main.transform.rotation * Quaternion.Euler(0, 10, 0));
            Instantiate(bullet, character.transform.position + offset, Camera.main.transform.rotation * Quaternion.Euler(0, -10, 0));
            Instantiate(bullet, character.transform.position + offset, Camera.main.transform.rotation);

            transform.position = character.transform.position;
            audio.PlayOneShot(audio.clip);

            time += waitTime;
            yield return new WaitForSeconds(waitTime);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Character player = collision.gameObject.GetComponent<Character>();
            mesh.SetActive(false);
            lights.SetActive(false);
            boxCollider.enabled = false;


            foreach(Powerup powerup in FindObjectsOfType<Powerup>())
            {
                if(powerup == this)
                {
                    StopAllCoroutines();
                    StartCoroutine(Use(player));
                }
                else if(powerup.inUse)
                {
                    powerup.StopAllCoroutines();
                    Destroy(powerup.gameObject);
                }
            }
        }
    }

}
