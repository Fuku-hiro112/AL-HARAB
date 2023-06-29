using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using static System.Net.Mime.MediaTypeNames;


// private変数は　_を付けてcamelCasing（キャメルケース）を仕様する
// public変数は　A最初大文字　パスカル ケース(Pascal)
// ローカル変数は　普通にcamelCasing（キャメルケース）

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

    // LayerIDを取得
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

        // LayerIDを取得
        _topLineLayer = LayerMask.NameToLayer("TopLine");
        _bottomLineLayer = LayerMask.NameToLayer("BottomLine");
        _playerLayer = LayerMask.NameToLayer("Player");

        LayerCollision(_topLineLayer,_bottomLineLayer,true);

        HpCurrent =  3;
        _whereLine = Line.Top;
    }

    void Update()
    {
        //TODO:これどうにかしよう
        _isGround = _ground.IsGroundJudg();

        _canJump = Input.GetKeyDown(KeyCode.Space) && _isGround;
        _bControl = _controlLostTime <= 0;
        _canChange = _chageJampLostTime <= 0;


        //操作不可時間をカウントする　※参照型!!
        ActionLostTime(ref _controlLostTime);//値が変わります
        ActionLostTime(ref _chageJampLostTime);//値が変わります

        if (_bControl == true)
        {
            Move();

            if (_canJump) Jump(_jumpPower);
        }

        if (IsHeadedGoal())//ゴールするまで
        {
            //経過時間を足す
            ClearTime += Time.deltaTime;
        }

        //TODO:ジャンプによって角度が変わる
        /*if (jumped)
        {
            transform.rotation = new Quaternion(,,);
        }*/
        //したレーン

        if (CanChange())
        {
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
            //
            LayerCollision(_topLineLayer, _bottomLineLayer, false);
        }

        //collider2dがfalseならこの後の処理を実行
        if (_groundCollid.enabled != false) return;
        //下の線路にいたかつ、ジャンプの最高到達点についたとき
        if (_whereLine == Line.Bottom && IsHighestPoint())
        {
            _whereLine = Line.Top;
            LayerCollision(_topLineLayer, true);
        }
        //上の線路にいたかつ、上の線路より下になったとき
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
        //falseだと表示なので表示する時はtrueになるよう分かりやすく !displayにした
        Physics2D.IgnoreLayerCollision(_playerLayer ,layer, !display);

        _groundCollid.enabled = display;//火花エフェクトが出ないようにfalseにしている
    }
    void LayerCollision(int layer1,int layer2, bool display)
    {
        Physics2D.IgnoreLayerCollision(_playerLayer, layer1, !display);
        Physics2D.IgnoreLayerCollision(_playerLayer, layer2, !display);

        _groundCollid.enabled = display;//火花エフェクトが出ないようにfalseにしている
    }

    /// <summary>
    ///行動不可時間をカウントする
    /// </summary>
    void ActionLostTime(ref float lostTime)
    {
        if (lostTime > 0)
        {
            lostTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 自動で右に動く
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
        //FIXME:マジックナンバーを削除、数式の簡略化　初期スピード1 Maxスピード6 ゲージMaxまで６秒
        _rb.velocity = new Vector3(_dashSpeed * (SpeedGage * 5/6 + 1f), _rb.velocity.y);
    }

    /// <summary>
    /// ジャンプする
    /// </summary>
    void Jump(float _jump_speed)
    {
        //どっちも変わらない？？
        _rb.velocity = new Vector2(_rb.velocity.x, _jump_speed);
        //_rb.AddForce(transform.up * _jump_speed,ForceMode2D.Impulse);
    }

    void Damage(int damage)
    {
        //damage分Hpを減らし、UIも更新
        SetHealth(damage);
        
        //Debug.Log(HpCurrent);
        //進まないようにする
        _controlLostTime = _knockBackTime;

        SpeedGage = 0;
        _rb.velocity = _knockBackPower;

        //TODO:無敵○○秒、その時間点滅

    }

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
            Destroy(other.gameObject);
        }
        else
        {
            Damage(_damage1);
            //TODO:15マス後ろから出現
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

    //以下、可読性が良いか真偽の必要あり
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
