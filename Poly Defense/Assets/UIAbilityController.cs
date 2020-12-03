using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAbilityController : MonoBehaviour
{
    public Character player;

    public TextMeshProUGUI[] abilityCosts = new TextMeshProUGUI[4];

    //Helper variable so we dont have to keep asking the player for the costs
    int[] costs;

    // Start is called before the first frame update
    void Start()
    {
        //Clip amount of costs to the amount of towers we have
        costs = new int[abilityCosts.Length];

        //Set corresponding costs
        int i = 0;
        foreach (GameObject t in player.towers)
        {
            OTower tower = t.GetComponent<OTower>();
            abilityCosts[i].SetText(tower.cost.ToString());
            costs[i] = tower.cost;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Change costs to res if we cant afford
        for(int i = 0; i < abilityCosts.Length; i++)
        {
            if(player.money < costs[i])
            {
                abilityCosts[i].color = Color.red;
            }
            else
            {
                abilityCosts[i].color = Color.green;
            }
        }
    }
}
