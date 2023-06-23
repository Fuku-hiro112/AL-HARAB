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

    private GroundCheck ground;
    private Rigidbody2D rb;
    private BoxCollider2D collider2d;

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
        collider2d = GetComponent<BoxCollider2D>();

        isMove = true;
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

        //TODO:ジャンプによって角度が変わる
        /*if (jumped)
        {
            transform.rotation = new Quaternion(,,);
        }*/

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
            collider2d.isTrigger = true;

        }

        //Triggerがオンならこの後の処理を実行
        if (collider2d.isTrigger != true) return;
        //下の線路にいた、ジャンプの最高到達点についたとき
        if (whereLine == Line.Bottom && Highest_point())
        {
            collider2d.isTrigger = false;
            whereLine = Line.Top;
        }
        //
        else if (whereLine == Line.Top && transform.position.y < -2f)
        {
            //Triggerを外す
            collider2d.isTrigger = false;
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
        //FIXME:マジックナンバーを削除、数式の簡略化　初期スピード1 Maxスピード6 ゲージMaxまで６秒
        rb.velocity = new Vector3(dash_speed * (speed_elapsed_time * 5/6 + 1f), rb.velocity.y);
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
    /// <summary>
    /// ジャンプした際の最高到達点
    /// </summary>
    bool Highest_point()
    {
        return rb.velocity.y <= 0;
    }

}
