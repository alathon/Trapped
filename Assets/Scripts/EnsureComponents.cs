using UnityEngine;
using System.Collections;

public class EnsureComponents : MonoBehaviour {
    void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        if (controller == null)
        {
            Instantiate(Resources.Load("GameController"));
        }
    }
}
