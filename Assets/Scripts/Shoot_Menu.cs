using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Menu : MonoBehaviour
{
    public Transform firepoint;
    public GameObject bulletPrefab;
    public float fireRate = 0.3f;

    private float fireCooldown = 0f;

    // Update is called once per frame
    void Update()
    {
        bool fireLeft = Input.GetButton("Fire1");
        bool fireRight = Input.GetButton("Fire2");

        if (fireCooldown <= 0.0f && (fireLeft || fireRight))
        {
            fireCooldown = fireRate;

            Quaternion direction = new Quaternion(0, 0, 0, 0);

            if (fireRight)
            {
                direction = new Quaternion(0, 0, 0, 0);
            } else if (fireLeft)
            {
                direction = new Quaternion(0, 0, 180, 0);
            }

            shoot(direction);
        }
        else if (fireCooldown > 0.0f)
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    private void shoot(Quaternion direction)
    {
        // TODO: Influence direction based on firepoint.rotation
        Instantiate(bulletPrefab, firepoint.position, direction);
    }
}
