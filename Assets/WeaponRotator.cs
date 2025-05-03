using System.Collections;
using UnityEngine;
using TMPro;

public class WeaponRotator : MonoBehaviour
{
    [Header("Weapon Sets")]
    public Transform[] weaponObjects;
    public Transform[] ultimateWeapons;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI ultimateDurationText;
    public TextMeshProUGUI ultimateReadyText;

    [Header("Player Stats")]
    public PlayerStats playerStats;

    private Transform currentWeapon;
    private bool usingUltimate = false;
    private Coroutine switchRoutine;

    void Start()
    {
        switchRoutine = StartCoroutine(SwitchWeaponTimer());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerStats != null && playerStats.CanUseUltimate() && !usingUltimate)
        {
            ActivateUltimate();
        }
    }

    IEnumerator SwitchWeaponTimer()
    {
        while (true)
        {
            float switchCountdown = Random.Range(7f, 10f);

            while (switchCountdown > 0f)
            {
                GameObject timerObject = GameObject.Find("Canvas/HUD/ScreenSpace/BottomLeft/HUD_ActionBar_WatchRadio01/PlayerLevel/HUD_PlayerLevel/Content/Label_PlayerLevel");
                if (timerObject != null)
                {
                    timerText = timerObject.GetComponent<TextMeshProUGUI>();
                    timerText.text = $"{Mathf.Ceil(switchCountdown)}";
                }

                switchCountdown -= Time.deltaTime;
                yield return null;
            }

            if (usingUltimate)
                SwitchToRandomUltimateWeapon();
            else
                SwitchToRandomWeapon();

            yield return null;
        }
    }

    IEnumerator UltimateDuration()
    {
        float ultimateTimeLeft = 30f;

        while (ultimateTimeLeft > 0f)
        {
            if (ultimateDurationText != null)
                ultimateDurationText.text = $"ULTIMATE ENDS IN: {Mathf.Ceil(Mathf.Max(ultimateTimeLeft, 0))}s";

            ultimateTimeLeft -= Time.deltaTime;
            yield return null;
        }

        EndUltimate();
    }

    void ActivateUltimate()
    {
        usingUltimate = true;

        if (ultimateReadyText != null)
            ultimateReadyText.gameObject.SetActive(false);

        SwitchToRandomUltimateWeapon();

        StartCoroutine(UltimateDuration());
    }

    void EndUltimate()
    {
        usingUltimate = false;

        if (ultimateDurationText != null)
            ultimateDurationText.text = "";

        if (playerStats != null)
            playerStats.ResetPoints();

        Debug.Log("[WeaponRotator] Ultimate ended. Returning to regular weapons.");
        SwitchToRandomWeapon();
    }

    void SwitchToRandomWeapon()
    {
        if (weaponObjects.Length <= 1) return;

        DisableAllWeapons();

        int currentIndex = System.Array.IndexOf(weaponObjects, currentWeapon);
        int newIndex;

        do
        {
            newIndex = Random.Range(0, weaponObjects.Length);
        } while (newIndex == currentIndex && weaponObjects.Length > 1);

        currentWeapon = weaponObjects[newIndex];
        currentWeapon.gameObject.SetActive(true);

        Debug.Log("[WeaponRotator] Switched to: " + currentWeapon.name);
    }

    void SwitchToRandomUltimateWeapon()
    {
        if (ultimateWeapons.Length == 0) return;

        DisableAllWeapons();

        int currentIndex = System.Array.IndexOf(ultimateWeapons, currentWeapon);
        int newIndex;

        do
        {
            newIndex = Random.Range(0, ultimateWeapons.Length);
        } while (newIndex == currentIndex && ultimateWeapons.Length > 1);

        currentWeapon = ultimateWeapons[newIndex];
        currentWeapon.gameObject.SetActive(true);

        Debug.Log("[WeaponRotator] Switched Ultimate to: " + currentWeapon.name);
    }

    void DisableAllWeapons()
    {
        foreach (var w in weaponObjects)
            w.gameObject.SetActive(false);

        foreach (var u in ultimateWeapons)
            u.gameObject.SetActive(false);
    }
}
