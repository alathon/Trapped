using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapEffectOnHit : MonoBehaviour {
    [SerializeField]
    protected int damage = 1;

    [SerializeField]
    protected bool destroyOnHit = true;

    [SerializeField]
    protected bool hitOnce = true;

    private List<GameObject> hits = new List<GameObject>();

    protected virtual void EffectState(UnitState state)
    {
        state.TakeDamage(this.damage);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject target = col.transform.gameObject;
        if (target.tag.Equals("Hitbox"))
        {
            target = target.transform.parent.gameObject;
        }

        if (hitOnce)
        {
            if (hits.Contains(target)) return;
            hits.Add(target);
        }
        
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
    }
}