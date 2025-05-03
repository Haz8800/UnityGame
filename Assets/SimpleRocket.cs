using UnityEngine;
using System.Collections;

public class SimpleRocket : MonoBehaviour
{
    [Header("Rocket Settings")]
    public float lifetime = 5f;
    public float blastRadius = 5f;
    public float explosionDamage = 100f;

    [Header("Explosion Settings")]
    public GameObject explosionEffect;
    public AudioClip explosionSound;

    [Header("Travel Sound Settings")]
    public AudioClip travelSound;

    private AudioSource travelAudioSource;
    private bool exploded = false;
    private Coroutine travelLoopCoroutine;

    void Awake()
    {
        travelAudioSource = gameObject.AddComponent<AudioSource>();
        travelAudioSource.clip = travelSound;
        travelAudioSource.loop = true;
        travelAudioSource.playOnAwake = false;
        travelAudioSource.volume = 1.3f;
        travelAudioSource.spatialBlend = 1f;
        travelAudioSource.minDistance = 3f;
        travelAudioSource.maxDistance = 100f;
        travelAudioSource.dopplerLevel = 0.5f;
        travelAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
    }

    public void StartTravelSound()
    {
        // Start playing travel sound immediately
        travelAudioSource.Play();

        // Optionally fade in loop behavior near end of flight
        travelLoopCoroutine = StartCoroutine(FadeToLoop());

        Destroy(gameObject, lifetime);
    }

    IEnumerator FadeToLoop()
    {
        yield return new WaitForSeconds(lifetime - 1f);
        if (!exploded && travelAudioSource != null && travelAudioSource.isPlaying)
        {
            travelAudioSource.loop = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (exploded) return;
        exploded = true;

        if (travelLoopCoroutine != null)
            StopCoroutine(travelLoopCoroutine);

        if (travelAudioSource != null && travelAudioSource.isPlaying)
            travelAudioSource.Stop();

        if (explosionSound != null)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1.8f);

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // AOE damage
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider col in colliders)
        {
            EnemyHealth enemy = col.GetComponent<EnemyHealth>();
            if (enemy != null)
                enemy.TakeDamage(explosionDamage);
        }

        if (CameraShake.instance != null)
        {
            CameraShake.instance.Shake(0.4f, 0.5f);
        }

        Destroy(gameObject);
    }
}