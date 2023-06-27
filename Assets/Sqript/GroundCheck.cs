using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string _groundTag = "Ground";
    public bool _isGround = false;
    private bool _isGroundEnter, _isGroundStay, _isGroundExit;

    //物理判定の更新毎に呼ぶ必要がある
    /// <summary>
    /// 地面に設置しているかを判定する
    /// </summary>
    public bool IsGroundJudg()
    {
        if (_isGroundEnter || _isGroundStay)
        {
            _isGround = true;
        }
        else if (_isGroundExit)
        {
            _isGround = false;
        }

        _isGroundEnter = false;
        _isGroundStay = false;
        _isGroundExit = false;

        return _isGround;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _groundTag)
        {
            _isGroundEnter = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == _groundTag)
        {
            _isGroundStay = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == _groundTag)
        {
            _isGroundExit = true;
        }
    }
}
