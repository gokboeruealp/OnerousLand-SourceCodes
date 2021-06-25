using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectile;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject newProjectile = Instantiate(projectile, gameObject.transform.position, gameObject.transform.rotation);

            Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
            rb.AddForce(newProjectile.transform.up * 15f, ForceMode2D.Impulse);
        }
    }
}
