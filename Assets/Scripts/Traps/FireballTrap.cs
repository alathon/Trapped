using UnityEngine;
using System.Collections;

public class FireballTrap : TrapAction {
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float fireballSpeed = 0.8f;

    public override void Trigger()
    {
        for (int i = 0; i < 1; i++)
        {
            GameObject fire = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
            fire.transform.SetParent(this.transform);
            fire.GetComponent<Rigidbody2D>().velocity = this.transform.up * this.fireballSpeed;

            //float xDir = Random.Range(1, 10) > 6 ? -1f : 1f;
            //float yDir = Random.Range(1, 10) > 6 ? -1f : 1f;
            //fire.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(0.3f, 0.6f) * xDir, Random.Range(0.3f, 0.6f) * yDir, 0f);
        }
        //GameObject pSystem = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
        //pSystem.transform.SetParent(this.transform);
        //pSystem.transform.RotateAroundLocal(Vector3.right, this.transform.rotation.z);
    }


}