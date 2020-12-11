using System.Collections;
using UnityEngine;

public class LazerTower : OTower
{
    private Transform target;
    private Monster targetEnemy;

    [Header("Attributes")]
    public float range = 15f;

    [Header("Unity Fields")]
    public string enemyTag = "Enemy";

    public Transform rotator;
    public float turnSpeed = 10f;

    public Transform firePoint;

    [Header("Laser Components")]
    public int dmgOverTime = 20;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    // Start is called before the first frame update
    void Start()
    {
        // per x sec
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        Monster[] enemies = GameObject.FindObjectsOfType<Monster>();
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
        } else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
            }
            return;
        }

        LockOnTarget();

        if (targetEnemy)
        {
            Laser();
        }
    }

    void Laser()
    {
        targetEnemy.TakeDamage(dmgOverTime * Time.deltaTime, transform.position);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;
        impactEffect.transform.position = target.position + dir.normalized;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void LockOnTarget()
    {
        // look at target
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotator.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles; // XYZ
        rotator.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

