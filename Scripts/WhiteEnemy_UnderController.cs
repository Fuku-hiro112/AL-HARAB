using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteEnemy_UnderController : MonoBehaviour
{
    public Transform playerTransform; // プレイヤーのTransformを格納する変数
    public float detectionRange = 20f; // プレイヤーとの距離のしきい値
    public float moveDistance = 10f; //上に移動する距離
    public float moveSpeed = 1.2f;

    private bool hasMoved = false; // 上に移動したかどうか

    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーとの距離を計算
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // プレイヤーが一定の距離以下に近づいた場合かつ上に移動していない場合
        if (distance <= detectionRange && !hasMoved)
        {
            // 上に移動
            transform.Translate(Vector2.up * moveDistance * moveSpeed);
            hasMoved = true; 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {


            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.KnockBack(transform.position);




        }
    }
}

