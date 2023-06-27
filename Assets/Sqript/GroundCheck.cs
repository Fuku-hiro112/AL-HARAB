using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string _groundTag = "Ground";
    public bool _isGround = false;
    private bool _isGroundEnter, _isGroundStay, _isGroundExit;

    //��������̍X�V���ɌĂԕK�v������
    /// <summary>
    /// �n�ʂɐݒu���Ă��邩�𔻒肷��
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
