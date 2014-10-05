using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D))]
public class ValidTrapPlacement : MonoBehaviour {

    private bool isValid = false;
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

    private PolygonCollider2D trapCollider;

    void Awake()
    {
        trapCollider = this.GetComponent<PolygonCollider2D>();
    }

    void LateUpdate()
    {
        foreach (Vector2 point in trapCollider.points)
        {
            Vector2 worldSpacePoint = this.transform.TransformPoint(point);
            var hit = Physics2D.Raycast(worldSpacePoint, Vector2.zero, 0f);
            if (hit.collider == null) continue;

            GameObject gObj = hit.collider.transform.gameObject;
            string gObjName = gObj.name;
            if (gObjName.Equals("NavObstacle"))
            {
                this.IsValid = false;
                return;
            }
        }

        this.IsValid = true;
    }
}