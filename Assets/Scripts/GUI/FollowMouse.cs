using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
    /// <summary>
    /// Placing = Clicked on, selecting position.
    /// Angling = Positioned, selecting angle.
    /// Placed  = Done angling.
    /// </summary>
    public enum PlacementState
    {
        Placing, Angling, Placed
    }

    public PlacementState state;

    /// <summary>
    /// When not placed, change position.
    /// When placed, change trap to face looking at the mouse cursor.
    /// </summary>
	void Update () {
        if (state == PlacementState.Placing || state == PlacementState.Angling)
        {
            Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (state == PlacementState.Placing)
            {
                transform.position = new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
            }
            else if (state == PlacementState.Angling)
            {
                var dir = mousePosInWorld - transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                //transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
            }
        }
	}
}