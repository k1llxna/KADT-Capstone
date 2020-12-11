using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public int cost;
    public int damage;

    public virtual void Use(Character player)
    {

    }

    public virtual void Use(ServerPlayer player)
    {

    }
}
