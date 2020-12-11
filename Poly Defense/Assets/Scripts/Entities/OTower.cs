using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OTower : Structure
{

    public int cost;

    public int level;
    public float damage;

    protected int sellPrice;

    public BoxCollider bounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetRepairCost()
    {
        //We want 1 mana to be equal to 10hp
        float totalRepair = maxHealth - health;
        int repairCost = Mathf.CeilToInt(totalRepair /= 10);
        return repairCost;
    }

    public int GetSellPrice()
    {
        return sellPrice;
    }

    public void Upgrade()
    {
        level++;
        cost += 100 * (level - 1);


        maxHealth += Mathf.RoundToInt(maxHealth * 0.25f);
        maxHealth *= 2;
        health = maxHealth;

        damage += Mathf.RoundToInt(maxHealth * 0.12f);
        damage *= 2;

        transform.localScale *= 1.2f;

        Placed();
    }

    public void Sell(Character player)
    {
        //Player gets 65% back upon selling
        player.GiveMoney(Mathf.RoundToInt(cost * 0.65f));

        //Kill object
        Die();
    }

    public void Repair()
    {
        if(health < maxHealth)
        {
            health += 10;

            if(health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    public void Placed()
    {
        NavMesh graph = FindObjectOfType<NavMesh>();

        graph.DisableBounds(bounds.bounds);
    }

    public override void Die()
    {
        NavMesh graph = FindObjectOfType<NavMesh>();

        graph.EnableBounds(bounds.bounds);

        base.Die();
    }

}
