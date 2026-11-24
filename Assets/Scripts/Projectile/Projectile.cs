using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField]
    [Min(0.01f)]
    protected float _maxLifespan;

    [SerializeField]
    protected float _speed;

    [SerializeField]
    protected bool _hasGravity;
    
    public float Speed { get { return _speed; } }
    
    public float MaxLifespan { get { return _maxLifespan; } }
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    protected Rigidbody _rigidbody;

    public Rigidbody Rigidbody {  get { return _rigidbody; } }
    
    [SerializeField]
    protected Entity _source;
    
    public Entity Source { get { return _source; } }

    [SerializeField]
    protected Hitbox _hitbox;

    [SerializeField]
    protected float _lifeTime;

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _hitbox = GetComponentInChildren<Hitbox>();
        
        if (_hitbox == null)
        {
            Debug.LogError("No Hitbox found", this);
            enabled = false;
            return;
        }
        
        _rigidbody.useGravity = _hasGravity;
        
        _hitbox.HitOccur += OnHit;
    }

    protected void Start()
    {
        OnProjectileStart();
    }

    protected void OnDestroy()
    {
        OnProjectileDestroy();
        
        if(_hitbox != null)
            _hitbox.HitOccur -= OnHit;
    }

    protected void Update()
    {
        _lifeTime += Time.deltaTime;

        if (_lifeTime >= _maxLifespan)
        {
            Destroy(gameObject);
            return;
        }

        ProjectileTick();
    }

    protected void FixedUpdate()
    {
        ProjectileFixedTick();
    }
    
    protected void OnHit(Entity hit, Entity source)
    {
        //Debug.Log($"Projectile fired by {source.name} hit {hit.name}");
        if (hit == source)
            return;
        
        OnProjectileHit(hit, source);
    }

    public void CountHit(Entity hit, Entity source)
    {
        OnHit(hit, source);
    }
    
    protected virtual void OnProjectileStart() { }
    
    protected virtual void OnProjectileDestroy() { }

    protected virtual void OnProjectileHit(Entity hit, Entity source)
    {
        if (hit == source)
            return;
        
        //Debug.Log($"Projectile shot by {source.name} hit {hit.name}");
        _rigidbody.linearVelocity = Vector3.zero;
        Destroy(gameObject);
    }

    protected virtual void ProjectileTick() { }
    
    protected virtual void ProjectileFixedTick() { }

    // Only create projectiles through this
    public static Projectile LaunchProjectile(Projectile projectile, Vector3 velocity, Entity source)
    {
        if (projectile == null || source == null)
            return null;
        
        Vector3 position = source.transform.position;
        Projectile launched = Instantiate(projectile, position
                                                      + velocity.normalized * 2.5f, source.transform.rotation);
        
        if (launched._hitbox == null)
        {
            //Debug.Log("Projectile missing hitbox");
            Destroy(launched.gameObject);
            return null;
        }
        
        //Debug.Log($"Projectile {launched.name} launched by {source.name}");
        //Debug.Log($"Projectile launched at {position}");
        
        launched._source = source;
        launched._rigidbody.linearVelocity = velocity;
        launched._hitbox.Parent = source;
        
        Quaternion rotation = Quaternion.LookRotation(velocity.normalized);
        launched.transform.rotation = rotation;
        
        return launched;
    }
}
