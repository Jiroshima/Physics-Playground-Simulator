using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    // References
    [SerializeField] private Transform[] weapons;
    [SerializeField] private GameObject[] weaponHUDs; // An array for multiple weapon HUDs

    // Keys
    [SerializeField] private KeyCode[] keys;

    // Settings
    [SerializeField] private float switchTime = 0.5f;  // Minimum switch time to prevent spamming

    private int selectedWeapon = 0;  // Start with the first weapon selected
    private float timeSinceLastSwitch = 0f;

    private void Start()
    {
        if (weapons.Length == 0 || weaponHUDs.Length == 0) return; // Early exit if setup is invalid

        SetWeapons();
        Select(selectedWeapon);  // Initial weapon selection
    }

    private void SetWeapons()
    {
        // Ensure that the size of the keys array matches the number of weapons
        if (keys.Length != weapons.Length)
        {
            keys = new KeyCode[weapons.Length];
        }

        // Initialize the weapons and HUDs
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);  // Disable all weapons initially
            if (i < weaponHUDs.Length)
            {
                weaponHUDs[i].SetActive(false);  // Disable all HUDs initially
            }
        }
    }

    private void Update()
    {
        timeSinceLastSwitch += Time.deltaTime;  // Track time since the last switch

        // Loop through the keys to check if one is pressed and switch the weapon
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime)
            {
                selectedWeapon = i;  // Switch to the pressed weapon
                timeSinceLastSwitch = 0f;  // Reset switch timer
            }
        }

        // Update weapon if needed
        Select(selectedWeapon);
    }

    private void Select(int weaponIndex)
    {
        // Only change if the weapon index has actually changed
        if (weapons[weaponIndex].gameObject.activeSelf) return;

        // Deactivate all weapons and HUDs first
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
            if (i < weaponHUDs.Length)
            {
                weaponHUDs[i].SetActive(false);
            }
        }

        // Activate the selected weapon and its HUD
        weapons[weaponIndex].gameObject.SetActive(true);
        if (weaponIndex < weaponHUDs.Length)
        {
            weaponHUDs[weaponIndex].SetActive(true);  // Show the correct HUD
        }
    }
}
