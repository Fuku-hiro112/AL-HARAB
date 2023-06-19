using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    //物理判定の更新毎に呼ぶ必要がある
    /// <summary>
    /// 地面に設置しているかを判定する
    /// </summary>
    public bool IsGround()
    {
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
