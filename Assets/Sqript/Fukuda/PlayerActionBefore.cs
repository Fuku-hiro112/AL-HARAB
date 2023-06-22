using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionBefore: MonoBehaviour
{
    public float speed_elapsed_time = 0;
    [SerializeField] float speed = 0.2f;

    [SerializeField] float jump_speed = 7.5f;
    [SerializeField] float line_change_jump = 2.5f;

    [SerializeField] GameObject goal;

    float goal_position;

    public static float time;

    private bool can_move;
    

    void Start()
    {
        goal_position = goal.transform.position.x;

        //goal_position < transform.position.x
        can_move = true;
    }


    void Update()
    {
        if (can_move)
        {
            Move();
        }

        if (Headed_goal())
        {
            time += Time.deltaTime;
        }

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Jamp(jump_speed);
        }*/
    }

    /// <summary>
    /// �E�ɓ���
    /// </summary>
    void Move()
    {
        bool max_speed = speed_elapsed_time > 6;

        if (max_speed)
        {
            speed_elapsed_time = 6;
        }
        else
        {
            speed_elapsed_time += Time.deltaTime;
        }

        //�g���b�R���E�Ɉړ����鑬�� �ړ�
        Vector2 trolley_speed = new Vector2(speed_elapsed_time * speed, 0);
        transform.Translate(trolley_speed);
        //transform.position = trolley_speed + transform.position;//transform.position�ŏ��������ǃG���[���ł��̂ł�߂�
    }
/// <summary>
/// 
/// </summary>
    void Jamp(float _jump_speed)
    {
        //rb.velocity = new Vector2(rb.velocity.x, _jump_speed);
        //transform.Translate(0,Mathf.Cos(Time.time));
    }
    /// <summary>
    /// �S�[���Ɍ������Ă��鎞�@�i�S�[�����Ă��Ȃ����j
    /// </summary>
    bool Headed_goal()
    {
        return goal_position > transform.position.x;
    }
}
