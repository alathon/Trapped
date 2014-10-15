using UnityEngine;
using System.Collections;

public class FireballTrap : TrapAction {
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float fireballSpeed = 0.8f;

    [SerializeField]
    private int fireballs = 5;

    protected override IEnumerator Action()
    {
        for (int i = 0; i < fireballs; i++)
        {
            GameObject fire = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
            fire.transform.SetParent(this.transform);
            fire.GetComponent<Rigidbody2D>().velocity = this.transform.up * this.fireballSpeed;
            //fire.transform.RotateAroundLocal(Vector3.right, this.transform.rotation.z);
            yield return new WaitForSeconds(this.duration / this.fireballs);
        }

        yield return StartCoroutine(PostAction());
        //GameObject pSystem = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
        //pSystem.transform.SetParent(this.transform);
        //pSystem.transform.RotateAroundLocal(Vector3.right, this.transform.rotation.z);
    }
}