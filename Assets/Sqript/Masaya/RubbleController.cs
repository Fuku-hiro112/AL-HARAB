using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleController : MonoBehaviour
{
    public Transform playerTransform;  // �v���C���[��Transform
    public Transform Rubbles; //�@�R�̊��I�̐e�I�u�W�F�N�g
    public Transform RubbleCluster; //�@��̊��I�̉�


    public float dropSpeed = 10f;   // �������x
    public float detectionDistance = 7f;  // ���I���������J�n����v���C���[�Ƃ̋���
    private bool isDropped = false;  // ���������ǂ���
    private Vector3 targetPosition; // ������̈ʒu

    private void Start()
    {
        targetPosition = transform.position; // �����ʒu�𗎉���̈ʒu�Ƃ���
    }

    private void Update()
    {
        // �v���C���[�Ƃ̋������w�肵���͈͓��ɓ������ꍇ�A�������J�n����
        if (Vector3.Distance(transform.position, playerTransform.position) <= detectionDistance)
        {
            if (!isDropped)
            {
                isDropped = true;
                targetPosition = new Vector3(transform.position.x, transform.position.y - 7.5f, transform.position.z);  // ���ɗ�������ʒu��ݒ�
            }
        }

        // ��������
        if (isDropped)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, dropSpeed * Time.deltaTime);

            // �ڕW�ʒu�ɓ��B������ړ��I��
            if (transform.position.y == 0)
            {
                isDropped = false;
            }
        }

        //�@�R�̊��I�������A���I�̉�𐶐�
        RubbleCluster.gameObject.SetActive(true);
        Rubbles.gameObject.SetActive(false);
    }
}

