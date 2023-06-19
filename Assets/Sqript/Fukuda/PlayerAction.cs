using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float speed_elapsed_time = 0;
    [SerializeField] float speed = 5;
    bool canMove;

    Rigidbody2D rb;

    //public float Speed_elapTime{get; set;}

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();



        canMove = true;
    }


    void Update()
    {

        if (canMove)
        {
            Move();
        }
        else
        {
            
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
}
