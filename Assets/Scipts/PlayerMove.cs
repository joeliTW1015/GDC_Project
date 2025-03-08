using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;
    
    [Header("Move")] 
    [SerializeField] float maxspeed; 
    [SerializeField] float acceleration, decceleration, velPow,friction;
    Vector2 moveForceDir;
    float faceDir;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float coyoteTime, jumpBufferTime, jumpCutValue , gravityScaleMultiplier;
    [SerializeField] int maxMultiJump;
    int jumpCount;
    
    public float gravityScaleValue;
    
    [Header("Slope")]
    [SerializeField] float slopeCheckDistance;
    [SerializeField] float slopeSpeedDecrease;

    [Header("Dash")]
    [SerializeField] float normalDashForce;
    [SerializeField] float superDashForce, dashCD, dashGap; //Gap is the time of input deprivation
    bool isDashing;
    bool canDash;


    bool isOnSlope;
    [Header("PhysicsMaterial")]

    [SerializeField] PhysicsMaterial2D slippyMaterial, frictionMaterial;


    [HideInInspector] public Rigidbody2D rb;
    BoxCollider2D playerCollider;
    [HideInInspector] public bool jumpCmd, isOnGround;
    float xIn;
    float lastGroundTime, lastJumpTime;



    // Start is called before the first frame update
    void Start()
    {
        jumpCount = 0;
        isDashing = false;
        canDash = true;
        faceDir = 1;
        isOnGround = true;
        jumpCmd = false;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        moveForceDir = Vector2.right;
        rb.sharedMaterial = slippyMaterial;
        isOnSlope = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDashing)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash(MusicSyncManager.onTime));
        }

        xIn = Input.GetAxis("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("jumpCMD");
            jumpCmd = true;
        }
        
        
    }

    void Move()
    {
        if(isDashing)
            return;

        float targetSpeed = isOnSlope? xIn * maxspeed * slopeSpeedDecrease : xIn * maxspeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) >= 0.01f)? acceleration : decceleration;
        float moveForce = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPow) * Mathf.Sign(speedDif);
        
        if(targetSpeed == 0 && rb.linearVelocity.x < 0.01f)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
           //Debug.Log("set0");
        }
        else
        {
            rb.AddForce(moveForce * moveForceDir.normalized);   
            // Debug.Log(moveForce);  
        } 

        //flip
        if(xIn != 0)
        {
            faceDir = xIn < 0 ? -1 : 1;
            if(xIn > 0)
                this.transform.localScale = new Vector3(  1 , transform.localScale.y, 1);
            else
                this.transform.localScale = new Vector3(  -1 , transform.localScale.y, 1);
        }

        //friction

        if(isOnGround && xIn == 0)
        {
            float frictionForce = Mathf.Min(Mathf.Abs(rb.linearVelocity.magnitude), friction);
            frictionForce *= Mathf.Sign(rb.linearVelocity.x);
            rb.AddForce(moveForceDir * -frictionForce, ForceMode2D.Impulse); 
        }

    }
    void Jump()
    {
        if(jumpCmd)
        {
            jumpCmd = false;
            lastJumpTime = jumpBufferTime;
        }
        else
        {
            lastJumpTime -= Time.deltaTime;   
        }

        if(isOnGround)
        {
            jumpCount = 0;
        }

        if((lastGroundTime > 0 && lastJumpTime > 0 )||(MusicSyncManager.onTime && jumpCount < maxMultiJump && lastJumpTime > 0))
        {
            jumpCount += 1;
            Debug.Log("Jump");
            lastJumpTime = 0;
            lastGroundTime = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        

        //JumpCut
        if(rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(jumpCutValue * Vector2.down, ForceMode2D.Impulse);
        }
        //FallGravity
        if(rb.linearVelocity.y < 0 && !isOnGround && !isDashing)
        {
            rb.gravityScale = gravityScaleValue * gravityScaleMultiplier;
        }
        else 
        {
            rb.gravityScale = gravityScaleValue;
        }
    }

    bool CheckGroundFun()
    {
        RaycastHit2D hit = Physics2D.CapsuleCast(playerCollider.bounds.center, playerCollider.size, CapsuleDirection2D.Vertical, 0, Vector2.down, 0.05f, groundMask);
        
        if(hit.collider != null)
        {
            //Debug.Log("onGround!");
            lastGroundTime = coyoteTime;
            return true;
        }
        else
        {
            lastGroundTime -= Time.deltaTime;
            return false;
        }
    }

    void SlopeCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, playerCollider.size.y / 2, 0), Vector2.down, slopeCheckDistance, groundMask);
        if(hit.collider != null)
        {

            if(Vector2.Angle(hit.normal, Vector2.up) > 10f  && Vector2.Angle(hit.normal, Vector2.up) < 60 && isOnGround)
            {
                moveForceDir = Vector2.Perpendicular(hit.normal) * -1;
                isOnSlope = true;
                
                if(xIn == 0)
                    rb.sharedMaterial = frictionMaterial;   
                else
                    rb.sharedMaterial = slippyMaterial;
            }
            else
            {
                moveForceDir = Vector2.right;
                rb.sharedMaterial = slippyMaterial;
                isOnSlope = false;
            }
                

            Debug.DrawRay(hit.point, hit.normal, Color.red);
            // Debug.Log(Vector2.Angle(hit.normal, Vector2.up));
        }
        else
        {
            moveForceDir = Vector2.right;
            rb.sharedMaterial = slippyMaterial;
            isOnSlope = false;
        }
    }

    void FixedUpdate() 
    {
        isOnGround = CheckGroundFun();
        Move();
        Jump();
    }

    IEnumerator Dash(bool onBeat)
    {
        canDash = false;
        isDashing = true;
        rb.linearVelocity = Vector2.right * faceDir * (onBeat ? superDashForce : normalDashForce);
        yield return new WaitForSeconds(dashGap);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        isDashing = false;

        if(!onBeat)
        {
            yield return new WaitForSeconds(dashCD);
        }
        canDash = true;
    }
    private void OnDisable() 
    {
        
        rb.linearVelocity = Vector2.zero;
    }
}
