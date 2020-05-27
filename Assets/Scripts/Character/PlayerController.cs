using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    
    [Header("移动参数")]
    private Rigidbody2D rb; 
    private float xVelocity;
    public int speed;
    public float slideSpeed;
    public bool canMove;

    [Header("跳跃参数")]
    public float yVelocity;
    public int jumpForce;
    public LayerMask groundLayer;
    public Vector2 wallJumpForce;

    [Header("按键检测")]
    public bool jumpPress;
    public bool catchPress;

    

    [Header("环境检测")]
    public Collision coll;
    public float footOffset;
    public float groundDistansce ;


    

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space ))
        {
            jumpPress = true;
        }
        catchPress = Input.GetKey(KeyCode.J);
    }

    private void FixedUpdate()
    {
        PhysicsCheck();
        GroundMovement();
        Airmovement();
        yVelocity = rb.velocity.y;
        jumpPress = false;
       
        //PoolManager.Instance.GetObj("Shadow");
    }

    void GroundMovement()
    {
        if (!canMove)
            return;
        xVelocity = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(speed * xVelocity, rb.velocity.y);

    }

    void Airmovement()
    {
        if(jumpPress && coll.isOnGround)
        {
            Jump(Vector2.up);
        }
            

        if(coll.isOnWall && !coll.isOnGround)
        {
            if(xVelocity!=0)
            {
                if (Input.GetKey(KeyCode.J))
                {
                    rb.bodyType = RigidbodyType2D.Static;
                }
                else
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    WallSide();

                }
                    
            }
            
            
        }
        if (coll.isOnWall && !coll.isOnGround && jumpPress)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            WallJump();
        }
         
        
        
       
    }

    

    void WallJump()
    {
        StartCoroutine(DisableMovement(0.2f));

        Vector2 wallDir = coll.isOnRightWall ? Vector2.left : Vector2.right;

        Jump(Vector2.up / 1.5f + wallDir / 1.5f);
    }

    void WallSide()
    {
        if (!canMove)
            return;
        bool pushingWall = false;
        if((rb.velocity.x > 0 && coll.isOnRightWall) || (rb.velocity.x < 0 && coll.isOnLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
    /*void WallJump()
    {
        if (!coll.isOnGround && coll.isOnWall)
        {
            if (Input.GetKey(KeyCode.J))
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
            else
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                bool pushingWall = false;
                if((rb.velocity.x >= 0 && coll.isOnRightWall) || (rb.velocity.x <= 0 && coll.isOnLeftWall))
                {
                    pushingWall = true;
                }
                else
                {
                    Debug.Log("SSSS");
                }
                float push = pushingWall ? 0 : rb.velocity.x;

                rb.velocity = new Vector2(push, -slideSpeed);
            }
            if(jumpPress && coll.isOnWall && !coll.isOnGround)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                Vector2 wallDir = coll.isOnRightWall ? Vector2.left : Vector2.right;
                Jump(Vector2.up / 1.5f + wallDir / 1.5f);
            }
        }
     
    }
    */

    void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);//抵消引力
        rb.velocity += dir * jumpForce;
    }

    void PhysicsCheck()
    {       
     
    }

    void FlipDirction()
    {
        if (xVelocity < 0)
        {
            transform.localScale = new Vector2(-30, 30);
        }
        else if (xVelocity > 0)
        {
            transform.localScale = new Vector2(30, 30);
        }
    }

}
