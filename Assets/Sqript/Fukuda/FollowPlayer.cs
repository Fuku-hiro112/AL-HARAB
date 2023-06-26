using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform Player;
    private float _followObjPosX;
    private int _playerDiff = 6; //Playerとの距離の差　（diff = difference）

    void Update()
    {
        //追従するオブジェクトのX座標
        _followObjPosX = Player.position.x + _playerDiff;
        
        transform.position = new Vector2(_followObjPosX, transform.position.y);
    }
}
