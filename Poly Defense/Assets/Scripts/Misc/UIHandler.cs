using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public Canvas canvas;
    public Slider slider;


    void Update()
    {
        slider.transform.LookAt(Camera.main.transform);
    }

    IEnumerator ShowUI()
    {
        canvas.enabled = true;

        yield return new WaitForSeconds(0.5f);

        canvas.enabled = false;
    }
}
