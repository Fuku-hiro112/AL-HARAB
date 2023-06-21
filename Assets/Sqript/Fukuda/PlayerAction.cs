using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAction : MonoBehaviour
{
    public float speed_elapsed_time = 0;
    [SerializeField] float speed = 5;
    [SerializeField] float jump_speed = 7.5f;
    [SerializeField] GameObject upper_line_tile;

    TilemapCollider2D upper_line_collider;
    
    private bool is_ground = false;
    private bool can_move;
    private bool can_change_line;
    private bool can_enable;
    private GroundCheck ground;
    private Rigidbody2D rb;

    //public float Speed_elapTime{get; set;}

    void Start()
    {
        ground = GameObject.Find("GroundCheck").GetComponent<GroundCheck>();
        rb = GetComponent<Rigidbody2D>();
        upper_line_collider = upper_line_tile.GetComponent<TilemapCollider2D>();

        can_move = true;
        can_change_line = false;
        can_enable = false;
    }


    void Update()
    {
        is_ground = ground.IsGround();

        if (can_move)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.Space) && is_ground)
        {
            Jamp();
        }

        if (( Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) ) 
            && is_ground)
        {
            can_change_line = true;
        }

        if (can_change_line)
        {
            Change_Line();
        }
    }

    /// <summary>
    /// ‰E‚É“®‚­
    /// </summary>
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
    }

    void Jamp()
    {
        rb.velocity = new Vector2(rb.velocity.x, jump_speed);
    }
    void Change_Line()
    {
        float pos_y = transform.position.y;
        can_enable = true;

        if (pos_y < -2)
        {
            Jamp();
            can_enable = true;
        }

        if (can_enable && rb.velocity.y <= 0)
        {
            upper_line_collider.enabled = true;
            can_change_line = false;
        }
    }
}
