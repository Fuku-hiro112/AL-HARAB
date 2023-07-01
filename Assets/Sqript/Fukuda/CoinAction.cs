using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class CoinAction : MonoBehaviour
{
    private ParticleSystem _particle;
    [SerializeField] Transform _coinPos;

    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
        _particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async UniTask AsyncCoinParticle(float nockBack, CancellationToken ct = default)
    {
        transform.position = _coinPos.position;
        _particle.Play();

        await UniTask.Delay(TimeSpan.FromSeconds(nockBack));

        _particle.Stop();
    }
}
