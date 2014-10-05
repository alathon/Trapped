using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
    public bool IsPlaced
    {
        get;
        set;
    }

    /// <summary>
    /// When not placed, change position.
    /// When placed, change trap to face looking at the mouse cursor.
    /// </summary>
	void Update () {
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!IsPlaced) { 
            transform.position = new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
        }
        else
        {
            var dir = mousePosInWorld - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
	}
}
