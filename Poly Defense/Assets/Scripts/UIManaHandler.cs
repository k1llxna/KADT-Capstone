using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManaHandler : MonoBehaviour
{
    Character player;
    public Slider slider;
    public TextMeshProUGUI text;

    int maxValue;
    int value;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Character>();

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (value != player.GetMana())
            UpdateUI();
    }

    void UpdateUI()
    {
        value = player.GetMana();
        maxValue = player.maxMana;

        slider.value = (value);
        slider.maxValue = maxValue;


        text.SetText(value.ToString());
    }


}
