using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRoomButton : MonoBehaviour
{

    public void ToggleVisability()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();

        if (rend.enabled)
            rend.enabled = false;
        else
            rend.enabled = true;
    }
}
