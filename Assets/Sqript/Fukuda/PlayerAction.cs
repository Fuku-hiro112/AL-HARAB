using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;


// private変数は　_を付けてcamelCasing（キャメルケース）を仕様する
// public変数は　A最初大文字　パスカル ケース(Pascal)
// ローカル変数は　普通にcamelCasing（キャメルケース）

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

    //スクリプト系
    private CoinAction _coinAction;
    private GroundCheck _ground;

    // LayerIDを取得
    private int _topLineLayer;
    private int _bottomLineLayer;
    private int _playerLayer;
    
    //定数
    private const int _offScreen = -7;
    private const int _oneMeter = 2;
    private const int _twoMeter = 4;
    private const int _oneDamage = 1;

    //enum系
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

        // LayerIDを取得
        _topLineLayer = LayerMask.NameToLayer("TopLine");
        _bottomLineLayer = LayerMask.NameToLayer("BottomLine");
        _playerLayer = LayerMask.NameToLayer("Player");

        //FIXDME: この2つの順番を上下変えたりすると_bottomもtureになってしまう
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
        _canChange = _changeJumpLostTime <= 0; //HACK: 関数とか使って分かりやすく出来ないかな？

        if (IsHeadedGoal())//ゴールするまで
        {
            //経過時間を足す
            ClearTime += Time.deltaTime;
        }

        //落下判定
        if (transform.position.y <= _offScreen)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezePosition;

            if (_isFalling) return;
            _isFalling = true;
            Damage(_oneDamage);
            var ct = this.GetCancellationTokenOnDestroy();
            await AsyncFall(ct);

        }

        //ノックバックした後止まる
        if (State == STATE.DAMAGED)
        {
            if (_rb.velocity.y > 0) return;
            if (!_ground.IsGround) return;
            _rb.velocity = Vector2.zero;
        }

        //操作不可時間をカウントする ※参照型!!                      //HACK: 参照型使いたくないなぁ
        ActionLostTime(ref _controlLostTime, ref _changeJumpLostTime);//値が変わります
        _controlLostTime = ActionLostTime(_controlLostTime);
        _changeJumpLostTime = ActionLostTime(_changeJumpLostTime);

        if (State != STATE.NOMAL) return;
        if (_bControl)
        {
            //右へ移動する
            Move();
            //ジャンプする
            if (_canJump) Jump(_jumpPower);
        }
        //ジャンプしてレールを切り替える
        LineChange();
    }

    /// <summary>
    /// Playerとの当たり判定を操作する
    /// </summary>
    void LayerCollision(int layer , bool display)// layerには、判定操作するLayerを　displayには表示するかしないかを
    {
        //falseだと表示なので表示する時はtrueになるよう分かりやすく !displayにした
        Physics2D.IgnoreLayerCollision(_playerLayer ,layer, !display);

        _groundCollid.enabled = display;//火花エフェクトが出ないようにfalseにしている
    }//引数違いのオーバーロード↓　2つ判定操作する際に使う
    void LayerCollision(int layer1,int layer2, bool display)
    {
        Physics2D.IgnoreLayerCollision(_playerLayer, layer1, !display);
        Physics2D.IgnoreLayerCollision(_playerLayer, layer2, !display);

        _groundCollid.enabled = display;
    }

    /// <summary>
    ///行動不可時間をカウントする
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
    /// 自動で右に動く
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
        //初期スピード1 Maxスピード6 ゲージMaxまで６秒
        _rb.velocity = new Vector3(_dashSpeed * (elapsed + _firstSpeed), _rb.velocity.y);
    }

    /// <summary>
    /// ジャンプする
    /// </summary>
    void Jump(float _jump_speed)
    {
        _animator.SetTrigger("Jump");
        _rb.velocity = new Vector2(_rb.velocity.x, _jump_speed);
    }

    /// <summary>
    /// 画面下に落ちた時の処理
    /// </summary>
    async UniTask AsyncFall(CancellationToken ct)
    {
        if (State == STATE.DEATH)//死んでるなら
        {
            _mode = GameMode.GameOver;
            return;
        }

        await UniTask.Delay(_flashInterval * 2 * _loopCount, cancellationToken: ct);

        SpeedGage = 0;

        if (_whereLine == Line.Bottom)
        {
            //TODO:マジックナンバーがあるから修正します　下も同様
            transform.position += new Vector3(-15, 5, 0); //-15 マス　下は-4
            
            Debug.Log("動いた");
        }
        else
        {
            transform.position += new Vector3(-15, 8, 0);//-15 マス　上は１
        }
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _isFalling = false;
    }

    /// <summary>
    /// 線路を変更する
    /// </summary>
    void LineChange()
    {
        //
        if (CanChange())
        {
            _animator.SetTrigger("Jump");
            if (_whereLine == Line.Bottom)//下レールにいる場合
            {
                //引数の値でジャンプする
                Jump(_botmJump);
                _whereLine = Line.Bottom;
            }
            else//上レールにいる場合
            {
                //引数の値でジャンプする
                Jump(_topJump);
                _whereLine = Line.Top;
            }
            //上と下の線路の当たり判定を無視する
            LayerCollision(_topLineLayer, _bottomLineLayer, false);
        }

        //collider2dがfalseならこの後の処理を実行
        if (_groundCollid.enabled != false) return; //NOTE: != false の方が見やすい？？
        //下の線路にいたかつ、ジャンプの最高到達点についたとき
        if (_whereLine == Line.Bottom && IsHighestPoint())
        {
            _whereLine = Line.Top;
            LayerCollision(_topLineLayer, true);//上の線路に当たるようにする

        }//上の線路にいたかつ、上の線路より下になったとき
        else if (_whereLine == Line.Top && transform.position.y < -2f)
        {
            _whereLine = Line.Bottom;
            LayerCollision(_bottomLineLayer, true);//下の線路に当たるようにする
        }
        //線路切り替えのクールタイムを代入している
        _changeJumpLostTime = _changeJumpCoolTime;
    }
    /// <summary>
    /// GetCompornentするものを入れている
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

//以下、可読性が良いか真偽の必要あり-----bool値---------------------------------------------
    /// <summary>
    /// ゴールに向かっている時　（ゴールしていない時）
    /// </summary>
    bool IsHeadedGoal()
    {
        return _goal.position.x > transform.position.x;
    }
    /// <summary>
    /// ジャンプした際の最高到達点
    /// </summary>
    bool IsHighestPoint()
    {
        return _rb.velocity.y <= 0;
    }
    /// <summary>
    /// 線路切り替えが出来るかどうか
    /// </summary>
    bool CanChange()
    {
          bool btnPush = (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0));
        return btnPush && _ground.IsGround && _playerCollid.enabled == true && _canChange;
    }

//-----------当たった時のメソッド------------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            //敵ならメーターを1つ使う
            Destroy_or_Damage(_oneMeter, other);
        }

        if (other.gameObject.tag == "obstacle")
        {
            //障害物ならメーターを2つ使う
            Destroy_or_Damage(_twoMeter, other);
        }
    }

    /// <summary>
    /// メーターによってダメージを受けるか、相手を壊すかを判定する
    /// </summary>
    void Destroy_or_Damage(int meter , Collider2D other)
    {
        //meterよりスピードゲージが溜まっていると
        if (SpeedGage >= meter)
        {
            SpeedGage -= meter;
            other.gameObject.SetActive(false);
        }
        else
        {
            if (State != STATE.NOMAL) return;
            State = STATE.DAMAGED;
            //ダメージを受けた処理　引数にはダメージを受けた値を
            _animator.SetTrigger("Crash");
            Damage(_oneDamage);
            KnockBack();
        }
    }
    /// <summary>
    /// 敵に当たってダメージを受けた時の処理
    /// </summary>
    void Damage(int damage, Action action = null)//HACK:Actionは試しに付けただけ
    {
        //damage分Hpを減らし、UIも更新
        SetHealth(damage);

        //Debug.Log(HpCurrent);

        SpeedGage = 0;

        //IsDeath = HpCurrent <= 0;
        if(HpCurrent<=0)State = STATE.DEATH;

        action?.Invoke();//HACK: 試しに付けただけ
    }
    /// <summary>
    /// ノックバック処理
    /// </summary>
    async void KnockBack()
    {
        //進まないようにする
        _controlLostTime = _knockBackTime;

        //velocityで後ろにノックバックする
        _rb.velocity = _knockBackPower;//NOTE: new vector2でそのまま書いた方が分かりやすい？

        _coinAction.CoinParticle();
        
        //生死判定
        if (State == STATE.DEATH)//後ろにノックバックしてからそのまま落ちるようにしたいのでUpdateに書こうかなぁ？
        {
            //真下に落下してGameOverにしたいので当たり判定を消している
            _playerCollid.enabled = false;

            _animator.SetBool("Death",true);
        }
        else
        {
            var ct = this.GetCancellationTokenOnDestroy();
            await AsyncFlash(ct);//点滅する

            //15マス後ろに行く
            transform.position += new Vector3(-15, 1, 0);
            State = STATE.NOMAL;
        }
    }
    /// <summary>
    /// 残機を減らし、UIにも反映する
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
    /// 点滅する処理
    /// </summary>
    async UniTask AsyncFlash(CancellationToken ct)
    {
        for (int i = 0; i < _loopCount; i++)
        {
            Debug.Log(_spriteRenderer.color.a);

            _spriteRenderer.color += new Color(0,0,0,-100);
            await UniTask.Delay(_flashInterval, cancellationToken:ct);

            Debug.Log(i+"回目");

            _spriteRenderer.color += new Color(0,0,0,100);
            await UniTask.Delay(_flashInterval, cancellationToken:ct);


        }
    }

}
