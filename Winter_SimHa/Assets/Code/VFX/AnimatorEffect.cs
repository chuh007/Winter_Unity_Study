using UnityEngine;

public class AnimatorEffect : MonoBehaviour, IPoolable
{
    [SerializeField] private string _clipName;
    private int _clipNameHash;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private readonly int _hdrColorHash = Shader.PropertyToID("_EmissionColor");
    
    [field:SerializeField] public PoolTypeSO PoolType { get; private set; }
    public GameObject GameObject => gameObject;
    private Pool _myPool;
    public void SetUpPool(Pool pool)
    {
        _myPool = pool;
    }

    public void ResetItem()
    {
        _animator.enabled = false;
    }
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _clipNameHash = Animator.StringToHash(_clipName);
    }

    public void SetEffectColor(Color color)
    {
        Material mat = _spriteRenderer.material;
        mat.SetColor(_hdrColorHash, color);
    }
    
    public void PlayAnimation(Vector3 position, Quaternion rot, Vector3 scale)
    {
        transform.localRotation = rot;
        transform.position = position;
        transform.localScale = scale;
        _animator.enabled = true;
        _animator.Play(_clipNameHash);
    }

    private void OnAnimationEndTrigger()
    {
        _myPool.Push(this);
    }

  
}
