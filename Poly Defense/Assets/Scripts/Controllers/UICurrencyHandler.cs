using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICurrencyHandler : MonoBehaviour
{
    Character player;
    public Slider slider;
    public TextMeshProUGUI text;

    int maxValue = 100;
    int value = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Character>();

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (value != player.GetMoney())
            UpdateUI();
    }

    void UpdateUI()
    {
        value = player.GetMoney();
        maxValue = player.maxMoney;

        slider.value = (value);
        slider.maxValue = maxValue;


        text.SetText(value.ToString());
    }


}
