﻿using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;

    private GameObject turret;

    private Renderer rend;
    private Color startColor;

    public Vector3 positionOffset;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void OnMouseDown()
    {
        if (turret != null)
        {
            // already turret built in this node
            return;
        }

        //build turret
        GameObject turretToBuild = BuildManager.instance.getTurretToBuild();
        turret = (GameObject)Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);
    }

    void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
