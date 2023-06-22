using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform Player;
    float followObj_posX;
    int player_diff = 6; //Playerとの距離の差　（diff = difference）

    void Update()
    {
        //追従するオブジェクトのX座標
        followObj_posX = Player.position.x + player_diff;
        
        transform.position = new Vector2(followObj_posX, transform.position.y);
    }
}
