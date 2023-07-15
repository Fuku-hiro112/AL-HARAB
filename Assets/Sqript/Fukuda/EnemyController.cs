using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //SpriteRenderer _thisSprite;
    Transform _player;
    float distance;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        //オブジェクトとプレイヤーの距離
        distance = transform.position.x - _player.position.x;
        if (distance < -4)
        {
            gameObject.SetActive(false);
        }
    }
}
