using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �C���X�y�N�^�[�Őݒ�
    public float speed = 5;
    public GroundCheck ground;

    //�v���C�x�[�g�ϐ�
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;

    // �W�����v�̗͂��w�肵�܂��B
    [SerializeField]
    private float jumpPower = 20;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�ݒu����𓾂�
        isGround = ground.IsGround();
    }

   

}
