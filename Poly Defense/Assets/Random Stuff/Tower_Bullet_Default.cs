using UnityEngine;

public class Tower_Bullet_Default : MonoBehaviour
{
    // target to persue
    private Transform target;
    public float speed = 50f;
    public int damage = 50;
    // public GameObject impactEffect

    public void Seek(Transform target_)
    {
        target = target_;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return; // incase it takes long
        }

        // bullet orientation

        Vector3 dir = target.position - transform.position;
        float frameDistance = speed * Time.deltaTime;

        if (dir.magnitude <= frameDistance) // prevent overshoots
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * frameDistance, Space.World);

    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage);
        }
    }
    void  HitTarget()
    {
        // add effect here
        // GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        // Destroy(effect, 2f);

    }
}
