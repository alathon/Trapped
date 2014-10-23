using UnityEngine;
using System.Collections;
using System;

public class WaveAwards : MonoBehaviour {
    private GameController controller;

	// Use this for initialization
	void Start () {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        controller.WaveChanged += OnWaveChanged; //  new GameController.WaveChangedHandler(OnWaveChanged);
        
	}

    void OnDestroy()
    {
        controller.WaveChanged -= OnWaveChanged;
    }

    public void OnWaveChanged(int newWave)
    {
        string level = Application.loadedLevelName;

        if(level.Equals("1")) {
            if(newWave == 1) {
                GameObject bomb = controller.GetComponent<TrapPrefabsManager>().GetTrapObjByName("Bomb Trap");
                controller.AddTraps(bomb, 3);
            }
        } else if(level.Equals("2")) {
            if(newWave == 1) {
                GameObject spike = controller.GetComponent<TrapPrefabsManager>().GetTrapObjByName("Spike Trap");
                GameObject bomb = controller.GetComponent<TrapPrefabsManager>().GetTrapObjByName("Bomb Trap");
                controller.AddTraps(spike, 2);
                controller.AddTraps(bomb, 2);
            }
            else if (newWave == 3)
            {
                GameObject arrow = controller.GetComponent<TrapPrefabsManager>().GetTrapObjByName("Arrow Trap");
                GameObject spike = controller.GetComponent<TrapPrefabsManager>().GetTrapObjByName("Spike Trap");
                GameObject bomb = controller.GetComponent<TrapPrefabsManager>().GetTrapObjByName("Bomb Trap");
                controller.AddTraps(spike, 2);
                controller.AddTraps(arrow, 3);
            }
        }
    }
}
