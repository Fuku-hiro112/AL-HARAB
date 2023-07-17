using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleController : MonoBehaviour
{
    public Transform playerTransform;  // プレイヤーのTransform
    public float detectionDistance = 7f;  // 敵が移動を開始するプレイヤーとの距離

    private bool isMoving = false;  // 移動中かどうか
    private float moveSpeed = 10f;   // 移動速度
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
                targetPosition = new Vector3(transform.position.x, transform.position.y - 8f, transform.position.z);  // 下に移動する位置を設定
            }
        }

        // 移動処理
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 目標位置に到達したら移動終了
            if (transform.position.y == 0)
            {
                isMoving = false;
            }
        }

    }
}
