using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;

    // Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    // Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    // Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    // Projectile properties
    public float minVelocity = 10f;
    public float maxVelocity = 100f;
    public float currentVelocity = 30f;

    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float currentAngle = 0f;

    public float minSize = 0.1f;
    public float maxSize = 4f;
    public float currentSize = 0.3f;

    // Shooting modes
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    // Adjusting shooting delay limits
    public float minShootingDelay = 0.05f; // Minimum shooting delay
    public float maxShootingDelay = 3f;   // Maximum shooting delay

    // UI elements for displaying keybinds and projectile properties
    public TextMeshProUGUI shootingDelayText;
    public TextMeshProUGUI velocityText;
    public TextMeshProUGUI angleText;
    public TextMeshProUGUI sizeText;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;

        // Initialize UI texts
        UpdateUI();
    }

    void Update()
    {
        HandleShootingInput();
        HandleProjectileAdjustments();
        UpdateWeaponAngle();

        // Adjust shooting delay with Z and C keys
        if (Input.GetKeyDown(KeyCode.Z))
        {
            shootingDelay = Mathf.Clamp(shootingDelay - 0.1f, minShootingDelay, maxShootingDelay);
            UpdateUI();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            shootingDelay = Mathf.Clamp(shootingDelay + 0.1f, minShootingDelay, maxShootingDelay);
            UpdateUI();
        }
    }

    private void HandleShootingInput()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single ||
                 currentShootingMode == ShootingMode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    // Adjusting Projectile Size, Velocity, and Angle using scroll and keys
    private void HandleProjectileAdjustments()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0 && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            currentVelocity = Mathf.Clamp(currentVelocity + scrollDelta * 10f, minVelocity, maxVelocity);
        }

        if (scrollDelta != 0 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            currentSize = Mathf.Clamp(currentSize + scrollDelta * 0.1f, minSize, maxSize);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            currentAngle = Mathf.Clamp(currentAngle + 30f * Time.deltaTime, minAngle, maxAngle);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            currentAngle = Mathf.Clamp(currentAngle - 30f * Time.deltaTime, minAngle, maxAngle);
        }

        // Update UI whenever adjustments happen
        UpdateUI();
    }

    private void FireWeapon()
    {
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirection().normalized;
        shootingDirection = Quaternion.Euler(currentAngle, 0, 0) * shootingDirection;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.localScale = Vector3.one * currentSize;
        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * currentVelocity, ForceMode.Impulse);

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirection()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;
        return direction;
    }

    private void UpdateWeaponAngle()
    {
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }

    private void UpdateUI()
    {
        if (shootingDelayText != null)
            shootingDelayText.text = $"Delay: {shootingDelay:F1}s";
        
        if (velocityText != null)
            velocityText.text = $"Velocity: {currentVelocity:F1}";
        
        if (angleText != null)
            angleText.text = $"Angle: {currentAngle:F1}°";
        
        if (sizeText != null)
            sizeText.text = $"Size: {currentSize:F1}";
    }
}