using UnityEngine;
using System.Collections;

public class TrapEffectOnHit : MonoBehaviour {
    [SerializeField]
    protected int damage = 1;

    [SerializeField]
    protected bool destroyOnHit = true;

    protected virtual void EffectState(UnitState state)
    {
        state.TakeDamage(this.damage);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject target = col.transform.gameObject;
        if (target.layer == LayerMask.NameToLayer("Obstacles"))
        {
            if(destroyOnHit) GameObject.Destroy(this.gameObject);
        }
        else if (target.tag.Equals("Enemy"))
        {
            UnitState state = target.GetComponent<UnitState>();
            if (state != null)
            {
                this.EffectState(state);
                if (destroyOnHit) GameObject.Destroy(this.gameObject);
            }
        }
        else if (target.tag.Equals("Hitbox"))
        {
            Transform tParent = target.transform.parent;
            if (tParent.GetComponent<AI>() != null)
            {
                UnitState state = tParent.GetComponent<UnitState>();
                if (state != null)
                {
                    this.EffectState(state);
                    if (destroyOnHit) GameObject.Destroy(this.gameObject);
                }
            }
            
        }
    }
}