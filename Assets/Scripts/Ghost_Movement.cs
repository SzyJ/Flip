using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Movement : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite warnSprite;
    public Sprite attackSprite;

    public GameObject attackEffect;

    public float flySpeed = 10.0f;

    public float warningTime = 1.0f;
    public float attackTime = 1.0f;

    public float attackDistance = 2.0f;

    public float attackDelay = 5.0f;
    public float delayTimer = 0.0f;

    private GameObject player;

    private Game_Controller game;
    private SpriteRenderer renderer;

    private bool attacking = false;
    private bool attackDone = false;
    private bool soundPlayed = false;
    private float attackingTime;

    public AudioSource deathSound;
    public AudioSource attackSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (!player)
        {
            Destroy(gameObject);
        }

        game = GameObject.Find("Game_Controller").GetComponent<Game_Controller>();
        this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        
        float delta = Time.deltaTime;

        float destX = player.transform.position.x;
        float destY = player.transform.position.y;

        Vector2 flyDirection = new Vector2(destX - transform.position.x, destY - transform.position.y);

        if (attacking)
        {
            inAttack(delta, flyDirection.magnitude);
            GetComponent<Rigidbody2D>().transform.eulerAngles = new Vector3(0.0f, (flyDirection.x > 0) ? 0.0f : 180.0f, Mathf.Sin(attackingTime * 20.0f) * 10.0f);
            return;
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
        attackingTime = 0.0f;

        if (delayTimer > 0.0f)
        {
            delayTimer -= delta;
        }

        attacking = delayTimer <= 0.0f && (flyDirection.magnitude < attackDistance);
    
        //flyDirection.Normalize();
        flyDirection *= delta * flySpeed;
        GetComponent<Rigidbody2D>().velocity = flyDirection;

        GetComponent<Rigidbody2D>().transform.eulerAngles = new Vector3(0.0f, (flyDirection.x > 0) ? 0.0f : 180.0f, 0.0f);
        
    }

    void inAttack(float delta, float playerDist)
    {
        attackingTime += delta;
        
        if (!soundPlayed)
        {
            attackSound.Play();
            soundPlayed = true;
        }

        if (attackingTime < warningTime)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = warnSprite;
        }
        else if (attackingTime < warningTime + attackTime) 
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = attackSprite;
            if (!attackDone)
            {
                Instantiate(attackEffect, transform.position, transform.rotation);
                attackDone = true;
            }

            if (playerDist < 1.0f &&
                attackingTime < warningTime + 0.02f)
            {
                game.PlayerDied();
            }
            
        }
        else
        {
            attackDone = false;
            attacking = false;
            soundPlayed = false;
            attackingTime = 0.0f;
            delayTimer = attackDelay;
        }

    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);

        if (hitInfo.name == "projectile(Clone)")
        {
            Kill();
            return;
        }
    }

    void Kill()
    {

        Debug.Log("Ghost Killed!!!!");
        deathSound.Play();
        game.EnemyKilled();
        Destroy(gameObject);
    }
}
