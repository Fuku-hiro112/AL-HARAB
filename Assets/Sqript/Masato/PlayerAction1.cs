using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAction1 : MonoBehaviour
{
    public float speed_elapsed_time = 0;
    [SerializeField] float speed = 5;
    [SerializeField] float jump_speed = 7.5f;
    [SerializeField] float line_change_jump = 2.5f;
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
            Jamp(jump_speed);
        }

//HACK:これ以降のコードがboolを多様しているため修正が必要
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
    /// 右に動く
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

    void Jamp(float _jump_speed)
    {
        rb.velocity = new Vector2(rb.velocity.x, _jump_speed);
    }

    void Change_Line()
    {
        float pos_y = transform.position.y;
        can_enable = true;

        if (pos_y <= -2)
        {
            Jamp(jump_speed);//FIXME:line_cahange_jampでジャンプしている
            can_enable = true;
        }
        else
        {
            Jamp(line_change_jump);
            upper_line_collider.enabled = false; //TODO:敵キャラや上段のObjも全てこれをする
            can_change_line = false;//=!can_change_line にすると反転する
        }

        if (can_enable && rb.velocity.y <= 0)
        {
            upper_line_collider.enabled = true; //TODO:敵キャラや上段のObjも全てこれをする
            can_change_line = false;
        }
    }
}
