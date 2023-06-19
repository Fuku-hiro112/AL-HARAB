using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    //��������̍X�V���ɌĂԕK�v������
    public bool IsGround()
    {
        //Debug.Log("�J������");
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

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Enter�͂���");
        if (collision.tag == groundTag)
        {
            isGroundEnter = true;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundStay = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundExit = true;
        }
    }
}
