using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // shooting modes
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
    }

    void Update()
    {
        // Checks if shooting mode is auto, then shooting mode is only true when LMB is held down. 
        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        // If shooting mode is single/burst then shooting mode is only true when LMB is clicked once. 
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
    
    private void FireWeapon()
    {
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirection().normalized;

        // Instantiate an actual object bullet instead of raycast, positioned at bulletSpawn and give it a default rotation.
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        // Points bullet at shooting direction
        bullet.transform.forward = shootingDirection;



        //applies force to the bullet by grabbing the rigidbody and adding a force. Multiplying by bullet velocity and setting force mode to impulse
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Burst Mode to prevent bullet stacking 
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) // we already shoot once before this check 
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
        //shoots a ray from the middle of the screen to check where we are pointing at 
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {   
            // Hitting something
            targetPoint = hit.point;
        }
        else
        {
            // Shooting the air/empty space
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        return direction + new Vector3(0,0, 0);
    }

}

