using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float speed_elapsed_time = 0;
    [SerializeField] float dash_speed = 1;
    [SerializeField] float jump_power = 2.5f;
    [SerializeField] float top_Jump = 2.5f;
    [SerializeField] float botm_Jump = 7.5f;
    int hp;

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
        hp =  3;
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

        if (Headed_goal())//�S�[������܂�
        {
            //�o�ߎ��Ԃ𑫂�
            clear_time += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump(jump_power);
        }

        //TODO:�W�����v�ɂ���Ċp�x���ς��
        /*if (jumped)
        {
            transform.rotation = new Quaternion(,,);
        }*/

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
            collider2d.enabled = false;

        }

        //collider2d��false�Ȃ炱�̌�̏��������s
        if (collider2d.enabled != false) return;
        //���̐��H�ɂ����A�W�����v�̍ō����B�_�ɂ����Ƃ�
        if (whereLine == Line.Bottom && Highest_point())
        {
            collider2d.enabled = true;
            whereLine = Line.Top;
        }
        //
        else if (whereLine == Line.Top && transform.position.y < -2f)
        {
            //
            collider2d.enabled = true;
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
        //FIXME:�}�W�b�N�i���o�[���폜�A�����̊ȗ����@�����X�s�[�h1 Max�X�s�[�h6 �Q�[�WMax�܂łU�b
        rb.velocity = new Vector3(dash_speed * (speed_elapsed_time * 5/6 + 1f), rb.velocity.y);
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    void Jump(float _jump_speed)
    {
        Debug.Log(_jump_speed);
        rb.velocity = new Vector2(rb.velocity.x, _jump_speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
        {

        }
    }

    /// <summary>
    /// �S�[���Ɍ������Ă��鎞�@�i�S�[�����Ă��Ȃ����j
    /// </summary>
    bool Headed_goal()
    {
        return goal.position.x > transform.position.x;
    }
    /// <summary>
    /// �W�����v�����ۂ̍ō����B�_
    /// </summary>
    bool Highest_point()
    {
        return rb.velocity.y <= 0;
    }

}
