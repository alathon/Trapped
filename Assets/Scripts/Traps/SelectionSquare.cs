using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Controllers;

public class SelectionSquare : MonoBehaviour {
    private FollowMouse followMouse;
    private TrapAction trapAction;
    private Image targetImage;
    private GameController controller;

    void Awake()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.followMouse = this.GetComponent<FollowMouse>();
        this.trapAction = this.GetComponent<TrapAction>();
        this.targetImage = transform.Find("ReadyIndicatorCanvas/TargetImage").GetComponent<Image>();
    }

    void OnMouseEnter()
    {
        if (this.followMouse.state == FollowMouse.PlacementState.Placed && 
            this.controller.GetCurrentPhase() == Phase.Action &&
            this.trapAction.CanUse())
        {
            Color prev = this.targetImage.color;
            this.targetImage.color = new Color(prev.r, prev.g, prev.b, 0.75f);
        }
    }

    void OnMouseExit()
    {
        if (this.followMouse.state == FollowMouse.PlacementState.Placed)
        {
            Color prev = this.targetImage.color;
            this.targetImage.color = new Color(prev.r, prev.g, prev.b, 0f);
        }
    }
}
