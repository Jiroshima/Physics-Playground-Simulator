using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    void Update()
    {
        // check for mouse left click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireWeapon();
        }
    }
    
    private void FireWeapon()
    {
        // Instantiate an actual object bullet instead of raycast, positioned at bulletSpawn and give it a default rotation.
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        //applies force to the bullet by grabbing the rigidbody and adding a force. Multiplying by bullet velocity and setting force mode to impulse
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
    }

}

