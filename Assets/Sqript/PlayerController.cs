using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // インスペクターで設定
    public float speed = 5;
    public GroundCheck ground;

    //プライベート変数
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;

    // ジャンプの力を指定します。
    [SerializeField]
    private float jumpPower = 20;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //設置判定を得る
        isGround = ground.IsGround();
    }

   

}
