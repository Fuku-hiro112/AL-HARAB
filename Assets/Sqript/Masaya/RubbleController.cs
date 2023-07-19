using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleController : MonoBehaviour
{
    public Transform playerTransform;  // プレイヤーのTransform
    public Transform Rubbles; //　３つの瓦礫の親オブジェクト
    public Transform RubbleCluster; //　一つの瓦礫の塊


    public float dropSpeed = 10f;   // 落下速度
    public float detectionDistance = 7f;  // 瓦礫が落下を開始するプレイヤーとの距離
    private bool isDropped = false;  // 落下中かどうか
    private Vector3 targetPosition; // 落下先の位置

    private void Start()
    {
        targetPosition = transform.position; // 初期位置を落下先の位置とする
    }

    private void Update()
    {
        // プレイヤーとの距離が指定した範囲内に入った場合、落下を開始する
        if (Vector3.Distance(transform.position, playerTransform.position) <= detectionDistance)
        {
            if (!isDropped)
            {
                isDropped = true;
                targetPosition = new Vector3(transform.position.x, transform.position.y - 7.5f, transform.position.z);  // 下に落下する位置を設定
            }
        }

        // 落下処理
        if (isDropped)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dropSpeed * Time.deltaTime);

            // 目標位置に到達したら移動終了
            if (transform.position.y == 0)
            {
                isDropped = false;
            }
        }

        //　３つの瓦礫を除去、瓦礫の塊を生成
        RubbleCluster.gameObject.SetActive(true);
        Rubbles.gameObject.SetActive(false);
    }
}

