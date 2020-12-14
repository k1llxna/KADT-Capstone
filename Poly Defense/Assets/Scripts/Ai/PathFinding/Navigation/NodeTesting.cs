using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class NodeTesting : MonoBehaviour
{
    MonoNode node;
    public blah testing;

    bool selected = false;

    private void Start()
    {
        node = GetComponent<MonoNode>();
    }


    private void OnMouseOver()
    {
        if (!selected)
        {

            if (Input.GetMouseButtonDown(0))
            {
                testing.setStart(node);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                testing.setFinish(node);
            }
            
            else if (Input.GetKeyDown("l"))
            {
                testing.setWall(node);
            }
            
        }
    }


}
