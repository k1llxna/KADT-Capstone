using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEffects : MonoBehaviour
{

    public int explosionForce;
    public int explosionRadius;
    public int upwardsModifier;

    public Transform explosionPoint;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(10, 11, true);

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.AddExplosionForce(explosionForce, explosionPoint.position, explosionRadius, upwardsModifier, ForceMode.Impulse);

        float ranx = Random.Range(-3, 3);
        float ranz = Random.Range(-3, 3);
        float rany = Random.Range(1, 3);

        rb.AddForce(new Vector3(ranx, rany, ranz), ForceMode.Impulse);

    }
}
