using System;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    // References
    [SerializeField] private Transform[] weapons;
    [SerializeField] private GameObject weaponHUD; // The UI container for the weapon-specific UI

    // Keys
    [SerializeField] private KeyCode[] keys;

    // Settings
    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSinceLastSwitch;

    private void Start()
    {
        SetWeapons();
        timeSinceLastSwitch = 0f;

        // Initially select the correct weapon
        Select(selectedWeapon);

        // Initially show the HUD for the selected weapon
        UpdateWeaponUI(selectedWeapon);
    }

    private void SetWeapons()
    {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i);
        }

        if (keys == null || keys.Length != weapons.Length)
        {
            keys = new KeyCode[weapons.Length];
        }
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        // Loop through all keys to check if one is pressed and switch the weapon
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime)
            {
                selectedWeapon = i;
            }
        }

        // Only change weapon if a key was pressed
        if (previousSelectedWeapon != selectedWeapon)
        {
            Select(selectedWeapon);
        }

        timeSinceLastSwitch += Time.deltaTime;
    }

    private void Select(int weaponIndex)
    {
        // Activate only the selected weapon and deactivate others
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == weaponIndex);
        }

        timeSinceLastSwitch = 0f;
        OnWeaponSelected(weaponIndex);
    }

    private void OnWeaponSelected(int weaponIndex)
    {
        // Debug log for weapon change
        Debug.Log("Weapon Changed: " + weapons[weaponIndex].name);

        // Show the UI for the selected weapon (for example, weapon with index 0)
        UpdateWeaponUI(weaponIndex);
    }

    // Show the UI for the selected weapon
    private void UpdateWeaponUI(int weaponIndex)
    {
        // Initially hide the HUD
        if (weaponHUD != null)
        {
            if (weaponIndex == 0) // Adjust the index based on your weapon setup
            {
                weaponHUD.SetActive(true); // Show UI when holding this weapon
            }
            else
            {
                weaponHUD.SetActive(false); // Hide UI when not holding this weapon
            }
        }
    }
}
