using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform firepoint;
    public GameObject bulletPrefab;
    public float fireRate = 0.3f;

    private float fireCooldown = 0f;
    
    // Update is called once per frame
    void Update()
    {
        if (fireCooldown <= 0.0f && Input.GetButton("Fire1"))
        {
            fireCooldown = fireRate;
            CmdShoot();
        }
        else if (fireCooldown > 0.0f)
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    private void CmdShoot()
    {
        GameObject projectile = (GameObject) Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
    }
}
