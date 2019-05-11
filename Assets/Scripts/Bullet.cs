using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15.0f;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    public EnemySpawner spawner;

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {

        Debug.Log(hitInfo.name);
        PlayerChar player = hitInfo.GetComponent<PlayerChar>();

        if (player == null)
        {
            if (hitInfo.name == "Enemy(Clone)")
            {
                Debug.Log("Hit!");
                spawner.onKill();
            }
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
