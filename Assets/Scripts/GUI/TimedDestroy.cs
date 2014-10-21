using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {
    [SerializeField]
    private float destroyTime = 5f;

	// Use this for initialization
	void Start () {
        Invoke("DestroyMe", this.destroyTime);
	}

    void DestroyMe()
    {
        GameObject.Destroy(this.gameObject);
    }
}
