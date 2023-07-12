using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �C���X�y�N�^�[�Őݒ�
    public float speed; //���x
    public SpriteRenderer sp;
    public float jumpSpeed; //�W�����v���x
    public float jumpHeight;//��������
    public float gravity; //�d��
    public GroundCheck ground; //�ڒn����
    public float jumpPower = 100f;



    //�v���C�x�[�g�ϐ�
    private Rigidbody2D rb = null;
    private bool isDamage { get; set; }



    void Speed()
    {
        //�E�ɓ��������^������
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


    //�Փ˔���̕ϐ����쐬
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

        //�m�b�N�o�b�N
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

        // �_���[�W���󂯂Ă���ꍇ�A�_�ł�����
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
            //��{�^����������Ă���B���A���݂̍������W�����v�����ʒu����
            //�����̌��߂��ʒu��艺�Ȃ�W�����v���p������
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

        //�ڒn����𓾂�
        isGround = ground.IsGround();*/



    /// <summary>
    /// ������΂��p�̊֐�
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
    //�m�b�N�o�b�N���A�_�ł��鏈��
    //</summary>
    //<param name="collision"></param>

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(OnDamage());
    }

    public IEnumerator OnDamage()
    {
        yield return new WaitForSeconds(3.0f);

        // �ʏ��Ԃɖ߂�
        isDamage = false;
        sp.color = new Color(1f, 1f, 1f, 1f);
    }
}
