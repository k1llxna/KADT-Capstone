using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIController : MonoBehaviour
{
    public OTower tower;

    //To turn off and On
    public Canvas canvas;

    public Image panel;
    public Slider healthBar;
    public Slider upgradeBar;
    public Slider sellBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradeCost;
    public TextMeshProUGUI repairCost;
    public TextMeshProUGUI sellCost;

    // Update is called once per frame
    void Update()
    {
        panel.transform.LookAt(Camera.main.transform);
    }

    void UpdateStuff()
    {
        healthBar.maxValue = tower.maxHealth;
        healthBar.value = tower.health;
        healthText.SetText(healthBar.value.ToString());

        repairCost.SetText(tower.GetRepairCost().ToString());
        sellCost.SetText(tower.GetSellPrice().ToString());
        levelText.SetText("Level " + tower.level);
    }

    IEnumerator ShowUI()
    {
        canvas.gameObject.SetActive(true);

        UpdateStuff();

        yield return new WaitForSeconds(0.1f);

        canvas.gameObject.SetActive(false);
    }

}
