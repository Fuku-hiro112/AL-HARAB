using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform Player;
    private float _followObjPosX;
    private int _playerDiff = 6; //Player�Ƃ̋����̍��@�idiff = difference�j

    void Update()
    {
        //�Ǐ]����I�u�W�F�N�g��X���W
        _followObjPosX = Player.position.x + _playerDiff;
        
        transform.position = new Vector2(_followObjPosX, transform.position.y);
    }
}
