using UnityEngine;

public class WeaponRandomSwitcher : MonoBehaviour
{
    public Transform weaponHolder;         // Parent of all weapon objects
    public float switchInterval = 7f;

    private float timer = 0f;
    private int currentWeaponIndex = -1;

    void Start()
    {
        SwitchToRandomWeapon(); // Start with one random weapon
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            timer = 0f;
            SwitchToRandomWeapon();
        }
    }

    void SwitchToRandomWeapon()
    {
        if (weaponHolder.childCount == 0) return;

        // Random index different from current (optional, prevents instant repeat)
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, weaponHolder.childCount);
        }
        while (randomIndex == currentWeaponIndex && weaponHolder.childCount > 1);

        // Activate one weapon, disable others
        for (int i = 0; i < weaponHolder.childCount; i++)
        {
            weaponHolder.GetChild(i).gameObject.SetActive(i == randomIndex);
        }

        currentWeaponIndex = randomIndex;

        Debug.Log("🔀 Switched to: " + weaponHolder.GetChild(randomIndex).name);
    }
}
