using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;

    public bool isOnGround;
    public bool isOnWall;
    public bool isOnRightWall;
    public bool isOnLeftWall;
    public int wallSide;

    [Header("Coyote Time")]
    public float coyoteTime;
    [Header("Collision")]
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer))
        {
            isOnGround = true;
        }
        else
        {
            if(isOnGround)
                Invoke("SetOnGround", coyoteTime);
        }

        isOnWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        isOnRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        isOnLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        wallSide = isOnRightWall ? -1 : 1;
    }

    void SetOnGround()
    {
        isOnGround = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var posirion = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
    
}
