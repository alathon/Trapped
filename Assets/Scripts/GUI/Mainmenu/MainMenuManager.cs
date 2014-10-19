using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
    [SerializeField]
    private GameObject gameControllerPrefab;

    public void StartGame()
    {
        Application.LoadLevel("1");
    }

    public void About()
    {

    }
}
