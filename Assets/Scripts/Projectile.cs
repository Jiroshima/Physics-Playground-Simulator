using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollissionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name + " !");
        }
    }
}
