using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBallAbility : Ability
{
    public GameObject prefab;

    public override void Use(Character player)
    {
        if (player.GetMana() >= cost)
        {
            player.UseMana(cost);
            GameObject newB = Instantiate(prefab, transform.position + new Vector3(0, 1, 0), Camera.main.transform.rotation);
        }
        else
        {
            Debug.Log("You dont have enough Mana to cast this spell");
        }
    }
}
