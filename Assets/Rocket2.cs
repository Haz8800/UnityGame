using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosionEffect;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
