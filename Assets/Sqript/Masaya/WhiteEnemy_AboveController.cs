using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteEnemy_AboveController : MonoBehaviour
{
    public Transform playerTransform;  // �v���C���[��Transform
    public float detectionDistance = 20f;  // �G���ړ����J�n����v���C���[�Ƃ̋���

    private bool isMoving = false;  // �ړ������ǂ���
    private float moveSpeed = 3f;   // �ړ����x
    private Vector3 targetPosition; // �ړ���̈ʒu

    private void Start()
    {
        targetPosition = transform.position; // �����ʒu���ړ���̈ʒu�Ƃ���
    }

    private void Update()
    {
        // �v���C���[�Ƃ̋������w�肵���͈͓��ɓ������ꍇ�A�ړ����J�n����
        if (Vector3.Distance(transform.position, playerTransform.position) <= detectionDistance)
        {
            if (!isMoving)
            {
                isMoving = true;
                targetPosition = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);  // ���Ɉړ�����ʒu��ݒ�
            }
        }

        // �ړ�����
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // �ڕW�ʒu�ɓ��B������ړ��I��
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