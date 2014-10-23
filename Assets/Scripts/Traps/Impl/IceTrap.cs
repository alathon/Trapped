using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceTrap : TrapAction {
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int numOfIce = 4;

    [SerializeField]
    private float timeBetweenIce = 0.35f;

    private List<GameObject> iceSpawns = new List<GameObject>();
    protected override IEnumerator Action()
    {
        // Spawn numOfIce prefabs in the cardinal directions.
        for (int i = 0; i < numOfIce; i++)
        {
            float offset = 0.20f + (0.15f * i);
            Vector2[] spawns = new Vector2[4];
            // North
            spawns[0] = this.transform.position + (this.transform.up * offset);
            // South
            spawns[1] = this.transform.position - (this.transform.up * offset);
            // East
            spawns[2] = this.transform.position + (this.transform.right * offset);
            // West
            spawns[3] = this.transform.position - (this.transform.right * offset);

            foreach(Vector2 v in spawns) {
                GameObject ice = (GameObject)Instantiate(this.prefab, v, Quaternion.identity);
                iceSpawns.Add(ice);
            }
            yield return new WaitForSeconds(this.timeBetweenIce);
        }
        yield return new WaitForSeconds(this.duration);
        foreach (GameObject gObj in iceSpawns)
        {
            GameObject.Destroy(gObj);
        }
        this.iceSpawns = new List<GameObject>();
        yield return StartCoroutine(PostAction());
    }

    protected virtual void Reset()
    {
        base.Reset();
        foreach (GameObject gObj in iceSpawns)
        {
            GameObject.Destroy(gObj);
        }
        this.iceSpawns = new List<GameObject>();
    }
}
