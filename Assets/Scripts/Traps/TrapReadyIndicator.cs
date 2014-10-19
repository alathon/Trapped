using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Controllers;

[RequireComponent(typeof(Collider2D))]
public class TrapReadyIndicator : MonoBehaviour {
    private Image circleImage;
    private GameController controller;
    private bool isActive = false;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        controller.PhaseChanged += new GameController.PhaseChangedHandler(OnPhaseChanged);
        circleImage = this.transform.Find("ReadyIndicatorCanvas/ReadyIndicator").GetComponent<Image>();
        circleImage.color = new Color(0, 255, 0, 1f);
    }

    public void OnPhaseChanged(Phase newPhase)
    {
        //this.OnUseUpdate(false);
    }

    public void OnUseUpdate(bool inUse)
    {
        if (inUse)
        {
            circleImage.color = new Color(255, 0, 0, 1f);
        }
        else
        {
            circleImage.color = new Color(0, 255, 0, 1f);
        }
    }

    //void OnMouseEnter()
    //{
    //    if (this.controller.GetPhase() == Phase.Action)
    //    {
    //        this.ShowReadyIndicator();
    //    }
    //}

    //void OnMouseExit()
    //{
    //    this.HideReadyIndicator();
    //}

    //void ShowReadyIndicator()
    //{
    //    Color prev = circleImage.color;
    //    circleImage.color = new Color(prev.r, prev.g, prev.b, 1f);
    //}

    //void HideReadyIndicator()
    //{
    //    Color prev = circleImage.color;
    //    circleImage.color = new Color(prev.r, prev.g, prev.b, 1f);
    //}
}
