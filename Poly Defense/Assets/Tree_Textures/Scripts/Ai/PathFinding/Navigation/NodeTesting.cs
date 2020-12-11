using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeTesting : MonoBehaviour
{

    blah main;


    // Start is called before the first frame update
    void Start()
    {
        main = FindObjectOfType<blah>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            main.setStart(transform);
        }
        else if(Input.GetMouseButtonDown(1))
        {
            main.setFinish(transform);
        }
        else if(Input.GetKeyDown("w"))
        {
            main.setWall(transform);
        }
    }
}
