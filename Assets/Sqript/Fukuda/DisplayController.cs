using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayController : MonoBehaviour
{
    Transform _player;
    float distance;
    [SerializeField]
    enum Object
    {
        Enemy,
        Obstacle,
        LineHole
    }
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
