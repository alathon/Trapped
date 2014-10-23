using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowTrap : TrapAction {

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float arrowSpeed = 1.5f;

    [SerializeField]
    private int arrows = 3;

    private List<GameObject> arrowSpawns = new List<GameObject>();

    protected override IEnumerator Action()
    {
        Transform spriteTransform = this.transform.Find("Sprite");  // We use the sprite's transform, because thats the one thats
                                                                    // rotated, not this one.
        for (int i = 0; i < arrows; i++)
        {
            GameObject arrow = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
            this.arrowSpawns.Add(arrow);
            arrow.transform.SetParent(this.transform);
            arrow.GetComponent<Rigidbody2D>().velocity = spriteTransform.up * this.arrowSpeed;
            var vel = arrow.GetComponent<Rigidbody2D>().velocity;
            var angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            //arrow.transform.RotateAroundLocal(Vector3.right, this.transform.rotation.z);
            yield return new WaitForSeconds(this.duration / this.arrows);
        }
        yield return StartCoroutine(PostAction());
        //GameObject pSystem = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
        //pSystem.transform.SetParent(this.transform);
        //pSystem.transform.RotateAroundLocal(Vector3.right, this.transform.rotation.z);
    }

    protected virtual void Reset()
    {
        base.Reset();
        foreach (GameObject gObj in arrowSpawns)
        {
            GameObject.Destroy(gObj);
        }
        this.arrowSpawns = new List<GameObject>();
    }

}