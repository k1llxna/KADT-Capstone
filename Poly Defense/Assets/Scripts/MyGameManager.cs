using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{

    public GameObject menuScreen;
    public GameObject gameOverScreen;

    public GameObject player;

    public Transform spawnpoint;

    GameObject baseObject;
    bool gameOver = false;
    bool paused;

    // Start is called before the first frame update
    void Awake()
    {
        player = Instantiate(player, spawnpoint.position, Quaternion.identity);

        baseObject = GameObject.FindGameObjectWithTag("Base");

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {                

        if(baseObject == null && !gameOver)
        {
            gameOver = true;
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
            gameOverScreen.GetComponent<Animator>().SetTrigger("Start");
            gameOverScreen.SetActive(true);
            Destroy(player);
        }
        else
        {
            if (Input.GetKeyDown("escape"))
            {
                menuScreen.SetActive(!menuScreen.activeSelf);
                paused = !paused;

                if(paused)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                }


            }
        }

    }



    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        SceneManager.LoadScene(2);
    }


}
