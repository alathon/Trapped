using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Controllers;

public class TrapReadyIndicator : MonoBehaviour {
    private Image circleImage;
    private GameController controller;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        controller.PhaseChanged += new GameController.PhaseChangedHandler(OnPhaseChanged);
        circleImage = this.transform.Find("ReadyIndicatorCanvas/ReadyIndicator").GetComponent<Image>();
        circleImage.color = new Color(0, 255, 0, 0f);
    }

    void OnDestroy()
    {
        controller.PhaseChanged -= OnPhaseChanged;
    }

    public void OnPhaseChanged(Phase newPhase)
    {
        if (this.circleImage != null)
        {
            if (newPhase == Phase.Action)
            {
                Color prev = this.circleImage.color;
                this.circleImage.color = new Color(prev.r, prev.g, prev.b, 1f);
            }
            else
            {
                Color prev = this.circleImage.color;
                this.circleImage.color = new Color(prev.r, prev.g, prev.b, 0f);
            }
        }
    }

    public void OnUseUpdate(bool inUse)
    {
        if (this.circleImage != null)
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
    }
}
