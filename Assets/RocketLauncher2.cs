using UnityEngine;
using System.Collections.Generic; // Add this for using Lists

public class ROCKETLauncher : MonoBehaviour
{
    [Header("Launcher Setup")]
    public Transform firePoint;
    [Header("Ammo Settings")]
public int maxAmmo = 10;
private int currentAmmo;

    public Transform rocketHolder;
    public GameObject rocketPrefab;
    public List<GameObject> rocketVisualPrefabs; // ⬅️ Now list of multiple heads
    public GameObject loadedRocket;

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

    private int currentRocketIndex = 0; // ⬅️ Track which head to load next

   void Start()
{
    audioSource = GetComponent<AudioSource>();

    if (recoilObject != null)
        recoilOriginalPos = recoilObject.localPosition;

    currentAmmo = maxAmmo; // 🧨 Set current ammo to maximum at start

    ReloadRocket();
}


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isReloading && loadedRocket != null)
        {
            FireLoadedRocket();
        }

        if (recoilObject != null)
        {
            recoilCurrentOffset = Vector3.Lerp(recoilCurrentOffset, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
            recoilObject.localPosition = recoilOriginalPos + recoilCurrentOffset;
        }
    }

    void FireLoadedRocket()
{
    if (currentAmmo <= 0)
    {
        Debug.Log("Out of ammo!");
        return;
    }

    isReloading = true;

    loadedRocket.transform.parent = null;

    Rigidbody rb = loadedRocket.GetComponent<Rigidbody>();
    if (rb == null) rb = loadedRocket.AddComponent<Rigidbody>();

    Collider col = loadedRocket.GetComponent<Collider>();
    if (col == null) col = loadedRocket.AddComponent<CapsuleCollider>();

    rb.linearVelocity = firePoint.forward * launchForce;

    if (launchSound != null && audioSource != null)
        audioSource.PlayOneShot(launchSound);

    SimpleRocket rocketScript = loadedRocket.GetComponent<SimpleRocket>();
    if (rocketScript != null)
    {
        rocketScript.StartTravelSound();
    }

    recoilCurrentOffset += recoilKickback;

    loadedRocket = null;

    currentAmmo--; // 🔥 reduce ammo after firing!

    Invoke(nameof(FinishReload), reloadTime);
}



    void FinishReload()
    {
        isReloading = false;

        if (reloadSound != null && audioSource != null)
            audioSource.PlayOneShot(reloadSound);

        ReloadRocket();
    }

    void ReloadRocket()
    {
        if (rocketVisualPrefabs.Count == 0)
        {
            Debug.LogWarning("No rockets left to load!");
            return;
        }

        // Load next rocket model
        if (currentRocketIndex >= rocketVisualPrefabs.Count)
            currentRocketIndex = 0; // Loop back if needed

        GameObject rocketVisualPrefab = rocketVisualPrefabs[currentRocketIndex];

        if (rocketVisualPrefab != null && rocketHolder != null)
        {
            GameObject newRocket = Instantiate(rocketVisualPrefab, rocketHolder.position, rocketHolder.rotation, rocketHolder);
            loadedRocket = newRocket;
        }

        currentRocketIndex++;
    }
}
