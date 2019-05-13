using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Movement : MonoBehaviour
{
    public float flySpeed = 10.0f;
    private GameObject player;

    private Game_Controller game;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (!player)
        {
            Destroy();
        }

        game = GameObject.Find("Game_Controller").GetComponent<Game_Controller>();

    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        float destX = player.transform.position.x;
        float destY = player.transform.position.y;
        
        Vector2 flyDirection = new Vector2(destX - transform.position.x, destY - transform.position.y);
        flyDirection.Normalize();

        flyDirection *= delta * flySpeed;

        GetComponent<Rigidbody2D>().transform.eulerAngles = new Vector3(0.0f, (flyDirection.x > 0) ? 0.0f : 180.0f, 0.0f);
        
        GetComponent<Rigidbody2D>().velocity = flyDirection;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);

        if (hitInfo.name == "projectile(Clone)")
        {
            Destroy();
        }

        if (hitInfo.name == "player")
        {
            game.PlayerDied();
        }
    }

    void Destroy()
    {
        game.EnemyKilled();
        Destroy(gameObject);
    }
}
