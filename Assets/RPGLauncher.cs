using UnityEngine;

public class RPGLauncher : MonoBehaviour
{
    [Header("Launcher Setup")]
    public Transform firePoint;                 // Where rocket spawns
    public Transform rocketHolder;              // Where rocket sits in launcher
    public GameObject rocketPrefab;             // Rocket with Rigidbody + SimpleRocket
    public GameObject rocketVisualPrefab;       // Static rocket model
    public GameObject loadedRocket;             // Current rocket inside launcher

    [Header("Rocket Settings")]
    public float launchForce = 25f;

    [Header("Reload Settings")]
    public float reloadTime = 2f;
    private bool isReloading = false;

    [Header("Audio Settings")]
    public AudioClip launchSound;
    public AudioClip reloadSound;
    private AudioSource audioSource;

    [Header("Recoil Settings")]
    public Transform recoilObject;
    public Vector3 recoilKickback = new Vector3(0f, 0f, -0.2f);
    public float recoilReturnSpeed = 4f;

    private Vector3 recoilOriginalPos;
    private Vector3 recoilCurrentOffset = Vector3.zero;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (recoilObject != null)
            recoilOriginalPos = recoilObject.localPosition;
    }

    void Update()
    {
        // Fire input
        if (Input.GetButtonDown("Fire1") && !isReloading && loadedRocket != null)
        {
            FireLoadedRocket();
        }

        // Smoothly return recoil
        if (recoilObject != null)
        {
            recoilCurrentOffset = Vector3.Lerp(recoilCurrentOffset, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
            recoilObject.localPosition = recoilOriginalPos + recoilCurrentOffset;
        }
    }

    void FireLoadedRocket()
    {
        isReloading = true;

        // Detach rocket
        loadedRocket.transform.parent = null;

        // Add Rigidbody and Collider if needed
        Rigidbody rb = loadedRocket.GetComponent<Rigidbody>();
        if (rb == null) rb = loadedRocket.AddComponent<Rigidbody>();

        Collider col = loadedRocket.GetComponent<Collider>();
        if (col == null) col = loadedRocket.AddComponent<CapsuleCollider>();

        // Launch the rocket in the red arrow (X+) direction
        rb.linearVelocity = firePoint.right * launchForce;

        // Play launch sound
        if (launchSound != null && audioSource != null)
            audioSource.PlayOneShot(launchSound);

        // Start rocket travel sound
        SimpleRocket rocketScript = loadedRocket.GetComponent<SimpleRocket>();
        if (rocketScript != null)
        {
            rocketScript.StartTravelSound();
        }

        // Apply recoil
        recoilCurrentOffset += recoilKickback;

        // Clear rocket and reload
        loadedRocket = null;
        Invoke(nameof(FinishReload), reloadTime);
    }

    void FinishReload()
    {
        isReloading = false;

        // Play reload sound
        if (reloadSound != null && audioSource != null)
            audioSource.PlayOneShot(reloadSound);

        // Instantiate new rocket visual
        if (rocketVisualPrefab != null && rocketHolder != null)
        {
            GameObject newRocket = Instantiate(rocketVisualPrefab, rocketHolder.position, rocketHolder.rotation, rocketHolder);
            loadedRocket = newRocket;
        }

        Debug.Log("RPG Reloaded");
    }
}

