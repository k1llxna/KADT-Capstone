using Photon.Pun;
using System.Collections;
using UnityEngine;

public class StandardOTowerServer : OTowerServer
{

    private Transform target;
    private Monster targetEnemy;

    [Header("Attributes")]
    public float range = 15f;

    [Header("Bullets")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Fields")]
    public string enemyTag = "Enemy";

    public Transform rotator;
    public float turnSpeed = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public int bulletsPerShot;

    // Start is called before the first frame update
    void Start()
    {
        // per x sec
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        Placed();
    }

    void UpdateTarget()
    {
        Monster[] enemies = FindObjectsOfType<Monster>();
        // store closest enemy found
        float shortestDistance = Mathf.Infinity;
        Monster nearestEnemy = null;

        foreach (Monster enemy in enemies)
        {
            if (enemy.IsAlive())
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy;
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetEnemy)
        {
            LockOnTarget();
        }

        if (fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }
    void LockOnTarget()
    {
        // look at target
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotator.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles; // XYZ
        rotator.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Tower_Bullet_Default bullet = bulletGO.GetComponent<Tower_Bullet_Default>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    [PunRPC]
    void RPC_SetTransform(Transform tower)
    {
        transform.position = tower.position;
        transform.rotation = tower.rotation;
    }
    [PunRPC]
    void RPC_Die()
    {
        base.Die();
    }
    [PunRPC]
    void RPC_Repair()
    {
        base.Repair();
    }
    [PunRPC]
    void RPC_Upgrade()
    {
        base.Upgrade();
    }
}

