using UnityEngine;
using System.Collections;

public class EnsureComponents : MonoBehaviour {
    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = (GameObject)Instantiate(Resources.Load("Player"));
            GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
            player.transform.position = spawnPoint.transform.position;
            GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
            mainCam.GetComponent<FollowingCamera>().SetTarget(player.transform);
        }

        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        if (controller == null)
        {
            Instantiate(Resources.Load("GameController"));
        }
    }
}
