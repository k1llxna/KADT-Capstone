using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRayAtMousePosition : MonoBehaviour
{
    public Transform hitPosition;
    public AiController ai;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100)){
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
                hitPosition.position = hit.point;

                ai.target = hitPosition;
                ai.UpdatePathfinding();
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue);
                print("hit nothing");
            }
        }
    }

}
