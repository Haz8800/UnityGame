using UnityEngine;
using System.Collections;

public class NuclearRocket : MonoBehaviour
{
    [Header("Rocket Settings")]
    public float upDuration = 2f;
    public float descendSpeed = 80f;
    public float blastRadius = 10f;
    public float explosionDamage = 200f;
    public float selfDestructTime = 8f;

    [Header("Explosion Settings")]
    public GameObject explosionEffect;
    public AudioClip explosionSound;

    [Header("Travel Sound Settings")]
    public AudioClip travelSound;

    [Header("Aiming")]
    public Camera playerCamera;
    public float aimRayDistance = 100f;

    private Vector3 targetPoint;
    private AudioSource travelAudioSource;
    private bool exploded = false;
    private Rigidbody rb;

    private bool launched = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        travelAudioSource = gameObject.AddComponent<AudioSource>();
        travelAudioSource.clip = travelSound;
        travelAudioSource.loop = true;
        travelAudioSource.playOnAwake = false;
        travelAudioSource.volume = 1.3f;
        travelAudioSource.spatialBlend = 1f;
        travelAudioSource.minDistance = 5f;
        travelAudioSource.maxDistance = 100f;
        travelAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
    }

    public void Launch()
    {
        if (launched) return;
        launched = true;

        LockTargetFromCamera();

        if (travelSound != null)
            travelAudioSource.Play();

        rb.useGravity = false;

        StartCoroutine(FlightPath());
        StartCoroutine(SelfDestructAfterTime());
    }

    private void LockTargetFromCamera()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("No camera assigned!");
            targetPoint = transform.position + Vector3.down * 50f;
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, aimRayDistance))
            targetPoint = hit.point;
        else
            targetPoint = playerCamera.transform.position + playerCamera.transform.forward * aimRayDistance;
    }

    IEnumerator FlightPath()
    {
        float timer = 0f;
        while (timer < upDuration)
        {
            rb.linearVelocity = Vector3.up * 30f;
            timer += Time.deltaTime;
            yield return null;
        }

        Vector3 descendDirection = (targetPoint - transform.position).normalized;
        rb.linearVelocity = descendDirection * descendSpeed;
    }

    IEnumerator SelfDestructAfterTime()
    {
        yield return new WaitForSeconds(selfDestructTime);
        if (!exploded) Explode();
    }

    void OnCollisionEnter(Collision other)
    {
        if (exploded) return;
        Explode();
    }

    private void Explode()
    {
        exploded = true;

        if (travelAudioSource.isPlaying)
            travelAudioSource.Stop();

        if (explosionSound != null)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1.8f);

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider col in colliders)
        {
            EnemyHealth enemy = col.GetComponent<EnemyHealth>();
            if (enemy != null)
                enemy.TakeDamage(explosionDamage);
        }

        if (CameraShake.instance != null)
            CameraShake.instance.Shake(0.5f, 0.6f);

        Destroy(gameObject);
    }
}
