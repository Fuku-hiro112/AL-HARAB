using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// private�ϐ��́@_��t����camelCasing�i�L�������P�[�X�j���d�l����
/// public�ϐ��́@A�ŏ��啶���@�p�X�J�� �P�[�X(Pascal)
/// ���[�J���ϐ��́@
/// </summary>
public class PlayerAction : MonoBehaviour
{
    public float SpeedGage = 0;
    [SerializeField] float _dashSpeed = 1;
    [SerializeField] float _jumpPower = 2.5f;
    [SerializeField] float _topJump = 2.5f;
    [SerializeField] float _botmJump = 8.0f;
    [SerializeField] float _knockBackTime = 0.5f;
    [SerializeField] Vector2 _knockBackPower = new Vector2(-3f,5f);
    [SerializeField] Image[] _imgHealth;

    public static int HpCurrent;
    public static float ClearTime;

    public float ControlLostTime;

    private bool _isGround = false;

    bool bControl;
    private bool _canJump;

    private GroundCheck _ground;
    private Rigidbody2D _rb;
    private BoxCollider2D _collider2d;

    private const int _oneMeter = 2;
    private const int _twoMeter = 4;

    private const int _damage1 = 1;
    private const int _damage2 = 2;
    private const int _damage3 = 3;

    private Transform _goal;

    private Line _whereLine;

    enum Line
    {
        Top,
        Bottom
    }

    void Start()
    {
        HpCurrent =  3;
        _whereLine = Line.Top;
        _rb = GetComponent<Rigidbody2D>();
        _ground = GameObject.Find("GroundCheck").GetComponent<GroundCheck>();
        _goal = GameObject.Find("Goal").GetComponent<Transform>();
        _collider2d = GetComponent<BoxCollider2D>();
        ControlLostTime = 0f;
    }

    void Update()
    {
        _canJump = Input.GetKeyDown(KeyCode.Space) && _isGround;
        bControl = ControlLostTime <= 0;

        _isGround = _ground.IsGroundJudg();

        //����s���Ԃ��J�E���g����
        if (ControlLostTime > 0)
        {
            ControlLostTime -= Time.deltaTime;
        }

        if (bControl == true)
        {
            Move();

            if (_canJump) Jump(_jumpPower);
        }

        if (IsHeadedGoal())//�S�[������܂�
        {
            //�o�ߎ��Ԃ𑫂�
            ClearTime += Time.deltaTime;
        }

        //TODO:�W�����v�ɂ���Ċp�x���ς��
        /*if (jumped)
        {
            transform.rotation = new Quaternion(,,);
        }*/
        //�������[��
        if (( Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) 
            && _isGround && _collider2d.enabled == true)
        {
            if (_whereLine == Line.Bottom)
            {
                //Debug.Log("��");
                Jump(_botmJump);
                _whereLine = Line.Bottom;
            }
            else
            {
                //Debug.Log("��");
                Jump(_topJump);
                _whereLine = Line.Top;
            }
            _collider2d.enabled = false;
        }

        //collider2d��false�Ȃ炱�̌�̏��������s
        if (_collider2d.enabled != false) return;
        //���̐��H�ɂ����A�W�����v�̍ō����B�_�ɂ����Ƃ�
        if (_whereLine == Line.Bottom && IsHighestPoint())
        {
            _whereLine = Line.Top;
            _collider2d.enabled = true;
        }
        //
        else if (_whereLine == Line.Top && transform.position.y < -2f)
        {
            //
            _whereLine = Line.Bottom;
            _collider2d.enabled = true;
        }   
    }


    /// <summary>
    /// �E�ɓ���
    /// </summary>
    void Move()
    {
        if (SpeedGage > 6)
        {
            SpeedGage = 6;
        }
        else
        {
            SpeedGage += Time.deltaTime;
            //Debug.Log(SpeedGage);
        }
        //FIXME:�}�W�b�N�i���o�[���폜�A�����̊ȗ����@�����X�s�[�h1 Max�X�s�[�h6 �Q�[�WMax�܂łU�b
        _rb.velocity = new Vector3(_dashSpeed * (SpeedGage * 5/6 + 1f), _rb.velocity.y);
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    void Jump(float _jump_speed)
    {
        //Debug.Log(_jump_speed);
        _rb.velocity = new Vector2(_rb.velocity.x, _jump_speed);
    }

    void Damage(int damage)
    {
        SetHealth(damage);
        
        //Debug.Log(HpCurrent);
        //�i�܂Ȃ��悤�ɂ���
        ControlLostTime = _knockBackTime;

        SpeedGage = 0;
        _rb.velocity = _knockBackPower;

        //TODO:���G�����b�A���̎��ԓ_��

    }

    //
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            Destroy_or_Damage(_oneMeter, other);
        }

        if (other.gameObject.tag == "obstacle")
        {
            Destroy_or_Damage(_twoMeter, other);
        }
    }
    /*
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            Destroy_or_Damage(_oneMeter, other);
        }

        if (other.gameObject.tag == "obstacle")
        {
            Destroy_or_Damage(_twoMeter,other);
        }
    }*/

    /// <summary>
    /// ���[�^�[�ɂ���ă_���[�W���󂯂邩�A������󂷂��𔻒肷��
    /// </summary>
    void Destroy_or_Damage(int meter , Collider2D other)
    {
        if (SpeedGage >= meter)
        {
            SpeedGage -= meter;
            Destroy(other.gameObject);
        }
        else
        {
            Damage(_damage1);
            //15�}�X��납��o��
        }
    }

    void SetHealth(int health)
    {
        HpCurrent -= health;

        for (int i = 0; i < _imgHealth.Length; i++)
        {
            _imgHealth[i].enabled = i < HpCurrent;
        }
    }

    //�ȉ��A�ǐ����ǂ����^�U�̕K�v����
    /// <summary>
    /// �S�[���Ɍ������Ă��鎞�@�i�S�[�����Ă��Ȃ����j
    /// </summary>
    bool IsHeadedGoal()
    {
        return _goal.position.x > transform.position.x;
    }
    /// <summary>
    /// �W�����v�����ۂ̍ō����B�_
    /// </summary>
    bool IsHighestPoint()
    {
        return _rb.velocity.y <= 0;
    }

}
