using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{


    [Header("移动参数")]
    private Rigidbody2D rb;
    private float x;
    private float y;
    private float xRaw;
    private float yRaw;


    public float wallJumpLerp;//冲刺速度向正常速度转化
    public int speed;//地面移动速度
    public float airSpeed;//空中移动速度
    public float slideSpeed;//下滑速度   
    public float dashSpeed = 20;//冲锋速度

    public bool canMove;//墙跳和冲刺期间不可移动
    public bool isDashing;//正在冲锋
    public bool hasDashed;//限制二次冲锋
    public bool wallJumped;

    [Header("跳跃参数")]
    public float yVelocity;
    public int jumpForce;
    public LayerMask groundLayer;//平台检测
    public Vector2 wallJumpForce;

    [Header("按键检测")]
    public bool jumpPress;
    public bool catchPress;
    public bool dashPress;
    public bool jumpDownPress;


    [Header("环境检测")]
    public Collision coll;
    public float footOffset;
    public float groundDistansce;

    private bool groundTouch;//限制二次冲锋

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();


       
       
        //监听事件添加
        EventCenter.Instance.AddEventListener("PlayerDead",PlayerDead);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPress = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            dashPress = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            jumpDownPress = true;
        }
        catchPress = Input.GetKey(KeyCode.J);
    }

    private void FixedUpdate()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");

        PhysicsCheck();
        GroundMovement();
        Airmovement();
        yVelocity = rb.velocity.y;
        jumpPress = false;
        dashPress = false;
        jumpDownPress = false;



        if (isDashing)//冲锋残影
            PoolManager.Instance.GetObj("Shadow");
    }




    void GroundMovement()//地面移动**空中移动**冲刺后的速度转化**跳下平台
    {

        if (!canMove)
            return;
        if (!wallJumped)
        {
            if (coll.isOnGround)
                rb.velocity = new Vector2(speed * x, rb.velocity.y);
            else
                rb.velocity = new Vector2(airSpeed * x, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(x * speed, 0)), wallJumpLerp * Time.fixedDeltaTime);
        }
        if (jumpDownPress && coll.groundColl.gameObject.tag == "Platform")//跳下平台（魂斗罗）
        {
            coll.OpenPlatColl();
        }

    }

    void Airmovement()//跳跃**墙跳**挂墙**冲锋
    {
        if (jumpPress && coll.isOnGround)
        {
            Jump(Vector2.up);
        }


        if (coll.isOnWall && !coll.isOnGround)
        {
            if (x != 0)
            {
                if (Input.GetKey(KeyCode.J))
                {
                    //rb.bodyType = RigidbodyType2D.Static;
                }
                else
                {
                    //rb.bodyType = RigidbodyType2D.Dynamic;
                    WallSide();

                }

            }
        }
        if (coll.isOnWall && !coll.isOnGround && jumpPress)
        {
            //rb.bodyType = RigidbodyType2D.Dynamic;
            WallJump();
        }

        if (dashPress && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
        }



    }

    void Dash(float x, float y)
    {
        hasDashed = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);
        rb.velocity += dir.normalized * dashSpeed;

        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()//设置冲刺重力
    {
        StartCoroutine(GroundDash());

        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;
        yield return new WaitForSeconds(.1f);

        //set gravity
        rb.gravityScale = 5;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;


    }

    IEnumerator GroundDash()//地面多次冲刺
    {
        yield return new WaitForSeconds(.25f);
        if (coll.isOnGround)
            hasDashed = false;
    }

    void WallJump()
    {
        StartCoroutine(DisableMovement(0.2f));

        Vector2 wallDir = coll.isOnRightWall ? Vector2.left : Vector2.right;

        Jump(Vector2.up / 1.5f + wallDir / 1.5f);
        wallJumped = true;

        GetComponent<BetterJumping>().enabled = true;

        hasDashed = false; //贴墙跳一次后可以冲刺
    }

    void WallSide()//贴墙下滑
    {
        if (!canMove)
            return;
        bool pushingWall = false;
        if ((rb.velocity.x > 0 && coll.isOnRightWall) || (rb.velocity.x < 0 && coll.isOnLeftWall))
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
        if (coll.isOnWall)
        {
            //hasDashed = false;    //是否开启贴墙冲刺
        }
        if (coll.isOnGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!coll.isOnGround && groundTouch)
        {
            groundTouch = false;
        }

        if (coll.isOnGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
        }
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;
    }

    void FlipDirction()
    {
        if (x < 0)
        {
            transform.localScale = new Vector2(-30, 30);
        }
        else if (x > 0)
        {
            transform.localScale = new Vector2(30, 30);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            EventCenter.Instance.EventTrigger("PlayerDead", "player");

        }
    }
   

   public void PlayerDead(object info)
   {
        Debug.Log("PlayerDead" + info);
   }
    

    
  
}
