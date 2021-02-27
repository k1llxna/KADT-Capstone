using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICounter : MonoBehaviour
{
    GameManager gm;

    public TextMeshProUGUI counterText;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if(gm.gameState == GameManager.GameState.WAITING)
        {
            counterText.enabled = true;
            counterText.SetText(gm.counter.ToString());
        }
        else
        {
            counterText.enabled = false;
        }
    }
}
