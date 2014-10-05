using UnityEngine;
using System.Collections;

public class RecolorTrapImage : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void OnPlacementValidChange(bool isValid)
    {
        this.spriteRenderer.color = isValid ? Color.white : Color.red;
    }
}
