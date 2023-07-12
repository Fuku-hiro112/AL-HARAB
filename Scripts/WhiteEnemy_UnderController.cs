using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteEnemy_UnderController : MonoBehaviour
{
    public Transform playerTransform; // �v���C���[��Transform���i�[����ϐ�
    public float detectionRange = 20f; // �v���C���[�Ƃ̋����̂������l
    public float moveDistance = 10f; //��Ɉړ����鋗��
    public float moveSpeed = 1.2f;

    private bool hasMoved = false; // ��Ɉړ��������ǂ���

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
        // �v���C���[�Ƃ̋������v�Z
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // �v���C���[�����̋����ȉ��ɋ߂Â����ꍇ����Ɉړ����Ă��Ȃ��ꍇ
        if (distance <= detectionRange && !hasMoved)
        {
            // ��Ɉړ�
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

