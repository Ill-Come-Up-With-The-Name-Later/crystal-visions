using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField]
    [Min(0.01f)]
    private float _maxLifespan;

    [SerializeField]
    private float _speed;
    
    public float Speed { get { return _speed; } }
    
    public float MaxLifespan { get { return _maxLifespan; } }
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody {  get { return _rigidbody; } }
    
    [SerializeField]
    private Entity _source;
    
    public Entity Source { get { return _source; } }

    [SerializeField]
    private Hitbox _hitbox;

    [SerializeField]
    private float _lifeTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _hitbox = GetComponentInChildren<Hitbox>();
        
        if (_hitbox == null)
        {
            Debug.LogError("No Hitbox found", this);
            enabled = false;
            return;
        }
        
        _hitbox.HitOccur += OnHit;
    }

    private void Start()
    {
        OnProjectileStart();
    }

    private void OnDestroy()
    {
        OnProjectileDestroy();
        
        if(_hitbox != null)
            _hitbox.HitOccur -= OnHit;
    }

    private void Update()
    {
        _lifeTime += Time.deltaTime;

        if (_lifeTime >= _maxLifespan)
        {
            Destroy(gameObject);
            return;
        }

        ProjectileTick();
    }

    private void FixedUpdate()
    {
        ProjectileFixedTick();
    }
    
    private void OnHit(Entity hit, Entity source)
    {
        OnProjectileHit(hit, source);
    }
    
    protected virtual void OnProjectileStart() { }
    
    protected virtual void OnProjectileDestroy() { }

    protected virtual void OnProjectileHit(Entity hit, Entity source)
    {
        Debug.Log($"Projectile shot by {source.name} hit {hit.name}");
        Destroy(gameObject);
    }

    protected virtual void ProjectileTick() { }
    
    protected virtual void ProjectileFixedTick() { }

    // Only create projectiles through this
    public static Projectile LaunchProjectile(Projectile projectile, Vector3 velocity, Entity source)
    {
        if (projectile == null || source == null)
            return null;
        
        Projectile launched = Instantiate(projectile, source.transform.position, source.transform.rotation);
        
        if (launched._hitbox == null)
        {
            Destroy(launched.gameObject);
            return null;
        }
        
        launched._source = source;
        launched._rigidbody.linearVelocity = velocity;
        launched._hitbox.Parent = source;
        return launched;
    }
}
