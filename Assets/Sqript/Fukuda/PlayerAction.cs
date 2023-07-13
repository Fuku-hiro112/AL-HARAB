using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;


// private�ϐ��́@_��t����camelCasing�i�L�������P�[�X�j���d�l����
// public�ϐ��́@A�ŏ��啶���@�p�X�J�� �P�[�X(Pascal)
// ���[�J���ϐ��́@���ʂ�camelCasing�i�L�������P�[�X�j

public class PlayerAction : MonoBehaviour
{
    [SerializeField] float _dashSpeed = 1;
    [SerializeField] float _jumpPower = 4.0f;
    [SerializeField] float _topJump = 2.5f;
    [SerializeField] float _botmJump = 9.0f;
    [SerializeField] float _knockBackTime = 1.0f;
    [SerializeField] float _changeJumpCoolTime = 2.0f;
    [SerializeField] Vector2 _knockBackPower = new Vector2(-3f,3f);
    [SerializeField] UnityEngine.UI.Image[] _imgHealth;
    [SerializeField] float _firstSpeed = 1.0f;
    [SerializeField] int _flashInterval = 100;
    [SerializeField] int _loopCount = 10;

    public static int HpCurrent = 3;
    public static float ClearTime;

    [NonSerialized] public float SpeedGage = 0;

    private float posX;
    private float _controlLostTime;
    private float _changeJumpLostTime;

    private bool _bControl,_canJump,_canChange,_isFalling = false;

    private Animator _animator;
    private Rigidbody2D _rb;
    private BoxCollider2D _playerCollid, _groundCollid;
    private Transform _goal;
    private SpriteRenderer _spriteRenderer;

    //�X�N���v�g�n
    private CoinAction _coinAction;
    private GroundCheck _ground;

    // LayerID���擾
    private int _topLineLayer;
    private int _bottomLineLayer;
    private int _playerLayer;
    
    //�萔
    private const int _offScreen = -7;
    private const int _oneMeter = 2;
    private const int _twoMeter = 4;
    private const int _oneDamage = 1;

    //enum�n
    [SerializeField]
    private Line _whereLine;
    public STATE State;
    private GameMode _mode;
    internal float speed_elapsed_time;

    enum Line
    {
        Top,
        Bottom
    }
    public enum STATE
    {
        NOMAL,
        DAMAGED,
        DEATH
    }
    enum GameMode
    {
        Play,
        GameOver,
        GameCrear
    }

    void Start()
    {
        GetComponent();

        _controlLostTime = 0f;
        _changeJumpLostTime = 0f;

        // LayerID���擾
        _topLineLayer = LayerMask.NameToLayer("TopLine");
        _bottomLineLayer = LayerMask.NameToLayer("BottomLine");
        _playerLayer = LayerMask.NameToLayer("Player");

        //FIXDME: ����2�̏��Ԃ��㉺�ς����肷���_bottom��ture�ɂȂ��Ă��܂�
        LayerCollision(_bottomLineLayer, false);
        LayerCollision(_topLineLayer,true);

        _isFalling = false;
        HpCurrent =  3;
        State = STATE.NOMAL;
        _whereLine = Line.Top;
        _mode = GameMode.Play;
    }

    async void Update()
    {
        _animator.SetFloat("AirSpeedY", _rb.velocity.y);
        _animator.SetBool("IsGround", _ground.IsGround);

        if (State == STATE.DEATH) _mode = GameMode.GameOver;

        Debug.Log(_ground.IsGround);

        _canJump = Input.GetKeyDown(KeyCode.Space) && _ground.IsGround;
        _bControl = _controlLostTime <= 0;
        _canChange = _changeJumpLostTime <= 0; //HACK: �֐��Ƃ��g���ĕ�����₷���o���Ȃ����ȁH

        if (IsHeadedGoal())//�S�[������܂�
        {
            //�o�ߎ��Ԃ𑫂�
            ClearTime += Time.deltaTime;
        }

        //��������
        if (transform.position.y <= _offScreen)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezePosition;

            if (_isFalling) return;
            _isFalling = true;
            Damage(_oneDamage);
            var ct = this.GetCancellationTokenOnDestroy();
            await AsyncFall(ct);

        }

        //�m�b�N�o�b�N������~�܂�
        if (State == STATE.DAMAGED)
        {
            if (_rb.velocity.y > 0) return;
            if (!_ground.IsGround) return;
            _rb.velocity = Vector2.zero;
        }

        //����s���Ԃ��J�E���g���� ���Q�ƌ^!!                      //HACK: �Q�ƌ^�g�������Ȃ��Ȃ�
        ActionLostTime(ref _controlLostTime, ref _changeJumpLostTime);//�l���ς��܂�
        _controlLostTime = ActionLostTime(_controlLostTime);
        _changeJumpLostTime = ActionLostTime(_changeJumpLostTime);

        if (State != STATE.NOMAL) return;
        if (_bControl)
        {
            //�E�ֈړ�����
            Move();
            //�W�����v����
            if (_canJump) Jump(_jumpPower);
        }
        //�W�����v���ă��[����؂�ւ���
        LineChange();
    }

    /// <summary>
    /// Player�Ƃ̓����蔻��𑀍삷��
    /// </summary>
    void LayerCollision(int layer , bool display)// layer�ɂ́A���葀�삷��Layer���@display�ɂ͕\�����邩���Ȃ�����
    {
        //false���ƕ\���Ȃ̂ŕ\�����鎞��true�ɂȂ�悤������₷�� !display�ɂ���
        Physics2D.IgnoreLayerCollision(_playerLayer ,layer, !display);

        _groundCollid.enabled = display;//�ΉԃG�t�F�N�g���o�Ȃ��悤��false�ɂ��Ă���
    }//�����Ⴂ�̃I�[�o�[���[�h���@2���葀�삷��ۂɎg��
    void LayerCollision(int layer1,int layer2, bool display)
    {
        Physics2D.IgnoreLayerCollision(_playerLayer, layer1, !display);
        Physics2D.IgnoreLayerCollision(_playerLayer, layer2, !display);

        _groundCollid.enabled = display;
    }

    /// <summary>
    ///�s���s���Ԃ��J�E���g����
    /// </summary>
    float ActionLostTime(float lostTime)
    {
        if (lostTime > 0)
        {
            lostTime -= Time.deltaTime;
        }
        return lostTime;
    }
    void ActionLostTime(ref float lostTime1,ref float lostTime2)
    {
        if (lostTime1 > 0)
        {
            lostTime1 -= Time.deltaTime;
        }
        if (lostTime2 > 0)
        {
            lostTime2 -= Time.deltaTime;
        }
    }

    /// <summary>
    /// �����ŉE�ɓ���
    /// </summary>
    void Move()
    {
        float elapsed = SpeedGage * 5 / 6;

        if (SpeedGage > 6)
        {
            SpeedGage = 6;
        }
        else
        {
            SpeedGage += Time.deltaTime;
        }
        //�����X�s�[�h1 Max�X�s�[�h6 �Q�[�WMax�܂łU�b
        _rb.velocity = new Vector3(_dashSpeed * (elapsed + _firstSpeed), _rb.velocity.y);
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    void Jump(float _jump_speed)
    {
        _animator.SetTrigger("Jump");
        _rb.velocity = new Vector2(_rb.velocity.x, _jump_speed);
    }

    /// <summary>
    /// ��ʉ��ɗ��������̏���
    /// </summary>
    async UniTask AsyncFall(CancellationToken ct)
    {
        if (State == STATE.DEATH)//����ł�Ȃ�
        {
            _mode = GameMode.GameOver;
            return;
        }

        await UniTask.Delay(_flashInterval * 2 * _loopCount, cancellationToken: ct);

        SpeedGage = 0;

        if (_whereLine == Line.Bottom)
        {
            //TODO:�}�W�b�N�i���o�[�����邩��C�����܂��@�������l
            transform.position += new Vector3(-15, 5, 0); //-15 �}�X�@����-4
            
            Debug.Log("������");
        }
        else
        {
            transform.position += new Vector3(-15, 8, 0);//-15 �}�X�@��͂P
        }
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _isFalling = false;
    }

    /// <summary>
    /// ���H��ύX����
    /// </summary>
    void LineChange()
    {
        //
        if (CanChange())
        {
            _animator.SetTrigger("Jump");
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
            //��Ɖ��̐��H�̓����蔻��𖳎�����
            LayerCollision(_topLineLayer, _bottomLineLayer, false);
        }

        //collider2d��false�Ȃ炱�̌�̏��������s
        if (_groundCollid.enabled != false) return; //NOTE: != false �̕������₷���H�H
        //���̐��H�ɂ������A�W�����v�̍ō����B�_�ɂ����Ƃ�
        if (_whereLine == Line.Bottom && IsHighestPoint())
        {
            _whereLine = Line.Top;
            LayerCollision(_topLineLayer, true);//��̐��H�ɓ�����悤�ɂ���

        }//��̐��H�ɂ������A��̐��H��艺�ɂȂ����Ƃ�
        else if (_whereLine == Line.Top && transform.position.y < -2f)
        {
            _whereLine = Line.Bottom;
            LayerCollision(_bottomLineLayer, true);//���̐��H�ɓ�����悤�ɂ���
        }
        //���H�؂�ւ��̃N�[���^�C���������Ă���
        _changeJumpLostTime = _changeJumpCoolTime;
    }
    /// <summary>
    /// GetCompornent������̂����Ă���
    /// </summary>
    void GetComponent()
    {
        _ground = GameObject.Find("GroundCheck").GetComponent<GroundCheck>();
        _goal = GameObject.Find("Goal").GetComponent<Transform>();
        _coinAction = GameObject.Find("CoinParticle").GetComponent<CoinAction>();
        _groundCollid = transform.Find("GroundCheck").GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _playerCollid = GetComponent<BoxCollider2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

//�ȉ��A�ǐ����ǂ����^�U�̕K�v����-----bool�l---------------------------------------------
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
        return btnPush && _ground.IsGround && _playerCollid.enabled == true && _canChange;
    }

//-----------�����������̃��\�b�h------------------------------------------------------------------------------------

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
            other.gameObject.SetActive(false);
        }
        else
        {
            if (State != STATE.NOMAL) return;
            State = STATE.DAMAGED;
            //�_���[�W���󂯂������@�����ɂ̓_���[�W���󂯂��l��
            _animator.SetTrigger("Crash");
            Damage(_oneDamage);
            KnockBack();
        }
    }
    /// <summary>
    /// �G�ɓ������ă_���[�W���󂯂����̏���
    /// </summary>
    void Damage(int damage, Action action = null)//HACK:Action�͎����ɕt��������
    {
        //damage��Hp�����炵�AUI���X�V
        SetHealth(damage);

        //Debug.Log(HpCurrent);

        SpeedGage = 0;

        //IsDeath = HpCurrent <= 0;
        if(HpCurrent<=0)State = STATE.DEATH;

        action?.Invoke();//HACK: �����ɕt��������
    }
    /// <summary>
    /// �m�b�N�o�b�N����
    /// </summary>
    async void KnockBack()
    {
        //�i�܂Ȃ��悤�ɂ���
        _controlLostTime = _knockBackTime;

        //velocity�Ō��Ƀm�b�N�o�b�N����
        _rb.velocity = _knockBackPower;//NOTE: new vector2�ł��̂܂܏���������������₷���H

        _coinAction.CoinParticle();
        
        //��������
        if (State == STATE.DEATH)//���Ƀm�b�N�o�b�N���Ă��炻�̂܂ܗ�����悤�ɂ������̂�Update�ɏ��������Ȃ��H
        {
            //�^���ɗ�������GameOver�ɂ������̂œ����蔻��������Ă���
            _playerCollid.enabled = false;

            _animator.SetBool("Death",true);
        }
        else
        {
            var ct = this.GetCancellationTokenOnDestroy();
            await AsyncFlash(ct);//�_�ł���

            //15�}�X���ɍs��
            transform.position += new Vector3(-15, 1, 0);
            State = STATE.NOMAL;
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
    /// <summary>
    /// �_�ł��鏈��
    /// </summary>
    async UniTask AsyncFlash(CancellationToken ct)
    {
        for (int i = 0; i < _loopCount; i++)
        {
            Debug.Log(_spriteRenderer.color.a);

            _spriteRenderer.color += new Color(0,0,0,-100);
            await UniTask.Delay(_flashInterval, cancellationToken:ct);

            Debug.Log(i+"���");

            _spriteRenderer.color += new Color(0,0,0,100);
            await UniTask.Delay(_flashInterval, cancellationToken:ct);


        }
    }

}
