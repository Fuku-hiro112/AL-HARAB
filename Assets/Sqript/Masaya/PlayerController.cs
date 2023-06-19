using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �C���X�y�N�^�[�Őݒ�
    public float speed; //���x
    public float jumpSpeed; //�W�����v���x
    public float jumpHeight;//��������
    public float gravity; //�d��
    public GroundCheck ground;�@//�ڒn����
    public AnimationCurve dashCurve; //����
    public AnimationCurve jumpCurve;

    // �W�����v�̗͂��w��
    private float jumpPower = 50;

    //�v���C�x�[�g�ϐ�
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
        //�E�ɓ��������^������
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
                jumpPos = transform.position.y; //�W�����v�����ʒu���L�^����
                isJump = true;
            }
            else
            {
                isJump = false;
            }
        }
        else if (isJump)
        {
            /*��{�^����������Ă���B���A���݂̍������W�����v�����ʒu����
            �����̌��߂��ʒu��艺�Ȃ�W�����v���p������*/
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


        //�ݒu����𓾂�
        isGround = ground.IsGround();

        //�A�j���[�V�����J�[�u�𑬓x�ɓK�p

        xSpeed *= dashCurve.Evaluate(dashTime);
    }
        

 }
