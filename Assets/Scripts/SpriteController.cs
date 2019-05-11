using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpriteController : NetworkBehaviour
{
    public Sprite player;
    public Sprite shoot_player;
    public Sprite enemy;
    public Sprite shoot_enemy;

    public float shootTime = 150.0f;

    private float currentShootTime = 0.0f;

    private bool isPlayer = false;
    private bool isShooting = false;
    // Start is called before the first frame update
    void Start()
    {
        isPlayer = isLocalPlayer;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = isPlayer ? player : enemy;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            currentShootTime += Time.deltaTime;

            if (currentShootTime > shootTime)
            {
                isShooting = false;
                currentShootTime = 0.0f;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = isPlayer ? player : enemy;
            }
        }


    }

    public void onShoot()
    {
        isShooting = true;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = isPlayer ? shoot_player : shoot_enemy;
    }
}
