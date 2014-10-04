using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
	void Update () {
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
	}
}
