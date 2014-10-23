using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
    [SerializeField]
    private GameObject gameControllerPrefab;

    public void StartGame()
    {
        GameObject player = (GameObject)Instantiate(Resources.Load("Player"));
        GameObject controllerObj = (GameObject)Instantiate(Resources.Load("GameController"));
        GameController controller = controllerObj.GetComponent<GameController>();
        player.GetComponent<UnitState>().Death += new UnitState.DeathHandler(controller.OnPlayerDeath);
        Application.LoadLevel("1");
    }

    public void About()
    {

    }
}
