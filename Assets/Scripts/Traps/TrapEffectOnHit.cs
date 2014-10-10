using UnityEngine;
using System.Collections;

public class TrapEffectOnHit : MonoBehaviour {
    [SerializeField]
    private int damage = 1;

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject target = col.transform.gameObject;
        if (target.layer == LayerMask.NameToLayer("Obstacles"))
        {
            GameObject.Destroy(this.gameObject);
        }
        else if (target.tag.Equals("Enemy"))
        {
            AI ai = target.GetComponent<AI>();
            if(ai != null)
                ai.TakeDamage(this.damage);
            GameObject.Destroy(this.gameObject);
        }
    }
}