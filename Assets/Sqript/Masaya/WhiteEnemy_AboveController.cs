using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteEnemy_AboveController : MonoBehaviour
{
    public Transform playerTransform;  // プレイヤーのTransform
    public float detectionDistance = 20f;  // 敵が移動を開始するプレイヤーとの距離

    private bool isMoving = false;  // 移動中かどうか
    private float moveSpeed = 3f;   // 移動速度
    private Vector3 targetPosition; // 移動先の位置

    private void Start()
    {
        targetPosition = transform.position; // 初期位置を移動先の位置とする
    }

    private void Update()
    {
        // プレイヤーとの距離が指定した範囲内に入った場合、移動を開始する
        if (Vector3.Distance(transform.position, playerTransform.position) <= detectionDistance)
        {
            if (!isMoving)
            {
                isMoving = true;
                targetPosition = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);  // 下に移動する位置を設定
            }
        }

        // 移動処理
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 目標位置に到達したら移動終了
            if (transform.position.y == -3)
            {
                isMoving = false;
            }
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