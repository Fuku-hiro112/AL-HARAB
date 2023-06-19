using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    //物理判定の更新毎に呼ぶ必要がある
    public bool IsGround()
    {
        //Debug.Log("開いたよ");
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
            Debug.Log("isGroundEnterだよ");
        }
        else if (isGroundExit)
        {
            isGround = false;
            Debug.Log("isGroundExitだよ");
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;

        return isGround;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Enterはした");
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
