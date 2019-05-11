using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Movement : MonoBehaviour
{
    private const float PI = 3.141592f;
    private const float COLL_CHECK_TOL = 0.2f;

    [SerializeField] private LayerMask m_CollisionMask;

    [SerializeField] private Transform m_Ground_Check;

    public float maxSpeed = 7;
    public float accel = 3.0f;
    public float slowDown = 0.6f;
    public float jumpSpeed = 5;
    public float jumpDuration = 150;
    public float airControl = 1.0f;
    public bool enableDoubleJump = true;
    public bool wallHitJumpReset = true;
    public float baseRotationSpeed = 1.0f;
    public float rotationSpeed = 650.0f;
    public float rotationAmlipfier = 2.0f;

    public float lowJumpMultiplyer = 2.0f;

    bool canDoubleJump = true;
    float currentJumpDuration;
    bool jumpKeyDown = false;
    bool canVariableJump = false;


    private float accelScale = 0;
    private bool facingRight = true;

    private float currentRotation = 0.0f;

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float horizontal = Input.GetAxis("Horizontal");
        float xVel = rb.velocity.x;
        float yVel = rb.velocity.y;

        float newXVel = 0.0f;

        float delta = Time.deltaTime;

        bool onTheGround = isOnGround();

        bool leftWallHit = isOnWallLeft();
        bool rightWallHit = isOnWallRight();

        if (horizontal > 0.01f || horizontal < -0.01f)
        {
            float accelChange = horizontal * accel * delta;
            accelScale += (onTheGround) ? accelChange : accelChange * airControl;

            if (accelScale > 0.5f * PI)
            {
                accelScale = 0.5f * PI;
            }
            else if (accelScale < -0.5f * PI)
            {
                accelScale = -0.5f * PI;
            }
        }
        else
        {
            if (onTheGround)
            {
                if (accelScale > -0.005f && accelScale < 0.005f)
                {
                    accelScale = 0.0f;
                }

                accelScale *= slowDown * delta;
            }
        }
        
        if ((leftWallHit && accelScale < 0.0f) || (rightWallHit && accelScale > 0.0f))
        {
            accelScale = 0.0f;
        }

        newXVel = Mathf.Sin(accelScale) * maxSpeed;
        rb.velocity = new Vector2(newXVel, yVel);

        float vertical = Input.GetAxis("Vertical");

        if (onTheGround)
        {
            currentRotation = 0.0f;
            resetRotation(rb);
            canDoubleJump = true;
        } else
        {

            float rotationDelta = ((!canDoubleJump && enableDoubleJump) ? rotationSpeed * rotationAmlipfier : rotationSpeed) * delta;

            currentRotation += rotationDelta;
            if (leftWallHit || rightWallHit || currentRotation >= 360.0f)
            {
                resetRotation(rb);
            } else
            {
                transform.Rotate(0.0f, 0.0f, -rotationDelta);
            }
        }

        if ((facingRight && horizontal < 0.0f) || (!facingRight && horizontal > 0.0f))
        {
            flip();
        }

        if (vertical > 0.1f)
        {
            if (!jumpKeyDown) // First Frame
            {
                jumpKeyDown = true;

                //rotationSpeed = baseRotationSpeed;

                bool wallHit = false;
                int wallHitDirection = 0;

                if (horizontal != 0)
                {
                    if (leftWallHit)
                    {
                        if (accelScale < 0.0f)
                        {
                            accelScale = 0.25f * PI;
                        }

                        if (!facingRight)
                        {
                            flip();
                        }
                        wallHit = true;
                        wallHitDirection = 1;
                    }
                    else if (rightWallHit)
                    {
                        if (accelScale > 0.0f)
                        {
                            accelScale = -0.25f * PI;
                        }

                        if (facingRight)
                        {
                            flip();
                        }
                        wallHit = true;
                        wallHitDirection = -1;
                    }


                    if (wallHit && wallHitJumpReset)
                    {
                        currentRotation = 0.0f;
                        canDoubleJump = true;
                    }

                }

                if (!wallHit)
                {
                    if (onTheGround || (canDoubleJump && enableDoubleJump))
                    {
                        rb.velocity = new Vector2(newXVel, this.jumpSpeed);

                        currentJumpDuration = 0.0f;
                        canVariableJump = true;

                        currentRotation = 0.0f;
                    }
                }
                else
                {
                    rb.velocity = new Vector2(this.jumpSpeed * wallHitDirection, this.jumpSpeed);
                    accelScale = wallHitDirection * 0.25f * PI;

                    currentJumpDuration = 0.0f;
                    canVariableJump = true;
                }

                if (!onTheGround && !wallHit)
                {
                    canDoubleJump = false;
                }

            } // Second Frame
            else if (canVariableJump)
            {
                currentJumpDuration += delta;

                if (jumpKeyDown && (currentJumpDuration < this.jumpDuration / 1000))
                {
                    rb.velocity = new Vector2(newXVel, this.jumpSpeed);
                }
            }
        }
        else
        {
            rb.AddForce(new Vector2(0.0f, -lowJumpMultiplyer));
            jumpKeyDown = false;
            canVariableJump = false;
        }
    }

    private bool isOnGround()
    {
        return checkPointCollision(m_Ground_Check.position, m_CollisionMask);
    }

    private bool isOnWallLeft()
    {
        bool retVal = false;
        
        float lengthToSearch = 0.1f;
        float colliderThreshold = 0.01f;
        
        Vector2 lineStart = new Vector2(this.transform.position.x - this.GetComponent<Renderer>().bounds.extents.x - colliderThreshold, this.transform.position.y);
        Vector2 vectorToSearch = new Vector2(lineStart.x - lengthToSearch, this.transform.position.y);
        
        RaycastHit2D hitLeft = Physics2D.Linecast(lineStart, vectorToSearch);
        retVal = hitLeft;
        
        return retVal;
    }


    private bool isOnWallRight()
    {
        bool retVal = false;
        
        float lengthToSearch = 0.1f;
        float colliderThreshold = 0.01f;
        
        Vector2 lineStart = new Vector2(this.transform.position.x + this.GetComponent<Renderer>().bounds.extents.x + colliderThreshold, this.transform.position.y);
        Vector2 vectorToSearch = new Vector2(lineStart.x + lengthToSearch, this.transform.position.y);
        
        RaycastHit2D hitRight = Physics2D.Linecast(lineStart, vectorToSearch);
        retVal = hitRight;
        
        return retVal;
    }

    private bool checkPointCollision(Vector3 pos, LayerMask colMask)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, COLL_CHECK_TOL, colMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private void flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void resetRotation(Rigidbody2D rb)
    {
        rb.transform.eulerAngles = new Vector3(0.0f, facingRight ? 0.0f : 180.0f, 0.0f);
    }
}
