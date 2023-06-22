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

        if (Headed_goal())//�S�[������܂�
        {
            //�o�ߎ��Ԃ𑫂�
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


//HACK:����ȍ~�̃R�[�h��bool�𑽗l���Ă��邽�ߏC�����K�v

        if (( Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) ) 
            && isGround)
        {
            if (whereLine == Line.Bottom)
            {
                Debug.Log("��");
                Jump(botm_Jump);
                whereLine = Line.Bottom;
            }
            else
            {
                Debug.Log("��");
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
            //Trigger���O��
            collider.isTrigger = false;
            whereLine = Line.Bottom;
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

        rb.velocity = new Vector3(dash_speed * speed_elapsed_time, rb.velocity.y);
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    void Jump(float _jump_speed)
    {
        Debug.Log(_jump_speed);
        rb.velocity = new Vector2(rb.velocity.x, _jump_speed);
    }

    /// <summary>
    /// �S�[���Ɍ������Ă��鎞�@�i�S�[�����Ă��Ȃ����j
    /// </summary>
    bool Headed_goal()
    {
        return goal.position.x > transform.position.x;
    }

}
/*
   /// <summary>
   /// ���H��ς���
   /// </summary>
   void Top_Line_Change()
   {
       Jamp(bottomLine_changeJump);

       colid.enabled = false;
       can_enable = true;
       Debug.Log("�ォ����");
   }
   /// <summary>
   /// ���H��ς���
   /// </summary>
   void Bottom_Line_Change()
   {
       Jamp(jump_speed);

       colid.enabled = false;
       can_enable = true;
       Debug.Log("��������");
   }*/

/*
can_enable = true;

//�ǂ���̐��H�����Ԃ�
if (UnderLine_flyable())
{
    Jamp(jump_speed);//FIXME:line_cahange_jamp�ŃW�����v���Ă���
    can_enable = true;
    Debug.Log("��������");
}
else
{
    Jamp(line_change_jump);
    //upper_line_collider.enabled = false; //TODO:�G�L�������i��Obj���S�Ă��������
    colid.enabled = false;
    can_change_line = false;//=!can_change_line �ɂ���Ɣ��]����
    Debug.Log("�ォ����");
}

//���̐��H�����񂾎� -> �������ɃR���C�_�[�𕜊�����
if (can_enable && rb.velocity.y <= 0)
{
    //upper_line_collider.enabled = true; //TODO:�G�L�������i��Obj���S�Ă��������
    colid.enabled = true;
    can_change_line = false;
}*/
