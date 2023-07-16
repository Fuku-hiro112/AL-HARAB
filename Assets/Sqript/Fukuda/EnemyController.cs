using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //SpriteRenderer _thisSprite;
    private Transform _player;
    private float _distance;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        //オブジェクトとプレイヤーの距離
        _distance = transform.position.x - _player.position.x;
        if (_distance < -4)
        {
            gameObject.SetActive(false);
        }
    }
}
