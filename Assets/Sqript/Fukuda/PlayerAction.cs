using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// private変数は　_を付けてcamelCasing（キャメルケース）を仕様する
/// public変数は　A最初大文字　パスカル ケース(Pascal)
/// ローカル変数は　
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

        //操作不可時間をカウントする
        if (ControlLostTime > 0)
        {
            ControlLostTime -= Time.deltaTime;
        }

        if (bControl == true)
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
        if (( Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) 
            && _isGround && _collider2d.enabled == true)
        {
            if (_whereLine == Line.Bottom)
            {
                //Debug.Log("下");
                Jump(_botmJump);
                _whereLine = Line.Bottom;
            }
            else
            {
                //Debug.Log("上");
                Jump(_topJump);
                _whereLine = Line.Top;
            }
            _collider2d.enabled = false;
        }

        //collider2dがfalseならこの後の処理を実行
        if (_collider2d.enabled != false) return;
        //下の線路にいた、ジャンプの最高到達点についたとき
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
    /// 右に動く
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
        //Debug.Log(_jump_speed);
        _rb.velocity = new Vector2(_rb.velocity.x, _jump_speed);
    }

    void Damage(int damage)
    {
        SetHealth(damage);
        
        //Debug.Log(HpCurrent);
        //進まないようにする
        ControlLostTime = _knockBackTime;

        SpeedGage = 0;
        _rb.velocity = _knockBackPower;

        //TODO:無敵○○秒、その時間点滅

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
    /// メーターによってダメージを受けるか、相手を壊すかを判定する
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
            //15マス後ろから出現
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

}
