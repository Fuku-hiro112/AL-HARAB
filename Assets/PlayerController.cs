using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �ʏ�̈ړ����x���w�肵�܂��B
    [SerializeField]
    private float speed = 5;

    // �W�����v�̗͂��w�肵�܂��B
    [SerializeField]
    private float jumpPower = 20;

    // �ݒu����̍ۂɔ���ΏۂƂȂ郌�C���[���w�肵�܂��B
    [SerializeField]
    private LayerMask groundLayer = 0;

    /// �v���C���̏ꍇ��true�A�X�e�[�W�J�n�O�܂��̓Q�[���I�[�o�[���ɂ�false
    /// </summary>
    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }
    bool isActive = false;

    // ���n���Ă���ꍇ��true�A�W�����v����false
    [SerializeField]
    bool isGrounded = false;

    // �ݒu����p�̃G���A
    Vector3 groundCheckA, groundCheckB;

    // �R���|�[�l���g�����O�ɎQ�Ƃ��Ă����ϐ�
    Animator animator;
    new Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // ���O�ɃR���|�[�l���g���Q��
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        

        // Box Collider 2D�̔���G���A���擾
        var collider = GetComponent<BoxCollider2D>();
        // �R���C�_�[�̒��S���W�ւ��炷
        groundCheckA = collider.offset;
        groundCheckB = collider.offset;
        // �R���C�_�[��bottom�ւ��炷
        groundCheckA.y += -collider.size.y * 0.5f;
        groundCheckB.y += -collider.size.y * 0.5f;
        // �͈͂�����
        Vector2 size = collider.size;
        size.x *= 0.75f;    // ����
        size.y *= 0.25f;    // ������4����1
        // �R���C�_�[�̉��������֍��E�ɂ��炷
        groundCheckA.x += -size.x * 0.5f;
        groundCheckB.x += size.x * 0.5f;
        // �R���C�_�[�̍��������֏㉺�ɂ��炷
        groundCheckA.y += -size.y * 0.5f;
        groundCheckB.y += size.y * 0.5f;
    }



    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            // x�������̈ړ�
            var velocity = rigidbody.velocity;
            velocity.x = speed;
            rigidbody.velocity = velocity;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                // �W�����v��Ԃɐݒ�
                isGrounded = false;
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

}
