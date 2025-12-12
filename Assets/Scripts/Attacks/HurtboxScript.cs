using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HurtboxScript : MonoBehaviour
{
    [SerializeField] HitLayer _hitLayers;
    public HitLayer Layers { get => _hitLayers; private set => _hitLayers = value; }

    public void RegisterHit(HitboxScript hitbox)
    {
        LogUtils.Log(this, $"Got hit by: {hitbox.gameObject.name}");        
    }

    public HurtHandler Master { get; private set; }
    void Awake()
    {
        Master = GetComponentInParent<HurtHandler>();
    }
}