using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    //��������̍X�V���ɌĂԕK�v������
    /// <summary>
    /// �n�ʂɐݒu���Ă��邩�𔻒肷��
    /// </summary>
    public bool IsGround()
    {
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
            Debug.Log("isGroundEnter����");
        }
        else if (isGroundExit)
        {
            isGround = false;
            Debug.Log("isGroundExit����");
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;

        return isGround;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundEnter = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundStay = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundExit = true;
        }
    }
}
