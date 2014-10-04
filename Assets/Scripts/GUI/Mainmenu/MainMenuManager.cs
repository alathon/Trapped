using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
    [SerializeField]
    private GameObject gameControllerPrefab;

    public void StartGame()
    {
        if (!GameObject.FindGameObjectWithTag("GameController"))
        {
            Instantiate(gameControllerPrefab);
        }
        Application.LoadLevel("Test");
    }

    public void About()
    {

    }
}
