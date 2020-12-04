using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint anotherTurret;

    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Start is called before the first frame update
    public void SelectBasicTurret()
    {
        buildManager.SelectTurretToBuild(standardTurret);
    }

    // Start is called before the first frame update
    public void SelectAnotherTurret()
    {
        buildManager.SelectTurretToBuild(anotherTurret);
    }
}
