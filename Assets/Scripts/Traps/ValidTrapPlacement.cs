using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ValidTrapPlacement : MonoBehaviour {

    private bool isValid = true;
    public bool IsValid
    {
        get { 
            return this.isValid; 
        }

        set {
            if (value == this.isValid)
            {
                return;
            }

            this.isValid = value;
            this.gameObject.SendMessage("OnPlacementValidChange", this.isValid);
        }
    }

    private Collider2D trapCollider;

    void Awake()
    {
        trapCollider = this.GetComponent<Collider2D>();
    }

    private int collisions = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject gObj = other.gameObject;
        if (gObj == this.gameObject) return;


        if (gObj.name.Equals("NavObstacle") || gObj.tag.Equals("Trap"))
        {
            this.IsValid = false;
            this.collisions += 1;
            return;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        GameObject gObj = other.gameObject;
        if (gObj == this.gameObject) return;

        if (gObj.name.Equals("NavObstacle") || gObj.tag.Equals("Trap"))
        {
            this.collisions -= 1;
        }

        if (this.collisions <= 0)
        {
            this.collisions = 0;
            this.IsValid = true;
        }
    }

    //void LateUpdate()
    //{
    //    foreach (RaycastHit2D hit in Physics2D.BoxCastAll(trapCollider.bounds.center, trapCollider.bounds.size, 0f, Vector2.zero)) {
    //        GameObject target = hit.transform.gameObject;

    //        // Hitting a trap? No go.
    //        if (target.tag.Equals("Trap"))
    //        {
    //            Debug.Log("Hit trap. Not valid!");
    //            this.IsValid = false;
    //            return;
    //        }
    //        else if (target.name.Equals("NavObstacle")) // Wall? Wall is okay, unless the sprite itself overlaps. Sprite is 32x32.
    //        {
    //            float dist = Vector2.Distance(target.transform.position, this.transform.position);
    //            Debug.Log("Distance to NavObstacle: " + dist);
    //            if (dist < 0.32f)
    //            {
    //                this.IsValid = false;
    //                return;
    //            }
    //        }
    //    }

    //    this.IsValid = true;
    //}

    //void LateUpdate()
    //{
    //    foreach (Collider2D col in Physics2D.BoxCastAll(trapCollider.bounds.center, trapCollider.bounds.size, 0f, )
    //    foreach (Vector2 point in trapCollider.points)
    //    {
    //        Vector2 worldSpacePoint = this.transform.TransformPoint(point);
    //        var hit = Physics2D.Raycast(worldSpacePoint, Vector2.zero, 0f);
    //        if (hit.collider == null) continue;

    //        GameObject gObj = hit.collider.transform.gameObject;
    //        string gObjName = gObj.name;
    //        if (gObjName.Equals("NavObstacle"))
    //        {
    //            this.IsValid = false;
    //            return;
    //        }
    //    }

    //    this.IsValid = true;
    //}
}