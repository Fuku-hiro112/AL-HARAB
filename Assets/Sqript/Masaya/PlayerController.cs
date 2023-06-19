using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // インスペクターで設定
    public float speed; //速度
    public float jumpSpeed; //ジャンプ速度
    public float jumpHeight;//高さ制限
    public float gravity; //重力
    public GroundCheck ground;　//接地判定
    public AnimationCurve dashCurve; //加速
    public AnimationCurve jumpCurve;

    // ジャンプの力を指定
    private float jumpPower = 50;

    //プライベート変数
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isJump = false;
    private float jumpPos = 0.0f;
    private float dashTime = 0.0f;
    private float jumpTime = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //右に等速直線運動する
        const int spped = 30;
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * spped);
        GetComponent<Rigidbody2D>().velocity = Vector2.right;
        dashTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.up * jumpPower;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
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
            /*上ボタンを押されている。かつ、現在の高さがジャンプした位置から
            自分の決めた位置より下ならジャンプを継続する*/
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


        //設置判定を得る
        isGround = ground.IsGround();

        //アニメーションカーブを速度に適用

        xSpeed *= dashCurve.Evaluate(dashTime);
    }
        

 }
