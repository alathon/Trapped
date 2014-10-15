﻿using UnityEngine;
using System.Collections;

public class TrapEffectOnHit : MonoBehaviour {
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private bool destroyOnHit = true;

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
                state.TakeDamage(this.damage);
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
                    state.TakeDamage(this.damage);
                    if (destroyOnHit) GameObject.Destroy(this.gameObject);
                }
            }
            
        }
    }
}