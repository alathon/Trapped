using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class DrawingOrderTrigger : MonoBehaviour {
    [SerializeField]
    private int onEnterOrder = -1;

    [SerializeField]
    private int onExitOrder = 1;

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject target = col.transform.gameObject;
        if (target.tag.Equals("Enemy") || target.tag.Equals("Player"))
        {
            target.GetComponent<SpriteRenderer>().sortingOrder = this.onEnterOrder;
        }
        //else if (target.tag.Equals("Hitbox"))
        //{
        //    target.transform.parent.GetComponent<SpriteRenderer>().sortingOrder = this.onEnterOrder;
        //}
    }

    void OnTriggerExit2D(Collider2D col)
    {
        GameObject target = col.transform.gameObject;
        if (target.tag.Equals("Enemy") || target.tag.Equals("Player"))
        {
            target.GetComponent<SpriteRenderer>().sortingOrder = this.onExitOrder;
        }
        //else if (target.tag.Equals("Hitbox"))
        //{
        //    target.transform.parent.GetComponent<SpriteRenderer>().sortingOrder = this.onExitOrder;
        //}
    }
}
