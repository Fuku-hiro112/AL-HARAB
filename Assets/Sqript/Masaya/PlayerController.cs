using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // インスペクターで設定
    public float speed; //速度
    public SpriteRenderer sp;
    public float jumpSpeed; //ジャンプ速度
    public float jumpHeight;//高さ制限
    public float gravity; //重力
    public GroundCheck ground; //接地判定
    public float jumpPower = 100f;



    //プライベート変数
    private Rigidbody2D rb = null;
    private bool isDamage { get; set; }



    void Speed()
    {
        //右に等速直線運動する
        const int speed = 100;
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed);
        GetComponent<Rigidbody2D>().velocity = Vector2.right;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0)
        {
            rb.velocity = Vector2.up * jumpPower;
            SoundManager.Instance.PlaySE(SESoundData.SE.Jump);
        }

    }


    //衝突判定の変数を作成
    private bool isknockingback;
    private Vector2 knockDir;

    [SerializeField]
    private float knockbackTime, knockbackForce;
    private float knockbackCounter;

    [SerializeField]
    private float invincibilityTime;
    private float invincibilityCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();

    }


    void Update()
    {
        Speed();
        Jump();

        //ノックバック
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
        }

        if (isknockingback)
        {
            knockbackCounter -= Time.deltaTime;
            rb.velocity = knockDir * knockbackForce;

            if (knockbackCounter <= 0)
            {
                isknockingback = false;
            }
            else
            {
                return;
            }
        }

        // ダメージを受けている場合、点滅させる
        if (isDamage)
        {

            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            sp.color = new Color(1f, 1f, level);


        }


    }


    void FixedUpdate()
    {

    }

    /*void JumpController()

    {

        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = speed;

        float ySpeed = -gravity;
        float verticalKey = Input.GetAxis("Vertical");

        if (isGround)
        {
            if (verticalKey > 0)
            {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //ジャンプした位置を記録する
                isJump = true;
            }
            else
            {
                isJump = false;
            }
        }

        else if (isJump)
        {
            //上ボタンを押されている。かつ、現在の高さがジャンプした位置から
            //自分の決めた位置より下ならジャンプを継続する
            if (verticalKey > 0 && jumpPos + jumpHeight > transform.position.y)
            {
                ySpeed = jumpSpeed;
            }
            else
            {
                isJump = false;
            }
        }
        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("run", true);
            xSpeed = speed;
        }
        else if (horizontalKey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetBool("run", true);
            xSpeed = -speed;
        }
        else
        {
            anim.SetBool("run", false);
            xSpeed = 0.0f;
        }
        rb.velocity = new Vector2(xSpeed, ySpeed);

        //接地判定を得る
        isGround = ground.IsGround();*/



    /// <summary>
    /// 吹き飛ばし用の関数
    /// </summary>
    /// <param name="position"></param>
    public void KnockBack(Vector3 position)
    {
        knockbackCounter = knockbackTime;
        isknockingback = true;

        knockDir = transform.position - position;

        knockDir.Normalize();
    }

    /// <summary>
    //ノックバック時、点滅する処理
    //</summary>
    //<param name="collision"></param>

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(OnDamage());
    }

    public IEnumerator OnDamage()
    {
        yield return new WaitForSeconds(3.0f);

        // 通常状態に戻す
        isDamage = false;
        sp.color = new Color(1f, 1f, 1f, 1f);
    }
}
