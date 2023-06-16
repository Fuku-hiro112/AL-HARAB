using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 通常の移動速度を指定します。
    [SerializeField]
    private float speed = 5;

    // ジャンプの力を指定します。
    [SerializeField]
    private float jumpPower = 20;

    // 設置判定の際に判定対象となるレイヤーを指定します。
    [SerializeField]
    private LayerMask groundLayer = 0;

    /// プレイ中の場合はtrue、ステージ開始前またはゲームオーバー時にはfalse
    /// </summary>
    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }
    bool isActive = false;

    // 着地している場合はtrue、ジャンプ中はfalse
    [SerializeField]
    bool isGrounded = false;

    // 設置判定用のエリア
    Vector3 groundCheckA, groundCheckB;

    // コンポーネントを事前に参照しておく変数
    Animator animator;
    new Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // 事前にコンポーネントを参照
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        

        // Box Collider 2Dの判定エリアを取得
        var collider = GetComponent<BoxCollider2D>();
        // コライダーの中心座標へずらす
        groundCheckA = collider.offset;
        groundCheckB = collider.offset;
        // コライダーのbottomへずらす
        groundCheckA.y += -collider.size.y * 0.5f;
        groundCheckB.y += -collider.size.y * 0.5f;
        // 範囲を決定
        Vector2 size = collider.size;
        size.x *= 0.75f;    // 横幅
        size.y *= 0.25f;    // 高さは4分の1
        // コライダーの横幅方向へ左右にずらす
        groundCheckA.x += -size.x * 0.5f;
        groundCheckB.x += size.x * 0.5f;
        // コライダーの高さ方向へ上下にずらす
        groundCheckA.y += -size.y * 0.5f;
        groundCheckB.y += size.y * 0.5f;
    }



    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            // x軸方向の移動
            var velocity = rigidbody.velocity;
            velocity.x = speed;
            rigidbody.velocity = velocity;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                // ジャンプ状態に設定
                isGrounded = false;
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

}
