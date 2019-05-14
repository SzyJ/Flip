using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Movement : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite warnSprite;
    public Sprite attackSprite;


    public float flySpeed = 10.0f;

    public float warningTime = 1.0f;
    public float attackTime = 1.0f;

    public float attackDistance = 0.7f;

    public float attackDelay = 5.0f;
    public float delayTimer = 0.0f;

    private GameObject player;

    private Game_Controller game;
    private SpriteRenderer renderer;

    private bool attacking = false;
    private float attackingTime;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (!player)
        {
            Destroy();
        }

        game = GameObject.Find("Game_Controller").GetComponent<Game_Controller>();
        this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
        
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;

        float destX = player.transform.position.x;
        float destY = player.transform.position.y;

        Vector2 flyDirection = new Vector2(destX - transform.position.x, destY - transform.position.y);

        if (delayTimer <= 0.0f || attacking)
        {
            attacking = (flyDirection.magnitude < attackDistance);
        }
        else
        {
            attacking = false;
            delayTimer -= delta;
        }
        

        if (!attacking)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
            attackingTime = 0.0f;
      
            //flyDirection.Normalize();
            flyDirection *= delta * flySpeed;
            
            GetComponent<Rigidbody2D>().velocity = flyDirection;
        }
        else
        {
            inAttack(delta);
        }

        GetComponent<Rigidbody2D>().transform.eulerAngles = new Vector3(0.0f, (flyDirection.x > 0) ? 0.0f : 180.0f, Mathf.Sin(attackingTime * 20.0f) * 10.0f);
        
    }

    void inAttack(float delta)
    {
        attackingTime += delta;
        
        if (attackingTime < warningTime)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = warnSprite;
        }
        else if (attackingTime < warningTime + attackTime) 
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attackSprite;
        }
        else
        {
            attackingTime = 0.0f;
            delayTimer = attackDelay;
        }

    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);

        if (hitInfo.name == "projectile(Clone)")
        {
            Destroy();
            return;
        }

        PlayerChar player = hitInfo.GetComponent<PlayerChar>();
        if (player != null)
        {
            Debug.Log("Player Killed!");
            game.PlayerDied();
        }
    }

    void Destroy()
    {
        game.EnemyKilled();
        Destroy(gameObject);
    }
}
