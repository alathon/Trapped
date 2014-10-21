using UnityEngine;
using System.Collections;

public class ProjectileEffect : MonoBehaviour {
    [SerializeField]
    protected int damage = 1;

    [SerializeField]
    protected bool destroyOnHit = true;

    protected bool firedByPlayer = false;

    public float speed = 4.5f;

    public void SetFiredByPlayer(bool val)
    {
        this.firedByPlayer = val;
    }

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

        string enemyTag = this.firedByPlayer ? "Enemy" : "Player";
        if (target.tag.Equals(enemyTag))
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
            if (tParent.tag.Equals(enemyTag))
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