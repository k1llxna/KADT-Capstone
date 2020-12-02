using UnityEngine;
using UnityEditor;

public class Editor_Tower : EditorWindow
{
    [MenuItem("Edit Game/Edit Tower")]
    public static void ShowWindow()
    {
        GetWindow<Editor_Tower>("Edit Tower");
    }

    private void OnGUI()
    {
        GUILayout.Label("A Tower Must Be Selected!", EditorStyles.boldLabel);
        GUILayout.Label("Increase Range of tower", EditorStyles.boldLabel);

        if (GUILayout.Button("Increment by 1"))
        {
            SetRange(1f);
        }

        if (GUILayout.Button("Decrement by 1"))
        {
            SetRange(-1f);
        }


        GUILayout.Space(10f);
        GUILayout.Label("Set Fire Type", EditorStyles.boldLabel);

        if (GUILayout.Button("Single"))
        {
            SetFireRate("Single");
        }

        if (GUILayout.Button("Double"))
        {
            SetFireRate("Double");
        }

        if (GUILayout.Button("Burst"))
        {
            SetFireRate("Burst");
        }
    }

    public void SetFireRate(string newFireRate)
    {
        foreach (GameObject g in Selection.gameObjects)
        {
            Tower t = g.GetComponent<Tower>();
            if (t)
            {
                switch (newFireRate)
                {
                    case "Single":
                        t.fireRate = 1;
                        break;

                    case "Double":
                        t.fireRate = 2;
                        break;

                    case "Burst":
                        t.fireRate = 3;
                        break;
                }
            }
        }
    }

    public void SetRange(float i)
    {
        foreach (GameObject g in Selection.gameObjects)
        {
            Tower t = g.GetComponent<Tower>();
            t.range += i;
        }
    }
}

//GUILayout.Space(10f);
//GUILayout.Label("Increase Damage of tower", EditorStyles.boldLabel);

//if (GUILayout.Button("+1"))
//{
//    SetDamage(1f);
//}

//if (GUILayout.Button("-1"))
//{
//    SetDamage(-1f);
//}

//public void SetDamage(float d)
//{
//    foreach (GameObject g in Selection.gameObjects)
//    {
//        Tower t = g.GetComponent<Tower>();
//        if (t)
//        {
//            if (t.damage == 0 && d == -1)
//            {
//            }
//            else
//            {
//                t.damage = t.damage + d;
//            }
//        }
//    }
//}