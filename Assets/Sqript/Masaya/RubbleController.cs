using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleController : MonoBehaviour
{
    public Transform playerTransform;  // �v���C���[��Transform
    public float detectionDistance = 7f;  // �G���ړ����J�n����v���C���[�Ƃ̋���

    private bool isMoving = false;  // �ړ������ǂ���
    private float moveSpeed = 10f;   // �ړ����x
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
                targetPosition = new Vector3(transform.position.x, transform.position.y - 8f, transform.position.z);  // ���Ɉړ�����ʒu��ݒ�
            }
        }

        // �ړ�����
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // �ڕW�ʒu�ɓ��B������ړ��I��
            if (transform.position.y == 0)
            {
                isMoving = false;
            }
        }

    }
}
