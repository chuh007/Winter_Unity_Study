using UnityEngine;

public class ParticleEffect : MonoBehaviour, IPoolable
{
    [SerializeField] private ParticleSystem _particle;
    [field: SerializeField] public PoolTypeSO PoolType { get; private set; } 
    //어디 누구처럼 private set 빼먹지 말기!!
    public GameObject GameObject => gameObject;
    private Pool _myPool;
    public void SetUpPool(Pool pool)
    {
        _myPool = pool;
    }

    public void ResetItem()
    {
        _particle.Simulate(0);
    }
    
    public void PlayParticle()
    {
        _particle.Play(true);
    }

    public void StopParticle()
    {
        _particle.Stop();
    }
}
