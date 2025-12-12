using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitboxScript : MonoBehaviour
{
    [SerializeField] HitLayer _hitLayers;
    
    void RegisterHit(HurtboxScript hurtbox)
    {
        LogUtils.Log(this, $"Hit target: {hurtbox.gameObject.name}");
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.TryGetComponent(out HurtboxScript hurtbox)) return;
        if((_hitLayers & hurtbox.Layers) == 0) return;
        
        hurtbox.RegisterHit(this);
        RegisterHit(hurtbox);
    }

    public AttackHandler Master { get; private set; }
    void Awake()
    {
        Master = GetComponentInParent<AttackHandler>();
    }
}