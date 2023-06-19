using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float speed_elapsed_time = 0;
    [SerializeField] float speed = 5;
    float jumpSpeed = 7.5f;
    bool canMove;
    private bool isGround = false;
    private GroundCheck ground;
    Rigidbody2D rb;

    //public float Speed_elapTime{get; set;}

    void Start()
    {
        ground = GameObject.Find("GroundCheck").GetComponent<GroundCheck>();
        rb = GetComponent<Rigidbody2D>();

        canMove = true;
    }


    void Update()
    {
        isGround = ground.IsGround();
        Debug.Log(isGround);

        if (canMove)
        {
            Move();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jamp();
        }
    }

    void Move()
    {
        if (speed_elapsed_time > 6)
        {
            speed_elapsed_time = 6;
        }
        else
        {
            speed_elapsed_time += Time.deltaTime;
        }

        rb.velocity = new Vector3(speed * speed_elapsed_time, rb.velocity.y);
        //Debug.Log(rb.velocity);
    }
    void Jamp()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }
}
