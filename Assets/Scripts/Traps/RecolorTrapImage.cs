using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecolorTrapImage : MonoBehaviour {
    private Image placementSquare;

    void Awake()
    {
        this.placementSquare = transform.Find("ReadyIndicatorCanvas/PlacementSquare").GetComponent<Image>();
    }

    void OnPlacementValidChange(bool isValid)
    {
        this.placementSquare.color = new Color(this.placementSquare.color.r, this.placementSquare.color.g, this.placementSquare.color.b, isValid ? 0f : 0.75f);
    }
}
