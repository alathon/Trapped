using UnityEngine;
using System.Collections;

public class ActionPhaseUIManager : MonoBehaviour {
    private GameObject actionPhaseGUI;
    private GameController controller;
    private CanvasGroup canvasGroup;

	// Use this for initialization
	void Start () {
        this.actionPhaseGUI = GameObject.FindGameObjectWithTag("ActionPhase_GUI");
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.canvasGroup = this.actionPhaseGUI.GetComponent<CanvasGroup>();
	}

    public void ActivateGUI()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void DeactivateGUI()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
