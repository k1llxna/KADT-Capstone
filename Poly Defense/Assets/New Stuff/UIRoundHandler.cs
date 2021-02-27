using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRoundHandler : MonoBehaviour
{

    GameManager gm;

    public TextMeshProUGUI counterText;
    public TextMeshProUGUI roundText;

    bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (gm.gameState == GameManager.GameState.WAITING)
        {
            counterText.enabled = true;
            counterText.SetText("Next Round Will Begin In " + gm.counter.ToString());
        }
        else
        {
            counterText.enabled = false;
        }

        if(gm.gameState == GameManager.GameState.STARTROUND && !started)
        {
            started = true;
            roundText.SetText("Round " + gm.round);
            roundText.GetComponent<Animator>().SetTrigger("Start");
        }
        else if(gm.gameState == GameManager.GameState.ENDROUND && !started)
        {
            started = true;
            roundText.SetText("Round " + gm.round + " Complete");
            roundText.GetComponent<Animator>().SetTrigger("Start");
        }
        else if(gm.gameState == GameManager.GameState.FIGHTING || gm.gameState == GameManager.GameState.WAITING)
        {
            started = false;
        }

    }
}
