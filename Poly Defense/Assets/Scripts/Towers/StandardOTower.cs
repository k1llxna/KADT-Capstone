using System.Collections;
using UnityEngine;

public class StandardOTower : OTower
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

    public AudioClip shootClip;

    // Start is called before the first frame update
    void Start()
    {
        // per x sec
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

        base.Start();
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
        if (target)
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

            if (target)
            {
                GetComponent<AudioSource>().PlayOneShot(shootClip);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

