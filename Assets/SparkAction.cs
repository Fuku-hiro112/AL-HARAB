using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SparkAction : MonoBehaviour
{
    //���x�ɂ���ĉΉԂ̗ʂ�ς�����
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

        // �p�[�e�B�N���V�X�e����Emission���W���[�����擾
        _emissionModule = _particleSystem.emission;
    }
    void Update()
    {
        _isGround = _ground.IsGroundJudg();

        Debug.Log(_isGround);

        if (_isGround)
        {
            // Emission��RateOverTime�̒l���X�V
            _emissionModule.rateOverTime = _speed / 6 * 15;

        }
        else
        {
            _emissionModule.rateOverTime = 0;
        }

        _speed = _playerAction.SpeedGage;

        Debug.Log(_speed);

        //TODO:GroundCheck���������Ă���̂Ő��H�؂�ւ�����Collider�QD���I�t�ɂ���
        //GroundCheck�̃X�N���v�g�𗝉�����
    }
}
