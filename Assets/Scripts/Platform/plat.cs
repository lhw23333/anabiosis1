using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plat : MonoBehaviour
{
    public Rigidbody2D rb;

    public Transform up;
    public Transform down;

    public Vector2 uppos;
    public Vector2 downpos;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        uppos = up.transform.position;
        downpos = down.transform.position;

        rb.velocity = new Vector2(0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= uppos.y && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(0, -5);
        }
        if (transform.position.y <= downpos.y && rb.velocity.y < 0)

        {
            rb.velocity = new Vector2(0, 5);
        }


    }
}
