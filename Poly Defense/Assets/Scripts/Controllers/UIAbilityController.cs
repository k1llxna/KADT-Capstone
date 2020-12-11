using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAbilityController : MonoBehaviour
{
    public Character player;

    public Canvas canvas;

    public TextMeshProUGUI[] towerCostTexts;
    public TextMeshProUGUI[] abilityCostTexts;

    //Helper variable so we dont have to keep asking the player for the costs
    int[] towerCosts;
    int[] abilityCosts;

    // Start is called before the first frame update
    void Start()
    {
        canvas.worldCamera = Camera.main;

        //Clip amount of costs to the amount of towers we have
        towerCosts = new int[towerCostTexts.Length];
        abilityCosts = new int[abilityCostTexts.Length];

        //Set corresponding costs
        int i = 0;
        foreach (GameObject t in player.towers)
        {
            OTower tower = t.GetComponent<OTower>();
            towerCostTexts[i].SetText(tower.cost.ToString());
            towerCosts[i] = tower.cost;
            i++;
        }

        i = 0;
        foreach (Ability ability in player.abilities)
        {
            abilityCostTexts[i].SetText(ability.cost.ToString());
            abilityCosts[i] = ability.cost;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Change costs to res if we cant afford
        for(int i = 0; i < towerCostTexts.Length; i++)
        {
            if(player.money < towerCosts[i])
            {
                towerCostTexts[i].color = Color.red;
            }
            else
            {
                towerCostTexts[i].color = Color.green;
            }
        }

        for (int i = 0; i < abilityCostTexts.Length; i++)
        {
            if (player.GetMana() < abilityCosts[i])
            {
                abilityCostTexts[i].color = Color.red;
            }
            else
            {
                abilityCostTexts[i].color = Color.blue;
            }
        }
    }
}
