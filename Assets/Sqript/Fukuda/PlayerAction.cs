using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float speed_elapsed_time = 0;
    [SerializeField] float dash_speed = 1;
    [SerializeField] float jump_power = 2.5f;
    [SerializeField] float top_Jump = 2.5f;
    [SerializeField] float botm_Jump = 7.5f;

    public static float clear_time;

    private bool isGround = false;
    private bool isMove;
    //private bool can_change_line;
    private bool isInvisibl;
    //private bool jumped;

    private GroundCheck ground;
    private Rigidbody2D rb;
    private BoxCollider2D collider;

    Transform goal;

    Line whereLine;

    enum Line
    {
        Top,
        Bottom
    }

    //public float Speed_elapTime{get; set;}

    void Start()
    {
        whereLine = Line.Top;
        rb = GetComponent<Rigidbody2D>();
        ground = GameObject.Find("GroundCheck").GetComponent<GroundCheck>();
        goal = GameObject.Find("Goal").GetComponent<Transform>();
        collider = GetComponent<BoxCollider2D>();

        isMove = true;
        //can_change_line = false;
        isInvisibl = false;
    }

    void Update()
    {
        isGround = ground.IsGround();

        if (isMove)
        {
            Move();
        }

        if (Headed_goal())//ゴールするまで
        {
            //経過時間を足す
            clear_time += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump(jump_power);
        }

        //TODO:
        /*if (jumped)
        {
            transform.rotation = new Quaternion(,,);
        }*/


//HACK:これ以降のコードがboolを多様しているため修正が必要

        if (( Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) ) 
            && isGround)
        {
            if (whereLine == Line.Bottom)
            {
                Debug.Log("下");
                Jump(botm_Jump);
                whereLine = Line.Bottom;
            }
            else
            {
                Debug.Log("上");
                Jump(top_Jump);
                whereLine = Line.Top;
            }
            collider.isTrigger = true;

        }

        //
        if (collider.isTrigger != true) return;
        if (whereLine == Line.Bottom && rb.velocity.y <= 0)
        {
            collider.isTrigger = false;
            whereLine = Line.Top;
        }
        else if (whereLine == Line.Top && transform.position.y < -2f)
        {
            //Triggerを外す
            collider.isTrigger = false;
            whereLine = Line.Bottom;
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

        rb.velocity = new Vector3(dash_speed * speed_elapsed_time, rb.velocity.y);
    }

    /// <summary>
    /// ジャンプする
    /// </summary>
    void Jump(float _jump_speed)
    {
        Debug.Log(_jump_speed);
        rb.velocity = new Vector2(rb.velocity.x, _jump_speed);
    }

    /// <summary>
    /// ゴールに向かっている時　（ゴールしていない時）
    /// </summary>
    bool Headed_goal()
    {
        return goal.position.x > transform.position.x;
    }

}
/*
   /// <summary>
   /// 線路を変える
   /// </summary>
   void Top_Line_Change()
   {
       Jamp(bottomLine_changeJump);

       colid.enabled = false;
       can_enable = true;
       Debug.Log("上から飛んだ");
   }
   /// <summary>
   /// 線路を変える
   /// </summary>
   void Bottom_Line_Change()
   {
       Jamp(jump_speed);

       colid.enabled = false;
       can_enable = true;
       Debug.Log("下から飛んだ");
   }*/

/*
can_enable = true;

//どちらの線路から飛ぶか
if (UnderLine_flyable())
{
    Jamp(jump_speed);//FIXME:line_cahange_jampでジャンプしている
    can_enable = true;
    Debug.Log("下から飛んだ");
}
else
{
    Jamp(line_change_jump);
    //upper_line_collider.enabled = false; //TODO:敵キャラや上段のObjも全てこれをする
    colid.enabled = false;
    can_change_line = false;//=!can_change_line にすると反転する
    Debug.Log("上から飛んだ");
}

//下の線路から飛んだ時 -> 落下時にコライダーを復活する
if (can_enable && rb.velocity.y <= 0)
{
    //upper_line_collider.enabled = true; //TODO:敵キャラや上段のObjも全てこれをする
    colid.enabled = true;
    can_change_line = false;
}*/
