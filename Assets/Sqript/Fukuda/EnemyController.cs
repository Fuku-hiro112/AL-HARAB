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
        //�I�u�W�F�N�g�ƃv���C���[�̋���
        _distance = transform.position.x - _player.position.x;
        if (_distance < -4)
        {
            gameObject.SetActive(false);
        }
    }
}
