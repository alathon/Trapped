using UnityEngine;
using System.Collections;

public class ChangeSortingOrder : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        col.GetComponent<SpriteRenderer>().sortingOrder = -1;
    }

    void OnTriggerExit(Collider col)
    {
        col.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
