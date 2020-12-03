using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Start is called before the first frame update
    public void PurchaseBasicTurret()
    {
        buildManager.SetTurretToBuild(buildManager.standardTurretPrefab);
    }

    // Start is called before the first frame update
    public void PurchaseAnotherTurret()
    {
        buildManager.SetTurretToBuild(buildManager.anotherTurretPrefab);
    }
}
