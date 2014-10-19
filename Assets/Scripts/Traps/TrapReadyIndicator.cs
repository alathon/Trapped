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
        controller.PhaseChanged += new GameController.PhaseChangedHandler(OnPhaseChange);
        circleImage = this.transform.Find("ReadyIndicatorCanvas/ReadyIndicator").GetComponent<Image>();
    }

    public void OnPhaseChange(Phase newPhase)
    {
        if (newPhase == Phase.Planning)
        {
            this.HideReadyIndicator();
        }
        else if (newPhase == Phase.Action)
        {
            this.OnUseUpdate(false);
        }

        this.isActive = newPhase == Phase.Action;
    }

    public void OnUseUpdate(bool inUse)
    {
        if (!isActive) return;

        if (inUse)
        {
            circleImage.color = new Color(255, 0, 0, circleImage.color.a);
        }
        else
        {
            circleImage.color = new Color(0, 255, 0, circleImage.color.a);
        }
    }

    void OnMouseEnter()
    {
        if (this.isActive)
        {
            this.ShowReadyIndicator();
        }
    }

    void OnMouseExit()
    {
        this.HideReadyIndicator();
    }

    void ShowReadyIndicator()
    {
        Color prev = circleImage.color;
        circleImage.color = new Color(prev.r, prev.g, prev.b, 1f);
    }

    void HideReadyIndicator()
    {
        Color prev = circleImage.color;
        circleImage.color = new Color(prev.r, prev.g, prev.b, 0f);
    }
}
