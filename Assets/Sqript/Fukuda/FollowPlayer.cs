using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform Player;
    float followObj_posX;
    int player_diff = 6; //Player�Ƃ̋����̍��@�idiff = difference�j

    void Update()
    {
        //�Ǐ]����I�u�W�F�N�g��X���W
        followObj_posX = Player.position.x + player_diff;
        
        transform.position = new Vector2(followObj_posX, transform.position.y);
    }
}
