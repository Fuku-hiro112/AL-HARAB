using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float speed_elapsed_time = 0;
    [SerializeField] float speed = 5;
    [SerializeField] float jump_speed = 7.5f;
    [SerializeField] float line_change_jump = 2.5f;

    public static float clear_time;

    private bool is_ground = false;
    private bool can_move;
    private bool can_change_line;
    private bool can_enable;
    private GroundCheck ground;
    private Rigidbody2D rb;

    Transform goal;

    float goal_position;
    //public float Speed_elapTime{get; set;}

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GameObject.Find("GroundCheck").GetComponent<GroundCheck>();
        goal = GameObject.Find("Goal").GetComponent<Transform>();

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

        if (Headed_goal())//�S�[������܂�
        {
            //�o�ߎ��Ԃ�
            clear_time += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && is_ground)
        {
            Jamp(jump_speed);
        }

//HACK:����ȍ~�̃R�[�h��bool�𑽗l���Ă��邽�ߏC�����K�v
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
    /// �E�ɓ���
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
    /// <summary>
    /// ���H��ς���
    /// </summary>
    void Change_Line()
    {
        float pos_y = transform.position.y;
        can_enable = true;

        if (pos_y <= -2)
        {
            Jamp(jump_speed);//FIXME:line_cahange_jamp�ŃW�����v���Ă���
            can_enable = true;
        }
        else
        {
            Jamp(line_change_jump);
            //upper_line_collider.enabled = false; //TODO:�G�L�������i��Obj���S�Ă��������
            can_change_line = false;//=!can_change_line �ɂ���Ɣ��]����
        }

        if (can_enable && rb.velocity.y <= 0)
        {
            //upper_line_collider.enabled = true; //TODO:�G�L�������i��Obj���S�Ă��������
            can_change_line = false;
        }
    }
    /// <summary>
    /// �S�[���Ɍ������Ă��鎞�@�i�S�[�����Ă��Ȃ����j
    /// </summary>
    bool Headed_goal()
    {
        return goal_position > transform.position.x;
    }
}
