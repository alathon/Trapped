using UnityEngine;
using System.Collections;
using System;

public class WaveAwards : MonoBehaviour {
    private GameController controller;

	// Use this for initialization
	void Start () {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        controller.WaveChanged += new GameController.WaveChangedHandler(OnWaveChanged);
        
	}

    public void OnWaveChanged(int newWave)
    {
        string level = Application.loadedLevelName;

        if(level.Equals("1")) {
            if(newWave == 1) {
                GameObject bomb = controller.GetComponent<TrapPrefabsManager>().GetTrapObjByName("Bomb Trap");
                controller.AddTraps(bomb, 3);
            } else if(newWave == 2) {
                GameObject spike = controller.GetComponent<TrapPrefabsManager>().GetTrapObjByName("Spike Trap");
                controller.AddTraps(spike, 2);
            }
        }
    }
}
