using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SparkAction : MonoBehaviour
{
    //速度によって火花の量を変えたい
    //SparkParticle.ParticleSystem.Emission.RateoverTime
    PlayerAction _playerAction;
    private GroundCheck _ground;
    private bool _isGround ;
    float _speed;

    ParticleSystem _particleSystem;
    private EmissionModule _emissionModule;

    void Start()
    {
        _playerAction = GameObject.Find("Player").GetComponent<PlayerAction>();
        _ground = GameObject.Find("GroundCheck").GetComponent<GroundCheck>();
        _particleSystem = GameObject.Find("SparkParticle").GetComponent<ParticleSystem>();

        // パーティクルシステムのEmissionモジュールを取得
        _emissionModule = _particleSystem.emission;
    }
    void Update()
    {
        _isGround = _ground.IsGroundJudg();

        Debug.Log(_isGround);

        if (_isGround)
        {
            // EmissionのRateOverTimeの値を更新
            _emissionModule.rateOverTime = _speed / 6 * 15;

        }
        else
        {
            _emissionModule.rateOverTime = 0;
        }

        _speed = _playerAction.SpeedGage;

        Debug.Log(_speed);

        //TODO:GroundCheckが反応しているので線路切り替え時はCollider２Dをオフにする
        //GroundCheckのスクリプトを理解する
    }
}
