using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using static System.Net.Mime.MediaTypeNames;


// private�ϐ��́@_��t����camelCasing�i�L�������P�[�X�j���d�l����
// public�ϐ��́@A�ŏ��啶���@�p�X�J�� �P�[�X(Pascal)
// ���[�J���ϐ��́@���ʂ�camelCasing�i�L�������P�[�X�j

public class PlayerAction : MonoBehaviour
{
    [SerializeField] float _dashSpeed = 1;
    [SerializeField] float _jumpPower = 2.5f;
    [SerializeField] float _topJump = 2.5f;
    [SerializeField] float _botmJump = 8.0f;
    [SerializeField] float _knockBackTime = 0.5f;
    [SerializeField] float _changeJampCoolTime = 2f;
    [SerializeField] Vector2 _knockBackPower = new Vector2(-3f,5f);
    [SerializeField] UnityEngine.UI.Image[] _imgHealth;

    public static int HpCurrent;
    public static float ClearTime;

    public float SpeedGage = 0;

    private float _controlLostTime;
    private float _chageJampLostTime;

    private bool _isGround, _bControl,_canJump,_canChange;

    private GroundCheck _ground;
    private Rigidbody2D _rb;
    private BoxCollider2D _playerCollid, _groundCollid;
    private Transform _goal;

    // LayerID���擾
    private int _topLineLayer;
    private int _bottomLineLayer;
    private int _playerLayer;

    private const int _oneMeter = 2;
    private const int _twoMeter = 4;

    private const int _damage1 = 1;
    private const int _damage2 = 2;
    private const int _damage3 = 3;

    private Line _whereLine;

    enum Line
    {
        Top,
        Bottom
    }

    void Start()
    {
        GetComponent();

        _controlLostTime = 0f;
        _chageJampLostTime = 0f;

        // LayerID���擾
        _topLineLayer = LayerMask.NameToLayer("TopLine");
        _bottomLineLayer = LayerMask.NameToLayer("BottomLine");
        _playerLayer = LayerMask.NameToLayer("Player");

        LayerCollision(_topLineLayer,_bottomLineLayer,true);

        HpCurrent =  3;
        _whereLine = Line.Top;
    }

    void Update()
    {
        //TODO:����ǂ��ɂ����悤
        _isGround = _ground.IsGroundJudg();

        _canJump = Input.GetKeyDown(KeyCode.Space) && _isGround;
        _bControl = _controlLostTime <= 0;
        _canChange = _chageJampLostTime <= 0;


        //����s���Ԃ��J�E���g����@���Q�ƌ^!!
        ActionLostTime(ref _controlLostTime);//�l���ς��܂�
        ActionLostTime(ref _chageJampLostTime);//�l���ς��܂�

        if (_bControl == true)
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

        if (CanChange())
        {
            if (_whereLine == Line.Bottom)//�����[���ɂ���ꍇ
            {
                //�����̒l�ŃW�����v����
                Jump(_botmJump);
                _whereLine = Line.Bottom;
            }
            else//�ヌ�[���ɂ���ꍇ
            {
                //�����̒l�ŃW�����v����
                Jump(_topJump);
                _whereLine = Line.Top;
            }
            //
            LayerCollision(_topLineLayer, _bottomLineLayer, false);
        }

        //collider2d��false�Ȃ炱�̌�̏��������s
        if (_groundCollid.enabled != false) return;
        //���̐��H�ɂ������A�W�����v�̍ō����B�_�ɂ����Ƃ�
        if (_whereLine == Line.Bottom && IsHighestPoint())
        {
            _whereLine = Line.Top;
            LayerCollision(_topLineLayer, true);
        }
        //��̐��H�ɂ������A��̐��H��艺�ɂȂ����Ƃ�
        else if (_whereLine == Line.Top && transform.position.y < -2f)
        {
            //
            _whereLine = Line.Bottom;
            LayerCollision(_bottomLineLayer,true);
        }
        _chageJampLostTime = _changeJampCoolTime;
    }
    /// <summary>
    /// 
    /// </summary>
    void LayerCollision(int layer , bool display)
    {
        //false���ƕ\���Ȃ̂ŕ\�����鎞��true�ɂȂ�悤������₷�� !display�ɂ���
        Physics2D.IgnoreLayerCollision(_playerLayer ,layer, !display);

        _groundCollid.enabled = display;//�ΉԃG�t�F�N�g���o�Ȃ��悤��false�ɂ��Ă���
    }
    void LayerCollision(int layer1,int layer2, bool display)
    {
        Physics2D.IgnoreLayerCollision(_playerLayer, layer1, !display);
        Physics2D.IgnoreLayerCollision(_playerLayer, layer2, !display);

        _groundCollid.enabled = display;//�ΉԃG�t�F�N�g���o�Ȃ��悤��false�ɂ��Ă���
    }

    /// <summary>
    ///�s���s���Ԃ��J�E���g����
    /// </summary>
    void ActionLostTime(ref float lostTime)
    {
        if (lostTime > 0)
        {
            lostTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// �����ŉE�ɓ���
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
        //�ǂ������ς��Ȃ��H�H
        _rb.velocity = new Vector2(_rb.velocity.x, _jump_speed);
        //_rb.AddForce(transform.up * _jump_speed,ForceMode2D.Impulse);
    }

    void Damage(int damage)
    {
        //damage��Hp�����炵�AUI���X�V
        SetHealth(damage);
        
        //Debug.Log(HpCurrent);
        //�i�܂Ȃ��悤�ɂ���
        _controlLostTime = _knockBackTime;

        SpeedGage = 0;
        _rb.velocity = _knockBackPower;

        //TODO:���G�����b�A���̎��ԓ_��

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            //�G�Ȃ烁�[�^�[��1�g��
            Destroy_or_Damage(_oneMeter, other);
        }

        if (other.gameObject.tag == "obstacle")
        {
            //��Q���Ȃ烁�[�^�[��2�g��
            Destroy_or_Damage(_twoMeter, other);
        }
    }

    /// <summary>
    /// ���[�^�[�ɂ���ă_���[�W���󂯂邩�A������󂷂��𔻒肷��
    /// </summary>
    void Destroy_or_Damage(int meter , Collider2D other)
    {
        //meter���X�s�[�h�Q�[�W�����܂��Ă����
        if (SpeedGage >= meter)
        {
            SpeedGage -= meter;
            Destroy(other.gameObject);
        }
        else
        {
            Damage(_damage1);
            //TODO:15�}�X��납��o��
        }
    }
    /// <summary>
    /// �c�@�����炵�AUI�ɂ����f����
    /// </summary>
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
    /// <summary>
    /// ���H�؂�ւ����o���邩�ǂ���
    /// </summary>
    bool CanChange()
    {
        bool btnPush = (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0));
        return btnPush && _isGround && _playerCollid.enabled == true && _canChange;
    }

    void GetComponent()
    {
        _ground = GameObject.Find("GroundCheck").GetComponent<GroundCheck>();
        _goal = GameObject.Find("Goal").GetComponent<Transform>();
        _groundCollid = transform.Find("GroundCheck").GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _playerCollid = GetComponent<BoxCollider2D>();
    }
}
