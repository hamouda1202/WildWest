using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{

    public float radius;
    public float explosionForce;


    private void OnTriggerEnter(Collider other)
    {
        Explode();
        Destroy(this.gameObject);
    }

    private void Explode()
    {
        // Instantiate animation

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
            }
        }
    }
}
